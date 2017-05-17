using System;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// Provides a set of supported Sql keywords used in <see cref="SqlClause"/> or other SqlManager classes.
    /// </summary>
    [Serializable]
    public enum SqlKeywords
    {
        /// <summary>
        /// Represents no keyword at all.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents SELECT Sql keyword.
        /// </summary>
        Select = 1,

        /// <summary>
        /// Represents INSERT INTO Sql keyword.
        /// </summary>
        InsertInto = 2,

        /// <summary>
        /// Represents DELETE FROM Sql keyword.
        /// </summary>
        DeleteFrom = 3,

        /// <summary>
        /// Represents UPDATE Sql keyword.
        /// </summary>
        Update = 4,

        /// <summary>
        /// Represents WHERE Sql keyword.
        /// </summary>
        Where = 5,

        /// <summary>
        /// Represents FROM Sql keyword.
        /// </summary>
        From = 11,

        /// <summary>
        /// Represents GROUP BY Sql keyword.
        /// </summary>
        GroupBy = 12,

        /// <summary>
        /// Represents ORDER BY Sql keyword.
        /// </summary>
        OrderBy = 13,

        /// <summary>
        /// Represents no keyword but a place holder for fields.
        /// </summary>
        Fields = 21,

        /// <summary>
        /// Represents VALUES Sql keyword in INSERT INTO xxx (xxx) VALUES (xxx) Sql expression.
        /// </summary>
        Values = 22,

        /// <summary>
        /// Represents SET Sql keyword.
        /// </summary>
        Set = 41,

        /// <summary>
        /// Represents SET Sql keyword in UPDATE xxx SET (xxx) = (xxx) Sql expression.
        /// </summary>
        SetFields = 42,

        /// <summary>
        /// Represents the equals sign in UPDATE xxx SET (xxx) = (xxx) Sql expression.
        /// </summary>
        EqualValues = 43,

        /// <summary>
        /// Represents EXISTS Sql keyword.
        /// </summary>
        Exists = 51,

        /// <summary>
        /// Represents IN Sql keyword.
        /// </summary>
        In = 52,

        /// <summary>
        /// Represents BEGIN Sql keyword.
        /// </summary>
        Begin = 6,

        /// <summary>
        /// Represents END Sql keyword.
        /// </summary>
        End = 7,

        /// <summary>
        /// Represents CREATE Sql keyword.
        /// </summary>
        Create = 91,

        /// <summary>
        /// Represents DROP Sql keyword.
        /// </summary>
        Drop = 92,

        /// <summary>
        /// Represents ALTER Sql keyword.
        /// </summary>
        Alter = 93,

        /// <summary>
        /// Represents GRANT Sql keyword.
        /// </summary>
        Grant = 94,
    }
}
