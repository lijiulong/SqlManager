using System.Configuration;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// Represents the ModelRegistration section within configuration file.
    /// </summary>
    public class ModelRegistrationSection : ConfigurationSection
    {
        /// <summary>
        /// Const string value for the name of <see cref="ModelRegistrationCollectionSection"/> in config file.
        /// </summary>
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

        /// <summary>
        /// Gets or sets the value of <see cref="ModelRegistrationCollectionSection"/>.
        /// </summary>
        [ConfigurationProperty(PATH_COLLECTION_NAME_STRING, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ModelRegistrationCollectionSection))]
        public ModelRegistrationCollectionSection Pathes
        {
            get
            {
                return (ModelRegistrationCollectionSection)this[PATH_COLLECTION_NAME_STRING];
            }
            set
            {
                this[PATH_COLLECTION_NAME_STRING] = value;
            }
        }
    }
}
