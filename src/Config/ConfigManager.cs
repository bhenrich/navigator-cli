using Tomlyn;

namespace Navigator.Config
{
    public static class ConfigManager
    {
        private static readonly string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/navigator/config.toml");

        public static NavigatorConfig LoadConfig()
        {
            if (!File.Exists(configPath))
            {
                var defaultConfig = GetDefaultConfig();
                SaveConfig(defaultConfig);
                return defaultConfig;
            }

            var tomlContent = File.ReadAllText(configPath);
            return Toml.ToModel<NavigatorConfig>(tomlContent);
        }

        public static void SaveConfig(NavigatorConfig config)
        {
            var tomlContent = Toml.FromModel(config);
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, tomlContent);
        }

        private static NavigatorConfig GetDefaultConfig()
        {
            return new NavigatorConfig
            {
                Colors = new ColorsConfig
                {
                    DirectoryColor = "#0000FF",
                    FileColor = "#FCBA03",
                    HiddenDirectoryColor = "#888888",
                    HiddenFileColor = "#888888",
                    SelectedBackgroundColor = "#000000",
                    SelectedForegroundColor = "#FFFFFF"
                },
                Keybinds = new KeybindsConfig
                {
                    EntryUp = "UpArrow",
                    EntryDown = "DownArrow",
                    // Open = "Enter",
                    DirDown = "RightArrow",
                    DirUp = "LeftArrow",
                    Exit = "Escape"
                }
            };
        }
    }
}