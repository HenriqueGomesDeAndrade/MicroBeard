using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Attributes
{
    public class DateFormatValidatorAttribute : RegularExpressionAttribute
    {
        public DateFormatValidatorAttribute()
            : base(@"^(?:\d{4})-(?:\d{2})-(?:\d{2})T(?:\d{2}):(?:\d{2}):(?:\d{2}(?:\.\d*)?)(?:(?:-(?:\d{2}):(?:\d{2})|Z)?)$")
        {
            ErrorMessage = "Invalid datetime format. The value must be passed using the ISO 8061 date format. Example: 2022-10-07T12:48:40.476Z\"";
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}
