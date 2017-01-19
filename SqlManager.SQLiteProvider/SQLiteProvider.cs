using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Franksoft.SqlManager.DbProviders
{
    public class SQLiteProvider : IDbProvider
    {
        public SQLiteProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.Connection = new SQLiteConnection(connectionString);
            this.Command = new SQLiteCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SQLiteDataAdapter();

            this.Connection.Open();
        }

        public SQLiteProvider(SQLiteConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Command = new SQLiteCommand();
            this.Command.Connection = this.Connection;
            this.Adapter = new SQLiteDataAdapter();
        }

        private SQLiteConnection Connection { get; set; }

        private SQLiteCommand Command { get; set; }

        private SQLiteDataAdapter Adapter { get; set; }

        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public string ConnectionString { get; set; }

        public Array Parameters { get; set; }

        public DbTransaction BeginTransaction()
        {
            SQLiteTransaction transaction = this.Connection.BeginTransaction();
            this.Command.Transaction = transaction;

            return transaction;
        }

        public DbTransaction BeginTransaction(IsolationLevel il)
        {
            SQLiteTransaction transaction = this.Connection.BeginTransaction(il);
            this.Command.Transaction = transaction;

            return transaction;
        }

        public int ExecuteNonQuery()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
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

        public DbParameter GetParameter(string parameterName, object value)
        {
            var parameter = this.Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }

        public DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            if (nameValuePairs != null)
            {
                foreach (KeyValuePair<string, object> nameValuePair in nameValuePairs)
                {
                    var parameter = this.GetParameter(nameValuePair.Key, nameValuePair.Value);
                    parameters.Add(parameter);
                }
            }

            return parameters.ToArray();
        }

        public DbParameter[] GetParameterArray(params object[] values)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            if (values != null)
            {
                foreach (object value in values)
                {
                    var parameter = this.GetParameter(null, value);
                    parameters.Add(parameter);
                }
            }

            return parameters.ToArray();
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
    }
}
