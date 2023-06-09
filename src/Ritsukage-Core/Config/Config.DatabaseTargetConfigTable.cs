using System.Runtime.Serialization;
using Tomlyn.Model;

namespace RUCore.Config
{
    public partial class Config
    {
        /// <summary>
        /// Config for database
        /// </summary>
        public DatabaseTargetConfigTable? Database { get; set; }

        public sealed class DatabaseTargetConfigTable : ITomlMetadataProvider
        {
            /// <summary>
            /// Type of database
            /// </summary>
            public string Type { get; init; } = string.Empty;

            /// <summary>
            /// Path to the SQLite database file.
            /// </summary>
            public string? Path { get; init; }

            /// <summary>
            /// Connection string for SQL Server.
            /// </summary>
            [DataMember(Name = "connect")]
            public string? ConnectString { get; init; }

            TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }

            public static DatabaseTargetConfigTable SqliteDatabaseTargetConfigTable(string path)
            {
                return new()
                {
                    Type = "sqlite",
                    Path = path
                };
            }

            public static DatabaseTargetConfigTable SqlServerDatabaseTargetConfigTable(string connectString)
            {
                return new()
                {
                    Type          = "sqlserver",
                    ConnectString = connectString
                };
            }
        }
    }
}
