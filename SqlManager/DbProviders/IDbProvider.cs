using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders
{
    /// <summary>
    /// Defines necessary properties and methods to impliment a DbProvider for SqlManager.
    /// <see cref="SqlManager"/>
    /// </summary>
    public interface IDbProvider : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="DbDataAdapter"/> object inside this DbProvider.
        /// </summary>
        DbDataAdapter Adapter { get; }

        /// <summary>
        /// Gets the <see cref="DbCommand"/> object inside this DbProvider.
        /// </summary>
        DbCommand Command { get; }

        /// <summary>
        /// Gets or sets the command text used in query methods in this DbProvider.
        /// </summary>
        string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Data.CommandType"/> used in query methods in this DbProvider.
        /// </summary>
        CommandType CommandType { get; set; }

        /// <summary>
        /// Gets the <see cref="DbConnection"/> object inside this DbProvider.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// Gets the connection string for this DbProvider.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets or sets the <see cref="DbParameter"/> array used in query methods in this DbProvider.
        /// </summary>
        DbParameter[] Parameters { get; set; }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        DbTransaction BeginTransaction();

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="il">Specifies the isolation level for the transaction.</param>
        /// <returns>An object representing the new transaction.</returns>
        DbTransaction BeginTransaction(IsolationLevel il);

        /// <summary>
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="CommandText"/>.</returns>
        DbDataReader ExecuteReader();

        /// <summary>
        /// Executes the <see cref="CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance using one of the <see cref="CommandBehavior"/>.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="CommandText"/>.</returns>
        DbDataReader ExecuteReader(CommandBehavior behavior);

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. 
        /// All other columns and rows are ignored.
        /// </summary>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        object ExecuteScalar();

        /// <summary>
        /// Executes <see cref="CommandText"/> with <see cref="DbDataAdapter.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dataTable">The name of the <see cref="DataTable"/> to use for table mapping.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>. 
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        int Fill(DataTable dataTable);

        /// <summary>
        /// Creates and returns a <see cref="DbParameter"/> object with a name and value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <returns>A <see cref="DbParameter"/> object.</returns>
        DbParameter GetParameter(string parameterName, object value);

        /// <summary>
        /// Creates and returns an array of <see cref="DbParameter"/> objects with values.
        /// </summary>
        /// <param name="values">Values to create the parameter array.</param>
        /// <returns>An array of <see cref="DbParameter"/> objects.</returns>
        DbParameter[] GetParameterArray(params object[] values);

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
        DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs);

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
        void Initialize(string connectionString);

        /// <summary>
        /// Executes <see cref="CommandText"/> with <see cref="DbDataAdapter.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        int Update(DataTable dataTable);
    }
}
