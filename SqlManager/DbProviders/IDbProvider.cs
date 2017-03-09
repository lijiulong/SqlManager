using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders
{
    public interface IDbProvider : IDisposable
    {
        string CommandText { get; set; }

        CommandType CommandType { get; set; }

        string ConnectionString { get; }

        DbParameter[] Parameters { get; set; }

        DbTransaction BeginTransaction();

        DbTransaction BeginTransaction(IsolationLevel il);

        int ExecuteNonQuery();

        DbDataReader ExecuteReader();

        DbDataReader ExecuteReader(CommandBehavior behavior);

        object ExecuteScalar();

        int Fill(DataTable dataTable);

        DbParameter GetParameter(string parameterName, object value);

        DbParameter[] GetParameterArray(params object[] values);

        DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs);

        void Initialize(string connectionString);

        int Update(DataTable dataTable);
    }
}
