using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager
{
    [Serializable]
    [XmlRoot("StandaloneQueries")]
    public class StandaloneQueries : List<Sql>
    {
        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (Sql sql in this)
            {
                dictionary[sql.Key] = sql.Command;
            }

            return dictionary;
        }
    }
}
