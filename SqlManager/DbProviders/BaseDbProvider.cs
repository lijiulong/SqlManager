using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// Aids implementation of the <see cref="IDbProvider"/> interface. 
    /// Inheritors of <see cref="BaseDbProvider"/> can get most of the functionality needed to fully implement a 
    /// <see cref="IDbProvider"/>.
    /// </summary>
    public abstract class BaseDbProvider : IDbProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual DbDataAdapter Adapter { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbCommand Command { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbConnection Connection { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbParameter[] Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbTransaction BeginTransaction()
        {
            DbTransaction transaction = this.Connection.BeginTransaction();
            this.Command.Transaction = transaction;

            return transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public virtual DbTransaction BeginTransaction(IsolationLevel il)
        {
            DbTransaction transaction = this.Connection.BeginTransaction(il);
            this.Command.Transaction = transaction;

            return transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int ExecuteNonQuery()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbDataReader ExecuteReader()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public virtual DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteReader(behavior);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object ExecuteScalar()
        {
            this.Command.CommandText = this.CommandText;
            this.Command.Parameters.Clear();
            if (this.Parameters != null)
            {
                this.Command.Parameters.AddRange(this.Parameters);
            }

            return this.Command.ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public virtual int Fill(DataTable dataTable)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual DbParameter GetParameter(string parameterName, object value)
        {
            var parameter = this.Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValuePairs"></param>
        /// <returns></returns>
        public virtual DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual DbParameter[] GetParameterArray(params object[] values)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public abstract void Initialize(string connectionString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public virtual int Update(DataTable dataTable)
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
