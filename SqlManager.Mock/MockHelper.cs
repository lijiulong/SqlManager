using System;

namespace Franksoft.SqlManager.Mock
{
    /// <summary>
    /// Provides public helper methods for mock classes.
    /// </summary>
    public static class MockHelper
    {
        /// <summary>
        /// Converts raw data string value to specified data type.
        /// </summary>
        /// <param name="type">Target type to convert the raw data string.</param>
        /// <param name="rawDataToConvert">Raw data string value to convert.</param>
        /// <returns>Converted value in specified data type.</returns>
        public static object ConvertToType(Type type, string rawDataToConvert)
        {
            object result = rawDataToConvert;

            if (type == typeof(bool))
            {
                result = bool.Parse(rawDataToConvert);
            }
            else if (type == typeof(byte))
            {
                result = byte.Parse(rawDataToConvert);
            }
            else if (type == typeof(short))
            {
                result = short.Parse(rawDataToConvert);
            }
            else if (type == typeof(int))
            {
                result = int.Parse(rawDataToConvert);
            }
            else if (type == typeof(long))
            {
                result = long.Parse(rawDataToConvert);
            }
            else if (type == typeof(float))
            {
                result = float.Parse(rawDataToConvert);
            }
            else if (type == typeof(double))
            {
                result = double.Parse(rawDataToConvert);
            }
            else if (type == typeof(DateTime))
            {
                result = DateTime.Parse(rawDataToConvert);
            }

            return result;
        }
    }
}
