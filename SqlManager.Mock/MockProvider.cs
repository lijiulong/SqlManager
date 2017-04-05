using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;
using Franksoft.SqlManager.Mock.Definition;

namespace Franksoft.SqlManager.Mock
{
    public class MockProvider : BaseDbProvider
    {
        public MockProvider()
        {
            Initialize(null);
        }

        public MockProvider(IDbProvider dbProvider)
        {
            Initialize(dbProvider);
        }

        private const string MOCK_DIRECTORY_DEFAULT_VALUE = @".\";

        private const string MOCK_DIRECTORY_CONFIG_KEY = "SqlManager.MockDirectory";

        private const string MOCK_REGISTRATION_SECTION_NAME = "MockRegistrationSection";

        private List<string> Mocks { get; set; }

        private XmlSerializer StandaloneQueriesMockXmlSerializer { get; set; }

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

        public IDbProvider DbMockProvider { get; private set; }

        public string MockDirectory { get; private set; }

        public ICollection<string> MockRegistration
        {
            get
            {
                return this.Mocks.AsReadOnly();
            }
        }

        public Dictionary<string, SqlMock> StandaloneQueriesMock { get; private set; }

        public override DbTransaction BeginTransaction()
        {
            if (this.DbMockProvider == null)
            {
                return null;
            }

            return this.DbMockProvider.BeginTransaction();
        }

        public override DbTransaction BeginTransaction(IsolationLevel il)
        {
            if (this.DbMockProvider == null)
            {
                return null;
            }

            return this.DbMockProvider.BeginTransaction(il);
        }

        public override void Dispose()
        {
            if (this.DbMockProvider == null)
            {
                return;
            }

            this.DbMockProvider.Dispose();
        }

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

        public override DbParameter GetParameter(string parameterName, object value)
        {
            DbParameter parameter = null;

            if (this.DbMockProvider == null)
            {
                parameter = new SQLiteParameter(parameterName, value);
            }
            else
            {
                parameter = this.DbMockProvider.GetParameter(parameterName, value);
            }

            return parameter;
        }

        public override void Initialize(string connectionString)
        {
            if (this.DbMockProvider == null)
            {
                return;
            }

            this.DbMockProvider.Initialize(connectionString);
        }

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

        private void Initialize(IDbProvider dbProvider)
        {
            InitializeMembers(dbProvider);
            InitializeConfiguration();
            InitializeMocks();
        }

        private void InitializeMembers(IDbProvider dbProvider)
        {
            this.Mocks = new List<string>();
            this.MockDirectory = MOCK_DIRECTORY_DEFAULT_VALUE;
            this.StandaloneQueriesMock = new Dictionary<string, SqlMock>();
            this.StandaloneQueriesMockXmlSerializer = new XmlSerializer(typeof(StandaloneQueriesMock));
            this.DbMockProvider = dbProvider;
        }

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
