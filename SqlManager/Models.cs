using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager
{
    [Serializable]
    [XmlRoot("Models")]
    public class Models : List<Model>
    {
    }
}
