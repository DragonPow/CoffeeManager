using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MainProject.ValidationRules
{
    public class NotNullableValidationRule : ValidationRule
    {
        public String ErrorMessage { get; set; } = "This field is required";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool rs = false;
            if (value != null)
            {
                if (value.GetType() == typeof(String))
                {
                    String input = value.ToString();
                    rs = (input.Length > 0) && !string.IsNullOrWhiteSpace(input);
                }
                else { ErrorMessage = "This type cannot be check : " + value.GetType().ToString(); }
            }

            if (rs)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, ErrorMessage);
            }
        }
    }
}
