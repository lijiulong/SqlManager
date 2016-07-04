using System.Configuration;

namespace Franksoft.SqlManager
{
    class ModelFilePathCollectionSection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModelFilePathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModelFilePathElement)element).Path;
        }
    }
}
