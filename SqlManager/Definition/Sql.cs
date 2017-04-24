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
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyValueTo(Sql source, Sql target)
        {
            target.Key = source.Key;
            target.Command = source.Command;
            target.CommandType = source.CommandType;

            source.CopyValueTo((SqlClause)target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteScalar(dbProvider, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public virtual DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public virtual int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="dataTable"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public virtual DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            Sql cloneResult = new Sql();
            this.CopyValueTo(cloneResult);

            return cloneResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public virtual void CopyValueTo(Sql target)
        {
            CopyValueTo(this, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
