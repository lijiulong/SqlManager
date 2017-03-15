using System;
using System.Collections;

namespace Franksoft.SqlManager.Diagnostic
{
    public class BeforeEventArgs : EventArgs
    {
        public BeforeEventArgs(string methodName)
        {
            this.MethodName = methodName;
        }

        public string MethodName { get; private set; }

        public ArrayList MethodParameters { get; set; }

        public bool Cancel { get; set; }
    }
}
