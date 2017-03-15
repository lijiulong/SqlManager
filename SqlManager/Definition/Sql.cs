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

        public static void CopyValueTo(Sql source, Sql target)
        {
            target.Key = source.Key;
            target.Command = source.Command;
            target.CommandType = source.CommandType;

            source.CopyValueTo((SqlClause)target);
        }

        public virtual int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        public virtual int ExecuteNonQuery(IDbProvider dbProvider, DbParameter[] parameters)
        {
            int result = -1;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteNonQuery();

            return result;
        }

        public virtual object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteScalar(dbProvider, null);
        }

        public virtual object ExecuteScalar(IDbProvider dbProvider, DbParameter[] parameters)
        {
            object result = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteScalar();

            return result;
        }

        public virtual DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        public virtual DataTable Fill(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DataTable result = new DataTable();

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            dbProvider.Fill(result);

            return result;
        }

        public virtual int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, null);
        }

        public virtual int Update(IDbProvider dbProvider, DataTable dataTable, DbParameter[] parameters)
        {
            int result = -1;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.Update(dataTable);

            return result;
        }

        public virtual DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        public virtual DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DbDataReader reader = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            reader = dbProvider.ExecuteReader();

            return reader;
        }

        public override object Clone()
        {
            Sql cloneResult = new Sql();
            this.CopyValueTo(cloneResult);

            return cloneResult;
        }

        public virtual void CopyValueTo(Sql target)
        {
            CopyValueTo(this, target);
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
