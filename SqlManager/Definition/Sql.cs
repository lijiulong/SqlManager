using System;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    public class Sql : SqlClause
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string Command { get; set; }

        [XmlAttribute]
        public CommandType CommandType { get; set; }

        public int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        public int ExecuteNonQuery(IDbProvider dbProvider, DbParameter[] parameters)
        {
            int result = -1;
            
            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteNonQuery();

            return result;
        }

        public object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteScalar(dbProvider, null);
        }

        public object ExecuteScalar(IDbProvider dbProvider, DbParameter[] parameters)
        {
            object result = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteScalar();

            return result;
        }

        public DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        public DataTable Fill(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DataTable result = new DataTable();
            
            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            dbProvider.Fill(result);

            return result;
        }

        public int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, null);
        }

        public int Update(IDbProvider dbProvider, DataTable dataTable, DbParameter[] parameters)
        {
            int result = -1;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.Update(dataTable);

            return result;
        }

        public DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        public DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DbDataReader reader = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            reader = dbProvider.ExecuteReader();

            return reader;
        }

        public override string ToString()
        {
            string result = base.ToString();

            if (string.IsNullOrEmpty(result))
            {
                result = this.Command;
            }

            return result;
        }
    }
}
