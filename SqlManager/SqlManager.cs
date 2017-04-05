using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager
{
    public sealed class SqlManager
    {
        static SqlManager()
        {
            if (Instance == null)
            {
                Instance = new SqlManager();
            }
        }

        private SqlManager()
        {
            StandaloneQueries standaloneQueries = new StandaloneQueries();
            this.StandaloneQueries = new Dictionary<string, Sql>();
            this.StandaloneQueriesXmlSerializer = new XmlSerializer(typeof(StandaloneQueries));

            foreach (string path in Initializer.Instance.ModelRegistration)
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (XmlReader reader = new XmlTextReader(stream))
                    {
                        if (this.StandaloneQueriesXmlSerializer.CanDeserialize(reader))
                        {
                            var queries = this.StandaloneQueriesXmlSerializer.Deserialize(reader) as StandaloneQueries;

                            if (queries != null)
                            {
                                standaloneQueries.AddRange(queries);
                            }
                        }
                    }
                }
            }

            this.StandaloneQueries = standaloneQueries.ToDictionary(Initializer.Instance.IgnoreDuplicateKeys);
        }

        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        public static SqlManager Instance { get; private set; }

        public Dictionary<string, Sql> StandaloneQueries { get; private set; }

        public IDbProvider DbProvider { get; set; }

        public bool IgnoreDuplicateKeys
        {
            get
            {
                return Initializer.Instance.IgnoreDuplicateKeys;
            }
        }

        public string ModelDirectory
        {
            get
            {
                return Initializer.Instance.ModelDirectory;
            }
        }

        public DbDataReader GetStandaloneQueryReader(string key)
        {
            return this.GetStandaloneQueryReader(key, null);
        }

        public DbDataReader GetStandaloneQueryReader(string key, DbParameter[] parameters)
        {
            DbDataReader reader = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                reader = sql.GetReader(this.DbProvider, parameters);
            }

            return reader;
        }

        public int ExecuteStandaloneNonQuery(string key)
        {
            return this.ExecuteStandaloneNonQuery(key, null);
        }

        public int ExecuteStandaloneNonQuery(string key, DbParameter[] parameters)
        {
            int result = -1;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.ExecuteNonQuery(this.DbProvider, parameters);
            }

            return result;
        }

        public object ExecuteStandaloneScalar(string key)
        {
            return this.ExecuteStandaloneScalar(key, null);
        }

        public object ExecuteStandaloneScalar(string key, DbParameter[] parameters)
        {
            object result = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.ExecuteScalar(this.DbProvider, parameters);
            }

            return result;
        }

        public string FindKey(string sqlCommandText)
        {
            string key = null;

            foreach (KeyValuePair<string, Sql> keyValuePair in this.StandaloneQueries)
            {
                string sqlText = keyValuePair.Value.ToString();
                if (string.Equals(sqlCommandText, sqlText, StringComparison.InvariantCultureIgnoreCase))
                {
                    key = keyValuePair.Key;
                    break;
                }
            }

            return key;
        }

        public string ProcessRelativePath(string relativePath)
        {
            return Initializer.Instance.ProcessRelativePath(relativePath);
        }
    }
}
