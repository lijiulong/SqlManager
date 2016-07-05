using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    [XmlRoot("Models")]
    public class Models : List<Model>
    {
        public Dictionary<string, Model> ToDictionary()
        {
            var dictionary = new Dictionary<string, Model>();
            foreach (Model model in this)
            {
                dictionary[model.Name] = model;
            }

            return dictionary;
        }
    }
}
