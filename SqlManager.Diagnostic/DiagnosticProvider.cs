using System;
using System.Collections;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.DbProviders;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// A wrapper class for all kinds of <see cref="IDbProvider"/> classes provided by SqlManager,
    /// with additional events before and after each call of query methods inside the class.
    /// </summary>
    public class DiagnosticProvider : BaseDbProvider
    {
        /// <summary>
        /// Initializes <see cref="DiagnosticProvider"/> instance with specific <see cref="IDbProvider"/> instance.
        /// </summary>
        /// <param name="dbProvider">
        /// <see cref="IDbProvider"/> instance used to initialize <see cref="DiagnosticProvider"/> instance.
        /// </param>
        public DiagnosticProvider(IDbProvider dbProvider)
        {
            this.DbProvider = dbProvider;
        }

        /// <summary>
        /// Gets the <see cref="DbDataAdapter"/> object inside this <see cref="DiagnosticProvider"/>.
        /// </summary>
        public override DbDataAdapter Adapter
        {
            get
            {
                return this.DbProvider.Adapter;
            }
        }

        /// <summary>
        /// Gets the <see cref="DbCommand"/> object inside this <see cref="DiagnosticProvider"/>.
        /// </summary>
        public override DbCommand Command
        {
            get
            {
                return this.DbProvider.Command;
            }
        }

        /// <summary>
        /// Gets the <see cref="DbConnection"/> object inside this <see cref="DiagnosticProvider"/>.
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                return this.DbProvider.Connection;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDbProvider"/> instance inside this <see cref="DiagnosticProvider"/>.
        /// </summary>
        public IDbProvider DbProvider { get; set; }

        #region Diagnostic Events

        /// <summary>
        /// Occurs when method <see cref="BeginTransaction()"/> or <see cref="BeginTransaction(IsolationLevel)"/>
        /// is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterBeginTransaction;

        /// <summary>
        /// Occurs when method <see cref="Dispose()"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterDispose;

        /// <summary>
        /// Occurs when method <see cref="ExecuteNonQuery()"/> is executed and value has returned.
        /// </summary>
        public event AfterSqlEventHandler AfterExecuteNonQuery;

        /// <summary>
        /// Occurs when method <see cref="ExecuteReader()"/> or <see cref="ExecuteReader(CommandBehavior)"/>
        /// is executed and value has returned.
        /// </summary>
        public event AfterSqlEventHandler AfterExecuteReader;

        /// <summary>
        /// Occurs when method <see cref="ExecuteScalar()"/> is executed and value has returned.
        /// </summary>
        public event AfterSqlEventHandler AfterExecuteScalar;

        /// <summary>
        /// Occurs when method <see cref="Fill(DataTable)"/> is executed and value has returned.
        /// </summary>
        public event AfterSqlEventHandler AfterFill;

        /// <summary>
        /// Occurs when method <see cref="Initialize(string)"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterInitialize;

        /// <summary>
        /// Occurs when method <see cref="Update(DataTable)"/> is executed and value has returned.
        /// </summary>
        public event AfterSqlEventHandler AfterUpdate;

        /// <summary>
        /// Occurs when method <see cref="BeginTransaction()"/> or <see cref="BeginTransaction(IsolationLevel)"/>
        /// is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeBeginTransaction;

        /// <summary>
        /// Occurs when method <see cref="Dispose()"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeDispose;

        /// <summary>
        /// Occurs when method <see cref="ExecuteNonQuery()"/> is called and not executed yet.
        /// </summary>
        public event BeforeSqlEventHandler BeforeExecuteNonQuery;

        /// <summary>
        /// Occurs when method <see cref="ExecuteReader()"/> or <see cref="ExecuteReader(CommandBehavior)"/>
        /// is called and not executed yet.
        /// </summary>
        public event BeforeSqlEventHandler BeforeExecuteReader;

        /// <summary>
        /// Occurs when method <see cref="ExecuteScalar()"/> is called and not executed yet.
        /// </summary>
        public event BeforeSqlEventHandler BeforeExecuteScalar;

        /// <summary>
        /// Occurs when method <see cref="Fill(DataTable)"/> is called and not executed yet.
        /// </summary>
        public event BeforeSqlEventHandler BeforeFill;

        /// <summary>
        /// Occurs when method <see cref="Initialize(string)"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeInitialize;

        /// <summary>
        /// Occurs when method <see cref="Update(DataTable)"/> is called and not executed yet.
        /// </summary>
        public event BeforeSqlEventHandler BeforeUpdate;

        #endregion

        /// <summary>
        /// Starts a database transaction, triggers <see cref="BeforeBeginTransaction"/> before this call,
        /// and <see cref="AfterBeginTransaction"/> after this call.
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public override DbTransaction BeginTransaction()
        {
            BeforeEventArgs be = new BeforeEventArgs("BeginTransaction");
            be.MethodParameters = null;
            BeforeBeginTransaction?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbTransaction result = null;

            if (!be.Cancel)
            {
                result = this.DbProvider.BeginTransaction();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("BeginTransaction", null, outputValues, executedTime, be.Cancel);
            AfterBeginTransaction?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Starts a database transaction with the specified isolation level,
        /// triggers <see cref="BeforeBeginTransaction"/> before this call,
        /// and <see cref="AfterBeginTransaction"/> after this call.
        /// </summary>
        /// <param name="il">Specifies the isolation level for the transaction.</param>
        /// <returns>An object representing the new transaction.</returns>
        public override DbTransaction BeginTransaction(IsolationLevel il)
        {
            BeforeEventArgs be = new BeforeEventArgs("BeginTransaction");
            be.MethodParameters = new ArrayList() { il };
            BeforeBeginTransaction?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbTransaction result = null;

            if (!be.Cancel)
            {
                IsolationLevel parameter = (IsolationLevel)be.MethodParameters[0];
                result = this.DbProvider.BeginTransaction(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("BeginTransaction", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterBeginTransaction?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Aids implementation of the <see cref="IDisposable"/> interface.
        /// Properties will be disposed and <see cref="GC.Collect()"/> will be executed.
        /// Triggers <see cref="BeforeDispose"/> before this call, and <see cref="AfterDispose"/> after this call.
        /// </summary>
        public override void Dispose()
        {
            BeforeEventArgs be = new BeforeEventArgs("Dispose");
            be.MethodParameters = null;
            BeforeDispose?.Invoke(this, be);
            int startTime = Environment.TickCount;

            if (!be.Cancel)
            {
                this.DbProvider.Dispose();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            AfterEventArgs ae = new AfterEventArgs("Dispose", null, null, executedTime, be.Cancel);
            AfterDispose?.Invoke(this, ae);
        }

        /// <summary>
        /// Executes the <see cref="IDbProvider.CommandText"/> against the <see cref="Connection"/> object.
        /// Triggers <see cref="BeforeExecuteNonQuery"/> before this call,
        /// and <see cref="AfterExecuteNonQuery"/> after this call.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery()
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "ExecuteNonQuery");
            be.MethodParameters = null;
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeExecuteNonQuery?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.ExecuteNonQuery();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "ExecuteNonQuery", null, outputValues, executedTime, be.Cancel);
            AfterExecuteNonQuery?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the <see cref="IDbProvider.CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance.
        /// Triggers <see cref="BeforeExecuteReader"/> before this call,
        /// and <see cref="AfterExecuteReader"/> after this call.
        /// </summary>
        /// <returns>
        /// A <see cref="DbDataReader"/> instance of the executed <see cref="IDbProvider.CommandText"/>.
        /// </returns>
        public override DbDataReader ExecuteReader()
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "ExecuteReader");
            be.MethodParameters = null;
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeExecuteReader?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbDataReader result = null;

            if (!be.Cancel)
            {
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.ExecuteReader();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "ExecuteReader", null, outputValues, executedTime, be.Cancel);
            AfterExecuteReader?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the <see cref="IDbProvider.CommandText"/> against the <see cref="Connection"/> object, 
        /// returns a <see cref="DbDataReader"/> instance using one of the <see cref="CommandBehavior"/>.
        /// Triggers <see cref="BeforeExecuteReader"/> before this call,
        /// and <see cref="AfterExecuteReader"/> after this call.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>
        /// A <see cref="DbDataReader"/> instance of the executed <see cref="IDbProvider.CommandText"/>.
        /// </returns>
        public override DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "ExecuteReader");
            be.MethodParameters = new ArrayList() { behavior };
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeExecuteReader?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbDataReader result = null;

            if (!be.Cancel)
            {
                CommandBehavior parameter = (CommandBehavior)be.MethodParameters[0];
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.ExecuteReader(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "ExecuteReader", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterExecuteReader?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. 
        /// All other columns and rows are ignored.
        /// Triggers <see cref="BeforeExecuteScalar"/> before this call,
        /// and <see cref="AfterExecuteScalar"/> after this call.
        /// </summary>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public override object ExecuteScalar()
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "ExecuteScalar");
            be.MethodParameters = null;
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeExecuteScalar?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            object result = null;

            if (!be.Cancel)
            {
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.ExecuteScalar();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "ExecuteScalar", null, outputValues, executedTime, be.Cancel);
            AfterExecuteScalar?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes <see cref="IDbProvider.CommandText"/> with <see cref="DbDataAdapter.Fill(DataTable)"/>.
        /// Triggers <see cref="BeforeFill"/> before this call, and <see cref="AfterFill"/> after this call.
        /// </summary>
        /// <param name="dataTable">The name of the <see cref="DataTable"/> to use for table mapping.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>. 
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public override int Fill(DataTable dataTable)
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "Fill");
            be.MethodParameters = new ArrayList() { dataTable };
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeFill?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                DataTable parameter = (DataTable)be.MethodParameters[0];
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.Fill(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "Fill", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterFill?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Initializes <see cref="IDbProvider"/> instance with the specified connection string.
        /// Triggers <see cref="BeforeInitialize"/> before this call,
        /// and <see cref="AfterInitialize"/> after this call.
        /// <para>
        /// This method is designed to support reusing a provider instance among different connection strings. 
        /// The idea is to avoid creating new instance, but only to execute this method when changing connection
        /// string. It is necessary to initialize <see cref="Adapter"/>, <see cref="Command"/> and 
        /// <see cref="Connection"/> properties here.
        /// </para>
        /// </summary>
        /// <param name="connectionString">Connection string for this <see cref="IDbProvider"/> instance.</param>
        public override void Initialize(string connectionString)
        {
            BeforeEventArgs be = new BeforeEventArgs("Initialize");
            be.MethodParameters = new ArrayList() { connectionString };
            BeforeInitialize?.Invoke(this, be);
            int startTime = Environment.TickCount;

            if (!be.Cancel)
            {
                string parameter = (string)be.MethodParameters[0];
                this.DbProvider.Initialize(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            AfterEventArgs ae = new AfterEventArgs("Initialize", ArrayList.ReadOnly(be.MethodParameters),
                null, executedTime, be.Cancel);
            AfterInitialize?.Invoke(this, ae);
        }

        /// <summary>
        /// Executes <see cref="IDbProvider.CommandText"/> with <see cref="DbDataAdapter.Update(DataTable)"/>.
        /// Triggers <see cref="BeforeUpdate"/> before this call, and <see cref="AfterUpdate"/> after this call.
        /// </summary>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public override int Update(DataTable dataTable)
        {
            BeforeSqlEventArgs be = new BeforeSqlEventArgs(DiagnosticHelper.ParseSqlKeywords(this.CommandText), "Update");
            be.MethodParameters = new ArrayList() { dataTable };
            be.SqlCommandText = this.CommandText;
            be.SqlCommandType = this.CommandType;
            be.SqlParameters = this.Parameters;
            BeforeUpdate?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                DataTable parameter = (DataTable)be.MethodParameters[0];
                this.DbProvider.CommandText = be.SqlCommandText;
                this.DbProvider.CommandType = be.SqlCommandType;
                this.DbProvider.Parameters = be.SqlParameters;
                result = this.DbProvider.Update(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterSqlEventArgs ae = new AfterSqlEventArgs(be.SqlCommandText, DiagnosticHelper.ParseSqlKeywords(be.SqlCommandText),
                be.SqlCommandType, be.SqlParameters, "Update", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterUpdate?.Invoke(this, ae);

            return result;
        }
    }
}
