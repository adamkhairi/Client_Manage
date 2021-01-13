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
        public MainWindow()
        {
            InitializeComponent();

        }

        AdoNet Ad = new AdoNet();
        int SelectedId;


        #region App Loading ==>

        private void ClientManage_Load(object sender, RoutedEventArgs e)
        {
            DataList.CanUserAddRows = false;
            //Get Clients and Cities
            GetClients();
            GetCitys();

        }

        #endregion


        #region Get Clients and fill DataList ==>

        void GetClients()
        {
            try
            {
                //Clear if it's already fill
                if (Ad.DataSet.Tables["Clients"] != null) Ad.DataSet.Tables["Clients"].Clear();

                //Open Conncection to DB
                Ad.connect();

                Ad.Adapter = new SqlDataAdapter("Select * From Clients", Ad.Cnx);

                ////Fill the DataSet with Result of the Sql Request wish in Adapter
                Ad.Adapter.Fill(Ad.DataSet, "Clients");
                Ad.disconnect();
                //Copy Clients Table in a DataTable
                //Ad.Datatable = Ad.DataSet.Tables["Clients"];
                DataList.ItemsSource = Ad.DataSet.Tables["Clients"].DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show(e + string.Empty);
            }
        }

        #endregion


        #region Get Cities From Db and add them to ComboBox ==>

        void GetCitys()
        {
            try
            {
                City.Items.Clear();
                CityFilter.Items.Clear();
                Ad.Adapter = new SqlDataAdapter("Select * From Citys", Ad.Cnx);

                ////Fill the DataSet with Result wish in Adapter
                Ad.Adapter.Fill(Ad.DataSet, "Citys");

                //Fill the UI DropDown with Cities DataTable
                City.ItemsSource = Ad.DataSet.Tables["Citys"].DefaultView;
                City.DisplayMemberPath = Ad.DataSet.Tables["Citys"].Columns[1].ColumnName;
                City.SelectedValuePath = Ad.DataSet.Tables["Citys"].Columns[0].ColumnName;

                //Fill the UI DropDown with Cities Filtre DataTable
                CityFilter.ItemsSource = Ad.DataSet.Tables["Citys"].DefaultView;
                CityFilter.DisplayMemberPath = Ad.DataSet.Tables["Citys"].Columns[1].ColumnName;
                CityFilter.SelectedValuePath = Ad.DataSet.Tables["Citys"].Columns[0].ColumnName;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + string.Empty);
            }
        }

        #endregion


        #region On Selection Of DataGrid Rows Changes Do ==>

        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = DataList.SelectedItem as DataRowView;
            if (row == null) return;

            //Turn On Updates
            Ad.IfUpdate = true;

            //Affect Infos To Inputs
            SelectedId = int.Parse(row.Row[0].ToString().Trim());
            FName.Text = row.Row[1].ToString().Trim();
            LName.Text = row.Row[2].ToString().Trim();
            Address.Text = row.Row[3].ToString().Trim();
            City.SelectedValue = row.Row[4].ToString().Trim();

            //Check If any update Done if so turn IfUpdate to false to allow Adding New Client
            foreach (UIElement item in Inputs.Children)
            {
                switch (item)
                {
                    //In Case item is TextBox
                    case TextBox box:
                        box.TextChanged += (se, ev) =>
                        {
                            Ad.IfUpdate = false;
                        };
                        break;

                    //In Case item is ComboBox
                    case ComboBox comboBox:
                        comboBox.SelectionChanged += (se, ev) =>
                        {
                            Ad.IfUpdate = false;
                        };
                        break;
                }
            }
        }

        #endregion


        #region Top left GitHub ICON Event ==>

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            const string url = @"https://github.com/adamkhairi/";
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + string.Empty);
            }
        }

        #endregion


        #region Button Add Clients Event ==>

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Ad.IfUpdate)
            {
                MessageBox.Show("Client Already Exist", "Already exist", MessageBoxButton.OK, MessageBoxImage.Warning);
                FName.Focus();
            }
            else if (IfEmpty())
            {
                MessageBox.Show("Please insert Infos !!", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                FName.Focus();
            }
            else
            {
                try
                {
                    //Create new row with same columns( New Client )
                    Ad.Row = Ad.DataSet.Tables["Clients"].NewRow();

                    //Fill Columns of new row with Form Inputs
                    Ad.Row[0] = (int.Parse(Ad.DataSet.Tables["Clients"].Rows[Ad.DataSet.Tables["Clients"].Rows.Count - 1][0]
                        .ToString() ?? string.Empty) + 1).ToString();
                    Ad.Row[1] = FName.Text.Trim();
                    Ad.Row[2] = LName.Text.Trim();
                    Ad.Row[3] = Address.Text.Trim();
                    Ad.Row[4] = City.SelectedValue.ToString()?.Trim();

                    //Add the new Client to the DataTable of Clients
                    Ad.DataSet.Tables["Clients"].Rows.Add(Ad.Row);
                    MessageBox.Show("Added !!");

                    //Clear Inputs
                    EmptyInputs();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + string.Empty);

                }
            }
        }

        #endregion


        #region Check If any Input Empty ==>

        private bool IfEmpty()
        {
            if (FName.Text.Trim().Equals(string.Empty) || LName.Text.Trim().Equals(string.Empty) ||
                Address.Text.Trim().Equals(string.Empty) || City.Text.Equals(string.Empty))
                return true;

            return false;
        }

        #endregion 


        #region Empty Inputs ==>

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

        #endregion


        #region Button Delete Client Info Event ==>

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var rows = Ad.DataSet.Tables["Clients"].Rows;
                bool ifNotDeleted = false;

                for (int i = 0; i < rows.Count; i++)
                {
                    if (SelectedId == int.Parse(rows[i][0].ToString()))
                    {
                        ifNotDeleted = true;
                        Ad.DataSet.Tables["Clients"].Rows[i].Delete();
                        MessageBox.Show("Delete Succ");
                        DataList.ItemsSource = Ad.DataSet.Tables["Clients"].DefaultView;

                        break;
                    }
                }
                if (ifNotDeleted == false)
                {
                    MessageBox.Show("Client Dos not Exist !");
                }
                EmptyInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + string.Empty);
                throw;
            }

        }

        #endregion


        #region Button Edit Client Info Event ==>

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rows = Ad.DataSet.Tables["Clients"].Rows;
                bool ifNotEdited = false;

                for (int i = 0; i < rows.Count; i++)
                {
                    if (SelectedId == int.Parse(rows[i][0].ToString()))
                    {
                        ifNotEdited = true;
                        Ad.DataSet.Tables["Clients"].Rows[i][1] = FName.Text.ToString().Trim();
                        Ad.DataSet.Tables["Clients"].Rows[i][2] = LName.Text.ToString().Trim();
                        Ad.DataSet.Tables["Clients"].Rows[i][3] = Address.Text.ToString().Trim();
                        Ad.DataSet.Tables["Clients"].Rows[i][4] = City.SelectedValue.ToString().Trim();

                        DataList.ItemsSource = Ad.DataSet.Tables["Clients"].DefaultView;

                        MessageBox.Show("Edit Succ !");
                        break;
                    }
                }
                if (ifNotEdited == false)
                {
                    MessageBox.Show("Client Dos not Exist !");
                }
                EmptyInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + string.Empty);
                throw;
            }

        }

        #endregion


        #region Button Save Into Database ==>

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ad.Builder = new SqlCommandBuilder(Ad.Adapter);
                MessageBox.Show(Ad.Builder.GetUpdateCommand() + string.Empty);
                Ad.Adapter.Update(Ad.DataSet, "Clients");
                MessageBox.Show("Save Done");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception + string.Empty);

            }
        }

        #endregion



    }
}