﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Client_Manage
{
    class AdoNet
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        private SqlDataAdapter adapter;
        private string connectionString;
        private DataTable dataTable;
        private DataSet dataSet;

        public SqlConnection Connection
        {
            get => connection;
            set => connection = value;
        }

        public SqlCommand Command
        {
            get => command;
            set => command = value;
        }

        public SqlDataReader Reader
        {
            get => reader;
            set => reader = value;
        }

        public SqlDataAdapter Adapter
        {
            get => adapter;
            set => adapter = value;
        }

        public string ConnectionString
        {
            get => connectionString;
        }

        public DataTable DataTable
        {
            get => dataTable;
            set => dataTable = value;
        }

        public DataSet DataSet
        {
            get => dataSet;
            set => dataSet = value;
        }

        public AdoNet()
        {
            connectionString = "Data Source=DESKTOP-AGEVIQ5;Initial Catalog=ClientManage;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            adapter = new SqlDataAdapter();
        }
    }
}
