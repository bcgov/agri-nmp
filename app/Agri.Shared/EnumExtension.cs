using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri.Shared
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First().GetCustomAttribute<DescriptionAttribute>()
                            ?.Description ?? enumValue.ToString();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First().GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? enumValue.ToString();
        }
    }
}