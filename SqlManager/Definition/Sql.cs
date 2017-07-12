using System;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;

namespace Franksoft.SqlManager.Definition
{
    /// <summary>
    /// A class represents the definition of one sql command inside definition file. 
    /// It has a <see cref="Key"/> property which is used to find this object from the definition collection.
    /// </summary>
    [Serializable]
    public class Sql : SqlClause
    {
        /// <summary>
        /// Gets or sets the key of this <see cref="Sql"/> object. It should be unique among all definitions.
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the command text of this <see cref="Sql"/> object. 
        /// It should not coexists with <see cref="SqlClause.ChildItems"/> property.
        /// </summary>
        [XmlAttribute]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Data.CommandType"/> value of this <see cref="Sql"/> object. 
        /// This property is preseved for future development.
        /// </summary>
        [XmlAttribute]
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Copies value including all child <see cref="SqlClause"/> items from source object to target object.
        /// </summary>
        /// <param name="source">The source object to copy value from.</param>
        /// <param name="target">The target object to copy value to.</param>
        public static void CopyValueTo(Sql source, Sql target)
        {
            target.Key = source.Key;
            target.Command = source.Command;
            target.CommandType = source.CommandType;

            source.CopyValueTo((SqlClause)target);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object as nonquery sql against the <see cref="IDbProvider"/> parameter,
        /// returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object as nonquery sql with parameters against the
        /// <see cref="IDbProvider"/> parameter, returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int ExecuteNonQuery(IDbProvider dbProvider, DbParameter[] parameters)
        {
            int result = -1;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter
        /// and returns the first column of the first row in the result set returned by the query.
        /// All other columns and rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public virtual object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteScalar(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/> parameter
        /// and returns the first column of the first row in the result set returned by the query.
        /// All other columns and rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public virtual object ExecuteScalar(IDbProvider dbProvider, DbParameter[] parameters)
        {
            object result = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.ExecuteScalar();

            return result;
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public virtual DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public virtual DataTable Fill(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DataTable result = new DataTable();

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            dbProvider.Fill(result);

            return result;
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public virtual int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object with parameters against the <see cref="IDbProvider"/> parameter via
        /// <see cref="IDbProvider.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public virtual int Update(IDbProvider dbProvider, DataTable dataTable, DbParameter[] parameters)
        {
            int result = -1;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            result = dbProvider.Update(dataTable);

            return result;
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter,
        /// returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public virtual DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="Sql"/> object against the <see cref="IDbProvider"/> parameter,
        /// returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql command.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql command.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public virtual DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters)
        {
            DbDataReader reader = null;

            dbProvider.CommandText = this.ToString();
            dbProvider.Parameters = parameters;
            dbProvider.CommandType = this.CommandType;
            reader = dbProvider.ExecuteReader();

            return reader;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            Sql cloneResult = new Sql();
            this.CopyValueTo(cloneResult);

            return cloneResult;
        }

        /// <summary>
        /// Copies value including all child <see cref="SqlClause"/> items from current instance to target object.
        /// </summary>
        /// <param name="target">The target object to copy value to.</param>
        public virtual void CopyValueTo(Sql target)
        {
            CopyValueTo(this, target);
        }

        /// <summary>
        /// Converts this instance to sql command text string.
        /// </summary>
        /// <returns>The sql command text string defined by this instance.</returns>
        public override string ToString()
        {
            string result = base.ToString();

            if (string.IsNullOrEmpty(result))
            {
                result = this.Command;
            }

            return result;
        }
    }
}
