using System.Configuration;

namespace Franksoft.SqlManager
{
    class ModelFileSection : ConfigurationSection
    {
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

        [ConfigurationProperty(PATH_COLLECTION_NAME_STRING, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ModelFilePathCollectionSection))]
        public ModelFilePathCollectionSection Pathes
        {
            get
            {
                return (ModelFilePathCollectionSection)this[PATH_COLLECTION_NAME_STRING];
            }
            set
            {
                this[PATH_COLLECTION_NAME_STRING] = value;
            }
        }
    }
}
