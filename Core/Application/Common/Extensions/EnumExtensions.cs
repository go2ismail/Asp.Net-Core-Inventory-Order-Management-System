using System.ComponentModel;
using System.Reflection;

namespace Application.Common.Extensions;

public static class EnumExtensions
{
    public static string ToFriendlyName<TEnum>(this TEnum value) where TEnum : Enum
    {
        var memberInfo = typeof(TEnum).GetMember(value.ToString());
        var descriptionAttribute = memberInfo[0]
            .GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute?.Description ?? value.ToString();
    }
}