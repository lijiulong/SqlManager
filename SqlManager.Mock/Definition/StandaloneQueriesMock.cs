using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Mock.Definition
{
    /// <summary>
    /// A class represents a collection of <see cref="SqlMock"/> definitions.
    /// </summary>
    [Serializable]
    [XmlRoot("StandaloneQueriesMock")]
    public class StandaloneQueriesMock : List<SqlMock>
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
        public Dictionary<string, SqlMock> ToDictionary(bool ignoreDuplicateKeys)
        {
            var dictionary = new Dictionary<string, SqlMock>();
            foreach (SqlMock sqlMock in this)
            {
                if (!ignoreDuplicateKeys && dictionary.ContainsKey(sqlMock.Key))
                {
                    throw new KeyDuplicateException(sqlMock.Key);
                }

                dictionary[sqlMock.Key] = sqlMock;
            }

            return dictionary;
        }
    }
}
