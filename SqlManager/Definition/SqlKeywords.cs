using System;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum SqlKeywords
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        Select = 1,

        /// <summary>
        /// 
        /// </summary>
        InsertInto = 2,

        /// <summary>
        /// 
        /// </summary>
        DeleteFrom = 3,

        /// <summary>
        /// 
        /// </summary>
        Update = 4,

        /// <summary>
        /// 
        /// </summary>
        Where = 5,        

        /// <summary>
        /// 
        /// </summary>
        From = 11,

        /// <summary>
        /// 
        /// </summary>
        GroupBy = 12,

        /// <summary>
        /// 
        /// </summary>
        OrderBy = 13,

        /// <summary>
        /// 
        /// </summary>
        Fields = 21,

        /// <summary>
        /// 
        /// </summary>
        Values = 22,

        /// <summary>
        /// 
        /// </summary>
        Set = 41,

        /// <summary>
        /// 
        /// </summary>
        SetFields = 42,

        /// <summary>
        /// 
        /// </summary>
        EqualValues = 43,

        /// <summary>
        /// 
        /// </summary>
        Exists = 51,

        /// <summary>
        /// 
        /// </summary>
        Begin = 6,

        /// <summary>
        /// 
        /// </summary>
        End = 7,

        /// <summary>
        /// 
        /// </summary>
        Create = 91,

        /// <summary>
        /// 
        /// </summary>
        Drop = 92,

        /// <summary>
        /// 
        /// </summary>
        Alter = 93,

        /// <summary>
        /// 
        /// </summary>
        Grant = 94,
    }
}
