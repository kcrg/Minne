using Xamarin.Forms;

namespace Minne.Controls
{
    public partial class BaseImageButton : ImageButton
    {
        private readonly FontImageSource fontImageSource = new FontImageSource();

        public BaseImageButton()
        {
            fontImageSource.FontFamily = "FontIcons";
            Source = fontImageSource;
        }

        public static readonly BindableProperty GlyphProperty = BindableProperty.Create(nameof(Glyph), typeof(string), typeof(BaseImageButton), default(string), BindingMode.OneWay);
        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly BindableProperty GlyphColorProperty = BindableProperty.Create(nameof(GlyphColor), typeof(Color), typeof(BaseImageButton), default(Color), BindingMode.OneWay);
        public Color GlyphColor
        {
            get => (Color)GetValue(GlyphColorProperty);
            set => SetValue(GlyphColorProperty, value);
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == GlyphProperty.PropertyName)
            {
                fontImageSource.Glyph = Glyph;
            }
            else if (propertyName == GlyphColorProperty.PropertyName)
            {
                fontImageSource.Color = GlyphColor;
            }
        }
    }
}