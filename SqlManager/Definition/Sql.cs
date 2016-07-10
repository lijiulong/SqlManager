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

        public SqlResult GetResult(IDbProvider dbProvider)
        {
            return GetResult(dbProvider, null);
        }

        public SqlResult GetResult(IDbProvider dbProvider, Array parameters)
        {
            SqlResult result = new SqlResult();

            string firstCommandKeyword = this.GetFirstCommandKeyword();
            SqlClause firstChildClause = null;
            if (this.ChildItems != null && this.ChildItems.Count > 0)
            {
                firstChildClause = this.ChildItems[0];
            }

            if (firstChildClause != null)
            {
                firstCommandKeyword = firstChildClause.Keyword.ToString();
            }

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;

            switch (firstCommandKeyword)
            {
                case "SELECT":
                    result.DataTableResult = new DataTable();
                    dbProvider.Fill(result.DataTableResult);
                    break;
                case "INSERT":
                case "INSERTINTO":
                    result.SingleResult = dbProvider.ExecuteScalar();
                    break;
                case "DELETE":
                case "DELETEFROM":
                case "UPDATE":
                case "BEGIN":
                default:
                    result.ReturnResult = dbProvider.ExecuteNonQuery();
                    break;
            }

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
