using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    public class BeforeSqlEventArgs : BeforeEventArgs
    {
        public BeforeSqlEventArgs(ReadOnlyCollection<SqlKeywords> keywordList, string methodName)
            : base(methodName)
        {
            this.KeywordList = keywordList;
        }

        public string SqlCommandText { get; set; }

        public CommandType SqlCommandType { get; set; }

        public ReadOnlyCollection<SqlKeywords> KeywordList { get; private set; }

        public DbParameter[] SqlParameters { get; set; }
    }
}
