using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace TimeManagement.Themes
{
    [MarkupExtensionReturnType(typeof(FontFamily))]
    public class MaterialDesignFontSCExtension : MarkupExtension
    {
        private static readonly Lazy<FontFamily> _notosanssc
            = new Lazy<FontFamily>(() =>
                new FontFamily(new Uri("pack://application:,,,/Resources/NotoSansSC/"), "./#Noto Sans SC"));

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _notosanssc.Value;
        }
    }
}