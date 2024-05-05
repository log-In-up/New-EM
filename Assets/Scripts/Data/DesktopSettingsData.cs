namespace Assets.Scripts.Data
{
    public class DesktopSettingsData : SettingsData
    {
        public int Height, Width;
        public bool Fullscreen;

        public DesktopSettingsData() : base()
        {
            Fullscreen = true;
        }
    }
}