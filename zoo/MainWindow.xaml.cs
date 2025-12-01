using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SQLite; 
using System.IO;

namespace zoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=Zoo.db;Version=3;Foreign Keys=True;";
        SQLiteConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            InicializarBaseDeDatos();
            sqlConnection = new SQLiteConnection(connectionString);
            MuestraZoos();
            MuestraAnimales();
        }

        /// <summary>
        /// Crea la base de datos y las tablas si no existen.
        /// </summary>
        private void InicializarBaseDeDatos()
        {
            if (!File.Exists("Zoo.db")) SQLiteConnection.CreateFile("Zoo.db");

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Zoo (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        Ubicacion TEXT NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS Animal (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        Nombre TEXT NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS AnimalZoo (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ZooId INTEGER NOT NULL,
                        AnimalId INTEGER NOT NULL,
                        FOREIGN KEY(ZooId) REFERENCES Zoo(Id) ON DELETE CASCADE,
                        FOREIGN KEY(AnimalId) REFERENCES Animal(Id) ON DELETE CASCADE
                    );";

                using (var cmd = new SQLiteCommand(sql, conn)) { cmd.ExecuteNonQuery(); }

                // Insertar datos de prueba si está vacía
                string check = "SELECT COUNT(*) FROM Zoo";
                using (var cmd = new SQLiteCommand(check, conn))
                {
                    if ((long)cmd.ExecuteScalar() == 0)
                    {
                        using (var insert = new SQLiteCommand("INSERT INTO Zoo (Ubicacion) VALUES ('Nueva York'), ('Tokio'), ('Berlín'); INSERT INTO Animal (Nombre) VALUES ('Tiburón'), ('León'), ('Panda');", conn))
                        { insert.ExecuteNonQuery(); }
                    }
                }
            }
        }

        #region MÉTODOS DE LECTURA

        private void MuestraZoos()
        {
            try
            {
                string query = "SELECT * FROM Zoo";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, sqlConnection);
                using (adapter)
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ListaZoos.DisplayMemberPath = "Ubicacion";
                    ListaZoos.SelectedValuePath = "Id";
                    ListaZoos.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MuestraAnimales()
        {
            try
            {
                string query = "SELECT * FROM Animal";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, sqlConnection);
                using (adapter)
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ListaAnimales.DisplayMemberPath = "Nombre";
                    ListaAnimales.SelectedValuePath = "Id";
                    ListaAnimales.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MuestraAnimalesAsociados()
        {
            try
            {
                string query = "SELECT a.Nombre, a.Id FROM Animal a " +
                               "INNER JOIN AnimalZoo az ON a.Id = az.AnimalId " +
                               "WHERE az.ZooId = @ZooId";
                SQLiteCommand cmd = new SQLiteCommand(query, sqlConnection);

                if (ListaZoos.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    using (adapter)
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ListaAnimalesAsociados.DisplayMemberPath = "Nombre";
                        ListaAnimalesAsociados.SelectedValuePath = "Id";
                        ListaAnimalesAsociados.ItemsSource = dt.DefaultView;
                    }
                }
                else { ListaAnimalesAsociados.ItemsSource = null; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

        #region EVENTOS DE SELECCIÓN

        private void ListaZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraAnimalesAsociados();
            if (ListaZoos.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ListaZoos.SelectedItem;
                miTextBox.Text = row["Ubicacion"].ToString();
            }
        }

        private void ListaAnimales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaAnimales.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ListaAnimales.SelectedItem;
                miTextBox.Text = row["Nombre"].ToString();
            }
        }

        #endregion

        #region BOTONES CRUD

        private void EjecutarSQL(string sql, string param, object val)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                if (param != null) cmd.Parameters.AddWithValue(param, val);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { sqlConnection.Close(); }
        }

        // --- ZOO ---
        private void BorrarZoo_Click(object sender, RoutedEventArgs e)
        {
            EjecutarSQL("DELETE FROM Zoo WHERE Id = @Id", "@Id", ListaZoos.SelectedValue);
            MuestraZoos();
        }

        private void AgregarZoo_Click(object sender, RoutedEventArgs e)
        {
            EjecutarSQL("INSERT INTO Zoo (Ubicacion) VALUES (@Val)", "@Val", miTextBox.Text);
            MuestraZoos();
        }

        private void ActualizarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaZoos.SelectedValue == null) return;
                sqlConnection.Open();
                string sql = "UPDATE Zoo SET Ubicacion = @Val WHERE Id = @Id";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@Val", miTextBox.Text);
                cmd.Parameters.AddWithValue("@Id", ListaZoos.SelectedValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { sqlConnection.Close(); MuestraZoos(); }
        }

        // --- ANIMAL ---
        private void BorrarAnimal_Click(object sender, RoutedEventArgs e)
        {
            EjecutarSQL("DELETE FROM Animal WHERE Id = @Id", "@Id", ListaAnimales.SelectedValue);
            MuestraAnimales();
        }

        private void AgregarAnimal_Click(object sender, RoutedEventArgs e)
        {
            EjecutarSQL("INSERT INTO Animal (Nombre) VALUES (@Val)", "@Val", miTextBox.Text);
            MuestraAnimales();
        }

        private void ActualizarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaAnimales.SelectedValue == null) return;
                sqlConnection.Open();
                string sql = "UPDATE Animal SET Nombre = @Val WHERE Id = @Id";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@Val", miTextBox.Text);
                cmd.Parameters.AddWithValue("@Id", ListaAnimales.SelectedValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { sqlConnection.Close(); MuestraAnimales(); }
        }

        // --- RELACIONES ---
        private void AgregarAnimalAZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaZoos.SelectedValue == null || ListaAnimales.SelectedValue == null)
                { MessageBox.Show("Selecciona Zoo y Animal"); return; }

                sqlConnection.Open();
                string sql = "INSERT INTO AnimalZoo (ZooId, AnimalId) VALUES (@Z, @A)";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@Z", ListaZoos.SelectedValue);
                cmd.Parameters.AddWithValue("@A", ListaAnimales.SelectedValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { sqlConnection.Close(); MuestraAnimalesAsociados(); }
        }

        private void QuitarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaZoos.SelectedValue == null || ListaAnimalesAsociados.SelectedValue == null) return;
                sqlConnection.Open();
                string sql = "DELETE FROM AnimalZoo WHERE ZooId = @Z AND AnimalId = @A";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@Z", ListaZoos.SelectedValue);
                cmd.Parameters.AddWithValue("@A", ListaAnimalesAsociados.SelectedValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { sqlConnection.Close(); MuestraAnimalesAsociados(); }
        }

        #endregion
    }
}