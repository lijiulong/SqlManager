using System;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders
{
    public interface IDbProvider : IDisposable
    {
        string ConnectionString { get; }

        string CommandText { get; set; }

        CommandType CommandType { get; set; }

        Array Parameters { get; set; }

        DbDataReader ExecuteReader();

        DbDataReader ExecuteReader(CommandBehavior behavior);

        int ExecuteNonQuery();

        object ExecuteScalar();

        int Fill(DataTable dataTable);

        int Update(DataTable dataTable);

        DbTransaction BeginTransaction();

        DbTransaction BeginTransaction(IsolationLevel il);

        DbParameter GetParameter(string parameterName, object value);
    }
}
