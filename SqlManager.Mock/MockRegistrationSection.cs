using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    /// <summary>
    /// Represents the MockRegistration section within configuration file.
    /// </summary>
    public class MockRegistrationSection : ConfigurationSection
    {
        /// <summary>
        /// Const string value for the name of <see cref="MockRegistrationCollectionSection"/> in config file.
        /// </summary>
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

        /// <summary>
        /// Gets or sets the value of <see cref="MockRegistrationCollectionSection"/>.
        /// </summary>
        [ConfigurationProperty(PATH_COLLECTION_NAME_STRING, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MockRegistrationCollectionSection))]
        public MockRegistrationCollectionSection Pathes
        {
            get
            {
                return (MockRegistrationCollectionSection)this[PATH_COLLECTION_NAME_STRING];
            }
            set
            {
                this[PATH_COLLECTION_NAME_STRING] = value;
            }
        }
    }
}
