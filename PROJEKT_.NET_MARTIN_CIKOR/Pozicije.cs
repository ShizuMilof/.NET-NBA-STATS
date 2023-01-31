using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialSkin.Controls;
using Bunifu.UI;
using System.Windows.Forms;

namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class Pozicije : MaterialForm
    {
        public Pozicije()
        {
            InitializeComponent();
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Pozicije_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Pozicije_Click(object sender, EventArgs e)
        {
          
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

            FormaStatistikaIgraca myForm = new FormaStatistikaIgraca();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }
    }
}
