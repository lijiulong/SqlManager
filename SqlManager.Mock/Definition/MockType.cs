using System;

namespace Franksoft.SqlManager.Mock.Definition
{
    [Serializable]
    public enum MockType
    {
        Other = 0,

        Select = 1,

        Update = 2,

        Delete = 3,
    }
}
