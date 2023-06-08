using System.Text.Json;

namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Extension methods for JsonElement
    /// </summary>
    public static class JsonElementExtensions
    {
        /// <summary>
        /// Check if the JsonElement has any values
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool HasValues(this JsonElement j)
        {
            return j.ValueKind == JsonValueKind.Array
                ? j.EnumerateArray().Any()
                : j.ValueKind == JsonValueKind.Object && j.EnumerateObject().Any();
        }
    }
}
