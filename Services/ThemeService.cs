namespace InsightJournal.Services
{
    public class ThemeService
    {
        private string _currentTheme = "dark";

        public event Action? OnThemeChanged;

        public string CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    Preferences.Set("AppTheme", value);
                    OnThemeChanged?.Invoke();
                }
            }
        }

        public ThemeService()
        {
            // Load saved theme preference (default to dark)
            _currentTheme = Preferences.Get("AppTheme", "dark");
        }

        public void SetTheme(string theme)
        {
            CurrentTheme = theme;
        }

        public bool IsDarkMode => _currentTheme == "dark";
    }
}