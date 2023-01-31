using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class oProgramu : MaterialForm
    {
        public oProgramu()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainForm myForm = new MainForm();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void oProgramu_Load(object sender, EventArgs e)
        {

        }
    }
}
