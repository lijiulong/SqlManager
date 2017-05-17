using System;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// Provides a set of supported Sql logical operators used in <see cref="SqlClause"/> or other SqlManager classes.
    /// </summary>
    [Serializable]
    public enum SqlLogicalOperator
    {
        /// <summary>
        /// Represents no logical operator at all.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents AND Sql logical operator.
        /// </summary>
        And = 1,

        /// <summary>
        /// Represents OR Sql logical operator.
        /// </summary>
        Or = 2,

        /// <summary>
        /// Represents NOT Sql logical operator.
        /// </summary>
        Not = 3
    }
}
