using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FinActions.Domain.Shared.Extensions;

public static class AttributeExtensions
{
    public static string GetDisplayNameAttribute<T>(this string fieldName)
    {
        MemberInfo[] info = typeof(T).GetMember(fieldName);
        if (info != null && info.Length > 0)
        {
            object[] attrs = info[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                return ((DisplayAttribute)attrs[0]).Name;
            }
        }

        return fieldName;
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());

        if (memberInfo is null || memberInfo!.Length == 0)
        {
            return enumValue.ToString();
        }

        object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

        if (attrs.Length == 0)
        {
            return enumValue.ToString();
        }

        var displayAttribute = (DisplayAttribute)attrs[0];
        return displayAttribute.Name ?? displayAttribute.Description!;
    }
}
