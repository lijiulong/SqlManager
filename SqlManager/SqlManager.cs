using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.Definition;
using Franksoft.SqlManager.DbProviders;

namespace Franksoft.SqlManager
{
    public class SqlManager
    {
        private XmlSerializer ModelsXmlSerializer { get; set; }

        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        public static SqlManager Instance { get; private set; }

        public Dictionary<string, Model> Models { get; private set; }

        public Dictionary<string, Sql> StandaloneQueries { get; private set; }

        public IDbProvider DbProvider { get; set; }

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
                Stream stream = new FileStream(path, FileMode.Open);
                XmlReader reader = new XmlTextReader(stream);

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

        public void Test()
        {

        }
    }
}
