using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Client_Manage
{
    class AdoNet
    {

        // Declaration des objets sql
        private SqlConnection cnx = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private SqlDataReader dataReader;
        private DataTable datatable = new DataTable();
        private SqlDataAdapter adapter = new SqlDataAdapter();
        private SqlDataAdapter adapter1 = new SqlDataAdapter();

        private DataSet dataSet = new DataSet();
        private DataRow row;
        private bool ifUpdate = false;
        private SqlCommandBuilder builder;

        public bool IfUpdate { get => IfUpdate1; set => IfUpdate1 = value; }
        public SqlCommandBuilder Builder { get => builder; set => builder = value; }
        public SqlConnection Cnx { get => cnx; set => cnx = value; }
        public SqlCommand Cmd { get => cmd; set => cmd = value; }
        public DataRow Row { get => row; set => row = value; }
        public bool IfUpdate1 { get => ifUpdate; set => ifUpdate = value; }
        public SqlDataReader DataReader { get => dataReader; set => dataReader = value; }
        public DataTable Datatable { get => datatable; set => datatable = value; }
        public SqlDataAdapter Adapter { get => adapter; set => adapter = value; }
        public DataSet DataSet { get => dataSet; set => dataSet = value; }
        public SqlDataAdapter Adapter1 { get => adapter1; set => adapter1 = value; }


        // declaration of connect 
        public void connect()
        {
            if (Cnx.State == ConnectionState.Closed || Cnx.State == ConnectionState.Broken)
            {
                Cnx.ConnectionString = "Data Source=DESKTOP-AGEVIQ5;Initial Catalog=clientDb;Integrated Security=True";
                Cnx.Open();
            }
        }

        // declaration de la methode deconnecter
        public void disconnect()
        {
            if (Cnx.State == ConnectionState.Open)
            {
                Cnx.Close();
            }
        }
    }

}
