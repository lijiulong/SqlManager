using System;
using System.Collections;

namespace Franksoft.SqlManager.Diagnostic
{
    public class AfterEventArgs : EventArgs
    {
        public AfterEventArgs(string methodName, ArrayList methodParameters, ArrayList outputValues, 
            int executedTime, bool isCanceled)
        {
            this.MethodName = methodName;
            this.MethodParameters = methodParameters;
            this.OutputValues = outputValues;
            this.ExecutedTime = executedTime;
            this.IsCanceled = isCanceled;
        }

        public string MethodName { get; private set; }

        public ArrayList MethodParameters { get; private set; }

        public ArrayList OutputValues { get; private set; }

        public int ExecutedTime { get; private set; }

        public bool IsCanceled { get; private set; }
    }
}
