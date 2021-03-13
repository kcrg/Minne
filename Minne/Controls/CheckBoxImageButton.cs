using Xamarin.Forms;

namespace Minne.Controls
{
    public class CheckBoxImageButton : ImageButton
    {
        private readonly FontImageSource fontImageSource = new();

        public CheckBoxImageButton()
        {
            Application.Current.Resources.TryGetValue("UncheckIcon", out object uncheckIcon);

            fontImageSource.FontFamily = "FontIcons";
            fontImageSource.Glyph = uncheckIcon.ToString();
            Source = fontImageSource;

            Clicked += CheckBoxImageButton_Clicked;
        }

        private void CheckBoxImageButton_Clicked(object sender, System.EventArgs e)
        {
            IsChecked = !IsChecked;
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBoxImageButton), default(bool), BindingMode.TwoWay);
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckBoxImageButton), default(Color), BindingMode.OneWay);
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsCheckedProperty.PropertyName)
            {
                object icon;

                if (IsChecked)
                {
                    Application.Current.Resources.TryGetValue("CheckIcon", out icon);
                }
                else
                {
                    Application.Current.Resources.TryGetValue("UncheckIcon", out icon);
                }

                fontImageSource.Glyph = icon.ToString();
            }
            else if (propertyName == ColorProperty.PropertyName)
            {
                fontImageSource.Color = Color;
            }
        }
    }
}