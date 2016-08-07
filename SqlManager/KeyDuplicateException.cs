using System;

namespace Franksoft.SqlManager
{
    public class KeyDuplicateException : Exception
    {
        private const string MESSAGE_TEMPLATE = "The key value: \"{0}\" is duplicated.";

        public KeyDuplicateException(string keyValue) : base(string.Format(MESSAGE_TEMPLATE, keyValue))
        {
            this.KeyValue = keyValue;
        }

        public KeyDuplicateException(object keyValue) : base(string.Format(MESSAGE_TEMPLATE, keyValue.ToString()))
        {
            this.KeyValue = keyValue;
        }

        public KeyDuplicateException(string message, object keyValue) : base(message)
        {
            this.KeyValue = keyValue;
        }

        public object KeyValue { get; private set; }
    }
}
