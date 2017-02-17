using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Mock.Definition
{
    [Serializable]
    public class SqlMock : List<MockConfig>
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public MockType Type { get; set; }

        public long ExecutedCounter { get; set; }

        public void InitializeMock()
        {
            this.SortMockConfigs();
        }

        public int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        public int ExecuteNonQuery(IDbProvider dbProvider, Array parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null
                && (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString)))
            {
                this.ExecutedCounter++;
                if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                {
                    dbProvider.Initialize(mockConfig.ConnectionString);
                }
                Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                int result = -1;

                dbProvider.CommandText = relatedSql.ToString();
                dbProvider.Parameters = parameters;
                dbProvider.CommandType = relatedSql.CommandType;
                result = dbProvider.ExecuteNonQuery();

                return result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        public object ExecuteScalar(IDbProvider dbProvider, Array parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null
                && (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString)))
            {
                this.ExecutedCounter++;
                if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                {
                    dbProvider.Initialize(mockConfig.ConnectionString);
                }
                Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                object result = null;

                dbProvider.CommandText = relatedSql.ToString();
                dbProvider.Parameters = parameters;
                dbProvider.CommandType = relatedSql.CommandType;
                result = dbProvider.ExecuteScalar();

                return result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        public DataTable Fill(IDbProvider dbProvider, Array parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null)
            {
                this.ExecutedCounter++;
                if (string.IsNullOrEmpty(mockConfig.CsvFilePath))
                {
                    mockConfig.Initialize();

                    return mockConfig.DataTable;
                }
                else if (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString))
                {
                    if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                    {
                        dbProvider.Initialize(mockConfig.ConnectionString);
                    }
                    Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                    DataTable result = new DataTable();

                    dbProvider.CommandText = relatedSql.ToString();
                    dbProvider.Parameters = parameters;
                    dbProvider.CommandType = relatedSql.CommandType;
                    dbProvider.Fill(result);

                    return result;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, dataTable, null);
        }

        public int Update(IDbProvider dbProvider, DataTable dataTable, Array parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null
                && (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString)))
            {
                this.ExecutedCounter++;
                if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                {
                    dbProvider.Initialize(mockConfig.ConnectionString);
                }
                Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                int result = -1;

                dbProvider.CommandText = relatedSql.ToString();
                dbProvider.Parameters = parameters;
                dbProvider.CommandType = relatedSql.CommandType;
                result = dbProvider.Update(dataTable);

                return result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        public DbDataReader GetReader(IDbProvider dbProvider, Array parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null)
            {
                this.ExecutedCounter++;                
                if (string.IsNullOrEmpty(mockConfig.CsvFilePath))
                {
                    mockConfig.Initialize();

                    return mockConfig.DataTable.CreateDataReader();
                }
                else if (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString))
                {
                    if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                    {
                        dbProvider.Initialize(mockConfig.ConnectionString);
                    }
                    Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                    DbDataReader reader = null;

                    dbProvider.CommandText = relatedSql.ToString();
                    dbProvider.Parameters = parameters;
                    dbProvider.CommandType = relatedSql.CommandType;
                    reader = dbProvider.ExecuteReader();

                    return reader;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public DbDataReader GetReader(IDbProvider dbProvider, Array parameters, CommandBehavior behavior)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null)
            {
                this.ExecutedCounter++;
                if (string.IsNullOrEmpty(mockConfig.CsvFilePath))
                {
                    mockConfig.Initialize();

                    return mockConfig.DataTable.CreateDataReader();
                }
                else if (!string.IsNullOrEmpty(mockConfig.ConnectionString) || !string.IsNullOrEmpty(dbProvider.ConnectionString))
                {
                    if (!string.IsNullOrEmpty(mockConfig.ConnectionString))
                    {
                        dbProvider.Initialize(mockConfig.ConnectionString);
                    }
                    Sql relatedSql = SqlManager.Instance.StandaloneQueries[this.Key];

                    DbDataReader reader = null;

                    dbProvider.CommandText = relatedSql.ToString();
                    dbProvider.Parameters = parameters;
                    dbProvider.CommandType = relatedSql.CommandType;
                    reader = dbProvider.ExecuteReader(behavior);

                    return reader;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void SortMockConfigs()
        {
            this.Sort((a, b) =>
            {
                int result = 0;

                if (a == null)
                {
                    if (b != null)
                    {
                        result = -1;
                    }
                }
                else if (b == null)
                {
                    result = 1;
                }
                else
                {
                    result = a.Sequence.CompareTo(b.Sequence);
                }

                return result;
            });
        }

        private MockConfig GetCurrentMockConfig()
        {
            MockConfig result = null;
            long repeatCounter = 0;

            foreach (MockConfig mockConfig in this)
            {
                if (mockConfig.Repeat == 0)
                {
                    result = mockConfig;
                    break;
                }
                else
                {
                    repeatCounter += mockConfig.Repeat;
                    if (repeatCounter > this.ExecutedCounter)
                    {
                        result = mockConfig;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
