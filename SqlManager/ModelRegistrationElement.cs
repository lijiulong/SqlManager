using System.Configuration;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// Represents configuration element for query definition file path within a configuration file.
    /// </summary>
    public class ModelRegistrationElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes <see cref="ModelRegistrationElement"/> instance
        /// with path and description value for the query definition file.
        /// </summary>
        /// <param name="path">Path value for the query definition file.</param>
        /// <param name="description">Description value for the query definition file.</param>
        public ModelRegistrationElement(string path, string description)
        {
            this.Path = path;
            this.Description = description;
        }

        /// <summary>
        /// Initializes <see cref="ModelRegistrationElement"/> instance with path value for the query definition file.
        /// </summary>
        /// <param name="path">Path value for the query definition file.</param>
        public ModelRegistrationElement(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelRegistrationElement()
        {
        }

        /// <summary>
        /// Gets and sets the path value for the query definition file.
        /// </summary>
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

        /// <summary>
        /// Gets and sets the description value for the query definition file.
        /// </summary>
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
