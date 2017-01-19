using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    class MockRegistrationSection : ConfigurationSection
    {
        public const string PATH_COLLECTION_NAME_STRING = "pathes";

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
