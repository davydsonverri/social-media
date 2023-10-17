using System.ComponentModel;
using System.Globalization;

namespace System
{
    public class DidTypeConverter : TypeConverter
    {
        private static readonly Type StringType = typeof(string);

        private static readonly Type GuidType = typeof(Guid);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == StringType || sourceType == GuidType)
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == StringType || destinationType == GuidType)
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is Guid)
            {
                Guid guid = (Guid)value;
                return new Did(guid);
            }

            string text = value as string;
            if (text != null)
            {
                return Did.Parse(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Did)
            {
                Did did = (Did)value;
                if (destinationType == StringType)
                {
                    return did.ToString();
                }

                if (destinationType == GuidType)
                {
                    return did.ToGuid();
                }
            }

            return base.ConvertTo(context, culture, value, destinationType)!;
        }
    }
}
