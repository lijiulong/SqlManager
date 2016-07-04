using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Franksoft.SqlManager
{
    public class SqlManager
    {
        private XmlSerializer ModelsXmlSerializer { get; set; }

        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        public static SqlManager Instance { get; private set; }

        public Dictionary<string, Model> Models { get; private set; }

        public Dictionary<string, string> StandaloneQueries { get; private set; }

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

                if(this.ModelsXmlSerializer.CanDeserialize(reader))
                {
                    var a = this.ModelsXmlSerializer.Deserialize(reader);
                }
                else if (this.StandaloneQueriesXmlSerializer.CanDeserialize(reader))
                {
                    var a = this.StandaloneQueriesXmlSerializer.Deserialize(reader);
                }
            }
        }

        public void Test()
        {

        }
    }
}
