Unit Conversion API

This ASP.NET Core Web API converts numeric values between units for length, temperature, and weight.

Run locally

1. From repository root run:

   dotnet run --project UnitConversionApi/UnitConversionApi.csproj

2. The app exposes these endpoints:

- POST /api/convert - perform conversion
- GET  /api/convert/units - list supported units per category

3. Example POST (curl):

curl -k -X POST "https://localhost:7271/api/Convert" -H "Content-Type: application/json" -d '{ "category": "Length", "fromUnit": "meter", "toUnit": "foot", "value": 1.0 }'

Example GET units (curl):

curl -k "https://localhost:7271/api/Convert/units"

Design notes

- Units are hardcoded for this exercise and stored as dictionaries in UnitConversionService.
- Length and weight use base units (meters, kilograms) with linear factors. Temperature uses dedicated formulas.
- The service is registered as a singleton and is stateless.
