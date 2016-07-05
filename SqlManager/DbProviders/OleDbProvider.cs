using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;

namespace Franksoft.SqlManager.DbProviders
{
    public class OleDbProvider : IDbProvider
    {
        private OleDbConnection Connection { get; set; }

        private OleDbCommand Command { get; set; }

        private OleDbDataAdapter Adapter { get; set; }

        public string ConnectionString { get; }

        public string CommandText { get; set; }

        public Array Parameters { get; set; }

        public OleDbProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.Connection = new OleDbConnection(connectionString);
            this.Command = new OleDbCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new OleDbDataAdapter();

            this.Connection.Open();
        }

        public DbTransaction BeginTransaction()
        {
            return this.Connection.BeginTransaction();
        }

        public DbTransaction BeginTransaction(IsolationLevel il)
        {
            return this.Connection.BeginTransaction(il);
        }

        public int ExecuteNonQuery()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if(this.Parameters!=null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteNonQuery();
        }

        public DbDataReader ExecuteReader()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader();
        }

        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader(behavior);
        }

        public object ExecuteScalar()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteScalar();
        }

        public int Fill(DataTable dataTable)
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }
            this.Adapter.SelectCommand = this.Command;
            return this.Adapter.Fill(dataTable);
        }

        public int Update(DataTable dataTable)
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }
            this.Adapter.SelectCommand = this.Command;
            return this.Adapter.Update(dataTable);
        }

        public void Dispose()
        {
            if( this.Command!=null)
            {
                this.Command.Cancel();
            }

            if (this.Connection != null && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }

            this.Adapter.Dispose();
            this.Adapter = null;
            this.Command.Dispose();
            this.Command = null;
            this.Connection.Dispose();
            this.Connection = null;

            GC.Collect();
            GC.WaitForFullGCComplete();
        }
    }
}
