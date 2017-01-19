using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    public class MockRegistrationElement : ConfigurationElement
    {
        public MockRegistrationElement(string path, string description)
        {
            this.Path = path;
            this.Description = description;
        }

        public MockRegistrationElement(string path)
        {
            this.Path = path;
        }
        
        public MockRegistrationElement()
        {
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

        [ConfigurationProperty("description", IsRequired = false)]
        public string Description
        {
            get
            {
                return (string)this["description"];
            }
            set
            {
                this["description"] = value;
            }
        }
    }
}
