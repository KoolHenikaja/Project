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
    public partial class Rplus3 : Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "";
        string database = "projet_air_plus";

        public Rplus3()
        {
            InitializeComponent();
            ViewTableLogR3();
            rech_textBox10.TextChanged += rech_textBox10_TextChanged;
        }

        public void ViewTableLogR3()
        {
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;
            MySqlConnection con = new MySqlConnection(conString);
            con.Open();
            string viewTable = "SELECT * FROM `r3` ORDER by nomEtu ASC ";
            MySqlCommand msc = new MySqlCommand(viewTable, con);
            MySqlDataReader rd = msc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            TableauR3_dataGridView1.DataSource = dt;
        }

        private void rech_textBox10_TextChanged(object sender, EventArgs e)
        {
            string searchValue = rech_textBox10.Text;

            // Connexion à la base de données
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête SQL pour rechercher les étudiants par nom
                    string query = "SELECT * FROM `r3` WHERE `nomEtu` LIKE @search OR `prenomEtu` LIKE @search OR `parcours` LIKE @search OR   `progEtu` LIKE @search OR `CIN` LIKE @search OR `dateNaiss` LIKE @search OR `Tel` LIKE @search OR `nomLog` LIKE @search";

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
                        TableauR3_dataGridView1.DataSource = dt;
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
