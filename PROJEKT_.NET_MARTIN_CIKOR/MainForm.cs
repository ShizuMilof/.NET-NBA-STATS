using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Liga;
using MaterialSkin.Controls;
using MaterialSkin;


namespace PROJEKT_.NET_MARTIN_CIKOR
{
   
    public partial class MainForm : MaterialForm 
    {

        private int CurrentRow { get; set; } //Sadrzava u sebi zadnji pritisnuti redak u dataGridView1
        private NbaLigaRepository _countryRepository = new NbaLigaRepository();
 
      
        private BindingSource _tableBindingSource = new BindingSource();
     
        public MainForm()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            InitializeComponent();
            materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple100, Primary.DeepPurple100, Primary.Blue900, Accent.Purple700, TextShade.BLACK);
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            bunifuDataGridView1.AutoGenerateColumns = false; //Ne generira extra stupce
            _tableBindingSource.DataSource = _countryRepository.GetLiga();
         
        }

     

        private void MainForm_Load_1(object sender, EventArgs e)
        {
            MaximizeBox = false;
            bunifuDataGridView1.DataSource = _tableBindingSource;
        
        }


        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CurrentRow = e.RowIndex; //Sprema u svojstvo CurrentRow
        }


        private void materialButton1_Click(object sender, EventArgs e)
        {
         
            var myForm2 = new Form1((NbaLiga)_tableBindingSource.List[CurrentRow]);
            this.Hide();
            myForm2.ShowDialog();
            this.Close();
        }

      



        private void materialButton2_Click(object sender, EventArgs e)
        {

            var myForm = new FormaStatistika();
            this.Hide();
            myForm.ShowDialog();
            this.Close();



        }

        private void bunifuDataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CurrentRow = e.RowIndex; //Sprema u svojstvo CurrentRow

            var myForm2 = new Form1((NbaLiga)_tableBindingSource.List[CurrentRow]);
            this.Hide();
            myForm2.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            var myForm = new oProgramu();
            this.Hide();
            myForm.ShowDialog();
            this.Close();

        }
    }
}

