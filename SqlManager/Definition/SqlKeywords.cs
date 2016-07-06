using System;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    public enum SqlKeywords
    {
        None = 0,

        Select = 1,

        InsertInto = 2,

        DeleteFrom = 3,

        Update = 4,

        Where = 5,

        From = 11,

        GroupBy = 12,

        OrderBy = 13,

        Values = 21,

        Set = 41,

        Exists = 51
    }
}
