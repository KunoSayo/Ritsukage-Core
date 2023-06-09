using System.Runtime.Serialization;
using Tomlyn.Model;

namespace RUCore.Config
{
    public partial class Config
    {
        /// <summary>
        /// Config for cache
        /// </summary>
        public List<CacheLayerConfigTable> Cache { get; } = new();

        /// <summary>
        /// Auto cleanup cache frequency
        /// </summary>
        public TimeSpan? CleanupFrequency { get; set; }

        public class CacheLayerConfigTable : ITomlMetadataProvider
        {
            /// <summary>
            /// Type of cache
            /// </summary>
            public string Type { get; init; } = string.Empty;

            /// <summary>
            /// Cache file path
            /// </summary>
            public string? Path { get; init; }

            /// <summary>
            /// Cache expire time
            /// </summary>
            [DataMember(Name = "expire")]
            public long? ExpireTime { get; init; }

            TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }

            public static CacheLayerConfigTable MemoryLayerConfigTable()
            {
                return new()
                {
                    Type = "memory"
                };
            }

            public static CacheLayerConfigTable FileLayerConfigTable(string path, TimeSpan? expireTime = null)
            {
                return new()
                {
                    Type = "file",
                    Path = path,
                    ExpireTime = (long?)expireTime?.TotalSeconds
                };
            }
        }
    }
}
