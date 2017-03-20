using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    public class DiagnosticProvider : BaseDbProvider
    {
        public DiagnosticProvider(IDbProvider dbProvider)
        {
            this.DbProvider = dbProvider;
        }

        public override DbDataAdapter Adapter
        {
            get
            {
                return this.DbProvider.Adapter;
            }
        }

        public override DbCommand Command
        {
            get
            {
                return this.DbProvider.Command;
            }
        }

        public override DbConnection Connection
        {
            get
            {
                return this.DbProvider.Connection;
            }
        }

        public IDbProvider DbProvider { get; set; }

        #region Diagnostic Events

        public event AfterMethodEventHandler AfterBeginTransaction;

        public event AfterMethodEventHandler AfterDispose;

        public event AfterSqlEventHandler AfterExecuteNonQuery;

        public event AfterSqlEventHandler AfterExecuteReader;

        public event AfterSqlEventHandler AfterExecuteScalar;

        public event AfterSqlEventHandler AfterFill;

        public event AfterMethodEventHandler AfterInitialize;

        public event AfterSqlEventHandler AfterUpdate;

        public event BeforeMethodEventHandler BeforeBeginTransaction;

        public event BeforeMethodEventHandler BeforeDispose;

        public event BeforeSqlEventHandler BeforeExecuteNonQuery;

        public event BeforeSqlEventHandler BeforeExecuteReader;

        public event BeforeSqlEventHandler BeforeExecuteScalar;

        public event BeforeSqlEventHandler BeforeFill;

        public event BeforeMethodEventHandler BeforeInitialize;

        public event BeforeSqlEventHandler BeforeUpdate;

        #endregion

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

        public override void Dispose()
        {
            BeforeEventArgs be = new BeforeEventArgs("Dispose");
            be.MethodParameters = null;
            BeforeBeginTransaction?.Invoke(this, be);
            int startTime = Environment.TickCount;

            if (!be.Cancel)
            {
                this.DbProvider.Dispose();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            AfterEventArgs ae = new AfterEventArgs("Dispose", null, null, executedTime, be.Cancel);
            AfterBeginTransaction?.Invoke(this, ae);
        }

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
