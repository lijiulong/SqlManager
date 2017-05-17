using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// A class represents the definition of sql command parts used to build a complete sql command.
    /// </summary>
    [Serializable]
    public class SqlClause : ICloneable
    {
        #region Constant Strings

        private const string SQLKEYWORDS_SELECT = "SELECT";

        private const string SQLKEYWORDS_INSERTINTO = "INSERT INTO";

        private const string SQLKEYWORDS_DELETEFROM = "DELETE FROM";

        private const string SQLKEYWORDS_UPDATE = "UPDATE";

        private const string SQLKEYWORDS_WHERE = "WHERE";

        private const string SQLKEYWORDS_FROM = "FROM";

        private const string SQLKEYWORDS_GROUPBY = "GROUP BY";

        private const string SQLKEYWORDS_ORDERBY = "ORDER BY";

        private const string SQLKEYWORDS_VALUES = "VALUES";

        private const string SQLKEYWORDS_SET = "SET";

        private const string SQLKEYWORDS_EQUALVALUES = "=";

        private const string SQLKEYWORDS_EXISTS = "EXISTS";

        private const string SQLKEYWORDS_IN = "IN";

        private const string SQLKEYWORDS_BEGIN = "BEGIN";

        private const string SQLKEYWORDS_END = "END";

        private const string SQLLOGICALOPERATOR_AND = "AND";

        private const string SQLLOGICALOPERATOR_OR = "OR";

        private const string SQLLOGICALOPERATOR_NOT = "NOT";

        private const string EMPTY_SPACE = " ";

        private const char COMMA = ',';

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="SqlKeywords"/> value of this instance.
        /// </summary>
        [XmlAttribute]
        public SqlKeywords Keyword { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SqlLogicalOperator"/> value of this instance.
        /// </summary>
        [XmlAttribute]
        public SqlLogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// Gets or sets the sql expression of this instance.
        /// This expression will be put before child item expressions when building sql command.
        /// </summary>
        [XmlAttribute]
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets list of child <see cref="SqlClause"/> objects for this instance.
        /// </summary>
        public List<SqlClause> ChildItems { get; set; }

        /// <summary>
        /// Copies value including all child <see cref="SqlClause"/> items from source object to target object.
        /// </summary>
        /// <param name="source">The source object to copy value from.</param>
        /// <param name="target">The target object to copy value to.</param>
        public static void CopyValueTo(SqlClause source, SqlClause target)
        {
            target.Keyword = source.Keyword;
            target.LogicalOperator = source.LogicalOperator;
            target.Expression = source.Expression;

            if (source.ChildItems != null)
            {
                target.ChildItems = new List<SqlClause>();
                foreach (SqlClause child in source.ChildItems)
                {
                    SqlClause clonedChild = new SqlClause();
                    child.CopyValueTo(clonedChild);
                    target.ChildItems.Add(clonedChild);
                }
            }
            else
            {
                target.ChildItems = null;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            SqlClause cloneResult = new SqlClause();
            this.CopyValueTo(cloneResult);

            return cloneResult;
        }

        /// <summary>
        /// Copies value including all child <see cref="SqlClause"/> items from current instance to target object.
        /// </summary>
        /// <param name="target">The target object to copy value to.</param>
        public virtual void CopyValueTo(SqlClause target)
        {
            CopyValueTo(this, target);
        }

        /// <summary>
        /// Converts this instance to sql command text string part.
        /// </summary>
        /// <returns>The sql command text string part defined by this instance.</returns>
        public override string ToString()
        {
            string result = string.Empty;
            string keyword = this.GetKeyword(this.Keyword);
            string logicalOperator = this.GetLogicalOperator(this.LogicalOperator);
            string expression = this.Expression;
            string childItems = string.Empty;
            bool childHasLogicalOperator = false;

            if (this.ChildItems != null)
            {
                foreach (SqlClause clause in this.ChildItems)
                {
                    string childString = clause.ToString();
                    if (clause.Keyword == SqlKeywords.None && clause.LogicalOperator == SqlLogicalOperator.None)
                    {
                        childString = COMMA + AddEmptySpaceBefore(childString);
                    }

                    if (clause.LogicalOperator != SqlLogicalOperator.None)
                    {
                        childHasLogicalOperator = true;
                    }
                    childItems += childString + EMPTY_SPACE;
                }
            }

            childItems = childItems.Trim(COMMA);

            result += keyword;
            result = AddEmptySpaceAfter(result);
            result += logicalOperator;

            if (this.LogicalOperator == SqlLogicalOperator.Not && this.Keyword == SqlKeywords.Exists)
            {
                result += logicalOperator;
                result = this.AddEmptySpaceAfter(result);
                result += keyword;
            }

            result = this.AddEmptySpaceAfter(result);

            if (this.Keyword == SqlKeywords.Exists
                || this.Keyword == SqlKeywords.In
                || this.Keyword == SqlKeywords.Fields
                || this.Keyword == SqlKeywords.SetFields
                || this.Keyword == SqlKeywords.Values
                || this.Keyword == SqlKeywords.EqualValues)
            {
                result += this.AddEmptySpaceAfter("(" + expression + this.AddEmptySpaceBefore(childItems) + ")");
            }
            else if (childHasLogicalOperator)
            {
                result += this.AddEmptySpaceAfter(expression + "(" + this.AddEmptySpaceBefore(childItems) + ")");
            }
            else
            {
                result += this.AddEmptySpaceAfter(expression + AddEmptySpaceBefore(childItems));
            }

            return result;
        }

        /// <summary>
        /// Converts one specific <see cref="SqlKeywords"/> value to string.
        /// </summary>
        /// <param name="keyword">The <see cref="SqlKeywords"/> value to convert.</param>
        /// <returns>The string value of the <see cref="SqlKeywords"/> parameter.</returns>
        private string GetKeyword(SqlKeywords keyword)
        {
            string result = string.Empty;

            switch (keyword)
            {
                case SqlKeywords.Select:
                    result = SQLKEYWORDS_SELECT;
                    break;
                case SqlKeywords.InsertInto:
                    result = SQLKEYWORDS_INSERTINTO;
                    break;
                case SqlKeywords.DeleteFrom:
                    result = SQLKEYWORDS_DELETEFROM;
                    break;
                case SqlKeywords.Update:
                    result = SQLKEYWORDS_UPDATE;
                    break;
                case SqlKeywords.Where:
                    result = SQLKEYWORDS_WHERE;
                    break;
                case SqlKeywords.From:
                    result = SQLKEYWORDS_FROM;
                    break;
                case SqlKeywords.GroupBy:
                    result = SQLKEYWORDS_GROUPBY;
                    break;
                case SqlKeywords.OrderBy:
                    result = SQLKEYWORDS_ORDERBY;
                    break;
                case SqlKeywords.Values:
                    result = SQLKEYWORDS_VALUES;
                    break;
                case SqlKeywords.Set:
                case SqlKeywords.SetFields:
                    result = SQLKEYWORDS_SET;
                    break;
                case SqlKeywords.EqualValues:
                    result = SQLKEYWORDS_EQUALVALUES;
                    break;
                case SqlKeywords.Exists:
                    result = SQLKEYWORDS_EXISTS;
                    break;
                case SqlKeywords.In:
                    result = SQLKEYWORDS_IN;
                    break;
                case SqlKeywords.Begin:
                    result = SQLKEYWORDS_BEGIN;
                    break;
                case SqlKeywords.End:
                    result = SQLKEYWORDS_END;
                    break;
                case SqlKeywords.Fields:
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts one specific <see cref="SqlLogicalOperator"/> value to string.
        /// </summary>
        /// <param name="logicalOperator">The <see cref="SqlLogicalOperator"/> value to convert.</param>
        /// <returns>The string value of the <see cref="SqlLogicalOperator"/> parameter.</returns>
        private string GetLogicalOperator(SqlLogicalOperator logicalOperator)
        {
            string result = string.Empty;

            switch (logicalOperator)
            {
                case SqlLogicalOperator.And:
                    result = SQLLOGICALOPERATOR_AND;
                    break;
                case SqlLogicalOperator.Or:
                    result = SQLLOGICALOPERATOR_OR;
                    break;
                case SqlLogicalOperator.Not:
                    result = SQLLOGICALOPERATOR_NOT;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Adds one empty space before the expression in parameter,
        /// and makes sure there is no extra space before or after the expression.
        /// </summary>
        /// <param name="expression">The expression to add an empty space before.</param>
        /// <returns>The expression with one empty space at the start and no space at the end.</returns>
        private string AddEmptySpaceBefore(string expression)
        {
            string result = expression;

            if (!string.IsNullOrEmpty(expression))
            {
                result = EMPTY_SPACE + result.Trim();
            }

            return result;
        }

        /// <summary>
        /// Adds one empty space after the expression in parameter,
        /// and makes sure there is no extra space before or after the expression.
        /// </summary>
        /// <param name="expression">The expression to add an empty space after.</param>
        /// <returns>The expression with one empty space at the end and no space at the start.</returns>
        private string AddEmptySpaceAfter(string expression)
        {
            string result = expression;

            if (!string.IsNullOrEmpty(expression))
            {
                result = result.Trim() + EMPTY_SPACE;
            }

            return result;
        }
    }
}
