using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Mock.Definition
{
    [Serializable]
    [XmlRoot("StandaloneQueriesMock")]
    public class StandaloneQueriesMock : List<SqlMock>
    {
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
