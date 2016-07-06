using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    [XmlRoot("StandaloneQueries")]
    public class StandaloneQueries : List<Sql>
    {
        public Dictionary<string, Sql> ToDictionary()
        {
            var dictionary = new Dictionary<string, Sql>();
            foreach (Sql sql in this)
            {
                dictionary[sql.Key] = sql;
            }

            return dictionary;
        }
    }
}
