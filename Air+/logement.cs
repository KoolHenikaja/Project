using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_
{
    public partial class logement : Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "";
        string database = "projet_air_plus";

        public logement()
        {
            InitializeComponent();
            ViewTableLog();
            rechLog_textBox10.TextChanged += rech_textBox10_TextChanged;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        public void ViewTableLog()
        {
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;
            MySqlConnection con = new MySqlConnection(conString);
            con.Open();
            string viewTable = "SELECT * FROM `logement`";
            MySqlCommand msc = new MySqlCommand(viewTable, con);
            MySqlDataReader rd = msc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            tableauLog.DataSource = dt;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rech_textBox10_TextChanged(object sender, EventArgs e)
        {
            string searchValue = rechLog_textBox10.Text;

            // Connexion à la base de données
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête SQL pour rechercher les étudiants par nom
                    string query = "SELECT * FROM `logement` WHERE `nomLog` LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Utilisation du paramètre LIKE pour la recherche
                        cmd.Parameters.AddWithValue("@search", "%" + searchValue + "%");

                        // Exécution de la commande
                        MySqlDataReader reader = cmd.ExecuteReader();

                        // Charger les résultats dans un DataTable
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Afficher les résultats dans le DataGridView
                        tableauLog.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la recherche : " + ex.Message);
                }
            }
        }
    }
}
