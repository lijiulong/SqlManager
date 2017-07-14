using System;

namespace Franksoft.SqlManager.Mock.Definition
{
    /// <summary>
    /// Provides a set of supported mock types used in <see cref="SqlMock"/> or other SqlManager classes.
    /// </summary>
    [Serializable]
    public enum MockType
    {
        /// <summary>
        /// Represents all other mock types not included here.
        /// </summary>
        Other = 0,

        /// <summary>
        /// Represents mock for select type of query.
        /// </summary>
        Select = 1,

        /// <summary>
        /// Represents mock for update type of query.
        /// </summary>
        Update = 2,

        /// <summary>
        /// Represents mock for delete type of query.
        /// </summary>
        Delete = 3,

        /// <summary>
        /// Represents mock for insert type of query.
        /// </summary>
        Insert = 4,
    }
}
