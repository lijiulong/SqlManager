using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Definition
{
    [Serializable]
    public class SqlClause
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

        private const string SQLLOGICALOPERATOR_AND = "AND";

        private const string SQLLOGICALOPERATOR_OR = "OR";

        private const string SQLLOGICALOPERATOR_NOT = "NOT";

        private const string EMPTY_SPACE = " ";

        private const char COMMA = ',';

        #endregion

        [XmlAttribute]
        public SqlKeywords Keyword { get; set; }

        [XmlAttribute]
        public SqlLogicalOperator LogicalOperator { get; set; }

        [XmlAttribute]
        public string Expression { get; set; }

        public List<SqlClause> ChildItems { get; set; }

        public override string ToString()
        {
            string result = string.Empty;
            string keyword = GetKeyword(this.Keyword);
            string logicalOperator = GetLogicalOperator(this.LogicalOperator);
            string expression = this.Expression;
            string childItems = string.Empty;
            bool childHasLogicalOperator = false;

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

            childItems = childItems.Trim(COMMA);

            result += keyword;
            result = AddEmptySpaceAfter(result);
            result += logicalOperator;

            if (this.LogicalOperator == SqlLogicalOperator.Not && this.Keyword == SqlKeywords.Exists)
            {
                result += logicalOperator;
                result = AddEmptySpaceAfter(result);
                result += keyword;
            }

            result = AddEmptySpaceAfter(result);
            if (this.Keyword == SqlKeywords.Exists || this.Keyword == SqlKeywords.Values)
            {
                result += AddEmptySpaceAfter("(" + expression + AddEmptySpaceBefore(childItems) + ")");
            }
            else if (childHasLogicalOperator)
            {
                result += AddEmptySpaceAfter(expression + "(" + AddEmptySpaceBefore(childItems) + ")");
            }
            else
            {
                result += AddEmptySpaceAfter(expression + AddEmptySpaceBefore(childItems));
            }

            return result;
        }

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
                    result = SQLKEYWORDS_SET;
                    break;
            }

            return result;
        }

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

        private string AddEmptySpaceBefore(string expression)
        {
            string result = expression;

            if (!string.IsNullOrEmpty(expression))
            {
                result = EMPTY_SPACE + result.Trim();
            }

            return result;
        }

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
