using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    [XmlRoot("Models")]
    public class Models : List<Model>
    {
        public Dictionary<string, Model> ToDictionary(bool ignoreDuplicateKeys)
        {
            var dictionary = new Dictionary<string, Model>();
            foreach (Model model in this)
            {
                if (!ignoreDuplicateKeys && dictionary.ContainsKey(model.Name))
                {
                    throw new KeyDuplicateException(model.Name);
                }

                dictionary[model.Name] = model;
            }

            return dictionary;
        }
    }
}
