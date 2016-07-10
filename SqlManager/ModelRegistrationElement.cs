using System.Configuration;

namespace Franksoft.SqlManager
{
    public class ModelRegistrationElement : ConfigurationElement
    {
        public ModelRegistrationElement(string path, string description)
        {
            this.Path = path;
            this.Description = description;
        }

        public ModelRegistrationElement(string path)
        {
            this.Path = path;
        }
        
        public ModelRegistrationElement()
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
