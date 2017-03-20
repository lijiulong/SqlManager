using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    public class SqlDiagnosticWrapper : Sql
    {
        #region Diagnostic Events

        public event AfterMethodEventHandler AfterExecuteNonQuery;

        public event AfterMethodEventHandler AfterExecuteScalar;

        public event AfterMethodEventHandler AfterFill;

        public event AfterMethodEventHandler AfterUpdate;

        public event AfterMethodEventHandler AfterGetReader;

        public event AfterMethodEventHandler AfterClone;

        public event AfterMethodEventHandler AfterCopyValueTo;

        public event AfterMethodEventHandler AfterToString;

        public event BeforeMethodEventHandler BeforeExecuteNonQuery;

        public event BeforeMethodEventHandler BeforeExecuteScalar;

        public event BeforeMethodEventHandler BeforeFill;

        public event BeforeMethodEventHandler BeforeUpdate;

        public event BeforeMethodEventHandler BeforeGetReader;

        public event BeforeMethodEventHandler BeforeClone;

        public event BeforeMethodEventHandler BeforeCopyValueTo;

        public event BeforeMethodEventHandler BeforeToString;

        #endregion

        public static SqlDiagnosticWrapper WrapSql(Sql sql)
        {
            SqlDiagnosticWrapper wrapper = new SqlDiagnosticWrapper();
            sql.CopyValueTo(wrapper);

            return wrapper;
        }

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
