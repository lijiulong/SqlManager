using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// A wrapper class for <see cref="Sql"/> class provided by SqlManager, with additional events before and after
    /// each call of query methods inside the class.
    /// </summary>
    /// <remarks>
    /// As this wrapper class inherits from <see cref="Sql"/> class, it's easy to get or change any information about
    /// the sql command to be executed. Thus, all events in this wrapper class will only use normal event handler
    /// instead of sql related event handler.
    /// </remarks>
    public class SqlDiagnosticWrapper : Sql
    {
        #region Diagnostic Events

        /// <summary>
        /// Occurs when method <see cref="ExecuteNonQuery(IDbProvider)"/> or
        /// <see cref="ExecuteNonQuery(IDbProvider, DbParameter[])"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterExecuteNonQuery;

        /// <summary>
        /// Occurs when method <see cref="ExecuteScalar(IDbProvider)"/> or
        /// <see cref="ExecuteScalar(IDbProvider, DbParameter[])"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterExecuteScalar;

        /// <summary>
        /// Occurs when method <see cref="Fill(IDbProvider)"/> or
        /// <see cref="Fill(IDbProvider, DbParameter[])"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterFill;

        /// <summary>
        /// Occurs when method <see cref="Update(IDbProvider, DataTable)"/> or
        /// <see cref="Update(IDbProvider, DataTable, DbParameter[])"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterUpdate;

        /// <summary>
        /// Occurs when method <see cref="GetReader(IDbProvider)"/> or
        /// <see cref="GetReader(IDbProvider, DbParameter[])"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterGetReader;

        /// <summary>
        /// Occurs when method <see cref="Clone()"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterClone;

        /// <summary>
        /// Occurs when method <see cref="CopyValueTo(Sql)"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterCopyValueTo;

        /// <summary>
        /// Occurs when method <see cref="ToString()"/> is executed and value has returned.
        /// </summary>
        public event AfterMethodEventHandler AfterToString;

        /// <summary>
        /// Occurs when method <see cref="ExecuteNonQuery(IDbProvider)"/> or
        /// <see cref="ExecuteNonQuery(IDbProvider, DbParameter[])"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeExecuteNonQuery;

        /// <summary>
        /// Occurs when method <see cref="ExecuteScalar(IDbProvider)"/> or
        /// <see cref="ExecuteScalar(IDbProvider, DbParameter[])"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeExecuteScalar;

        /// <summary>
        /// Occurs when method <see cref="Fill(IDbProvider)"/> or
        /// <see cref="Fill(IDbProvider, DbParameter[])"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeFill;

        /// <summary>
        /// Occurs when method <see cref="Update(IDbProvider, DataTable)"/> or
        /// <see cref="Update(IDbProvider, DataTable, DbParameter[])"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeUpdate;

        /// <summary>
        /// Occurs when method <see cref="GetReader(IDbProvider)"/> or
        /// <see cref="GetReader(IDbProvider, DbParameter[])"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeGetReader;

        /// <summary>
        /// Occurs when method <see cref="Clone()"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeClone;

        /// <summary>
        /// Occurs when method <see cref="CopyValueTo(Sql)"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeCopyValueTo;

        /// <summary>
        /// Occurs when method <see cref="ToString()"/> is called and not executed yet.
        /// </summary>
        public event BeforeMethodEventHandler BeforeToString;

        #endregion

        /// <summary>
        /// Wraps a single <see cref="Sql"/> object and returns a <see cref="SqlDiagnosticWrapper"/> object.
        /// Property values will be copied to the new object.
        /// </summary>
        /// <param name="sql">The <see cref="Sql"/> object to wrap.</param>
        /// <returns><see cref="SqlDiagnosticWrapper"/> object with property values copied from parameter.</returns>
        public static SqlDiagnosticWrapper WrapSql(Sql sql)
        {
            SqlDiagnosticWrapper wrapper = new SqlDiagnosticWrapper();
            sql.CopyValueTo(wrapper);

            return wrapper;
        }

        /// <summary>
        /// Converts entire collection inside <see cref="SqlManager.StandaloneQueries"/>
        /// </summary>
        public static void ConvertEntireCollection()
        {
            var collection = SqlManager.Instance.StandaloneQueries;
            Dictionary<string, Sql> temp = new Dictionary<string, Sql>();

            foreach (KeyValuePair<string, Sql> keyValuePair in collection)
            {
                temp[keyValuePair.Key] = WrapSql(keyValuePair.Value);
            }

            foreach (KeyValuePair<string, Sql> keyValuePair in temp)
            {
                collection[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object as nonquery sql against the <see cref="IDbProvider"/>
        /// parameter, triggers <see cref="BeforeExecuteNonQuery"/> before this call,
        /// and <see cref="AfterExecuteNonQuery"/> after this call. Returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery(IDbProvider dbProvider)
        {
            BeforeEventArgs be = new BeforeEventArgs("ExecuteNonQuery");
            be.MethodParameters = new ArrayList() { dbProvider };
            BeforeExecuteNonQuery?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                IDbProvider paramter = (IDbProvider)be.MethodParameters[0];
                result = base.ExecuteNonQuery(paramter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("ExecuteNonQuery", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterExecuteNonQuery?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object as nonquery sql with parameters against the 
        /// <see cref="IDbProvider"/> parameter, triggers <see cref="BeforeExecuteNonQuery"/> before this call,
        /// and <see cref="AfterExecuteNonQuery"/> after this call. Returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery(IDbProvider dbProvider, DbParameter[] parameters)
        {
            BeforeEventArgs be = new BeforeEventArgs("ExecuteNonQuery");
            be.MethodParameters = new ArrayList() { dbProvider, parameters };
            BeforeExecuteNonQuery?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DbParameter[] paramter2 = (DbParameter[])be.MethodParameters[1];
                result = base.ExecuteNonQuery(paramter1, paramter2);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("ExecuteNonQuery", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterExecuteNonQuery?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter, triggers
        /// <see cref="BeforeExecuteScalar"/> before this call, and <see cref="AfterExecuteScalar"/> after this call.
        /// Returns the first column of the first row in the result set returned by the query. All other columns and
        /// rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public override object ExecuteScalar(IDbProvider dbProvider)
        {
            BeforeEventArgs be = new BeforeEventArgs("ExecuteScalar");
            be.MethodParameters = new ArrayList() { dbProvider };
            BeforeExecuteScalar?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            object result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter = (IDbProvider)be.MethodParameters[0];
                result = base.ExecuteScalar(paramter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("ExecuteScalar", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterExecuteScalar?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/>
        /// parameter, triggers <see cref="BeforeExecuteScalar"/> before this call,
        /// and <see cref="AfterExecuteScalar"/> after this call. Returns the first column of the first row in the
        /// result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public override object ExecuteScalar(IDbProvider dbProvider, DbParameter[] parameters)
        {
            BeforeEventArgs be = new BeforeEventArgs("ExecuteScalar");
            be.MethodParameters = new ArrayList() { dbProvider, parameters };
            BeforeExecuteScalar?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            object result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DbParameter[] paramter2 = (DbParameter[])be.MethodParameters[1];
                result = base.ExecuteScalar(paramter1, paramter2);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("ExecuteScalar", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterExecuteScalar?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Fill(DataTable)"/>, triggers <see cref="BeforeFill"/> before this call,
        /// and <see cref="AfterFill"/> after this call.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public override DataTable Fill(IDbProvider dbProvider)
        {
            BeforeEventArgs be = new BeforeEventArgs("Fill");
            be.MethodParameters = new ArrayList() { dbProvider };
            BeforeFill?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DataTable result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter = (IDbProvider)be.MethodParameters[0];
                result = base.Fill(paramter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("Fill", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterFill?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/> 
        /// parameter via <see cref="IDbProvider.Fill(DataTable)"/>, triggers <see cref="BeforeFill"/> before this 
        /// call, and <see cref="AfterFill"/> after this call.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public override DataTable Fill(IDbProvider dbProvider, DbParameter[] parameters)
        {
            BeforeEventArgs be = new BeforeEventArgs("Fill");
            be.MethodParameters = new ArrayList() { dbProvider, parameters };
            BeforeFill?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DataTable result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DbParameter[] paramter2 = (DbParameter[])be.MethodParameters[1];
                result = base.Fill(paramter1, paramter2);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("Fill", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterFill?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Update(DataTable)"/>, triggers <see cref="BeforeUpdate"/> before this call,
        /// and <see cref="AfterUpdate"/> after this call.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public override int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            BeforeEventArgs be = new BeforeEventArgs("Update");
            be.MethodParameters = new ArrayList() { dbProvider, dataTable };
            BeforeUpdate?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DataTable paramter2 = (DataTable)be.MethodParameters[0];
                result = base.Update(paramter1, paramter2);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("Update", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterUpdate?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/>
        /// parameter via <see cref="IDbProvider.Update(DataTable)"/>, triggers <see cref="BeforeUpdate"/> before this
        /// call, and <see cref="AfterUpdate"/> after this call.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public override int Update(IDbProvider dbProvider, DataTable dataTable, DbParameter[] parameters)
        {
            BeforeEventArgs be = new BeforeEventArgs("Update");
            be.MethodParameters = new ArrayList() { dbProvider, dataTable, parameters };
            BeforeUpdate?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            int result = -1;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DataTable paramter2 = (DataTable)be.MethodParameters[1];
                DbParameter[] paramter3 = (DbParameter[])be.MethodParameters[2];
                result = base.Update(paramter1, paramter2, paramter3);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("Update", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterUpdate?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter, triggers
        /// <see cref="BeforeGetReader"/> before this call, and <see cref="AfterGetReader"/> after this call.
        /// Returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public override DbDataReader GetReader(IDbProvider dbProvider)
        {
            BeforeEventArgs be = new BeforeEventArgs("GetReader");
            be.MethodParameters = new ArrayList() { dbProvider };
            BeforeGetReader?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbDataReader result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter = (IDbProvider)be.MethodParameters[0];
                result = base.GetReader(paramter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("GetReader", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterGetReader?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Executes the wrapped <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter, triggers
        /// <see cref="BeforeGetReader"/> before this call, and <see cref="AfterGetReader"/> after this call.
        /// Returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public override DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters)
        {
            BeforeEventArgs be = new BeforeEventArgs("GetReader");
            be.MethodParameters = new ArrayList() { dbProvider, parameters };
            BeforeGetReader?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            DbDataReader result = null;

            if (!be.Cancel)
            {
                IDbProvider paramter1 = (IDbProvider)be.MethodParameters[0];
                DbParameter[] paramter2 = (DbParameter[])be.MethodParameters[1];
                result = base.GetReader(paramter1, paramter2);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("GetReader", ArrayList.ReadOnly(be.MethodParameters),
                outputValues, executedTime, be.Cancel);
            AfterGetReader?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the the wrapped <see cref="Sql"/> object.
        /// Triggers <see cref="BeforeClone"/> before this call, and <see cref="AfterClone"/> after this call.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            BeforeEventArgs be = new BeforeEventArgs("Clone");
            be.MethodParameters = null;
            BeforeClone?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            object result = null;

            if (!be.Cancel)
            {
                SqlDiagnosticWrapper cloneResult = new SqlDiagnosticWrapper();
                this.CopyValueTo(cloneResult);

                result = cloneResult;
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("Clone", null, outputValues, executedTime, be.Cancel);
            AfterClone?.Invoke(this, ae);

            return result;
        }

        /// <summary>
        /// Copies value including all child <see cref="SqlClause"/> items from current wrapped <see cref="Sql"/>
        /// object to target object.
        /// Triggers <see cref="BeforeCopyValueTo"/> before this call, and <see cref="AfterCopyValueTo"/> after this call.
        /// </summary>
        /// <param name="target">The target object to copy value to.</param>
        public override void CopyValueTo(Sql target)
        {
            BeforeEventArgs be = new BeforeEventArgs("CopyValueTo");
            be.MethodParameters = new ArrayList() { target };
            BeforeCopyValueTo?.Invoke(this, be);
            int startTime = Environment.TickCount;

            if (!be.Cancel)
            {
                Sql parameter = (Sql)be.MethodParameters[0];
                base.CopyValueTo(parameter);
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            AfterEventArgs ae = new AfterEventArgs("CopyValueTo", ArrayList.ReadOnly(be.MethodParameters),
                null, executedTime, be.Cancel);
            AfterCopyValueTo?.Invoke(this, ae);
        }

        /// <summary>
        /// Converts this wrapped <see cref="Sql"/> object to sql command text string.
        /// Triggers <see cref="BeforeToString"/> before this call, and <see cref="AfterToString"/> after this call.
        /// </summary>
        /// <returns>The sql command text string defined by this instance.</returns>
        public override string ToString()
        {
            BeforeEventArgs be = new BeforeEventArgs("ToString");
            be.MethodParameters = null;
            BeforeToString?.Invoke(this, be);
            ArrayList outputValues = new ArrayList();
            int startTime = Environment.TickCount;
            string result = null;

            if (!be.Cancel)
            {
                result = base.ToString();
            }

            int endTime = Environment.TickCount;
            int executedTime = endTime - startTime;
            outputValues.Add(result);
            AfterEventArgs ae = new AfterEventArgs("ToString", null, outputValues, executedTime, be.Cancel);
            AfterToString?.Invoke(this, ae);

            return result;
        }
    }
}
