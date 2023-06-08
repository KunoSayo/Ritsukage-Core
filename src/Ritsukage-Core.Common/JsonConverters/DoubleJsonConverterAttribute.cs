using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Attribute for double to string converter
    /// </summary>
    public class DoubleJsonConverterAttribute : JsonConverterAttribute
    {
        private readonly int _precision;

        /// <summary>
        /// Attribute for double to string converter
        /// </summary>
        /// <param name="precision"></param>
        public DoubleJsonConverterAttribute(int precision) : base(null!)
        {
            _precision = precision;
        }

        /// <summary>
        /// CreateConverter
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            return new Double2StringJsonConverter(_precision);
        }
    }
}
