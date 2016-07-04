using System.Configuration;

namespace Franksoft.SqlManager
{
    class ModelFileSection : ConfigurationSection
    {
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

        public const string PATH_ADD_NAME_STRING = "path";

        [ConfigurationProperty(PATH_COLLECTION_NAME_STRING, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ModelFilePathCollectionSection), AddItemName = PATH_ADD_NAME_STRING)]
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
