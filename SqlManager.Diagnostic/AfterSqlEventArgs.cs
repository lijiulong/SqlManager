using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    public class AfterSqlEventArgs : AfterEventArgs
    {
        public AfterSqlEventArgs(string sqlCommandText, ReadOnlyCollection<SqlKeywords> keywordList,
            CommandType sqlCommandType, DbParameter[] sqlParameters, string methodName,
            ArrayList methodParameters, ArrayList outputValues, int executedTime, bool isCanceled)
            : base(methodName, methodParameters, outputValues, executedTime, isCanceled)
        {
            this.SqlCommandText = sqlCommandText;
            this.KeywordList = keywordList;
            this.SqlCommandType = sqlCommandType;
            this.SqlParameters = sqlParameters;
        }

        public string SqlCommandText { get; private set; }

        public CommandType SqlCommandType { get; private set; }

        public ReadOnlyCollection<SqlKeywords> KeywordList { get; private set; }

        public DbParameter[] SqlParameters { get; private set; }
    }
}
