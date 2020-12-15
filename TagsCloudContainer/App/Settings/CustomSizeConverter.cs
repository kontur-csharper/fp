using System;
using System.ComponentModel;
using System.Globalization;

namespace TagsCloudContainer.App.Settings
{
    public class CustomSizeConverter : ExpandableObjectConverter
    {
        private readonly TypeConverter converter;

        public CustomSizeConverter()
        {
            converter = TypeDescriptor.GetConverter(typeof(int));
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return converter.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            var convertedObj = converter.ConvertFrom(context, culture, value);
            if ((int)convertedObj <= 0)
                throw new ArgumentException("Width and height should be greater than 0");
            return convertedObj;
        }
    }
}
