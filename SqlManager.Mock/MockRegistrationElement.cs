using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    /// <summary>
    /// Represents configuration element for mock definition file path within a configuration file.
    /// </summary>
    public class MockRegistrationElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes <see cref="MockRegistrationElement"/> instance
        /// with path and description value for the mock definition file.
        /// </summary>
        /// <param name="path">Path value for the mock definition file.</param>
        /// <param name="description">Description value for the mock definition file.</param>
        public MockRegistrationElement(string path, string description)
        {
            this.Path = path;
            this.Description = description;
        }

        /// <summary>
        /// Initializes <see cref="MockRegistrationElement"/> instance with path value for the mock definition file.
        /// </summary>
        /// <param name="path">Path value for the mock definition file.</param>
        public MockRegistrationElement(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MockRegistrationElement()
        {
        }

        /// <summary>
        /// Gets or sets the path value for the mock definition file.
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
        /// Gets or sets the description value for the mock definition file.
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
