using System.Configuration;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// Represents a collection of <see cref="ModelRegistrationElement"/> configuration elements.
    /// </summary>
    public class ModelRegistrationCollectionSection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a <see cref="ModelRegistrationElement"/> instance.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModelRegistrationElement();
        }

        /// <summary>
        /// Gets the element key for a specified <see cref="ModelRegistrationElement"/> instance.
        /// </summary>
        /// <param name="element">The <see cref="ModelRegistrationElement"/> to return the key for.</param>
        /// <returns>
        /// Path of the specified <see cref="ModelRegistrationElement"/> instance that acts as the key.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModelRegistrationElement)element).Path;
        }
    }
}
