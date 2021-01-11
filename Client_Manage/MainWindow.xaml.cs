using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Client_Manage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //Initialise Variables 
        private string connString = "Data Source=DESKTOP-AGEVIQ5;Initial Catalog=ClientManage;Integrated Security=True";
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
        private SqlConnection cnx;

        public MainWindow()
        {
            InitializeComponent();
            //Fill DropDowns With City
            GetCities();

            //Get All Client
            showAll();
        }
        ///////-->


        //Get All Clients
        private void showAll()
        {
            using (cnx = new SqlConnection(connString))
            {
                try
                {
                    //// Connected Mode ////
                    cnx.Open(); //Open Cnx
                    string Query = @"SELECT u.clientId AS 'ID', u.FName AS 'First Name', u.LName AS 'Last Name', u.cAddress AS 'Address', c.cityName AS 'City' FROM Client u 
                    INNER Join Cities c ON u.CityId = c.cityId; ";

                    SqlCommand cmd = new SqlCommand(Query, cnx);//Send Query To Db
                    dataAdapter = new SqlDataAdapter(cmd);//Retrieve Result 
                    DataTable table = new DataTable("Client");
                    dataAdapter.Fill(table);//Fill dataAdapter with the Result Table
                    cnx.Close(); //Close Cnx
                    DataList.ItemsSource = table.DefaultView;//Append the Result Table to the DataList in XAML
                    DataTableReader reader = new DataTableReader(table);// Read Table

                    while (reader.Read())
                    {
                        // DataList.Columns.Insert(0,(DataGridColumn)reader.GetString(0));
                        //DataList.Items.Add(reader.GetProviderSpecificValue(0));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBox.Show(e.Message);
                }

            }
        }
        ///////-->


        //Fill DropDowns With Cities
        private void GetCities()
        {
            using (cnx = new SqlConnection(connString))
            {
                try
                {
                    cnx.Open();
                    string Query = "Select * From Cities ";
                    SqlCommand sql = new SqlCommand(Query, cnx);
                    SqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        string city = reader.GetString(1);
                        //Fill dropdown of Controls
                        City.Items.Add(city);
                        //Fill dropdown of Filter
                        CityFilter.Items.Add(city);
                    }
                    cnx.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        ///////-->


        private void CityFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change 
        }

        //Top left GitHub ICON Event
        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            string url = @"https://github.com/adamkhairi/";
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
        ///////-->

    }
}