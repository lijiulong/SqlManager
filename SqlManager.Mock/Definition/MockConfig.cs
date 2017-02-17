using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Franksoft.SqlManager.Mock.Definition
{
    [Serializable]
    public class MockConfig
    {
        private const string DELIMITER_DEFAULT_VALUE = ",";

        private const string ENCODING_NAME_DEFAULT_VALUE = "ASCII";

        private const string COLUMN_NAME_DEFAULT_VALUE_TEMPLATE = "COLUMN{0}";

        private List<string> HeaderList { get; set; }

        private List<Type> TypeList { get; set; }

        private DataTable DataTableSource { get; set; }

        [XmlAttribute]
        public int Sequence { get; set; }

        [XmlAttribute]
        public int Repeat { get; set; }

        [XmlAttribute]
        public string ConnectionString { get; set; }

        [XmlAttribute]
        public string CsvFilePath { get; set; }

        [XmlAttribute]
        public string Delimiter { get; set; }

        [XmlAttribute]
        public string EncodingName { get; set; }

        [XmlAttribute]
        public bool IsIncludeHeader { get; set; }

        [XmlAttribute]
        public bool IsIncludeType { get; set; }

        public long ExecutedCounter { get; private set; }

        public IList<string> Headers
        {
            get
            {
                return this.HeaderList.AsReadOnly();
            }
        }

        public IList<Type> Types
        {
            get
            {
                return this.TypeList.AsReadOnly();
            }
        }

        public DataTable DataTable
        {
            get
            {
                return this.DataTableSource;
            }
        }

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

            string path = SqlManager.ProcessRelativePath(this.CsvFilePath);
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
                        }
                    }
                }
            }
        }
    }
}
