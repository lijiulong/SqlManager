using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders.Mock
{
    public class MockProvider : IDbProvider
    {
        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public string ConnectionString { get; set; }

        public Array Parameters { get; set; }

        public DbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public DbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public int Fill(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public DbParameter GetParameter(string parameterName, object value)
        {
            throw new NotImplementedException();
        }

        public DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs)
        {
            throw new NotImplementedException();
        }

        public DbParameter[] GetParameterArray(params object[] values)
        {
            throw new NotImplementedException();
        }

        public int Update(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
