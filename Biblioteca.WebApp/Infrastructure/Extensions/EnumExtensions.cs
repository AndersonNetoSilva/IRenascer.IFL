namespace IFL.WebApp.Infrastructure.Extensions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?
                .GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null) return string.Empty;

            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

                if (displayAttribute != null)
                {
                    return displayAttribute.Name;
                }
            }

            // Se não achar o [Display], retorna o nome normal como fallback
            return enumValue.ToString();
        }
    }
}
