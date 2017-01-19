using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    public class MockRegistrationCollectionSection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MockRegistrationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MockRegistrationElement)element).Path;
        }
    }
}
