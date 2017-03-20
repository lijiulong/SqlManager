using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Diagnostic
{
    public static class DiagnosticHelper
    {
        public static ReadOnlyCollection<SqlKeywords> ParseSqlKeywords(string sqlCommandText)
        {
            List<SqlKeywords> result = new List<SqlKeywords>();

            string onelineSql = string.Format(" {0} ", sqlCommandText.Replace(Environment.NewLine, " "));

            if (onelineSql.IndexOf(" ALTER ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Alter);
            }

            if (onelineSql.IndexOf(" BEGIN ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Begin);
            }

            if (onelineSql.IndexOf(" CREATE ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Create);
            }

            if (onelineSql.IndexOf(" DELETE ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.DeleteFrom);
            }

            if (onelineSql.IndexOf(" DROP ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Drop);
            }

            if (onelineSql.IndexOf(" END ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.End);
            }

            if (onelineSql.IndexOf(" EXISTS ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Exists);
            }

            if (onelineSql.IndexOf(" FROM ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.From);
            }

            if (onelineSql.IndexOf(" GRANT ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Grant);
            }

            if (onelineSql.IndexOf(" GROUP BY ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.GroupBy);
            }

            if (onelineSql.IndexOf(" INSERT INTO ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.InsertInto);
            }

            if (onelineSql.IndexOf(" ORDER BY ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.OrderBy);
            }

            if (onelineSql.IndexOf(" SELECT ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Select);
            }

            if (onelineSql.IndexOf(" SET ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Set);
            }

            if (onelineSql.IndexOf(" UPDATE ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Update);
            }

            if (onelineSql.IndexOf(" VALUES ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Values);
            }

            if (onelineSql.IndexOf(" WHERE ", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                result.Add(SqlKeywords.Where);
            }

            return result.AsReadOnly();
        }
    }
}
