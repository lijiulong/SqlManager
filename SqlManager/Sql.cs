using System;
using System.Collections.Generic;
using System.Text;

namespace Franksoft.SqlManager
{
    [Serializable]
    public class Sql
    {
        public string Key { get; set; }

        public string Command { get; set; }
    }
}
