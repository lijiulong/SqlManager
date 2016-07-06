using System;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    public enum SqlLogicalOperator
    {
        None = 0,

        And = 1,

        Or = 2,

        Not = 3
    }
}
