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
        private AdoNet adoNet = new AdoNet();
        //private string connString = "Data Source=DESKTOP-AGEVIQ5;Initial Catalog=ClientManage;Integrated Security=True";
        //private SqlDataAdapter dataAdapter;
        //private DataSet dataSet;
        //private SqlConnection cnx;

        public MainWindow()
        {
            InitializeComponent();

        }
        ///////-->
        private void ClientManage_Load(object sender, EventArgs e)
        {
            //Fill DropDowns With City
            GetCity();
            //Get All Client
        }

        //Get All Clients
        private void showAll()
        {
            //using (cnx = new SqlConnection(connString))
            //{
            //    try
            //    {
            //        //// Connected Mode ////
            //        //cnx.Open(); //Open Cnx
            //        //string Query = @"SELECT u.clientId AS 'ID', u.FName AS 'First Name', u.LName AS 'Last Name', u.cAddress AS 'Address', c.cityName AS 'City' FROM Client u 
            //        //INNER Join Cities c ON u.CityId = c.cityId; ";

            //        //SqlCommand cmd = new SqlCommand(Query, cnx);//Send Query To Db
            //        //dataAdapter = new SqlDataAdapter(cmd);//Retrieve Result 
            //        //DataTable table = new DataTable("Client");
            //        //dataAdapter.Fill(table);//Fill dataAdapter with the Result Table
            //        //cnx.Close(); //Close Cnx
            //        //DataList.ItemsSource = table.DefaultView;//Append the Result Table to the DataList in XAML
            //        //DataTableReader reader = new DataTableReader(table);// Read Table

            //        //while (reader.Read())
            //        //{
            //        //    // DataList.Columns.Insert(0,(DataGridColumn)reader.GetString(0));
            //        //    //DataList.Items.Add(reader.GetProviderSpecificValue(0));
            //        //}
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //        MessageBox.Show(e.Message);
            //    }

            //}
        }
        ///////-->

        void GetCity()
        {
            //adoNet = new AdoNet();

            try
            {
                string req = @"Select * From Cities; SELECT u.clientId AS 'ID', u.FName AS 'First Name', u.LName AS 'Last Name', u.cAddress AS 'Address', c.cityName AS 'City' FROM Client u INNER Join Cities c ON u.CityId = c.cityId; ";
                //Set the Request
                adoNet.Command.CommandText = req;
                //Connection with Db
                adoNet.Command.Connection = adoNet.Connection;
                //Send Command and Get Result in Adapter
                adoNet.Adapter.SelectCommand = adoNet.Command;
                //Fill the DataSet with Result wish in Adapter
                adoNet.Adapter.Fill(adoNet.DataSet);

                //Separate the 2 Tables 
                //Cities and Clients from DataSet To DataTable Cities/Clients
                adoNet.CitiesDataTable = adoNet.DataSet.Tables[0];
                adoNet.ClientsDataTable = adoNet.DataSet.Tables[1];
                //Set Name to the tow tables
                adoNet.CitiesDataTable.TableName = "Cities";
                adoNet.ClientsDataTable.TableName = "Clients";
                MessageBox.Show(adoNet.DataSet.Tables[0].ToString());
                MessageBox.Show(adoNet.DataSet.Tables[1].ToString());

                this.DataList.ItemsSource = adoNet.DataSet.Tables[1].DefaultView;
                //Fill DropDowns With Cities
                foreach (var item in adoNet.CitiesDataTable.Select())
                {
                    City.Items.Add(item[1]);
                    CityFilter.Items.Add(item[1]);
                }
                ///////-->
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



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