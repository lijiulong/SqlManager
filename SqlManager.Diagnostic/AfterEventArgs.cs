using System;
using System.Collections;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Provides data for <see cref="AfterMethodEventHandler"/> typed events in <see cref="DiagnosticProvider"/> or
    /// <see cref="SqlDiagnosticWrapper"/>.
    /// </summary>
    public class AfterEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterEventArgs"/> class with related method name, method
        /// parameters, method output values, method executed time and if the method is canceled in
        /// <see cref="BeforeMethodEventHandler"/> typed events.
        /// </summary>
        /// <remarks>
        /// Usually one method only has one output value, but if the method has out type parameters, they can be
        /// treated as output values as well. The first item of outputValues parameter should always be the return
        /// value of the method itself if it has.
        /// </remarks>
        /// <param name="methodName">Name of the method which invloked this event.</param>
        /// <param name="methodParameters">Parameter values of the method which invloked this event.</param>
        /// <param name="outputValues">Output values of the method which invloked this event.</param>
        /// <param name="executedTime">Executed time (milliseconds) of the method which invloked this event.</param>
        /// <param name="isCanceled">
        /// Whether the method is canceled in <see cref="BeforeMethodEventHandler"/> typed events.
        /// </param>
        public AfterEventArgs(string methodName, ArrayList methodParameters, ArrayList outputValues, 
            int executedTime, bool isCanceled)
        {
            this.MethodName = methodName;
            this.MethodParameters = ArrayList.ReadOnly(methodParameters);
            this.OutputValues = outputValues;
            this.ExecutedTime = executedTime;
            this.IsCanceled = isCanceled;
        }

        /// <summary>
        /// Gets the name of the method which invloked this event.
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Gets the parameter values of the method which invloked this event.
        /// </summary>
        public ArrayList MethodParameters { get; private set; }

        /// <summary>
        /// Gets the output values of the method which invloked this event.
        /// </summary>
        /// <remarks>
        /// Usually one method only has one output value, but if the method has out type parameters, they can be
        /// treated as output values as well. The first item of outputValues parameter should always be the return
        /// value of the method itself if it has.
        /// </remarks>
        public ArrayList OutputValues { get; private set; }

        /// <summary>
        /// Gets the executed time (milliseconds) of the method which invloked this event.
        /// </summary>
        public int ExecutedTime { get; private set; }

        /// <summary>
        /// Gets a boolean value indicates whether the method is canceled in
        /// <see cref="BeforeMethodEventHandler"/> typed events.
        /// </summary>
        public bool IsCanceled { get; private set; }
    }
}
