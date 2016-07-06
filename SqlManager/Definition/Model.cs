using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    public class Model
    {
        public Model()
        {
            this.predefinedSqlDictionary = null;
        }

        private Dictionary<string, Sql> predefinedSqlDictionary;

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsReadOnly { get; set; }

        [XmlArray("Fields")]
        public Fields Fields { get; set; }

        [XmlArray("PredefinedSqls")]
        public PredefinedSqls PredefinedSqls { get; set; }

        [XmlIgnore]
        public Dictionary<string, Sql> PredefinedSqlDictionary
        {
            get
            {
                if (this.predefinedSqlDictionary == null && this.PredefinedSqls != null)
                {
                    this.predefinedSqlDictionary = new Dictionary<string, Sql>();
                    foreach (Sql sql in this.PredefinedSqls)
                    {
                        this.predefinedSqlDictionary[sql.Key] = sql;
                    }
                }

                return this.predefinedSqlDictionary;
            }
        }
    }
}
