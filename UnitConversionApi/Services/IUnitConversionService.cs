using UnitConversionApi.Models;

namespace UnitConversionApi.Services
{
    public interface IUnitConversionService
    {
        bool TryConvert(ConversionRequest request, out ConversionResponse response, out string? error);
        IReadOnlyDictionary<ConversionCategory, IEnumerable<string>> GetSupportedUnits();
    }
}
