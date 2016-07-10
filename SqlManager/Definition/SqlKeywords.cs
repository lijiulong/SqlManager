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

        Fields = 21,

        Values = 22,

        Set = 41,

        Exists = 51,

        Begin = 6,

        End = 7,
    }
}
