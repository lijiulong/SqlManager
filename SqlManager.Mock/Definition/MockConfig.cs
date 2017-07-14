using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Mock.Definition
{
    /// <summary>
    /// A class represents the configuration for mock such as where to execute this sql query.
    /// </summary>
    [Serializable]
    public class MockConfig
    {
        /// <summary>
        /// Const string value for the default value of <see cref="Delimiter"/> property.
        /// </summary>
        private const string DELIMITER_DEFAULT_VALUE = ",";

        /// <summary>
        /// Const string value for the default value of <see cref="EncodingName"/> property.
        /// </summary>
        private const string ENCODING_NAME_DEFAULT_VALUE = "ASCII";

        /// <summary>
        /// Const string value for the template of column name default value.
        /// </summary>
        private const string COLUMN_NAME_DEFAULT_VALUE_TEMPLATE = "COLUMN{0}";

        /// <summary>
        /// Gets or sets column header name list.
        /// </summary>
        private List<string> HeaderList { get; set; }

        /// <summary>
        /// Gets or sets column type list.
        /// </summary>
        private List<Type> TypeList { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DataTable"/> object contains data read from CSV file.
        /// </summary>
        private DataTable DataTableSource { get; set; }

        /// <summary>
        /// Gets or sets the sequence for this <see cref="MockConfig"/> item.
        /// </summary>
        [XmlAttribute]
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the repeat times for this <see cref="MockConfig"/> item. If this property is set to 0, this
        /// item will always repeat.
        /// </summary>
        [XmlAttribute]
        public int Repeat { get; set; }

        /// <summary>
        /// Gets or sets the connection string for this <see cref="MockConfig"/> item when database is used as
        /// data source.
        /// </summary>
        [XmlAttribute]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the CSV file path for this <see cref="MockConfig"/> item when CSV file is used as data source.
        /// </summary>
        [XmlAttribute]
        public string CsvFilePath { get; set; }

        /// <summary>
        /// Gets or sets the delimiter for this <see cref="MockConfig"/> item when CSV file is used as data source.
        /// </summary>
        [XmlAttribute]
        public string Delimiter { get; set; }

        /// <summary>
        /// Gets or sets the encoding name for this <see cref="MockConfig"/> item when CSV file is used as data source.
        /// </summary>
        [XmlAttribute]
        public string EncodingName { get; set; }

        /// <summary>
        /// Gets or sets if header is included for this <see cref="MockConfig"/> item when CSV file is used as data
        /// source.
        /// </summary>
        [XmlAttribute]
        public bool IsIncludeHeader { get; set; }

        /// <summary>
        /// Gets or sets if data type is included for this <see cref="MockConfig"/> item when CSV file is used as data
        /// source.
        /// </summary>
        [XmlAttribute]
        public bool IsIncludeType { get; set; }

        /// <summary>
        /// Gets the column header strings for this <see cref="MockConfig"/> item when CSV file is used as data source.
        /// </summary>
        [XmlIgnore]
        public IList<string> Headers
        {
            get
            {
                return this.HeaderList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the column types for this <see cref="MockConfig"/> item when CSV file is used as data source.
        /// </summary>
        [XmlIgnore]
        public IList<Type> Types
        {
            get
            {
                return this.TypeList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Data.DataTable"/> object contains data read from CSV file when CSV file is used
        /// as data source.
        /// </summary>
        [XmlIgnore]
        public DataTable DataTable
        {
            get
            {
                return this.DataTableSource;
            }
        }

        /// <summary>
        /// Initializes this <see cref="MockConfig"/> object when <see cref="CsvFilePath"/> has value.
        /// </summary>
        public void Initialize()
        {
            if (string.IsNullOrEmpty(this.ConnectionString) && string.IsNullOrEmpty(this.CsvFilePath))
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.CsvFilePath))
            {
                this.InitializeCsvFile();
            }
        }

        /// <summary>
        /// Initializes data inside configured CSV file according to configurations related to CSV.
        /// </summary>
        private void InitializeCsvFile()
        {
            if (string.IsNullOrEmpty(this.Delimiter))
            {
                this.Delimiter = DELIMITER_DEFAULT_VALUE;
            }

            if (string.IsNullOrEmpty(this.EncodingName))
            {
                this.EncodingName = ENCODING_NAME_DEFAULT_VALUE;
            }

            if (this.IsIncludeHeader)
            {
                this.HeaderList = new List<string>();
            }

            if (this.IsIncludeType)
            {
                this.TypeList = new List<Type>();
            }

            this.DataTableSource = new DataTable();

            string path = SqlManager.Instance.ProcessRelativePath(this.CsvFilePath);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding(this.EncodingName)))
                    {
                        if (this.IsIncludeHeader)
                        {
                            string header = streamReader.ReadLine();
                            this.HeaderList.AddRange(header.Split(new[] { this.Delimiter }, StringSplitOptions.None));
                        }

                        if (this.IsIncludeType)
                        {
                            string typeString = streamReader.ReadLine();
                            string[] types = typeString.Split(new[] { this.Delimiter }, StringSplitOptions.None);

                            foreach (string typeName in types)
                            {
                                Type type = typeof(string);
                                if (!string.IsNullOrEmpty(typeName))
                                {
                                    Type definedType = Type.GetType(typeName);
                                    if (definedType != null)
                                    {
                                        type = definedType;
                                    }
                                }

                                this.TypeList.Add(type);
                            }
                        }

                        if (this.HeaderList != null)
                        {
                            for (int i = 0; i < this.HeaderList.Count; i++)
                            {
                                string columnName = this.HeaderList[i];
                                this.DataTableSource.Columns.Add(columnName, typeof(string));
                            }
                        }

                        if (this.TypeList != null)
                        {
                            for (int i = 0; i < this.TypeList.Count; i++)
                            {
                                Type columnType = this.TypeList[i];

                                if (columnType == null)
                                {
                                    columnType = typeof(string);
                                }

                                if (this.DataTableSource.Columns.Count > i)
                                {
                                    this.DataTableSource.Columns[i].DataType = columnType;
                                }
                                else
                                {
                                    string columnName = string.Format(COLUMN_NAME_DEFAULT_VALUE_TEMPLATE, i);
                                    this.DataTableSource.Columns.Add(columnName, typeof(string));
                                }
                            }
                        }

                        while (!streamReader.EndOfStream)
                        {
                            string dataString = streamReader.ReadLine();
                            string[] rawDataRow = dataString.Split(new[] { this.Delimiter }, StringSplitOptions.None);

                            DataRow row = this.DataTableSource.NewRow();
                            for (int i = 0; i < rawDataRow.Length; i++)
                            {
                                Type dataType = this.DataTableSource.Columns[i].DataType;
                                string rawData = rawDataRow[i];
                                row[i] = MockHelper.ConvertToType(dataType, rawData);
                            }
                            this.DataTableSource.Rows.Add(row);
                        }
                    }
                }
            }
        }
    }
}
