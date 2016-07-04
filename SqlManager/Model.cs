using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager
{
    [Serializable]
    public class Model
    {
        private Dictionary<string, string> predefinedSqlDictionary;

        public string Name { get; set; }

        public bool IsReadOnly { get; set; }

        public Fields Fields { get; set; }

        [XmlArray("PredefinedSqls")]
        public PredefinedSqls PredefinedSqls { get; set; }

        [XmlIgnore]
        public Dictionary<string, string> PredefinedSqlDictionary
        {
            get
            {
                if (this.predefinedSqlDictionary == null && this.PredefinedSqls != null)
                {
                    this.predefinedSqlDictionary = new Dictionary<string, string>();
                    foreach (Sql sql in this.PredefinedSqls)
                    {
                        this.predefinedSqlDictionary[sql.Key] = sql.Command;
                    }
                }

                return this.predefinedSqlDictionary;
            }
        }

        public Model()
        {
            this.predefinedSqlDictionary = null;
        }
    }
}
