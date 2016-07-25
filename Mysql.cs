using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace M77
{
    public class Mysql
    {
        private MySqlConnection connection;
        public static string server = "";
        public static string database = "";
        public static string user = "";
        public static string password = "";

        public static bool show_error = true;

        public bool rows = false;

        public Mysql()
        {
            Initialize();
        }

        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + Mysql.server + ";" + "DATABASE=" + Mysql.database + ";" + "UID=" + Mysql.user + ";" + "PASSWORD=" + Mysql.password + ";";
            connection = new MySqlConnection(connectionString);
        }

        public static bool isOpen() {
            Mysql test = new Mysql();         
            bool connected = test.OpenConnection();
            test.CloseConnection();
            return connected;
        }

        public bool OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                if (show_error)
                {
                    switch (ex.Number)
                    {
                        case 1042:
                            MessageBox.Show("MYSQL: No se pudo conectar al servidor.");
                            break;
                        case 1045:
                            MessageBox.Show("MYSQL: Usuario/Password incorrectos.");
                            break;
                        case 1044:
                            MessageBox.Show("MYSQL: Acceso denegado a la tabla.");
                            break;
                        default:
                            MessageBox.Show("MYSQL: (" + ex.Number + ") " + ex.Message);
                            break;
                    }
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                if (show_error)
                {
                    MessageBox.Show(ex.Message);
                }
                return false;
            }
        }

        public bool Ejecutar(string query)
        {
            bool rs = false;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                try
                {
                    if (cmd.ExecuteNonQuery() > 0) { rs = true; }
                }
                catch (MySqlException ex)
                {
                    if (show_error)
                    {
                        switch (ex.Number)
                        {
                            case 1451: // Existen campos enlazados a clave referenciada
                                MessageBox.Show("MYSQL: Existen datos enlazados a este campo, no se puede eliminar.");
                                break;
                            case 1062: // Duplicados en UNIQUE
                                MessageBox.Show("MYSQL: Elemento duplicado, ya se encuentra registrado.");
                                break;
                            default:
                                MessageBox.Show("MYSQL: (" + ex.Number + ") " + ex.Message);
                                break;
                        }
                    }
                    rs = false;
                }
                this.CloseConnection();
            }
            return rs;
        }

        public DataTable Select(string query)
        {
            DataTable rs = new DataTable();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(rs);
                this.CloseConnection();
            }
            Rows(rs);
            return rs;
        }

        public void Rows(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                rows = true;
            }
            else
            {
                rows = false;
            }
        }
    }
}
