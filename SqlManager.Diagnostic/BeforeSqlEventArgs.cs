using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Provides data for <see cref="BeforeSqlEventHandler"/> typed events in <see cref="DiagnosticProvider"/> or
    /// <see cref="SqlDiagnosticWrapper"/>.
    /// </summary>
    public class BeforeSqlEventArgs : BeforeEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeSqlEventArgs"/> class with list of keywords inside
        /// sql command text, and related method name.
        /// </summary>
        /// <param name="keywordList">List of keywords inside the sql command text.</param>
        /// <param name="methodName">Name of the method which invloked this event.</param>
        public BeforeSqlEventArgs(IList<SqlKeywords> keywordList, string methodName)
            : base(methodName)
        {
            this.KeywordList = new ReadOnlyCollection<SqlKeywords>(keywordList);
        }

        /// <summary>
        /// Gets or sets the sql command text to be executed by the method which invloked this event.
        /// </summary>
        public string SqlCommandText { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CommandType"/> value of the sql command text.
        /// </summary>
        public CommandType SqlCommandType { get; set; }

        /// <summary>
        /// Gets a list of keywords inside the sql command text.
        /// </summary>
        public ReadOnlyCollection<SqlKeywords> KeywordList { get; private set; }

        /// <summary>
        /// Gets or sets the array of <see cref="DbParameter"/> used by the sql command text.
        /// </summary>
        public DbParameter[] SqlParameters { get; set; }
    }
}
