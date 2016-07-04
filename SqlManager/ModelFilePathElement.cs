using System.Configuration;

namespace Franksoft.SqlManager
{
    class ModelFilePathElement : ConfigurationElement
    {
        public ModelFilePathElement(string key, string path)
        {
            this.Key = key;
            this.Path = path;
        }

        public ModelFilePathElement(string path)
        {
            this.Path = path;
        }
        
        public ModelFilePathElement()
        {
        }

        [ConfigurationProperty("key", IsRequired = false)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}
