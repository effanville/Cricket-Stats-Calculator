using System.Windows.Media;

namespace GUISupport
{
    public static class Colours
    {
        public static Color BackgroundMainColour { get; set; } = Colors.AliceBlue;

        private static Brush fBackgroundBrush = new SolidColorBrush(BackgroundMainColour);
        public static Brush BackgroundBrush
        {
            get { return fBackgroundBrush; }
            set { fBackgroundBrush = value; }
        }

        public static Color BackgroundSecondColour { get; set; } = Colors.LightBlue;

        private static Brush fBackgroundSecondaryBrush = new SolidColorBrush(BackgroundSecondColour);
        public static Brush BackgroundSecondaryBrush
        {
            get { return fBackgroundSecondaryBrush; }
            set { fBackgroundSecondaryBrush = value; }
        }
        public static Color ButtonColour { get; set; } = Colors.Lavender;

        private static Brush fDefaultButtonBrush = new SolidColorBrush(ButtonColour);
        public static Brush DefaultButtonBrush
        {
            get { return fDefaultButtonBrush; }
            set { fDefaultButtonBrush = value; }
        }
    }
}
