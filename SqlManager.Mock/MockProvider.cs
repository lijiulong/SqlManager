using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;
using Franksoft.SqlManager.Mock.Definition;

namespace Franksoft.SqlManager.Mock
{
    /// <summary>
    /// A wrapper class for all kinds of <see cref="IDbProvider"/> classes provided by SqlManager,
    /// with the ability to replace query results defined by mock configurations.
    /// </summary>
    public class MockProvider : BaseDbProvider
    {
        /// <summary>
        /// Initializes <see cref="MockProvider"/> instance with no parameter. This kind of instance can only be used
        /// to get query results from defined CSV files.
        /// </summary>
        public MockProvider()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initializes <see cref="MockProvider"/> instance with specific <see cref="IDbProvider"/> instance.
        /// The <see cref="IDbProvider"/> instance in parameter will be used to execute queries from defined database.
        /// </summary>
        /// <param name="dbProvider">
        /// <see cref="IDbProvider"/> instance used to execute queries from defined database.
        /// </param>
        public MockProvider(IDbProvider dbProvider)
        {
            Initialize(dbProvider);
        }

        /// <summary>
        /// Const string value for the default value of MockDirectory configuration item in application settings file.
        /// </summary>
        private const string MOCK_DIRECTORY_DEFAULT_VALUE = @".\";

        /// <summary>
        /// Const string value for key of MockDirectory config item in application settings file.
        /// </summary>
        private const string MOCK_DIRECTORY_CONFIG_KEY = "SqlManager.MockDirectory";

        /// <summary>
        /// Const string value for the name of mock registration section in application settings file.
        /// </summary>
        private const string MOCK_REGISTRATION_SECTION_NAME = "MockRegistrations";

        /// <summary>
        /// Gets or sets list of registered mock definition file pathes.
        /// </summary>
        private List<string> Mocks { get; set; }

        /// <summary>
        /// Gets or set XmlSerializer for StandaloneQueriesMock files.
        /// </summary>
        private XmlSerializer StandaloneQueriesMockXmlSerializer { get; set; }

        /// <summary>
        /// Gets the <see cref="DbDataAdapter"/> object inside this <see cref="MockProvider"/>. The value of
        /// Adapter property in <see cref="DbMockProvider"/> will be returned instead if <see cref="DbMockProvider"/>
        /// is not null. Otherwise null value will be returned.
        /// </summary>
        public override DbDataAdapter Adapter
        {
            get
            {
                if (this.DbMockProvider != null)
                {
                    return this.DbMockProvider.Adapter;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="DbCommand"/> object inside this <see cref="MockProvider"/>. The value of
        /// Command property in <see cref="DbMockProvider"/> will be returned instead if <see cref="DbMockProvider"/>
        /// is not null. Otherwise null value will be returned.
        /// </summary>
        public override DbCommand Command
        {
            get
            {
                if (this.DbMockProvider != null)
                {
                    return this.DbMockProvider.Command;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="DbConnection"/> object inside this <see cref="MockProvider"/>. The value of
        /// Connection property in <see cref="DbMockProvider"/> will be returned instead if
        /// <see cref="DbMockProvider"/> is not null. Otherwise null value will be returned.
        /// </summary>
        public override DbConnection Connection
        {
            get
            {
                if (this.DbMockProvider != null)
                {
                    return this.DbMockProvider.Connection;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IDbProvider"/> instance used to execute queries from defined database.
        /// </summary>
        public IDbProvider DbMockProvider { get; private set; }

        /// <summary>
        /// Gets the value of MockDirectory config item.
        /// </summary>
        public string MockDirectory { get; private set; }

        /// <summary>
        /// Gets collection of registered mock definition file pathes.
        /// </summary>
        public ICollection<string> MockRegistration
        {
            get
            {
                return this.Mocks.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the dictionary of sql query mocks.
        /// </summary>
        public Dictionary<string, SqlMock> StandaloneQueriesMock { get; private set; }

        /// <summary>
        /// Starts a database transaction using the same method inside <see cref="DbMockProvider"/> property.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// Null value will be returned if <see cref="DbMockProvider"/> property is null or incorrect.
        /// </returns>
        public override DbTransaction BeginTransaction()
        {
            if (this.DbMockProvider == null)
            {
                return null;
            }

            return this.DbMockProvider.BeginTransaction();
        }

        /// <summary>
        /// Starts a database transaction with the specified isolation level using the same method inside
        /// <see cref="DbMockProvider"/> property.
        /// </summary>
        /// <param name="il">Specifies the isolation level for the transaction.</param>
        /// <returns>
        /// An object representing the new transaction.
        /// Null value will be returned if <see cref="DbMockProvider"/> property is null or incorrect.
        /// </returns>
        public override DbTransaction BeginTransaction(IsolationLevel il)
        {
            if (this.DbMockProvider == null)
            {
                return null;
            }

            return this.DbMockProvider.BeginTransaction(il);
        }

        /// <summary>
        /// Executes the <see cref="BaseDbProvider.Dispose()"/> method inside <see cref="DbMockProvider"/> property.
        /// </summary>
        public override void Dispose()
        {
            if (this.DbMockProvider == null)
            {
                return;
            }

            this.DbMockProvider.Dispose();
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.ExecuteNonQuery(IDbProvider, DbParameter[])"/> method.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery()
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);

            if (sqlMock != null)
            {
                return sqlMock.ExecuteNonQuery(this.DbMockProvider, this.Parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.GetReader(IDbProvider, DbParameter[])"/> method.
        /// </summary>
        /// <returns>
        /// A <see cref="DbDataReader"/> instance of the executed <see cref="BaseDbProvider.CommandText"/>.
        /// </returns>
        public override DbDataReader ExecuteReader()
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);
            if (sqlMock != null)
            {
                return sqlMock.GetReader(this.DbMockProvider, this.Parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.GetReader(IDbProvider, DbParameter[], CommandBehavior)"/> method.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>
        /// A <see cref="DbDataReader"/> instance of the executed <see cref="BaseDbProvider.CommandText"/>.
        /// </returns>
        public override DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);
            if (sqlMock != null)
            {
                return sqlMock.GetReader(this.DbMockProvider, this.Parameters, behavior);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.ExecuteScalar(IDbProvider, DbParameter[])"/> method.
        /// </summary>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public override object ExecuteScalar()
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);
            if (sqlMock != null)
            {
                return sqlMock.ExecuteScalar(this.DbMockProvider, this.Parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.Fill(IDbProvider, DbParameter[])"/> method.
        /// </summary>
        /// <param name="dataTable">The name of the <see cref="DataTable"/> to use for table mapping.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>. 
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public override int Fill(DataTable dataTable)
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);
            if (sqlMock != null)
            {
                int result = -1;
                dataTable = sqlMock.Fill(this.DbMockProvider, this.Parameters);
                if (dataTable != null)
                {
                    result = dataTable.Rows.Count;
                }

                return result;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="DbParameter"/> object with a name and value
        /// using the same method inside <see cref="DbMockProvider"/> property.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <returns>A <see cref="DbParameter"/> object.</returns>
        public override DbParameter GetParameter(string parameterName, object value)
        {
            DbParameter parameter = null;

            if (this.DbMockProvider != null)
            {
                parameter = this.DbMockProvider.GetParameter(parameterName, value);
            }

            return parameter;
        }

        /// <summary>
        /// Executes the same method inside <see cref="DbMockProvider"/> property in oreder to 
        /// initialize it or change connection string.
        /// </summary>
        /// <param name="connectionString">Connection string for this <see cref="IDbProvider"/> instance.</param>
        public override void Initialize(string connectionString)
        {
            if (this.DbMockProvider == null)
            {
                return;
            }

            this.DbMockProvider.Initialize(connectionString);
        }

        /// <summary>
        /// Finds related <see cref="SqlMock"/> object for <see cref="BaseDbProvider.CommandText"/> and executes
        /// <see cref="SqlMock.Update(IDbProvider, DataTable, DbParameter[])"/> method.
        /// </summary>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public override int Update(DataTable dataTable)
        {
            SqlMock sqlMock = this.MapToMock(this.CommandText);
            if (sqlMock != null)
            {
                return sqlMock.Update(this.DbMockProvider, dataTable, this.Parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Helps to initialize <see cref="MockProvider"/> instance with specific <see cref="IDbProvider"/> instance.
        /// </summary>
        /// <param name="dbProvider"></param>
        private void Initialize(IDbProvider dbProvider)
        {
            InitializeMembers(dbProvider);
            InitializeConfiguration();
            InitializeMocks();
        }

        /// <summary>
        /// Initializes all members in the instance.
        /// </summary>
        /// <param name="dbProvider">
        /// <see cref="IDbProvider"/> instance used to execute queries from defined database.
        /// </param>
        private void InitializeMembers(IDbProvider dbProvider)
        {
            this.Mocks = new List<string>();
            this.MockDirectory = MOCK_DIRECTORY_DEFAULT_VALUE;
            this.StandaloneQueriesMock = new Dictionary<string, SqlMock>();
            this.StandaloneQueriesMockXmlSerializer = new XmlSerializer(typeof(StandaloneQueriesMock));
            this.DbMockProvider = dbProvider;
        }

        /// <summary>
        /// Initializes mock definition file pathes and other configuration item values.
        /// </summary>
        private void InitializeConfiguration()
        {
            string mockDirectory = ConfigurationManager.AppSettings[MOCK_DIRECTORY_CONFIG_KEY];
            if (!string.IsNullOrEmpty(mockDirectory))
            {
                this.MockDirectory = mockDirectory;
            }
            this.MockDirectory = SqlManager.Instance.ProcessRelativePath(this.MockDirectory);

            var mocks = ConfigurationManager.GetSection(MOCK_REGISTRATION_SECTION_NAME) as MockRegistrationSection;
            if (mocks != null)
            {
                foreach (MockRegistrationElement path in mocks.Pathes)
                {
                    this.Mocks.Add(SqlManager.Instance.ProcessRelativePath(path.Path));
                }
            }

            if (!string.IsNullOrEmpty(this.MockDirectory) && Directory.Exists(this.MockDirectory))
            {
                var files = Directory.GetFiles(this.MockDirectory, "*.xml");
                foreach (string path in files)
                {
                    this.Mocks.RemoveAll(s => s == path);
                    this.Mocks.Add(path);
                }
            }
        }

        /// <summary>
        /// Initializes and deserializes mock definition files.
        /// </summary>
        private void InitializeMocks()
        {
            StandaloneQueriesMock standaloneQueriesMock = new StandaloneQueriesMock();

            foreach (string path in this.MockRegistration)
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (XmlReader reader = new XmlTextReader(stream))
                    {
                        if (this.StandaloneQueriesMockXmlSerializer.CanDeserialize(reader))
                        {
                            var queries = this.StandaloneQueriesMockXmlSerializer.Deserialize(reader) as StandaloneQueriesMock;

                            if (queries != null)
                            {
                                standaloneQueriesMock.AddRange(queries);
                            }
                        }
                    }
                }
            }

            this.StandaloneQueriesMock = standaloneQueriesMock.ToDictionary(SqlManager.Instance.IgnoreDuplicateKeys);
        }

        /// <summary>
        /// Maps a given sql command text to <see cref="SqlMock"/> object inside <see cref="StandaloneQueriesMock"/>.
        /// </summary>
        /// <param name="commandText">The sql command text need to map.</param>
        /// <returns><see cref="SqlMock"/> object or null value if not found.</returns>
        private SqlMock MapToMock(string commandText)
        {
            SqlMock result = null;
            string key = string.Empty;

            foreach (KeyValuePair<string, Sql> keyValuePair in SqlManager.Instance.StandaloneQueries)
            {
                string sqlText = keyValuePair.Value.ToString();
                if (string.Equals(commandText, sqlText, StringComparison.InvariantCultureIgnoreCase))
                {
                    key = keyValuePair.Key;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(key) && this.StandaloneQueriesMock.ContainsKey(key))
            {
                result = this.StandaloneQueriesMock[key];
            }

            return result;
        }
    }
}
