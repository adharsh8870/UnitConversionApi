using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models
{
    public class ConversionRequest
    {
        [Required]
        public ConversionCategory Category { get; set; }

        [Required]
        public string FromUnit { get; set; } = string.Empty;

        [Required]
        public string ToUnit { get; set; } = string.Empty;

        [Required]
        public double Value { get; set; }
    }

}
