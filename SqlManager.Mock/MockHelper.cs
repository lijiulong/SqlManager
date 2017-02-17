using System;

namespace Franksoft.SqlManager.Mock
{
    internal static class MockHelper
    {
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
