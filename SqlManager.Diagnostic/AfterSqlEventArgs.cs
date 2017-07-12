using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Provides data for <see cref="AfterSqlEventHandler"/> typed events in <see cref="DiagnosticProvider"/> or
    /// <see cref="SqlDiagnosticWrapper"/>.
    /// </summary>
    public class AfterSqlEventArgs : AfterEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterSqlEventArgs"/> class with executed sql command text,
        /// list of keywords inside sql command text, sql command type, array of sql parameters, related method name,
        /// method parameters, method output values, method executed time and if the method is canceled in
        /// <see cref="BeforeSqlEventHandler"/> typed events.
        /// </summary>
        /// <param name="sqlCommandText">Sql command text executed by the method which invloked this event.</param>
        /// <param name="keywordList">List of keywords inside the sql command text.</param>
        /// <param name="sqlCommandType">The <see cref="CommandType"/> value of the sql command text.</param>
        /// <param name="sqlParameters">Array of <see cref="DbParameter"/> used by the sql command text.</param>
        /// <param name="methodName">Name of the method which invloked this event.</param>
        /// <param name="methodParameters">Parameter values of the method which invloked this event.</param>
        /// <param name="outputValues">Output values of the method which invloked this event.</param>
        /// <param name="executedTime">Executed time (milliseconds) of the method which invloked this event.</param>
        /// <param name="isCanceled">
        /// Whether the method is canceled in <see cref="BeforeSqlEventHandler"/> typed events.
        /// </param>
        public AfterSqlEventArgs(string sqlCommandText, IList<SqlKeywords> keywordList,
            CommandType sqlCommandType, DbParameter[] sqlParameters, string methodName,
            ArrayList methodParameters, ArrayList outputValues, int executedTime, bool isCanceled)
            : base(methodName, methodParameters, outputValues, executedTime, isCanceled)
        {
            this.SqlCommandText = sqlCommandText;
            this.KeywordList = new ReadOnlyCollection<SqlKeywords>(keywordList);
            this.SqlCommandType = sqlCommandType;
            this.SqlParameters = sqlParameters;
        }

        /// <summary>
        /// Gets the sql command text executed by the method which invloked this event.
        /// </summary>
        public string SqlCommandText { get; private set; }

        /// <summary>
        /// Gets the <see cref="CommandType"/> value of the sql command text.
        /// </summary>
        public CommandType SqlCommandType { get; private set; }

        /// <summary>
        /// Gets a list of keywords inside the sql command text.
        /// </summary>
        public ReadOnlyCollection<SqlKeywords> KeywordList { get; private set; }

        /// <summary>
        /// Gets the array of <see cref="DbParameter"/> used by the sql command text.
        /// </summary>
        public DbParameter[] SqlParameters { get; private set; }
    }
}
