using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SERVERAPI.Attributes
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        private string PropertyName { get; set; }
        private object[] DesiredValues { get; set; }

        public RequiredIfAttribute(string propertyName, params object[] desiredValues)
        {
            PropertyName = propertyName;
            DesiredValues = desiredValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            object propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (DesiredValues.Contains(propertyValue))
            {
                return base.IsValid(value, context);
            }

            return ValidationResult.Success;
        }
    }
}