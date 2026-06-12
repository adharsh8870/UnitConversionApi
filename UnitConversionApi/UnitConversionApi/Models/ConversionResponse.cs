namespace UnitConversionApi.Models
{
    public class ConversionResponse
    {
        public double Input { get; set; }
        public string FromUnit { get; set; } = string.Empty;
        public string ToUnit { get; set; } = string.Empty;
        public double Output { get; set; }
    }
}
