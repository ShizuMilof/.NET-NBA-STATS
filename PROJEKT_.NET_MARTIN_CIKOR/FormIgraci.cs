
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
using System.Linq;

namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class FormIgraci : Form1
    {
        private BindingSource _tableBindingSource1 = new BindingSource();
        private BindingSource _tableBindingSource = new BindingSource();
        private NbaLigaRepository _countryRepository = new NbaLigaRepository();
        private NbaLigaIgrac nbaLigaIgracVar;



        public FormIgraci( NbaLiga NbaLigaIgrac)
        {
            InitializeComponent();
            _tableBindingSource1.DataSource = _countryRepository.GetTimIgracSolo(NbaLigaIgrac.id);


            _tableBindingSource1.DataSource = _countryRepository.GetTimIgracSolo(nbaLigaIgracVar.ID);
            bunifuDataGridView1.AutoGenerateColumns = false; //Ne generira extra stupce
            List<NbaLigaIgrac> TimList = _countryRepository.GetTimIgracSolo(nbaLigaIgracVar.ID);
            //NbaLigaIgrac Tim = _countryRepository.GetTimIgracSolo(NbaLigaIgrac.ID);
            NbaLigaIgrac Tim = TimList.FirstOrDefault();
            List<NbaLiga> DodatneInformacije = NbaLigaRepository.ucitajpodatke();
            DodatneInformacije.FirstOrDefault(x => x.id == Tim.ID);
            for (int i = 0; i < DodatneInformacije.Count; i++)
            {
                if (DodatneInformacije[i].id == Tim.ID)
                {

                    bunifuPictureBox2.Image = DodatneInformacije[i].image;
                }
            }

            _tableBindingSource1.DataSource = Tim;

        }

        public FormIgraci(NbaLigaIgrac nbaLigaIgrac)
        {
            InitializeComponent();
            nbaLigaIgracVar = nbaLigaIgrac;
            _tableBindingSource1.DataSource = _countryRepository.GetTimIgracSolo(nbaLigaIgracVar.ID);
            bunifuDataGridView1.AutoGenerateColumns = false; //Ne generira extra stupce
            List<NbaLigaIgrac> TimList = _countryRepository.GetTimIgracSolo(nbaLigaIgracVar.ID);
            //NbaLigaIgrac Tim = _countryRepository.GetTimIgracSolo(NbaLigaIgrac.ID);
            NbaLigaIgrac Tim = TimList.FirstOrDefault();
            List<NbaLigaIgrac> DodatneInformacije = NbaLigaRepository.ucitajpodatkeZaIgraca();
            DodatneInformacije.FirstOrDefault(x => x.ID == Tim.ID);
            for (int i = 0; i < DodatneInformacije.Count; i++)
            {
                if (DodatneInformacije[i].ID == Tim.ID)
                {
                    Tim.height_feet = DodatneInformacije[i].height_feet;
                    Tim.height_inches = DodatneInformacije[i].height_inches;
                    Tim.position= DodatneInformacije[i].position;
                    Tim.weight_pounds = DodatneInformacije[i].weight_pounds;


                    bunifuPictureBox2.Image = DodatneInformacije[i].image;
                }
            }

            _tableBindingSource1.DataSource = Tim;



        }

        private void FormIgraci_Load(object sender, EventArgs e)
        {
            bunifuDataGridView1.DataSource = _tableBindingSource1;
            MaximizeBox = false;
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bunifuDataGridView1.DataSource = _tableBindingSource1;

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

            this.Close();
            Form1 Form1 = new Form1();
            Form1.Show();
        }

        private void materialButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
