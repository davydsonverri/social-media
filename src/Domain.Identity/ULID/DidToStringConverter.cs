using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Identity.ULID
{
    public class DidToStringConverter : ValueConverter<Did, string>
    {
        private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 26);

        public DidToStringConverter() : this(null)
        {
        }

        public DidToStringConverter(ConverterMappingHints? mappingHints = null)
            : base(
                    convertToProviderExpression: x => x.ToString(),
                    convertFromProviderExpression: x => Did.Parse(x),
                    mappingHints: defaultHints.With(mappingHints))
        {
        }
    }
}
