using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// A class represents a collection of <see cref="Sql"/> definitions.
    /// </summary>
    [Serializable]
    [XmlRoot("StandaloneQueries")]
    public class StandaloneQueries : List<Sql>
    {
        /// <summary>
        /// Converts this collection instance to dictionary.
        /// </summary>
        /// <param name="ignoreDuplicateKeys">
        /// If this parameter is set to false, a <see cref="KeyDuplicateException"/> error will be thrown
        /// when duplicated key is found during the convertion. Otherwise, the last value will replace the old one.
        /// </param>
        /// <exception cref="KeyDuplicateException">
        /// Duplicated key is found during the convertion and the parameter "ignoreDuplicateKeys" is set to false.
        /// </exception>
        /// <returns>The dictionary instance contains key and value.</returns>
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
