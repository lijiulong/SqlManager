using System;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// Represents errors that occur when duplicated keys are detected.
    /// </summary>
    public class KeyDuplicateException : Exception
    {
        /// <summary>
        /// Const string value for template of the error message.
        /// </summary>
        private const string MESSAGE_TEMPLATE = "The key value: \"{0}\" is duplicated.";

        /// <summary>
        /// Initializes <see cref="KeyDuplicateException"/> instance with string type key value.
        /// </summary>
        /// <param name="keyValue">Value of string type duplicated key.</param>
        public KeyDuplicateException(string keyValue) : base(string.Format(MESSAGE_TEMPLATE, keyValue))
        {
            this.KeyValue = keyValue;
        }

        /// <summary>
        /// Initializes <see cref="KeyDuplicateException"/> instance with object type key value.
        /// </summary>
        /// <param name="keyValue">Value of object type duplicated key.</param>
        public KeyDuplicateException(object keyValue) : base(string.Format(MESSAGE_TEMPLATE, keyValue.ToString()))
        {
            this.KeyValue = keyValue;
        }

        /// <summary>
        /// Initializes <see cref="KeyDuplicateException"/> instance with customized error message
        /// and key value.
        /// </summary>
        /// <param name="message">Customized error message.</param>
        /// <param name="keyValue">Value of duplicated key.</param>
        public KeyDuplicateException(string message, object keyValue) : base(message)
        {
            this.KeyValue = keyValue;
        }

        /// <summary>
        /// Gets the value of duplicated key.
        /// </summary>
        public object KeyValue { get; private set; }
    }
}
