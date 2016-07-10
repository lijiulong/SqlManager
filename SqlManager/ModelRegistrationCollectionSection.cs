using System.Configuration;

namespace Franksoft.SqlManager
{
    public class ModelRegistrationCollectionSection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModelRegistrationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModelRegistrationElement)element).Path;
        }
    }
}
