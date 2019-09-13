using Xamarin.Forms;

namespace FF12RngHelper
{
    /// <summary>
    /// A custom Toggle Button class
    /// </summary>
    public class ToggleSwitch : Switch
    {
        public static readonly BindableProperty SwitchOffColorProperty = BindableProperty.Create(nameof(SwitchOffColor), typeof(Color), typeof(ToggleSwitch), Color.Default);

        public Color SwitchOffColor
        {
            get { return (Color)GetValue(SwitchOffColorProperty); }
            set { SetValue(SwitchOffColorProperty, value); }
        }

        public static readonly BindableProperty SwitchOnColorProperty = BindableProperty.Create(nameof(SwitchOnColor), typeof(Color), typeof(ToggleSwitch), Color.Default);

        public Color SwitchOnColor
        {
            get { return (Color)GetValue(SwitchOnColorProperty); }
            set { SetValue(SwitchOnColorProperty, value); }
        }

        public static readonly BindableProperty SwitchThumbColorProperty = BindableProperty.Create(nameof(SwitchThumbColor), typeof(Color), typeof(ToggleSwitch), Color.Default);

        public Color SwitchThumbColor
        {
            get { return (Color)GetValue(SwitchThumbColorProperty); }
            set { SetValue(SwitchThumbColorProperty, value); }
        }

        public static readonly BindableProperty SwitchThumbImageProperty = BindableProperty.Create(nameof(SwitchThumbImage), typeof(string), typeof(ToggleSwitch), string.Empty);

        public string SwitchThumbImage
        {
            get { return (string)GetValue(SwitchThumbImageProperty); }
            set { SetValue(SwitchThumbImageProperty, value); }
        }
    }
}
