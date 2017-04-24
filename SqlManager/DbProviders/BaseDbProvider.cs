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
        /// Gets the <see cref="DbDataAdapter"/> object inside this DbProvider.
        /// </summary>
        public virtual DbDataAdapter Adapter { get; protected set; }

        /// <summary>
        /// Gets the <see cref="DbCommand"/> object inside this DbProvider.
        /// </summary>
        public virtual DbCommand Command { get; protected set; }

        /// <summary>
        /// Gets or sets the command text used in query methods in this DbProvider.
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Data.CommandType"/> used in query methods in this DbProvider.
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets the <see cref="DbConnection"/> object inside this DbProvider.
        /// </summary>
        public virtual DbConnection Connection { get; protected set; }

        /// <summary>
        /// Gets the connection string for this DbProvider.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbParameter"/> array used in query methods in this DbProvider.
        /// </summary>
        public DbParameter[] Parameters { get; set; }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public virtual DbTransaction BeginTransaction()
        {
            DbTransaction transaction = this.Connection.BeginTransaction();
            this.Command.Transaction = transaction;

            return transaction;
        }

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="il">Specifies the isolation level for the transaction.</param>
        /// <returns>An object representing the new transaction.</returns>
        public virtual DbTransaction BeginTransaction(IsolationLevel il)
        {
            DbTransaction transaction = this.Connection.BeginTransaction(il);
            this.Command.Transaction = transaction;

            return transaction;
        }

        /// <summary>
        /// Aids implementation of the <see cref="IDisposable"/> interface. 
        /// Properties will be disposed and <see cref="GC.Collect()"/> will be executed.
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
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
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
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="CommandText"/>.</returns>
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
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance using one of the <see cref="CommandBehavior"/>.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="CommandText"/>.</returns>
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
        /// Executes the query and returns the first column of the first row in the result set returned by the query. 
        /// All other columns and rows are ignored.
        /// </summary>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
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
        /// Executes <see cref="CommandText"/> with <see cref="DbDataAdapter.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dataTable">The name of the <see cref="DataTable"/> to use for table mapping.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>. 
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
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
        /// Creates and returns a <see cref="DbParameter"/> object with a name and value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <returns>A <see cref="DbParameter"/> object.</returns>
        public virtual DbParameter GetParameter(string parameterName, object value)
        {
            var parameter = this.Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Creates and returns an array of <see cref="DbParameter"/> objects with values.
        /// </summary>
        /// <param name="values">Values to create the parameter array.</param>
        /// <returns>An array of <see cref="DbParameter"/> objects.</returns>
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
        /// Creates and returns an array of <see cref="DbParameter"/> objects with 
        /// <see cref="KeyValuePair{string, object}"/> objects.
        /// </summary>
        /// <param name="nameValuePairs">
        /// <see cref="KeyValuePair{string, object}"/> objects to create the parameter array 
        /// with parameter name as <see cref="KeyValuePair{string, object}.Key"/> and 
        /// parameter value as <see cref="KeyValuePair{string, object}.Value"/>.
        /// </param>
        /// <returns>An array of <see cref="DbParameter"/> objects.</returns>
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
        /// Initializes <see cref="IDbProvider"/> instance with the specified connection string.
        /// <para>
        /// This method is designed to support reusing a provider instance among different connection strings. 
        /// The idea is to avoid creating new instance, but only to execute this method when changing connection
        /// string. It is necessary to initialize <see cref="Adapter"/>, <see cref="Command"/> and 
        /// <see cref="Connection"/> properties here.
        /// </para>
        /// </summary>
        /// <param name="connectionString">Connection string for this <see cref="IDbProvider"/> instance.</param>
        public abstract void Initialize(string connectionString);

        /// <summary>
        /// Executes <see cref="CommandText"/> with <see cref="DbDataAdapter.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
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
