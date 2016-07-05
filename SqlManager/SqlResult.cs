using System.Data;

namespace Franksoft.SqlManager
{
    public class SqlResult
    {
        public object SingleResult { get; internal set; }

        public DataTable DataTableResult { get; internal set; }

        public int ReturnResult { get; internal set; }
    }
}
