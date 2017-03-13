using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Franksoft.SqlManager.DbProviders
{
    public class SqlClientProvider : BaseDbProvider
    {
        public SqlClientProvider()
        {
        }

        public SqlClientProvider(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public SqlClientProvider(SqlConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SqlCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SqlDataAdapter();
        }

        private SqlConnection Connection { get; set; }

        private SqlCommand Command { get; set; }

        private SqlDataAdapter Adapter { get; set; }

        public override DbTransaction BeginTransaction()
        {
            SqlTransaction transaction = this.Connection.BeginTransaction();
            this.Command.Transaction = transaction;

            return transaction;
        }

        public override DbTransaction BeginTransaction(IsolationLevel il)
        {
            SqlTransaction transaction = this.Connection.BeginTransaction(il);
            this.Command.Transaction = transaction;

            return transaction;
        }

        public override void Dispose()
        {
            if (this.Command != null)
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

        public override int ExecuteNonQuery()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteNonQuery();
        }

        public override DbDataReader ExecuteReader()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader();
        }

        public override DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader(behavior);
        }

        public override object ExecuteScalar()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteScalar();
        }

        public override int Fill(DataTable dataTable)
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

        public override DbParameter GetParameter(string parameterName, object value)
        {
            var parameter = this.Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }

        public override void Initialize(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString) && !string.Equals(this.ConnectionString, connectionString))
            {
                this.ConnectionString = connectionString;
                this.Connection = new SqlConnection(connectionString);
                this.Command = new SqlCommand();
                this.Command.Connection = this.Connection;
                this.Adapter = new SqlDataAdapter();

                this.Connection.Open();
            }
        }

        public override int Update(DataTable dataTable)
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
    }
}
