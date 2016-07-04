using System;
using System.Collections.Generic;
using System.Text;

namespace Franksoft.SqlManager
{
    public class SqlManager
    {
        public static SqlManager Instance { get; private set; }

        static SqlManager()
        {
            if (Instance == null)
            {
                Instance = new SqlManager();
            }
        }

        private SqlManager()
        {
            var a = Initializer.Instance.RegisteredFiles.Count;
        }

        public void Test()
        {

        }
    }
}
