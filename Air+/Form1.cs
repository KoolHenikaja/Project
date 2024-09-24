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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadform(new menu());
        }

        public void loadform(object Form)
        {
            if (this.main_panel.Controls.Count > 0)
                this.main_panel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.main_panel.Controls.Add(f);
            this.main_panel.Tag = f;
            f.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            loadform(new menu());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new etudiant());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new logement());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new rdc());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadform(new Rplus1());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loadform(new Rplus2());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadform(new Rplus3());
        }

        private void close_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
