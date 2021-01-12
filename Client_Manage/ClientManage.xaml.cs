using MahApps.Metro.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Client_Manage
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //Initialise Variables 
        private readonly AdoNet adoNet = new AdoNet();
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
            GetStarted();
            //Get All Client
        }


        private void GetStarted()
        {
            //adoNet = new AdoNet();

            try
            {
                //SqlCommand => 2 Tables = Clients and Cities
                const string req = @"Select * From Citys; Select * From Clients;";

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
                adoNet.CitiesDataTable.TableName = "Citys";
                adoNet.ClientsDataTable.TableName = "Clients";

                //DataColumn[] pk = { adoNet.DataSet.Tables[1].Columns["Client Id"] };
                //adoNet.DataSet.Tables[1].PrimaryKey = pk;

                //Fill the UI Table (DataGrid) with  Clients DataTable
                DataList.ItemsSource = adoNet.ClientsDataTable.DefaultView;

                //Fill DropDowns With Cities
                City.ItemsSource = adoNet.CitiesDataTable.DefaultView;

                //City.DisplayMemberPath = "cityName";
                City.DisplayMemberPath = adoNet.CitiesDataTable.Columns[1].ColumnName;

                //City.SelectedValuePath = "cityId";
                City.SelectedValuePath = adoNet.CitiesDataTable.Columns[1].ColumnName;

                //
                DataList.SelectionChanged += (s, e) =>
                {
                    DataRowView row = DataList.SelectedItem as DataRowView;

                    if (row == null) return;

                    adoNet.IfUpdate = true;
                    //cientId.Text = row.Row[0].ToString().Trim();
                    FName.Text = row.Row[1].ToString()?.Trim() ?? string.Empty;
                    LName.Text = row.Row[2].ToString()?.Trim() ?? string.Empty;
                    Address.Text = row.Row[3].ToString()?.Trim() ?? string.Empty;
                    City.SelectedValue = row.Row[4].ToString()?.Trim();

                    //Check If any update Done if so turn IfUpdate to false to allow Adding New Client
                    foreach (UIElement item in Inputs.Children)
                    {
                        switch (item)
                        {
                            //In Case item is TextBox
                            case TextBox box:
                                box.TextChanged += (s, e) =>
                                {
                                    adoNet.IfUpdate = false;
                                };
                                break;

                            //In Case item is ComboBox
                            case ComboBox comboBox:
                                comboBox.SelectionChanged += (s, e) =>
                                {
                                    adoNet.IfUpdate = false;
                                };
                                break;
                        }
                    }
                };
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
            const string url = @"https://github.com/adamkhairi/";
            try
            {
                Process.Start(url);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
        ///////-->


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
        }

        //Add Button
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (adoNet.IfUpdate)
            {
                MessageBox.Show("Client Already Exist", "Already exist", MessageBoxButton.OK, MessageBoxImage.Warning);
                FName.Focus();
            }
            else if (IfEmpty())
            {
                MessageBox.Show("Please insert Infos !!");
                FName.Focus();
            }
            //If IfEmpty = true
            else
            {
                try
                {
                    //Create new row with same columns( New Client )
                    var client = adoNet.ClientsDataTable.NewRow();

                    //Fill Columns of new row with Form Inputs
                    client[0] = (int.Parse(adoNet.ClientsDataTable.Rows[adoNet.ClientsDataTable.Rows.Count - 1][0]
                        .ToString() ?? string.Empty) + 1).ToString();
                    client[1] = FName.Text.Trim();
                    client[2] = LName.Text.Trim();
                    client[3] = Address.Text.Trim();
                    client[4] = City.SelectedValue.ToString()?.Trim();

                    //if (cientId.Text.Trim() != client[0].ToString().Trim())
                    //{
                    //Add the new Client to the DataTable of Clients
                    adoNet.ClientsDataTable.Rows.Add(client);
                    MessageBox.Show("Ok !!");

                    //cientId.Text = int.Parse((cientId.Text )+ 1).ToString();
                    //Empty all Inputs
                    EmptyInputs();

                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(ex + string.Empty);
                    throw;
                }
            }
        }
        ///////-->

        /// Check If any Input Empty
        private bool IfEmpty()
        {
            if (FName.Text.Trim().Equals(string.Empty) || LName.Text.Trim().Equals(string.Empty) ||
                Address.Text.Trim().Equals(string.Empty) || City.Text.Equals(string.Empty))
                return true;

            return false;
        }
        ///////-->


        ///Empty Inputs
        private void EmptyInputs()
        {
            foreach (var item in Inputs.Children)
            {
                switch (item)
                {
                    case TextBox box:
                        box.Text = "";
                        break;
                    case ComboBox box:
                        box.SelectedIndex = -1;
                        break;
                }
            }
        }
        ///////-->

        //
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adoNet.Builder = new SqlCommandBuilder(adoNet.Adapter);
                adoNet.Adapter.Update(adoNet.DataSet, "Clients");
                MessageBox.Show("Save Done");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception + string.Empty);
                throw;
            }
        }

        ///////-->
    }
}