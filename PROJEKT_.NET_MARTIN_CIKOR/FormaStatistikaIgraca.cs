using Liga;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
using MaterialSkin.Controls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;


namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class FormaStatistikaIgraca : MaterialForm
    {
        SqlConnection Con = new SqlConnection(@"Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!");

        private NbaLigaRepository _countryRepository = new NbaLigaRepository();
        private BindingSource _tableBindingSource1 = new BindingSource();
        private BindingSource _tableBindingSource2 = new BindingSource();
        DataTable dataTable = new DataTable();

        public FormaStatistikaIgraca()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            InitializeComponent();
            materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple100, Primary.DeepPurple100, Primary.Blue900, Accent.Purple700, TextShade.BLACK);
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            _tableBindingSource1.DataSource = _countryRepository.GetTimIgracZaStatistiku();
            DataGv.AutoGenerateColumns = false; //Ne generira extra stupce
        }

        private void FormaStatistikaIgraca_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sTUDENTI_PINDataSet5.cikor_cikorIgraci' table. You can move, or remove it, as needed.
            this.cikor_cikorIgraciTableAdapter2.Fill(this.sTUDENTI_PINDataSet5.cikor_cikorIgraci);
            MaximizeBox = false;
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            da.Fill(dataTable);
            List<string> positions = new List<string>();
            positions.Add("ODABERI POZICIJU ZA FILTRIRATI");
            positions.AddRange(dataTable.AsEnumerable().Select(row => row.Field<string>("position")).Distinct().ToList());
            materialComboBox1.DataSource = positions;
            materialComboBox1.SelectedIndex = 0;
        }


        private void materialComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialComboBox1.SelectedIndex == 0)
            {
                ResetDataGridView();
                return;
            }
            if (materialComboBox1.SelectedValue != null)
            {
                string selectedPosition = materialComboBox1.SelectedValue.ToString();
                var filteredData = dataTable.AsEnumerable().Where(row => row.Field<string>("position") == selectedPosition);
                DataGv.DataSource = filteredData.CopyToDataTable();
            }
        }


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (DataGv.DataSource is DataTable)
            {
                DataView dv = new DataView((DataTable)DataGv.DataSource);
                dv.Sort = "first_name ASC";
                DataGv.DataSource = dv;
            }
            else if (DataGv.DataSource is BindingSource)
            {
                DataTable dt = ((DataTable)((BindingSource)DataGv.DataSource).DataSource).Clone();
                foreach (DataGridViewRow row in DataGv.Rows)
                {
                    dt.LoadDataRow(((DataRowView)row.DataBoundItem).Row.ItemArray, true);
                }

                DataView dv = new DataView(dt);
                dv.Sort = "first_name ASC";
                DataGv.DataSource = dv;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name ASC";
            DataGv.DataSource = dv;
        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name ASC";
            DataGv.DataSource = dv;
        }

        public void bunifuButton2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name DESC";
            DataGv.DataSource = dv;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            FormaStatistika myForm = new FormaStatistika();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            MainForm myForm = new MainForm();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            txtSearch2.Text = "";
        }

        private void ResetDataGridView()
        {
            DataGv.DataSource = dataTable;
        }
    
     
    

        private void txtSearch2_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearch2.Text.Trim();
            var results = dataTable.AsEnumerable().Where(r => (r.Field<string>("first_name") + " " + r.Field<string>("last_name")).Contains(searchValue));
            if (results.Any())
            {
                if (DataGv.DataSource != null)
                {
                    DataGv.DataSource = null;
                }
                DataGv.DataSource = results.CopyToDataTable();
            }
            else   
            {
               MessageBox.Show("NEMA REZULTATA, POKUŠAJTE PONOVNO.");              
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Kreirajte DataAdapter objekt
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            // Kreirajte DataTable objekt
            DataTable dt = new DataTable();
            // Punite DataTable objekt podacima iz baze
            da.Fill(dt);
            // Kreirajte DataView objekt od vašeg DataTable objekta
            DataView dv = new DataView(dt);
            dv.Sort = "first_name ASC";
            // Povežite DataView s DataGridView kontrolom
            DataGv.DataSource = dv;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name DESC";
            DataGv.DataSource = dv;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "last_name ASC";
            DataGv.DataSource = dv;
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "last_name DESC";         
            DataGv.DataSource = dv;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string maxValue = "";
            List<int> maxRowIndexes = new List<int>();
            for (int i = 0; i < DataGv.Rows.Count; i++)
            {
                if (DataGv.Rows[i].Cells[3].Value != null)
                {
                    if (string.Compare(DataGv.Rows[i].Cells[3].Value.ToString(), maxValue) > 0)
                    {
                        maxValue = DataGv.Rows[i].Cells[3].Value.ToString();
                        maxRowIndexes.Clear();
                        maxRowIndexes.Add(i);
                    }
                    else if (string.Compare(DataGv.Rows[i].Cells[3].Value.ToString(), maxValue) == 0)
                    {
                        maxRowIndexes.Add(i);
                    }
                }
            }

            if (maxRowIndexes.Count == 1)
            {
                string message = "NAJVIŠLJA OSOBA S VISINOM OD " + maxValue + " JE :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else if (maxRowIndexes.Count > 1)
            {
                string message = "NAJVIŠLJE OSOBE S VISINOM OD " + maxValue + " SU :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else

            {
                MessageBox.Show("Stupac ne sadrži podatke.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string maxValue = "";
            List<int> maxRowIndexes = new List<int>();
            for (int i = 0; i < DataGv.Rows.Count; i++)
            {
                if (DataGv.Rows[i].Cells[2].Value != null)
                {
                    if (string.Compare(DataGv.Rows[i].Cells[2].Value.ToString(), maxValue) > 0)
                    {
                        maxValue = DataGv.Rows[i].Cells[2].Value.ToString();
                        maxRowIndexes.Clear();
                        maxRowIndexes.Add(i);
                    }
                    else if (string.Compare(DataGv.Rows[i].Cells[2].Value.ToString(), maxValue) == 0)
                    {
                        maxRowIndexes.Add(i);
                    }
                }
            }

            if (maxRowIndexes.Count == 1)
            {
                string message = "OSOBA S NAJVEĆOM DUŽINOM STOPALA KOJA IZNOSI " + maxValue + " JE :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else if (maxRowIndexes.Count > 1)
            {
                string message = "OSOBE S NAJVEĆOM DUŽINOM STOPALA KOJA IZNOSI " + maxValue + " SU :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Stupac ne sadrži podatke.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string minValue = "999999"; //initialize to a large value
            List<int> minRowIndexes = new List<int>();

            for (int i = 0; i < DataGv.Rows.Count; i++)
            {
                if (DataGv.Rows[i].Cells[5].Value != null)
                {
                    if (string.Compare(DataGv.Rows[i].Cells[5].Value.ToString(), minValue) < 0)
                    {
                        minValue = DataGv.Rows[i].Cells[5].Value.ToString();
                        minRowIndexes.Clear();
                        minRowIndexes.Add(i);
                    }
                    else if (string.Compare(DataGv.Rows[i].Cells[5].Value.ToString(), minValue) == 0)
                    {
                        minRowIndexes.Add(i);
                    }
                }
            }
            if (minRowIndexes.Count == 1)
            {
                string message = "TEŽINA NAJLAKŠEG IGRAČA IZNOSI " + minValue + ", A TO JE IGRAČ :\n";
                foreach (int rowIndex in minRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = minRowIndexes[0];
                MessageBox.Show(message);
            }
            else if (minRowIndexes.Count > 1)
            {
                string message = "TEŽINA NAJLAKŠIH IGRAČA IZNOSI " + minValue + ", A TI IGRAČI SU :\n";
                foreach (int rowIndex in minRowIndexes)
                {
                    DataGv.Rows[rowIndex].Selected = true;
                    message += DataGv.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + DataGv.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                DataGv.FirstDisplayedScrollingRowIndex = minRowIndexes[0];
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Stupac ne sadrži podatke.");
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorIgraci", Con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name ASC";
            DataGv.DataSource = dv;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var myForm2 = new Pozicije();
            this.Hide();
            myForm2.ShowDialog();
        }
    }
}