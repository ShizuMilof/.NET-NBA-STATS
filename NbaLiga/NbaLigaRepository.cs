using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Liga;
using System.Data.Common;
using System.Data.SqlClient;
using Bunifu.UI.WinForms;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Forms;

namespace Liga
{
   
    public class NbaLigaRepository
    {
      
        public static string connectionString = "Data Source=193.198.57.183; Initial Catalog = STUDENTI_PIN;User Id = pin; Password = Vsmti1234!";
        private object oCommand;

        //DOBIVAMO LIGU
        public List<NbaLiga> GetLiga()
        {
            List<NbaLiga> LigaREST = new List<NbaLiga>();
            string url = "https://www.balldontlie.io/api/v1/teams";
            string sjson = CallRestMethod(url);
            var json = JObject.Parse(sjson);
            var Nba = json.GetValue("data");

            foreach (JObject item in Nba)
            {
                LigaREST.Add(new NbaLiga
                {
                   id = (int)item.GetValue("id"),
                   name = (string)item.GetValue("name")
                });
            }

            for (int i = 0; i < LigaREST.Count; i++)
            {
                Console.WriteLine(LigaREST[i].id);
            }
            return LigaREST;

        }

        public void AddPodatkeZaIgraca()  ///FUNKCIJA S KOJOM DODAJEMO U BAZU STVARI S APIJA, ODNOSNO SPREMLJENE PA ONDA U  BAZU DA NEE MORAM RUCNO UPISIVAT
        {
            List<NbaLigaIgrac> ligica = new List<NbaLigaIgrac>();
            ligica = GetTimIgracZaStatistiku();

            
            string sSqlConnectionString = "Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!";
            using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {
                oConnection.Open();
                foreach (NbaLigaIgrac l in ligica)
                {
                    oCommand.CommandText = "INSERT INTO cikor_igracistatistika (team_full_name, ID,height_feet, height_inches, position, weight_pounds) VALUES('" + l.team_full_name + "', '" + l.ID + "', '" + l.height_feet + "', '" + l.height_inches + "', '" + l.position + "','" +l.weight_pounds+"')";
                    using (DbDataReader oReader = oCommand.ExecuteReader())
                    {
                    }
                }
               
               
               
            }
        }




        //DOBIVAMO PODATKE ZA TIM, POJEDINAČNO
        public NbaLiga GetTimPodatci(int id) //prima redak onaj iz dataGridView1
        {
        // ne treba treenutno AddPodatkeZaKlub();

            NbaLiga LigaREST = new NbaLiga();
            string url = "https://www.balldontlie.io/api/v1/teams";
            string sjson = CallRestMethod(url);
            var json = JObject.Parse(sjson);
            var Nba = json.GetValue("data");





            foreach (JObject item in Nba)
            {
                if (id == (int)item.GetValue("id"))
                {
                    LigaREST.id = (int)item.GetValue("id");
                    LigaREST.abbreviation = (string)item.GetValue("abbreviation");
                    LigaREST.city = (string)item.GetValue("city");
                    LigaREST.conference = (string)item.GetValue("conference");
                    LigaREST.division = (string)item.GetValue("division");
                    LigaREST.full_name = (string)item.GetValue("full_name");
                    LigaREST.name = (string)item.GetValue("name");
                }
            }
            Console.WriteLine(LigaREST.id);
            return LigaREST; //Vraca specificni indeks
        }



        public static List<NbaLiga> ucitajpodatke()
        {
            List<NbaLiga> nbaLigaBaza = new List<NbaLiga>();
            WebClient wc = new WebClient();
            try
            {
                using (DbConnection connection = new SqlConnection(connectionString))
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM cikor_PodatciTimova";
                    connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string imageUrl = (string)reader["image"];
                            byte[] imageBytes = wc.DownloadData(imageUrl);
                            Bitmap ms = new Bitmap(new MemoryStream(imageBytes));

                            nbaLigaBaza.Add(new NbaLiga()
                            {
                                id = (int)reader["id"],
                                full_name = (string)reader["full_name"],
                                stadion = (string)reader["stadion"],
                                trener = (string)reader["trener"],
                                image = ms
                            });
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return nbaLigaBaza;
        }




        //UČITAVAMO PODATKE ZA IGRAČA(); DOBIVAMO PODATKE IZ BAZE U DGV, ONO ŠTO NEMA U APIJU
        public static List<NbaLigaIgrac> ucitajpodatkeZaIgraca()
        {
            List<NbaLigaIgrac> nbaLigaBaza = new List<NbaLigaIgrac>();
            WebClient wc = new WebClient();
            using (DbConnection connection = new SqlConnection(connectionString))
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM cikor_cikorIgraci";
                connection.Open();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        string imageUrl = (string)reader["image"];
                        byte[] imageBytes = wc.DownloadData(imageUrl);
                        Bitmap ms = new Bitmap(new MemoryStream(imageBytes));

                        nbaLigaBaza.Add(new NbaLigaIgrac()
                        {
                            ID = (int)reader["ID"],
                            first_name = (string)reader["first_name"],
                            last_name = (string)reader["last_name"],
                            height_feet = (string)reader["height_feet"],
                            height_inches = (string)reader["height_inches"],
                            position = (string)reader["position"],
                            weight_pounds = (string)reader["weight_pounds"],
                            image = ms
                        });
                    }
                }
                connection.Close();
            }
            return nbaLigaBaza;
        }


        //DOBIVAMO PODATKE ZA IGRACE OD EKIPA//KAD STISNEMO IGRACA PA ZA NJEGA
        public List<NbaLigaIgrac> GetTimIgracSolo(int ID)
        {

            List<NbaLigaIgrac> LigaIgraciREST = new List<NbaLigaIgrac>();
            string url1 = "https://www.balldontlie.io/api/v1/players/?per_page=100";
            string url2 = "https://www.balldontlie.io/api/v1/players/?page=28&?per_page=100?";
            string url3 = "https://www.balldontlie.io/api/v1/players/?page=3&?per_page=100";
            string url4 = "https://www.balldontlie.io/api/v1/players/?page=4&?per_page=100";
            string url5 = "https://www.balldontlie.io/api/v1/players/?page=5?per_page=100";
            string url6 = "https://www.balldontlie.io/api/v1/players/?page=6?per_page=100";
            string url7 = "https://www.balldontlie.io/api/v1/players/?page=7?per_page=100";


            string sjson1 = CallRestMethod(url1);
            var json1 = JObject.Parse(sjson1);

            string sjson2 = CallRestMethod(url2);
            var json2 = JObject.Parse(sjson2);

            string sjson3 = CallRestMethod(url3);
            var json3 = JObject.Parse(sjson3);

            string sjson4 = CallRestMethod(url4);
            var json4 = JObject.Parse(sjson4);

            string sjson5 = CallRestMethod(url5);
            var json5 = JObject.Parse(sjson5);

            string sjson6 = CallRestMethod(url6);
            var json6 = JObject.Parse(sjson6);

            string sjson7 = CallRestMethod(url7);
            var json7 = JObject.Parse(sjson7);


            var Nba1 = json1.GetValue("data");
            var Nba2 = json2.GetValue("data");
            var Nba3 = json3.GetValue("data");
            var Nba4 = json4.GetValue("data");
            var Nba5 = json5.GetValue("data");
            var Nba6 = json6.GetValue("data");
            var Nba7 = json7.GetValue("data");



            foreach (JObject item in Nba1)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {
                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")
                });


                //string sSqlConnectionString = "Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!";
                //using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
                //using (DbCommand oCommand = oConnection.CreateCommand())
                //{
                //    oConnection.Open();
                //    oCommand.CommandText = "INSERT INTO cikor_cikorIgraci (ID, first_name,last_name, height_feet,height_inches,position,weight_pounds) VALUES('" + (int)item.SelectToken("id") + "', '" + (string)item.SelectToken("first_name") + "', '" + (string)item.SelectToken("last_name") + "', '" + (string)item.SelectToken("height_feet") + "', '" + (string)item.SelectToken("height_inches") + "', '" + (string)item.SelectToken("position") + "', '" + (string)item.SelectToken("weight_pounds") + "')";
                //    using DbDataReader oReader = oCommand.ExecuteReader();
                //}

            }

            foreach (JObject item in Nba2)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });


            }

            foreach (JObject item in Nba3)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });
              
            }

            foreach (JObject item in Nba4)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });
              

            }

            foreach (JObject item in Nba5)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });
              

            }

            foreach (JObject item in Nba6)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });
             


            }
            foreach (JObject item in Nba7)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });



            }


            return LigaIgraciREST.Where(x => x.ID == ID).ToList();
        }




        public List<NbaLigaIgrac> GetTimIgracZaStatistiku()
        {
        
            List<NbaLigaIgrac> LigaIgraciREST = new List<NbaLigaIgrac>();
            string url1 = "https://www.balldontlie.io/api/v1/players/?per_page=100";
            string url2 = "https://www.balldontlie.io/api/v1/players/?page=28&?per_page=100?";
            string url3 = "https://www.balldontlie.io/api/v1/players/?page=3&?per_page=100";
            string url4 = "https://www.balldontlie.io/api/v1/players/?page=4&?per_page=100";
            string url5 = "https://www.balldontlie.io/api/v1/players/?page=5?per_page=100";
            string url6 = "https://www.balldontlie.io/api/v1/players/?page=6?per_page=100";
            string url7 = "https://www.balldontlie.io/api/v1/players/?page=7?per_page=100";


            string sjson1 = CallRestMethod(url1);
            var json1 = JObject.Parse(sjson1);

            string sjson2 = CallRestMethod(url2);
            var json2 = JObject.Parse(sjson2);

            string sjson3 = CallRestMethod(url3);
            var json3 = JObject.Parse(sjson3);

            string sjson4 = CallRestMethod(url4);
            var json4 = JObject.Parse(sjson4);

            string sjson5 = CallRestMethod(url5);
            var json5 = JObject.Parse(sjson5);

            string sjson6 = CallRestMethod(url6);
            var json6 = JObject.Parse(sjson6);

            string sjson7 = CallRestMethod(url7);
            var json7 = JObject.Parse(sjson7);


            var Nba1 = json1.GetValue("data");
            var Nba2 = json2.GetValue("data");
            var Nba3 = json3.GetValue("data");
            var Nba4 = json4.GetValue("data");
            var Nba5 = json5.GetValue("data");
            var Nba6 = json6.GetValue("data");
            var Nba7 = json7.GetValue("data");



            foreach (JObject item in Nba1)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {
                  
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    team_full_name=(string)item.SelectToken("team").SelectToken("full_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds=(string)item.SelectToken("weight_pounds")
                });


                //string sSqlConnectionString = "Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!";
                //using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
                //using (DbCommand oCommand = oConnection.CreateCommand())
                //{
                //    oConnection.Open();
                //    oCommand.CommandText = "INSERT INTO cikor_igracistatistika1 (ID, first_name, last_name, team_full_name,height_feet, height_inches,position,weight_pounds) VALUES('" + (int)item.SelectToken("id") + "', '" + (string)item.SelectToken("first_name") + "', '" + (string)item.SelectToken("last_name") + "', '" + (string)item.SelectToken("team").SelectToken("full_name") + "', '" + (string)item.SelectToken("height_feet") + "', '" + (string)item.SelectToken("height_inches") + "', '" +(string)item.SelectToken("position") +"','"+ (string)item.SelectToken("weight_pounds") + "')";
                //    using DbDataReader oReader = oCommand.ExecuteReader();
                //}

            }

            foreach (JObject item in Nba2)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                 
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });


            }

            foreach (JObject item in Nba3)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                 
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });

            }

            foreach (JObject item in Nba4)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });


            }

            foreach (JObject item in Nba5)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });


            }

            foreach (JObject item in Nba6)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });



            }
            foreach (JObject item in Nba7)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position"),
                    weight_pounds = (string)item.SelectToken("weight_pounds")

                });



            }


            return LigaIgraciREST;
        }








        //DOBIVAMO PODATKE ZA UTAKMICE OD EKIPE
        public List<NbaLigaUtakmica> GetTimUtakmice(int id_team)
        {

            List<NbaLigaUtakmica> LigaUtakmiceREST = new List<NbaLigaUtakmica>();
            string url = "https://www.balldontlie.io/api/v1/games";
            string url2 = "https://www.balldontlie.io/api/v1/games/?page=2&per_page=100";
            string url3 = "https://www.balldontlie.io/api/v1/games/?page=3&per_page=100";
            string url4 = "https://www.balldontlie.io/api/v1/games/?page=4&per_page=100";
            string url5 = "https://www.balldontlie.io/api/v1/games/?page=5&per_page=100";
            string url6 = "https://www.balldontlie.io/api/v1/games/?page=6&per_page=100";

            string sjson = CallRestMethod(url);
            var json = JObject.Parse(sjson);


            string sjson2 = CallRestMethod(url2);
            var json2 = JObject.Parse(sjson2);

            string sjson3 = CallRestMethod(url3);
            var json3 = JObject.Parse(sjson3);

            string sjson4 = CallRestMethod(url4);
            var json4 = JObject.Parse(sjson4);

            string sjson5 = CallRestMethod(url5);
            var json5 = JObject.Parse(sjson5);

            string sjson6 = CallRestMethod(url6);
            var json6 = JObject.Parse(sjson6);


            var Nba = json.GetValue("data");
            var Nba2 = json2.GetValue("data");
            var Nba3 = json3.GetValue("data");
            var Nba4 = json4.GetValue("data");

            foreach (JObject item in Nba)
            {
                LigaUtakmiceREST.Add(new NbaLigaUtakmica
                {

                    home_team_id = (int)item.SelectToken("home_team").SelectToken("id"),
                    visitor_team_id = (int)item.SelectToken("visitor_team").SelectToken("id"),
                    home_team_full_name = (string)item.SelectToken("home_team").SelectToken("full_name"),
                    visitor_team_full_name = (string)item.SelectToken("visitor_team").SelectToken("full_name"),
                   
                    id = (int)item.SelectToken("id"),
                    date = (string)item.SelectToken("date"),
                    home_team_score = (string)item.SelectToken("home_team_score"),
                    season = (string)item.SelectToken("season"),
                    visitor_team_score = (string)item.SelectToken("visitor_team_score"),

                });


            }
            foreach (JObject item in Nba2)
            {
                LigaUtakmiceREST.Add(new NbaLigaUtakmica
                {

                    home_team_id = (int)item.SelectToken("home_team").SelectToken("id"),
                    visitor_team_id = (int)item.SelectToken("visitor_team").SelectToken("id"),
                    home_team_full_name = (string)item.SelectToken("home_team").SelectToken("full_name"),
                    visitor_team_full_name = (string)item.SelectToken("visitor_team").SelectToken("full_name"),
                    id = (int)item.SelectToken("id"),
                    date = (string)item.SelectToken("date"),
                    home_team_score = (string)item.SelectToken("home_team_score"),
                    season = (string)item.SelectToken("season"),
                    visitor_team_score = (string)item.SelectToken("visitor_team_score"),

                });


            }
            foreach (JObject item in Nba3)
            {
                LigaUtakmiceREST.Add(new NbaLigaUtakmica
                {

                    home_team_id = (int)item.SelectToken("home_team").SelectToken("id"),
                    visitor_team_id = (int)item.SelectToken("visitor_team").SelectToken("id"),
                    home_team_full_name = (string)item.SelectToken("home_team").SelectToken("full_name"),
                    visitor_team_full_name = (string)item.SelectToken("visitor_team").SelectToken("full_name"),
                    id = (int)item.SelectToken("id"),
                    date = (string)item.SelectToken("date"),
                    home_team_score = (string)item.SelectToken("home_team_score"),
                    season = (string)item.SelectToken("season"),
                    visitor_team_score = (string)item.SelectToken("visitor_team_score"),

                });


            }
            foreach (JObject item in Nba4)
            {
                LigaUtakmiceREST.Add(new NbaLigaUtakmica
                {

                    home_team_id = (int)item.SelectToken("home_team").SelectToken("id"),
                    visitor_team_id = (int)item.SelectToken("visitor_team").SelectToken("id"),
                    home_team_full_name = (string)item.SelectToken("home_team").SelectToken("full_name"),
                    visitor_team_full_name = (string)item.SelectToken("visitor_team").SelectToken("full_name"),
                    id = (int)item.SelectToken("id"),
                    date = (string)item.SelectToken("date"),
                    home_team_score = (string)item.SelectToken("home_team_score"),
                    season = (string)item.SelectToken("season"),
                    visitor_team_score = (string)item.SelectToken("visitor_team_score"),

                });


            }

            return LigaUtakmiceREST.Where(x => x.home_team_id == id_team).ToList();
        }







        //DOBIVAMO PODATKE ZA IGRACE OD EKIPA
        public List<NbaLigaIgrac> GetTimIgrac(int home_team_id)
        {

            List<NbaLigaIgrac> LigaIgraciREST = new List<NbaLigaIgrac>();
            string url1 = "https://www.balldontlie.io/api/v1/players/?per_page=100";
            string url2 = "https://www.balldontlie.io/api/v1/players/?page=18&?per_page=100?";
            string url3 = "https://www.balldontlie.io/api/v1/players/?page=3&?per_page=100";
            string url4 = "https://www.balldontlie.io/api/v1/players/?page=8&?per_page=100";
            string url5 = "https://www.balldontlie.io/api/v1/players/?page=12?per_page=100";
            string url6 = "https://www.balldontlie.io/api/v1/players/?page=30?per_page=100";
            string url7 = "https://www.balldontlie.io/api/v1/players/?page=44?per_page=100";


            string sjson1 = CallRestMethod(url1);
            var json1 = JObject.Parse(sjson1);

            string sjson2 = CallRestMethod(url2);
            var json2 = JObject.Parse(sjson2);

            string sjson3 = CallRestMethod(url3);
            var json3 = JObject.Parse(sjson3);

            string sjson4 = CallRestMethod(url4);
            var json4 = JObject.Parse(sjson4);

            string sjson5 = CallRestMethod(url5);
            var json5 = JObject.Parse(sjson5);

            string sjson6 = CallRestMethod(url6);
            var json6 = JObject.Parse(sjson6);

            string sjson7 = CallRestMethod(url7);
            var json7 = JObject.Parse(sjson7);


            var Nba1 = json1.GetValue("data");
            var Nba2 = json2.GetValue("data");
            var Nba3=  json3.GetValue("data");
            var Nba4 = json4.GetValue("data");
            var Nba5=  json5.GetValue("data");
            var Nba6 = json6.GetValue("data");
            var Nba7 = json7.GetValue("data");

            foreach (JObject item in Nba1)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }

            foreach (JObject item in Nba2)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }

            foreach (JObject item in Nba3)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }

            foreach (JObject item in Nba4)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }

            foreach (JObject item in Nba5)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }

            foreach (JObject item in Nba6)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }
            foreach (JObject item in Nba7)
            {
                LigaIgraciREST.Add(new NbaLigaIgrac
                {

                    team_id = (int)item.SelectToken("team").SelectToken("id"),
                    ID = (int)item.SelectToken("id"),
                    first_name = (string)item.SelectToken("first_name"),
                    last_name = (string)item.SelectToken("last_name"),
                    height_feet = (string)item.SelectToken("height_feet"),
                    height_inches = (string)item.SelectToken("height_inches"),
                    position = (string)item.SelectToken("position")

                });

            }




            return LigaIgraciREST.Where(x => x.team_id == home_team_id).ToList();
        }




        ///STATISTIKU DOBIVAMO
        public List<NbaStatistika> GetStatistika()
        {
            List<NbaStatistika> LigaREST = new List<NbaStatistika>();


            string url =  "https://www.balldontlie.io/api/v1/stats/?per_page=100&page=1200";
            string url2 = "https://www.balldontlie.io/api/v1/stats/?per_page=100&page=1203";
          

            string sjson = CallRestMethod(url);
            string sjson2 = CallRestMethod(url2);
        

            var json = JObject.Parse(sjson);
            var json2 = JObject.Parse(sjson2);
          
                         

            var Nba = json.GetValue("data");
            var Nba2 = json2.GetValue("data");
          

            foreach (JObject item in Nba)
            {


                LigaREST.Add(new NbaStatistika
                {

                    first_name = (string)item.SelectToken("player").SelectToken("first_name"),
                    last_name = (string)item.SelectToken("player").SelectToken("last_name"),
                    ast = (string)item.SelectToken("ast"),
                    blk = (string)item.SelectToken("blk"),
                    dreb = (string)item.SelectToken("dreb"),
                    fg3_pct = (string)item.SelectToken("fg3_pct"),
                    min = (string)item.SelectToken("min"),
                    pts = (string)item.SelectToken("pts"),
                    reb = (string)item.SelectToken("reb"),
                    stl = (string)item.SelectToken("stl"),
                    turnover = (string)item.SelectToken("turnover")

                });


                //string sSqlConnectionString = "Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!";
                //using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
                //using (DbCommand oCommand = oConnection.CreateCommand())
                //{
                //    oConnection.Open();
                //    oCommand.CommandText = "INSERT INTO cikor_cikorStatistika3 (first_name,last_name, ast,blk,dreb,fg3_pct,min,pts,reb,stl,turnover) VALUES('" + (string)item.SelectToken("player").SelectToken("first_name") + "', '" + (string)item.SelectToken("player").SelectToken("last_name") + "', '" + (string)item.SelectToken("ast") + "', '" + (string)item.SelectToken("blk") + "', '" + (string)item.SelectToken("dreb") + "', '" + (string)item.SelectToken("fg3_pct") + "', '" + (string)item.SelectToken("min") + "', '" + (string)item.SelectToken("pts") + "', '" + (string)item.SelectToken("reb") + "', '" + (string)item.SelectToken("stl") + "', '" + (string)item.SelectToken("turnover") + "')";
                //    using DbDataReader oReader = oCommand.ExecuteReader();


                //}

            }



            foreach (JObject item in Nba2)
            {


                LigaREST.Add(new NbaStatistika
                {

                    first_name = (string)item.SelectToken("player").SelectToken("first_name"),
                    last_name = (string)item.SelectToken("player").SelectToken("last_name"),
                    ast = (string)item.SelectToken("ast"),
                    blk = (string)item.SelectToken("blk"),
                    dreb = (string)item.SelectToken("dreb"),
                    fg3_pct = (string)item.SelectToken("fg3_pct"),
                    min = (string)item.SelectToken("min"),
                    pts = (string)item.SelectToken("pts"),
                    reb = (string)item.SelectToken("reb"),
                    stl = (string)item.SelectToken("stl"),
                    turnover = (string)item.SelectToken("turnover")

                });

                //string sSqlConnectionString = "Server=193.198.57.183; Database=STUDENTI_PIN; User ID=pin; Password=Vsmti1234!";
                //using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
                //using (DbCommand oCommand = oConnection.CreateCommand())
                //{
                //    oConnection.Open();
                //    oCommand.CommandText = "INSERT INTO cikor_cikorStatistika3 (first_name,last_name, ast,blk,dreb,fg3_pct,min,pts,reb,stl,turnover) VALUES('" + (string)item.SelectToken("player").SelectToken("first_name") + "', '" + (string)item.SelectToken("player").SelectToken("last_name") + "', '" + (string)item.SelectToken("ast") + "', '" + (string)item.SelectToken("blk") + "', '" + (string)item.SelectToken("dreb") + "', '" + (string)item.SelectToken("fg3_pct") + "', '" + (string)item.SelectToken("min") + "', '" + (string)item.SelectToken("pts") + "', '" + (string)item.SelectToken("reb") + "', '" + (string)item.SelectToken("stl") + "', '" + (string)item.SelectToken("turnover") + "')";
                //    using DbDataReader oReader = oCommand.ExecuteReader();
                //}
            }


            return LigaREST;

        }

        private static string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }
    }
}
