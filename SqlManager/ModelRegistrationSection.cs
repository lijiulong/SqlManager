using System.Configuration;

namespace Franksoft.SqlManager
{
    class ModelRegistrationSection : ConfigurationSection
    {
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

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
