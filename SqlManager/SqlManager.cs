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
            this.ModelsXmlSerializer = new XmlSerializer(typeof(Models));
            this.StandaloneQueriesXmlSerializer = new XmlSerializer(typeof(StandaloneQueries));
            foreach (string path in Initializer.Instance.ModelRegistration)
            {
                using (Stream stream = new FileStream(path, FileMode.Open))
                {
                    using (XmlReader reader = new XmlTextReader(stream))
                    {
                        if (this.ModelsXmlSerializer.CanDeserialize(reader))
                        {
                            var models = this.ModelsXmlSerializer.Deserialize(reader) as Models;
                            if (models != null)
                            {
                                this.Models = models.ToDictionary();
                            }
                        }
                        else if (this.StandaloneQueriesXmlSerializer.CanDeserialize(reader))
                        {
                            var standaloneQueries = this.StandaloneQueriesXmlSerializer.Deserialize(reader) as StandaloneQueries;

                            if (standaloneQueries != null)
                            {
                                this.StandaloneQueries = standaloneQueries.ToDictionary();
                            }
                        }
                    }
                }
            }
        }

        private XmlSerializer ModelsXmlSerializer { get; set; }

        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        public static SqlManager Instance { get; private set; }

        public Dictionary<string, Model> Models { get; private set; }

        public Dictionary<string, Sql> StandaloneQueries { get; private set; }

        public IDbProvider DbProvider { get; set; }

        public SqlResult GetStandaloneQueryResult(string key)
        {
            return GetStandaloneQueryResult(key, null);
        }

        public SqlResult GetStandaloneQueryResult(string key, Array parameters)
        {
            SqlResult result = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.GetResult(this.DbProvider, parameters);
            }

            return result;
        }

        public DbDataReader GetStandaloneQueryReader(string key)
        {
            return GetStandaloneQueryReader(key, null);
        }

        public DbDataReader GetStandaloneQueryReader(string key, Array parameters)
        {
            DbDataReader reader = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                reader = sql.GetReader(this.DbProvider, parameters);
            }

            return reader;
        }
    }
}
