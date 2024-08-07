namespace Navigator.Config
{
    public class NavigatorConfig
    {
        public ColorsConfig Colors { get; set; }
        public KeybindsConfig Keybinds { get; set; }
    }

    public class ColorsConfig
    {
        public string DirectoryColor { get; set; }
        public string FileColor { get; set; }
        public string HiddenDirectoryColor { get; set; }
        public string HiddenFileColor { get; set; }
        public string SelectedBackgroundColor { get; set; }
        public string SelectedForegroundColor { get; set; }
    }

    public class KeybindsConfig
    {
        public string EntryUp { get; set; }
        public string EntryDown { get; set; }
        public string Open { get; set; }
        public string DirDown { get; set; }
        public string DirUp { get; set; }
        public string Exit { get; set; } // New keybind for exiting the tool
    }
}