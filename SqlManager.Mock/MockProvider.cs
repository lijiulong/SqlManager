using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Mock.Definition;

namespace Franksoft.SqlManager.Mock
{
    public class MockProvider : IDbProvider
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

        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public string ConnectionString { get; set; }

        public Array Parameters { get; set; }

        public Dictionary<string, SqlMock> StandaloneQueriesMock { get; private set; }

        public string MockDirectory { get; private set; }

        public ICollection<string> MockRegistration
        {
            get
            {
                return this.Mocks.AsReadOnly();
            }
        }

        public DbTransaction BeginTransaction()
        {
            return null;
        }

        public DbTransaction BeginTransaction(IsolationLevel il)
        {
            return null;
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public int Fill(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public DbParameter GetParameter(string parameterName, object value)
        {
            throw new NotImplementedException();
        }

        public DbParameter[] GetParameterArray(params KeyValuePair<string, object>[] nameValuePairs)
        {
            throw new NotImplementedException();
        }

        public DbParameter[] GetParameterArray(params object[] values)
        {
            throw new NotImplementedException();
        }

        public int Update(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Initialize(IDbProvider dbProvider)
        {
            InitializeMembers();
            InitializeConfiguration();
            InitializeMocks();
        }

        private void InitializeMembers()
        {
            this.Mocks = new List<string>();
            this.MockDirectory = MOCK_DIRECTORY_DEFAULT_VALUE;
            this.StandaloneQueriesMock = new Dictionary<string, SqlMock>();
            this.StandaloneQueriesMockXmlSerializer = new XmlSerializer(typeof(StandaloneQueriesMock));
        }

        private void InitializeConfiguration()
        {
            string mockDirectory = ConfigurationManager.AppSettings[MOCK_DIRECTORY_CONFIG_KEY];
            if (!string.IsNullOrEmpty(mockDirectory))
            {
                this.MockDirectory = mockDirectory;
            }
            this.MockDirectory = SqlManager.ProcessRelativePath(this.MockDirectory);

            var mocks = ConfigurationManager.GetSection(MOCK_REGISTRATION_SECTION_NAME) as MockRegistrationSection;
            if (mocks != null)
            {
                foreach (MockRegistrationElement path in mocks.Pathes)
                {
                    this.Mocks.Add(SqlManager.ProcessRelativePath(path.Path));
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
    }
}
