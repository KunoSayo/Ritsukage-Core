using Tomlyn;
using Tomlyn.Model;

namespace RUCore.Config
{
    public partial class Config : ITomlMetadataProvider
    {
        TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }

        /// <summary>
        /// Save config to file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task Save(string path)
        {
            var data = Toml.FromModel(this);
            return File.WriteAllTextAsync(path, data);
        }

        /// <summary>
        /// Load config from file
        /// </summary>
        /// <param name="path">path for config file</param>
        /// <returns></returns>
        public static Config Load(string path)
        {
            var data = File.ReadAllText(path);
            return Toml.Parse(data).ToModel<Config>();
        }
    }
}
