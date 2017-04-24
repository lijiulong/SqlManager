using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("StandaloneQueries")]
    public class StandaloneQueries : List<Sql>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreDuplicateKeys"></param>
        /// <returns></returns>
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
