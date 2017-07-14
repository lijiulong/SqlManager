using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;

using Franksoft.SqlManager.DbProviders;
using Franksoft.SqlManager.Definition;

namespace Franksoft.SqlManager.Mock.Definition
{
    /// <summary>
    /// A class represents the mock definition of one sql command inside definition file.
    /// It has a <see cref="Key"/> property which is used to find this object from the definition collection.
    /// </summary>
    [Serializable]
    public class SqlMock
    {
        /// <summary>
        /// Gets or sets the key of this <see cref="SqlMock"/> object. It should be unique among all definitions.
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MockType"/> value of this <see cref="SqlMock"/> object.
        /// </summary>
        /// <remarks>
        /// This property is now optional, and not used by SqlManager now.
        /// </remarks>
        [XmlAttribute]
        public MockType Type { get; set; }

        /// <summary>
        /// Gets or sets the executed counter of this <see cref="SqlMock"/> object.
        /// </summary>
        [XmlIgnore]
        public long ExecutedCounter { get; set; }

        /// <summary>
        /// Gets or sets list of <see cref="MockConfig"/> objects for this <see cref="SqlMock"/> object.
        /// </summary>
        public List<MockConfig> MockConfigs { get; set; }

        /// <summary>
        /// Helps to initialize <see cref="SqlMock"/> instance.
        /// </summary>
        public void InitializeMock()
        {
            this.SortMockConfigs();
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object as nonquery sql inside configured database in current
        /// <see cref="MockConfig"/> item, returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object as nonquery sql with parameters inside configured database in
        /// current <see cref="MockConfig"/> item, returns the number of rows affected.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(IDbProvider dbProvider, DbParameter[] parameters)
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

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object inside configured database in current <see cref="MockConfig"/>
        /// item and returns the first column of the first row in the result set returned by the query. All other
        /// columns and rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public object ExecuteScalar(IDbProvider dbProvider)
        {
            return this.ExecuteNonQuery(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object with parameters inside configured database in current
        /// <see cref="MockConfig"/> item and returns the first column of the first row in the result set returned by
        /// the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <returns>First column of the first row in the result set returned by the query.</returns>
        public object ExecuteScalar(IDbProvider dbProvider, DbParameter[] parameters)
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

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object inside configured database in current <see cref="MockConfig"/>
        /// item via <see cref="IDbProvider.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public DataTable Fill(IDbProvider dbProvider)
        {
            return this.Fill(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object with parameters inside configured database in current
        /// <see cref="MockConfig"/> item via <see cref="IDbProvider.Fill(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the <see cref="DataSet"/>.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public DataTable Fill(IDbProvider dbProvider, DbParameter[] parameters)
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

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object inside configured database in current
        /// <see cref="MockConfig"/> item via <see cref="IDbProvider.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public int Update(IDbProvider dbProvider, DataTable dataTable)
        {
            return this.Update(dbProvider, dataTable, null);
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object with parameters inside configured database in current
        /// <see cref="MockConfig"/> item via <see cref="IDbProvider.Update(DataTable)"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="dataTable">The <see cref="DataTable"/> used to update the data source.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <returns>The number of rows successfully updated from the <see cref="DataTable"/>.</returns>
        public int Update(IDbProvider dbProvider, DataTable dataTable, DbParameter[] parameters)
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

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object inside configured database in current <see cref="MockConfig"/>
        /// item, returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public DbDataReader GetReader(IDbProvider dbProvider)
        {
            return this.GetReader(dbProvider, null);
        }

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object with parameters inside configured database in current
        /// <see cref="MockConfig"/> item, returns a <see cref="DbDataReader"/> instance.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters)
        {
            MockConfig mockConfig = this.GetCurrentMockConfig();
            if (mockConfig != null)
            {
                this.ExecutedCounter++;
                if (!string.IsNullOrEmpty(mockConfig.CsvFilePath))
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

        /// <summary>
        /// Executes this <see cref="SqlMock"/> object with parameters inside configured database in current
        /// <see cref="MockConfig"/> item, returns a <see cref="DbDataReader"/> instance using one of the
        /// <see cref="CommandBehavior"/>.
        /// </summary>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> object to execute this sql mock.</param>
        /// <param name="parameters">The <see cref="DbParameter"/> objects to execute this sql mock.</param>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="DbDataReader"/> instance of the executed <see cref="Sql"/> object.</returns>
        public DbDataReader GetReader(IDbProvider dbProvider, DbParameter[] parameters, CommandBehavior behavior)
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

        /// <summary>
        /// Sorts <see cref="MockConfig"/> items inside this object according to <see cref="MockConfig.Sequence"/>
        /// </summary>
        private void SortMockConfigs()
        {
            this.MockConfigs.Sort((a, b) =>
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

        /// <summary>
        /// Gets the current <see cref="MockConfig"/> item according to <see cref="MockConfig.Sequence"/> and
        /// <see cref="MockConfig.Repeat"/> and also <see cref="ExecutedCounter"/> in current object.
        /// </summary>
        /// <returns></returns>
        private MockConfig GetCurrentMockConfig()
        {
            MockConfig result = null;
            long repeatCounter = 0;

            foreach (MockConfig mockConfig in this.MockConfigs)
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
