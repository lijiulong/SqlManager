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
            return ExecuteNonQuery(dbProvider, null);
        }

        public int ExecuteNonQuery(IDbProvider dbProvider, Array parameters)
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
            return ExecuteScalar(dbProvider, null);
        }

        public object ExecuteScalar(IDbProvider dbProvider, Array parameters)
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
            return Fill(dbProvider, null);
        }

        public DataTable Fill(IDbProvider dbProvider, Array parameters)
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
            return Update(dbProvider, null);
        }

        public int Update(IDbProvider dbProvider, DataTable dataTable, Array parameters)
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
            return GetReader(dbProvider, null);
        }

        public DbDataReader GetReader(IDbProvider dbProvider, Array parameters)
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

        private string GetFirstCommandKeyword()
        {
            string result = string.Empty;

            string command = this.Command.Trim();
            string[] commandWords = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandWords.Length > 0)
            {
                result = commandWords[0].ToUpper();
            }

            return result;
        }
    }
}
