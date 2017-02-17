using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Franksoft.SqlManager.DbProviders
{
    public abstract class BaseDbProvider : IDbProvider
    {
        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public string ConnectionString { get; set; }

        public Array Parameters { get; set; }

        public abstract DbTransaction BeginTransaction();

        public abstract DbTransaction BeginTransaction(IsolationLevel il);

        public abstract void Dispose();

        public abstract int ExecuteNonQuery();

        public abstract DbDataReader ExecuteReader();

        public abstract DbDataReader ExecuteReader(CommandBehavior behavior);

        public abstract object ExecuteScalar();

        public abstract int Fill(DataTable dataTable);

        public abstract DbParameter GetParameter(string parameterName, object value);

        public DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs)
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

        public DbParameter[] GetParameterArray(params object[] values)
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

        public abstract void Initialize(string connectionString);

        public abstract int Update(DataTable dataTable);
    }
}
