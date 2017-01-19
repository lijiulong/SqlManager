using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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
    }
}
