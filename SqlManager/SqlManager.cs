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
            Models models = new Models();
            StandaloneQueries standaloneQueries = new StandaloneQueries();

            this.Models = new Dictionary<string, Model>();
            this.StandaloneQueries = new Dictionary<string, Sql>();
            this.ModelsXmlSerializer = new XmlSerializer(typeof(Models));
            this.StandaloneQueriesXmlSerializer = new XmlSerializer(typeof(StandaloneQueries));

            foreach (string path in Initializer.Instance.ModelRegistration)
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (XmlReader reader = new XmlTextReader(stream))
                    {
                        if (this.ModelsXmlSerializer.CanDeserialize(reader))
                        {
                            var model = this.ModelsXmlSerializer.Deserialize(reader) as Models;
                            if (model != null)
                            {
                                models.AddRange(model);
                            }
                        }
                        else if (this.StandaloneQueriesXmlSerializer.CanDeserialize(reader))
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

            this.Models = models.ToDictionary(Initializer.Instance.IgnoreDuplicateKeys);
            this.StandaloneQueries = standaloneQueries.ToDictionary(Initializer.Instance.IgnoreDuplicateKeys);
        }

        private XmlSerializer ModelsXmlSerializer { get; set; }

        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        public static SqlManager Instance { get; private set; }

        public Dictionary<string, Model> Models { get; private set; }

        public Dictionary<string, Sql> StandaloneQueries { get; private set; }

        public IDbProvider DbProvider { get; set; }

        public DbDataReader GetStandaloneQueryReader(string key)
        {
            return this.GetStandaloneQueryReader(key, null);
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

        public int ExecuteStandaloneNonQuery(string key)
        {
            return this.ExecuteStandaloneNonQuery(key, null);
        }

        public int ExecuteStandaloneNonQuery(string key, Array parameters)
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

        public object ExecuteStandaloneScalar(string key, Array parameters)
        {
            object result = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.ExecuteScalar(this.DbProvider, parameters);
            }

            return result;
        }
    }
}
