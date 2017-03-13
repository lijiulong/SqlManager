using System;
using System.Data.Common;

namespace Franksoft.SqlManager.Definition
{
    public class ExecuteQueryEventArgs : EventArgs
    {
        public Sql SqlToExecute { get; set; }

        public DbParameter[] Parameters { get; set; }

        public bool Cancel { get; set; }
    }
}
