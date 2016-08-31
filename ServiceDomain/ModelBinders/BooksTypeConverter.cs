using ServiceDomain.DTOs;
using System;
using System.ComponentModel;
using System.Globalization;

namespace ServiceDomain.ModelBinders
{
    public class BooksTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
            {
                uBookDto book;
                if(uBookDto.TryParse((string)value, out book))
                {                    
                    return book;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}