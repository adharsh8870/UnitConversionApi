using System.Globalization;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services
{
    public class UnitConversionService : IUnitConversionService
    {
        private readonly Dictionary<string, double> _lengthToMeters = new(StringComparer.OrdinalIgnoreCase)
        {
            { "meter", 1 },
            { "meters", 1 },
            { "m", 1 },
            { "kilometer", 1000 },
            { "kilometers", 1000 },
            { "km", 1000 },
            { "centimeter", 0.01 },
            { "centimeters", 0.01 },
            { "cm", 0.01 },
            { "millimeter", 0.001 },
            { "mm", 0.001 },
            { "inch", 0.0254 },
            { "inches", 0.0254 },
            { "in", 0.0254 },
            { "foot", 0.3048 },
            { "feet", 0.3048 },
            { "ft", 0.3048 },
            { "yard", 0.9144 },
            { "yd", 0.9144 },
            { "mile", 1609.344 },
            { "mi", 1609.344 }
        };

        private readonly Dictionary<string, double> _volumeToCubicMeters = new(StringComparer.OrdinalIgnoreCase)
        {
            { "cubicmeter", 1 }, { "cubicmeters", 1 }, { "m3", 1 }, { "m^3", 1 },
            { "liter", 0.001 }, { "liters", 0.001 }, { "l", 0.001 },
            { "milliliter", 0.000001 }, { "ml", 0.000001 },
            { "gallon", 0.003785411784 }, { "gallons", 0.003785411784 }, { "gal", 0.003785411784 }
        };

        private readonly Dictionary<string, double> _areaToSquareMeters = new(StringComparer.OrdinalIgnoreCase)
        {
            { "squaremeter", 1 }, { "squaremeters", 1 }, { "m2", 1 }, { "m^2", 1 },
            { "squarefoot", 0.09290304 }, { "squarefeet", 0.09290304 }, { "ft2", 0.09290304 },
            { "acre", 4046.8564224 }
        };

        private readonly Dictionary<string, double> _speedToMetersPerSecond = new(StringComparer.OrdinalIgnoreCase)
        {
            { "m/s", 1 }, { "mps", 1 }, { "meterpersecond", 1 },
            { "km/h", 1000.0/3600.0 }, { "kph", 1000.0/3600.0 }, { "kmh", 1000.0/3600.0 },
            { "mph", 1609.344/3600.0 }
        };

        private readonly Dictionary<string, double> _weightToKilograms = new(StringComparer.OrdinalIgnoreCase)
        {
            { "kilogram", 1 },
            { "kilograms", 1 },
            { "kg", 1 },
            { "gram", 0.001 },
            { "g", 0.001 },
            { "milligram", 0.000001 },
            { "mg", 0.000001 },
            { "pound", 0.45359237 },
            { "lbs", 0.45359237 },
            { "pounds", 0.45359237 },
            { "lb", 0.45359237 },
            { "ounce", 0.028349523125 },
            { "oz", 0.028349523125 }
        };

        public bool TryConvert(ConversionRequest request, out ConversionResponse response, out string? error)
        {
            response = new ConversionResponse { Input = request.Value, FromUnit = request.FromUnit, ToUnit = request.ToUnit };
            error = null;

            double output;
            switch (request.Category)
            {
                case ConversionCategory.Length:
                    if (!TryConvertLength(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                case ConversionCategory.Weight:
                    if (!TryConvertWeight(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                case ConversionCategory.Temperature:
                    if (!TryConvertTemperature(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                case ConversionCategory.Volume:
                    if (!TryConvertVolume(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                case ConversionCategory.Area:
                    if (!TryConvertArea(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                case ConversionCategory.Speed:
                    if (!TryConvertSpeed(request.Value, request.FromUnit, request.ToUnit, out output, out error))
                        return false;
                    response.Output = output;
                    return true;
                default:
                    error = "Unsupported category.";
                    return false;
            }
        }

        public IReadOnlyDictionary<ConversionCategory, IEnumerable<string>> GetSupportedUnits()
        {
            return new Dictionary<ConversionCategory, IEnumerable<string>>
            {
                { ConversionCategory.Length, _lengthToMeters.Keys },
                { ConversionCategory.Weight, _weightToKilograms.Keys },
                { ConversionCategory.Temperature, new[] { "Celsius", "Fahrenheit", "Kelvin" } },
                { ConversionCategory.Volume, _volumeToCubicMeters.Keys },
                { ConversionCategory.Area, _areaToSquareMeters.Keys },
                { ConversionCategory.Speed, _speedToMetersPerSecond.Keys }
            };
        }

        private bool TryConvertVolume(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            if (!_volumeToCubicMeters.TryGetValue(fromUnit, out var fromFactor))
            {
                error = $"Unknown volume unit '{fromUnit}'.";
                return false;
            }
            if (!_volumeToCubicMeters.TryGetValue(toUnit, out var toFactor))
            {
                error = $"Unknown volume unit '{toUnit}'.";
                return false;
            }

            var inCubicMeters = value * fromFactor;
            result = inCubicMeters / toFactor;
            return true;
        }

        private bool TryConvertArea(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            if (!_areaToSquareMeters.TryGetValue(fromUnit, out var fromFactor))
            {
                error = $"Unknown area unit '{fromUnit}'.";
                return false;
            }
            if (!_areaToSquareMeters.TryGetValue(toUnit, out var toFactor))
            {
                error = $"Unknown area unit '{toUnit}'.";
                return false;
            }

            var inSqMeters = value * fromFactor;
            result = inSqMeters / toFactor;
            return true;
        }

        private bool TryConvertSpeed(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            if (!_speedToMetersPerSecond.TryGetValue(fromUnit, out var fromFactor))
            {
                error = $"Unknown speed unit '{fromUnit}'.";
                return false;
            }
            if (!_speedToMetersPerSecond.TryGetValue(toUnit, out var toFactor))
            {
                error = $"Unknown speed unit '{toUnit}'.";
                return false;
            }

            var inMps = value * fromFactor;
            result = inMps / toFactor;
            return true;
        }

        private bool TryConvertLength(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            if (!_lengthToMeters.TryGetValue(fromUnit, out var fromFactor))
            {
                error = $"Unknown length unit '{fromUnit}'.";
                return false;
            }
            if (!_lengthToMeters.TryGetValue(toUnit, out var toFactor))
            {
                error = $"Unknown length unit '{toUnit}'.";
                return false;
            }

            // Convert to meters then to target
            var inMeters = value * fromFactor;
            result = inMeters / toFactor;
            return true;
        }

        private bool TryConvertWeight(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            if (!_weightToKilograms.TryGetValue(fromUnit, out var fromFactor))
            {
                error = $"Unknown weight unit '{fromUnit}'.";
                return false;
            }
            if (!_weightToKilograms.TryGetValue(toUnit, out var toFactor))
            {
                error = $"Unknown weight unit '{toUnit}'.";
                return false;
            }

            var inKg = value * fromFactor;
            result = inKg / toFactor;
            return true;
        }

        private bool TryConvertTemperature(double value, string fromUnit, string toUnit, out double result, out string? error)
        {
            result = 0;
            error = null;
            var f = fromUnit.Trim().ToLowerInvariant();
            var t = toUnit.Trim().ToLowerInvariant();

            // normalize common names
            if (f == "c" ) f = "celsius";
            if (f == "f" ) f = "fahrenheit";
            if (f == "k" ) f = "kelvin";
            if (t == "c" ) t = "celsius";
            if (t == "f" ) t = "fahrenheit";
            if (t == "k" ) t = "kelvin";

            double celsius;
            switch (f)
            {
                case "celsius":
                case "c":
                    celsius = value;
                    break;
                case "fahrenheit":
                case "f":
                    celsius = (value - 32) * 5.0 / 9.0;
                    break;
                case "kelvin":
                case "k":
                    celsius = value - 273.15;
                    break;
                default:
                    error = $"Unknown temperature unit '{fromUnit}'.";
                    return false;
            }

            switch (t)
            {
                case "celsius":
                case "c":
                    result = celsius;
                    return true;
                case "fahrenheit":
                case "f":
                    result = celsius * 9.0 / 5.0 + 32;
                    return true;
                case "kelvin":
                case "k":
                    result = celsius + 273.15;
                    return true;
                default:
                    error = $"Unknown temperature unit '{toUnit}'.";
                    return false;
            }
        }
    }
}
