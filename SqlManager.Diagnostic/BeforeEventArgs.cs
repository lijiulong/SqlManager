using System;
using System.Collections;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Provides data for <see cref="BeforeMethodEventHandler"/> typed events in <see cref="DiagnosticProvider"/> or
    /// <see cref="SqlDiagnosticWrapper"/>.
    /// </summary>
    public class BeforeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeEventArgs"/> class with related method name.
        /// </summary>
        /// <param name="methodName">Name of the method which invloked this event.</param>
        public BeforeEventArgs(string methodName)
        {
            this.MethodName = methodName;
        }

        /// <summary>
        /// Gets the name of the method which invloked this event.
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Gets or sets the parameter values of the method which invloked this event.
        /// </summary>
        public ArrayList MethodParameters { get; set; }

        /// <summary>
        /// Gets or sets whether to cancel the method in <see cref="BeforeMethodEventHandler"/> typed events.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
