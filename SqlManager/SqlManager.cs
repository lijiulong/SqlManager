using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// Core class of SqlManager library. Entrance of most SqlManager API.
    /// </summary>
    public sealed class SqlManager
    {
        /// <summary>
        /// Static constructor to initialize singleton instance of <see cref="SqlManager"/>.
        /// </summary>
        static SqlManager()
        {
            if (Instance == null)
            {
                Instance = new SqlManager();
            }
        }

        /// <summary>
        /// Private constructor ensures singleton.
        /// </summary>
        private SqlManager()
        {
            StandaloneQueries standaloneQueries = new StandaloneQueries();
            this.StandaloneQueries = new Dictionary<string, Sql>();
            this.StandaloneQueriesXmlSerializer = new XmlSerializer(typeof(StandaloneQueries));

            foreach (string path in Initializer.Instance.ModelRegistration)
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (XmlReader reader = new XmlTextReader(stream))
                    {
                        if (this.StandaloneQueriesXmlSerializer.CanDeserialize(reader))
                        {
                            var queries = this.StandaloneQueriesXmlSerializer.Deserialize(reader) as StandaloneQueries;

                            if (queries != null)
                            {
                                standaloneQueries.AddRange(queries);
                            }
                        }
                    }
                }
            }

            this.StandaloneQueries = standaloneQueries.ToDictionary(Initializer.Instance.IgnoreDuplicateKeys);
        }

        /// <summary>
        /// Gets or set XmlSerializer for StandaloneQueries files.
        /// </summary>
        private XmlSerializer StandaloneQueriesXmlSerializer { get; set; }

        /// <summary>
        /// Gets the singleton instance of <see cref="SqlManager"/>.
        /// </summary>
        public static SqlManager Instance { get; private set; }

        /// <summary>
        /// Gets the dictionary of sql queries.
        /// </summary>
        public Dictionary<string, Sql> StandaloneQueries { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="IDbProvider"/> instance of <see cref="SqlManager"/>.
        /// </summary>
        public IDbProvider DbProvider { get; set; }

        /// <summary>
        /// Gets the value of IgnoreDuplicateKeys config item.
        /// </summary>
        public bool IgnoreDuplicateKeys
        {
            get
            {
                return Initializer.Instance.IgnoreDuplicateKeys;
            }
        }

        /// <summary>
        /// Gets the value of ModelDirectory config item.
        /// </summary>
        public string ModelDirectory
        {
            get
            {
                return Initializer.Instance.ModelDirectory;
            }
        }

        /// <summary>
        /// Executes <see cref="Sql"/> according to key, 
        /// returns a <see cref="DbDataReader"/> instance as result.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of executed <see cref="Sql"/>.</returns>
        public DbDataReader GetStandaloneQueryReader(string key)
        {
            return this.GetStandaloneQueryReader(key, null);
        }

        /// <summary>
        /// Executes <see cref="Sql"/> according to key and parameters, 
        /// returns a <see cref="DbDataReader"/> instance as result.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <param name="parameters">Parameters used in this query.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of executed <see cref="Sql"/>.</returns>
        public DbDataReader GetStandaloneQueryReader(string key, DbParameter[] parameters)
        {
            DbDataReader reader = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                reader = sql.GetReader(this.DbProvider, parameters);
            }

            return reader;
        }

        /// <summary>
        /// Executes nonquery sql according to key, returns the number of rows affected.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteStandaloneNonQuery(string key)
        {
            return this.ExecuteStandaloneNonQuery(key, null);
        }

        /// <summary>
        /// Executes nonquery sql according to key and parameters, returns the number of rows affected.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <param name="parameters">Parameters used in this query.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteStandaloneNonQuery(string key, DbParameter[] parameters)
        {
            int result = -1;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.ExecuteNonQuery(this.DbProvider, parameters);
            }

            return result;
        }

        /// <summary>
        /// Executes scalar sql command according to key, 
        /// returns the first column of the first row in the result set returned by the query. 
        /// All other columns and rows are ignored.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public object ExecuteStandaloneScalar(string key)
        {
            return this.ExecuteStandaloneScalar(key, null);
        }

        /// <summary>
        /// Executes scalar sql command according to key and parameters, 
        /// returns the first column of the first row in the result set returned by the query. 
        /// All other columns and rows are ignored.
        /// </summary>
        /// <param name="key">The key defined in sql query file.</param>
        /// <param name="parameters">Parameters used in this query.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public object ExecuteStandaloneScalar(string key, DbParameter[] parameters)
        {
            object result = null;

            if (this.StandaloneQueries.ContainsKey(key))
            {
                Sql sql = this.StandaloneQueries[key];
                result = sql.ExecuteScalar(this.DbProvider, parameters);
            }

            return result;
        }

        /// <summary>
        /// Finds the key of specified sql command text.
        /// </summary>
        /// <param name="sqlCommandText">Sql command text need to find key. Case of the text will be ignored.</param>
        /// <returns>Key of sql command text. If not found, return null value.</returns>
        public string FindKey(string sqlCommandText)
        {
            string key = null;

            foreach (KeyValuePair<string, Sql> keyValuePair in this.StandaloneQueries)
            {
                string sqlText = keyValuePair.Value.ToString();
                if (string.Equals(sqlCommandText, sqlText, StringComparison.InvariantCultureIgnoreCase))
                {
                    key = keyValuePair.Key;
                    break;
                }
            }

            return key;
        }

        /// <summary>
        /// Processes relative path according to root path from configuration.
        /// </summary>
        /// <remarks>
        /// This method cannot be static since root path need to be initialized from app.config.
        /// </remarks>
        /// <param name="relativePath">Possible relative path.</param>
        /// <returns>
        /// Full path combined with root path.
        /// If original path is not relative path, it will be returned directly.
        /// </returns>
        public string ProcessRelativePath(string relativePath)
        {
            return Initializer.Instance.ProcessRelativePath(relativePath);
        }
    }
}
