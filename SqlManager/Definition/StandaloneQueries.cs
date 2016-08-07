using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    [XmlRoot("StandaloneQueries")]
    public class StandaloneQueries : List<Sql>
    {
        public Dictionary<string, Sql> ToDictionary(bool ignoreDuplicateKeys)
        {
            var dictionary = new Dictionary<string, Sql>();
            foreach (Sql sql in this)
            {
                if (!ignoreDuplicateKeys && dictionary.ContainsKey(sql.Key))
                {
                    throw new KeyDuplicateException(sql.Key);
                }

                dictionary[sql.Key] = sql;
            }

            return dictionary;
        }
    }
}
