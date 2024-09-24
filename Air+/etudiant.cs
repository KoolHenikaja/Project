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
    public partial class etudiant : Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "";
        string database = "projet_air_plus";

        public etudiant()
        {
            InitializeComponent();
            ViewTableEtudiant();
            dataGridView1.CellClick += dataGridView1_CellClick;
            rech_textBox10.TextChanged += rech_textBox10_TextChanged;

            ControlChamp();

            LoadLogements();
               // Associer l'événement SelectedIndexChanged à la ComboBox
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void ControlChamp()
        {
            textBox8.KeyPress += new KeyPressEventHandler(textBox8_KeyPress);
            textBox7.KeyPress += textBox7_KeyPress;
            textBox6.KeyPress += new KeyPressEventHandler(textBox6_KeyPress);
            textBox2.KeyPress += new KeyPressEventHandler(textBox2_KeyPress);
            textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
        }

        private void ViderChamps()
        {
            // Réinitialiser chaque TextBox à une chaîne vide
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;

            comboBox1.SelectedIndex = -1;
        }


        //combobox

        private void LoadLogements()
        {
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête pour récupérer les logements
                    string query = "SELECT `idEtage`, `nomLog` FROM `logement`";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Effacer les éléments précédents de la ComboBox
                            comboBox1.Items.Clear();

                            // Remplir la ComboBox avec les noms de logement
                            while (reader.Read())
                            {
                                // Créer un objet avec le nom et l'ID correspondant
                                comboBox1.Items.Add(new ComboboxItem
                                {
                                    Text = reader["nomLog"].ToString(),
                                    Value = reader["idEtage"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors du chargement des logements : " + ex.Message);
                }
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text; // Ce qui sera affiché dans la ComboBox
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                ComboboxItem selectedItem = (ComboboxItem)comboBox1.SelectedItem;
                textBox5.Text = selectedItem.Value; // Afficher l'idEtage dans textBox5
            }
        }


        //combobox

        private void ajout_button_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }


            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            // Utilisation de 'using' pour s'assurer que la connexion est fermée automatiquement
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête SQL avec des paramètres
                    string insert = "INSERT INTO `etudiant` (`nomEtu`, `prenomEtu`, `parcours`, `progEtu`, `CIN`, `dateNaiss`, `Tel`, `idEtage`) " +
                                    "VALUES (@nomEtu, @prenomEtu, @parcours, @progEtu, @CIN, @dateNaiss, @Tel, @idEtage)";

                    using (MySqlCommand msc = new MySqlCommand(insert, con))
                    {
                        // Ajouter les paramètres à la commande
                        msc.Parameters.AddWithValue("@nomEtu", textBox1.Text);
                        msc.Parameters.AddWithValue("@prenomEtu", textBox2.Text);
                        msc.Parameters.AddWithValue("@parcours", textBox3.Text);
                        msc.Parameters.AddWithValue("@progEtu", textBox4.Text);
                        msc.Parameters.AddWithValue("@CIN", textBox8.Text);
                        msc.Parameters.AddWithValue("@dateNaiss", textBox7.Text);
                        msc.Parameters.AddWithValue("@Tel", textBox6.Text);
                        msc.Parameters.AddWithValue("@idEtage", textBox5.Text);

                        // Exécuter la commande et obtenir le nombre de lignes affectées
                        int i = msc.ExecuteNonQuery();

                        // Afficher un message indiquant le nombre de lignes ajoutées
                        MessageBox.Show(i > 0 ? "Étudiant ajouté avec succès !" : "Aucun étudiant ajouté.");
                    }

                    // Rafraîchir l'affichage du tableau des étudiants
                    ViewTableEtudiant();
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    // Gestion des erreurs
                    MessageBox.Show("Erreur lors de l'ajout de l'étudiant : " + ex.Message);
                }
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ViewTableEtudiant()
        {
            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            // Utilisation d'un bloc using pour assurer la fermeture de la connexion
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête pour récupérer les données de toutlogement
                    string viewTable = "SELECT * FROM `toutlogement`ORDER by idEtage ASC";
                    using (MySqlCommand msc = new MySqlCommand(viewTable, con))
                    {
                        using (MySqlDataReader rd = msc.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(rd);

                            // Assigner les données au DataGridView
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gestion des erreurs et affichage d'un message
                    MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
                }
            }
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Vérifier que la cellule cliquée est valide (pour éviter les erreurs)
            if (e.RowIndex >= 0)
            {
                // Récupérer la ligne sélectionnée
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Extraire les données de chaque colonne (ajustez les index en fonction de vos colonnes)
                textBox1.Text = row.Cells["nomEtu"].Value.ToString();  // Par exemple, pour le nom
                textBox2.Text = row.Cells["prenomEtu"].Value.ToString();  // Pour le prénom
                textBox3.Text = row.Cells["parcours"].Value.ToString();  // Pour le parcours
                textBox4.Text = row.Cells["progEtu"].Value.ToString();  // Pour le programme
                textBox5.Text = row.Cells["idEtage"].Value.ToString();  // Pour l'étage
                textBox6.Text = row.Cells["Tel"].Value.ToString();      // Pour le téléphone
                textBox7.Text = row.Cells["dateNaiss"].Value.ToString(); // Pour la date de naissance
                textBox8.Text = row.Cells["CIN"].Value.ToString();      // Pour le CIN
                textBox9.Text = row.Cells["idEtu"].Value.ToString();    // Pour l'ID de l'étudiant

                // Mettre à jour la sélection dans comboBox1 pour correspondre à 'nomLog'
                string nomLog = row.Cells["nomLog"].Value.ToString();
                //comboBox1.SelectedItem = nomLog;

                // Rechercher dans la ComboBox l'élément dont la propriété 'Text' correspond à 'nomLog'
                foreach (var item in comboBox1.Items)
                {
                    ComboboxItem comboItem = item as ComboboxItem;
                    if (comboItem != null && comboItem.Text == nomLog)
                    {
                        comboBox1.SelectedItem = comboItem; // Sélectionner l'élément dans comboBox1
                        break; // Une fois l'élément trouvé, sortir de la boucle
                    }
                }
            }
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
                    string query = "SELECT * FROM `toutlogement` WHERE `nomEtu` LIKE @search OR `prenomEtu` LIKE @search OR `parcours` LIKE @search OR   `progEtu` LIKE @search OR `CIN` LIKE @search OR `dateNaiss` LIKE @search OR `Tel` LIKE @search OR `nomLog` LIKE @search";

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
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la recherche : " + ex.Message);
                }
            }
        }




        private void modifier_button_Click(object sender, EventArgs e)
        {
            string idEtu = textBox9.Text;
            if (string.IsNullOrWhiteSpace(idEtu))
            {
                MessageBox.Show("Veuillez entrer un ID d'étudiant valide.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
                    {
                        MessageBox.Show("Veuillez remplir tous les champs.");
                        return;
                    }


            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            // Utilisation de 'using' pour s'assurer que la connexion est fermée correctement
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête SQL pour mettre à jour les informations d'un étudiant (basé sur l'id)
                    string updateQuery = "UPDATE `etudiant` SET `nomEtu` = @nomEtu, `prenomEtu` = @prenomEtu, `parcours` = @parcours, " +
                                         "`progEtu` = @progEtu, `CIN` = @CIN, `dateNaiss` = @dateNaiss, `Tel` = @Tel, `idEtage` = @idEtage " +
                                         "WHERE `idEtu` = @idEtu";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
                    {
                        // Ajout des paramètres de la requête
                        cmd.Parameters.AddWithValue("@nomEtu", textBox1.Text);
                        cmd.Parameters.AddWithValue("@prenomEtu", textBox2.Text);
                        cmd.Parameters.AddWithValue("@parcours", textBox3.Text);
                        cmd.Parameters.AddWithValue("@progEtu", textBox4.Text);
                        cmd.Parameters.AddWithValue("@CIN", textBox8.Text);
                        cmd.Parameters.AddWithValue("@dateNaiss", textBox7.Text);
                        cmd.Parameters.AddWithValue("@Tel", textBox6.Text);
                        cmd.Parameters.AddWithValue("@idEtage", textBox5.Text);

                        // Vous devez obtenir l'ID de l'étudiant à modifier (par exemple à partir d'un champ de texte)
                        cmd.Parameters.AddWithValue("@idEtu", textBox9.Text);

                        // Exécution de la commande
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Affichage d'un message de confirmation
                        MessageBox.Show(rowsAffected > 0 ? "Étudiant modifié avec succès !" : "Modification échouée.");
                    }

                    // Rafraîchir l'affichage du tableau après modification
                    ViewTableEtudiant();
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    // Gestion des erreurs
                    MessageBox.Show("Erreur lors de la modification de l'étudiant : " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            // Assurez-vous que l'ID de l'étudiant à supprimer est fourni
            string idEtu = textBox9.Text; // Utilisation de textBox9 pour l'ID de l'étudiant
            if (string.IsNullOrWhiteSpace(idEtu))
            {
                MessageBox.Show("Veuillez entrer un ID d'étudiant valide.");
                return;
            }

            string conString = "server=" + server + ";uid=" + uid + ";pwd=" + password + ";database=" + database;

            // Utilisation de 'using' pour s'assurer que la connexion est fermée correctement
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();

                    // Requête SQL pour supprimer l'étudiant basé sur l'ID
                    string deleteQuery = "DELETE FROM `etudiant` WHERE `idEtu` = @idEtu";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, con))
                    {
                        // Ajout du paramètre
                        cmd.Parameters.AddWithValue("@idEtu", idEtu);

                        // Exécution de la commande
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Affichage d'un message de confirmation
                        MessageBox.Show(rowsAffected > 0 ? "Étudiant supprimé avec succès !" : "Aucun étudiant trouvé avec cet ID.");
                    }

                    // Rafraîchir l'affichage du tableau après suppression
                    ViewTableEtudiant();
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    // Gestion des erreurs
                    MessageBox.Show("Erreur lors de la suppression de l'étudiant : " + ex.Message);
                }
            }
        }




        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Vérifier si le caractère est une lettre, un espace, un tiret ou une touche de contrôle (comme Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-')
            {
                e.Handled = true; // Empêcher l'entrée de ce caractère
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Vérifier si le caractère est une lettre ou une touche de contrôle (comme Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Empêcher l'entrée de ce caractère
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Vérifier si le caractère est un chiffre ou une touche de contrôle (comme Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Empêcher l'entrée de ce caractère
            }
            else if (((TextBox)sender).Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Limiter la longueur à 10 chiffres
            }
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser uniquement les chiffres et les barres obliques
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
            {
                e.Handled = true;
            }

            // Limiter la longueur et le format
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            if ((text.Length == 2 || text.Length == 5) && e.KeyChar != '/')
            {
                e.Handled = true;
            }
            else if (text.Length >= 10)
            {
                e.Handled = true;
            }
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Vérifier si le caractère est un chiffre ou une touche de contrôle (comme Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Empêcher l'entrée de ce caractère
            }
            else if (((TextBox)sender).Text.Length >= 12 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Limiter la longueur à 12 chiffres
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViderChamps();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
