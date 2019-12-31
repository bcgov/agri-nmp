using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First().GetCustomAttribute<DescriptionAttribute>()
                            .Description;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First().GetCustomAttribute<DisplayAttribute>()
                            .Name;
        }
    }
}