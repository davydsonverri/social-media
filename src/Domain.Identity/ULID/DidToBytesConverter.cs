using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Identity.ULID
{
    public class DidToBytesConverter : ValueConverter<Did, byte[]>
    {
        private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 16);

        public DidToBytesConverter() : this(mappingHints: null)
        {
        }

        public DidToBytesConverter(ConverterMappingHints? mappingHints = null)
            : base(
                    convertToProviderExpression: x => x.ToByteArray(),
                    convertFromProviderExpression: x => new Did(x),
                    mappingHints: defaultHints.With(mappingHints))
        {
        }
    }
}
