using System.Configuration;

namespace Franksoft.SqlManager.Mock
{
    /// <summary>
    /// Represents a collection of <see cref="MockRegistrationElement"/> configuration elements.
    /// </summary>
    public class MockRegistrationCollectionSection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a <see cref="MockRegistrationElement"/> instance.
        /// </summary>
        /// <returns>New <see cref="MockRegistrationElement"/> instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MockRegistrationElement();
        }

        /// <summary>
        /// Gets the element key for a specified <see cref="MockRegistrationElement"/> instance.
        /// </summary>
        /// <param name="element">The <see cref="MockRegistrationElement"/> to return the key for.</param>
        /// <returns>
        /// Path of the specified <see cref="MockRegistrationElement"/> instance that acts as the key.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MockRegistrationElement)element).Path;
        }
    }
}
