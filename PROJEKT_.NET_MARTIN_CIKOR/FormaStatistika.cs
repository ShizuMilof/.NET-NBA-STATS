using Bunifu.UI.WinForms;
using Liga;
using MaterialSkin;
using System;
using MaterialSkin.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;

namespace PROJEKT_.NET_MARTIN_CIKOR
{
    public partial class FormaStatistika : MaterialForm
    {
        SqlConnection Con = new SqlConnection(@"Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!");
        private NbaLigaRepository _countryRepository = new NbaLigaRepository();
        private BindingSource _tableBindingSource1 = new BindingSource();
        private BindingSource _tableBindingSource = new BindingSource();
        DataTable dataTable2 = new DataTable();
        

        public FormaStatistika()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;

            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple100, Primary.DeepPurple100, Primary.Blue900, Accent.Purple700, TextShade.BLACK);
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            _tableBindingSource1.DataSource = _countryRepository.GetStatistika();
            bunifuDataGridView1.AutoGenerateColumns = false; //Ne generira extra stupce
        }

       
        

        private void FormaStatistika_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sTUDENTI_PINDataSet8.cikor_cikorStatistika3' table. You can move, or remove it, as needed.
            this.cikor_cikorStatistika3TableAdapter2.Fill(this.sTUDENTI_PINDataSet8.cikor_cikorStatistika3);
            MaximizeBox = false;
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorStatistika3", Con);
            da.Fill(dataTable2);
        }

        private void PerformSearch()
        {
            string searchValue = txtSearch.Text;
            
            foreach (DataGridViewRow row in bunifuDataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "first_name")
                    {
                        if (cell.Value.ToString().Contains(searchValue))
                        {
                            row.Visible = true;
                            break;
                        }
                        else
                        {
                            if (bunifuDataGridView1.CurrentRow != row)
                                row.Visible = false;
                            else
                            {
                                int currentRowIndex = bunifuDataGridView1.CurrentRow.Index;
                                bool foundNextVisible = false;
                                for (int i = currentRowIndex + 1; i < bunifuDataGridView1.Rows.Count; i++)
                                {
                                    if (bunifuDataGridView1.Rows[i].Visible)
                                    {
                                        bunifuDataGridView1.CurrentCell = bunifuDataGridView1.Rows[i].Cells[0];
                                        foundNextVisible = true;
                                        break;
                                    }
                                }
                                if (!foundNextVisible)
                                {
                                    for (int i = 0; i < currentRowIndex; i++)
                                    {
                                        if (bunifuDataGridView1.Rows[i].Visible)
                                        {
                                            bunifuDataGridView1.CurrentCell = bunifuDataGridView1.Rows[i].Cells[0];
                                            foundNextVisible = true;
                                            break;
                                        }
                                    }
                                }
                                if (!foundNextVisible)
                                {
                                    bunifuDataGridView1.CurrentCell = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var myForm = new FormaStatistikaIgraca();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void materialButton2_Click_1(object sender, EventArgs e)
        {
            MainForm myForm = new MainForm();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }
     

        private void button1_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
          
            string searchValue = txtSearch.Text.Trim();
            var results = dataTable2.AsEnumerable().Where(r => (r.Field<string>("first_name") + " " + r.Field<string>("last_name")).Contains(searchValue));
            if (results.Any())
            {
                if (bunifuDataGridView1.DataSource != null)
                {
                      bunifuDataGridView1.DataSource = null;
                }
                bunifuDataGridView1.DataSource = results.CopyToDataTable();
            }
            else
            {
                MessageBox.Show("NEMA REZULTATA, POKUŠAJTE PONOVNO.");

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string maxValue = "100000";
            List<int> maxRowIndexes = new List<int>();
            for (int i = 0; i < bunifuDataGridView1.Rows.Count; i++)
            {
                if (bunifuDataGridView1.Rows[i].Cells[7].Value != null)
                {
                    if (string.Compare(bunifuDataGridView1.Rows[i].Cells[7].Value.ToString(), maxValue) > 0)
                    {
                        maxValue = bunifuDataGridView1.Rows[i].Cells[7].Value.ToString();
                        maxRowIndexes.Clear();
                        maxRowIndexes.Add(i);
                    }
                    else if (string.Compare(bunifuDataGridView1.Rows[i].Cells[7].Value.ToString(), maxValue) == 0)
                    {
                        maxRowIndexes.Add(i);
                    }
                }
            }

            if (maxRowIndexes.Count == 1)
            {
                string message = "OSOBA S NAJVIŠE POENA KOJI IZNOSE " + maxValue + " JE :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    bunifuDataGridView1.Rows[rowIndex].Selected = true;
                    message += bunifuDataGridView1.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + bunifuDataGridView1.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                bunifuDataGridView1.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else if (maxRowIndexes.Count > 1)
            {
                string message = "OSOBE S NAJVIŠE POENA KOJI IZNOSE " + maxValue + " SU :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    bunifuDataGridView1.Rows[rowIndex].Selected = true;
                    message += bunifuDataGridView1.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + bunifuDataGridView1.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                bunifuDataGridView1.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else

            {
                MessageBox.Show("Stupac ne sadrži podatke.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Kreirajte DataAdapter objekt
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorStatistika3", Con);
            // Kreirajte DataTable objekt
            DataTable dt = new DataTable();
            // Punite DataTable objekt podacima iz baze
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name DESC";
            bunifuDataGridView1.DataSource = dv;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string maxValue = "100000";
            List<int> maxRowIndexes = new List<int>();
            for (int i = 0; i < bunifuDataGridView1.Rows.Count; i++)
            {
                if (bunifuDataGridView1.Rows[i].Cells[7].Value != null)
                {
                    if (string.Compare(bunifuDataGridView1.Rows[i].Cells[7].Value.ToString(), maxValue) > 0)
                    {
                        maxValue = bunifuDataGridView1.Rows[i].Cells[7].Value.ToString();
                        maxRowIndexes.Clear();
                        maxRowIndexes.Add(i);
                    }
                    else if (string.Compare(bunifuDataGridView1.Rows[i].Cells[7].Value.ToString(), maxValue) == 0)
                    {
                        maxRowIndexes.Add(i);
                    }
                }
            }

            if (maxRowIndexes.Count == 1)
            {
                string message = "OSOBA S NAJVIŠE POENA KOJI IZNOSE " + maxValue + " JE :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    bunifuDataGridView1.Rows[rowIndex].Selected = true;
                    message += bunifuDataGridView1.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + bunifuDataGridView1.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                bunifuDataGridView1.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else if (maxRowIndexes.Count > 1)
            {
                string message = "OSOBE S NAJVIŠE POENA KOJI IZNOSE " + maxValue + " SU :\n";
                foreach (int rowIndex in maxRowIndexes)
                {
                    bunifuDataGridView1.Rows[rowIndex].Selected = true;
                    message += bunifuDataGridView1.Rows[rowIndex].Cells["first_name"].Value.ToString() + " " + bunifuDataGridView1.Rows[rowIndex].Cells["last_name"].Value.ToString() + "\n";
                }
                bunifuDataGridView1.FirstDisplayedScrollingRowIndex = maxRowIndexes[0];
                MessageBox.Show(message);
            }
            else

            {
                MessageBox.Show("Stupac ne sadrži podatke.");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Kreirajte DataAdapter objekt
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorStatistika3", Con);
            // Kreirajte DataTable objekt
            DataTable dt = new DataTable();
            // Punite DataTable objekt podacima iz baze
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "first_name ASC";
            bunifuDataGridView1.DataSource = dv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kreirajte DataAdapter objekt
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM cikor_cikorStatistika3", Con);
            // Kreirajte DataTable objekt
            DataTable dt = new DataTable();
            // Punite DataTable objekt podacima iz baze
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "reb DESC";
            bunifuDataGridView1.DataSource = dv;
        }
    }
}
