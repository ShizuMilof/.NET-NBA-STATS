using Liga;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using Bunifu.UI.WinForms;

namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class Form1 : MaterialForm
    {
        private int CurrentRow { get; set; } //Sadrzava u sebi zadnji pritisnuti redak

        private NbaLigaRepository _countryRepository = new NbaLigaRepository();
        private BindingSource _tableBindingSource1 = new BindingSource();
        private BindingSource _tableBindingSource2 = new BindingSource();
        private BindingSource _tableBindingSource3 = new BindingSource();
        private BindingSource _tableBindingSource4 = new BindingSource();


      


        public Form1(NbaLiga NbaLiga)
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple100, Primary.DeepPurple100, Primary.Blue900, Accent.Purple700, TextShade.BLACK);
            InitializeComponent();
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            bunifuDataGridView1.AutoGenerateColumns = false; //Ne generira extra stupce
            bunifuDataGridView2.AutoGenerateColumns = false; //Ne generira extra stupce
            bunifuDataGridView3.AutoGenerateColumns = false; //Ne generira extra stupce
            NbaLiga Tim = _countryRepository.GetTimPodatci(NbaLiga.id);
            List<NbaLiga> DodatneInformacije = NbaLigaRepository.ucitajpodatke();
            DodatneInformacije.Find(x => x.id == Tim.id);
            for(int i = 0; i < DodatneInformacije.Count; i++)
            {
                if (DodatneInformacije[i].id == Tim.id)
                {
                    Tim.trener = DodatneInformacije[i].trener;
                    Tim.stadion = DodatneInformacije[i].stadion;
                    bunifuPictureBox1.Image = DodatneInformacije[i].image;
                }
            }    
            
            _tableBindingSource1.DataSource = Tim;
            _tableBindingSource2.DataSource = _countryRepository.GetTimUtakmice(NbaLiga.id);
            _tableBindingSource3.DataSource = _countryRepository.GetTimIgrac(NbaLiga.id);
            _tableBindingSource4.DataSource = _countryRepository.GetTimIgracSolo(NbaLiga.id);
            
        }

        public Form1()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm myForm = new MainForm();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          

            bunifuDataGridView1.DataSource = _tableBindingSource1;

            bunifuDataGridView2.DataSource = _tableBindingSource2;

            bunifuDataGridView3.DataSource = _tableBindingSource3;
            MaximizeBox = false;

        }

    
       


        
        

       
        

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bunifuDataGridView2.DataSource = _tableBindingSource1;

        }

        private void bunifuDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void bunifuDataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bunifuDataGridView3.DataSource = _tableBindingSource3;
            CurrentRow = e.RowIndex; //Sprema u svojstvo CurrentRow

          

        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
        



            
            var myForm2 = new FormIgraci((NbaLigaIgrac)_tableBindingSource3.List[CurrentRow]);
            myForm2.Show();
        
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
      
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            MainForm myForm = new MainForm();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }
    }
}
