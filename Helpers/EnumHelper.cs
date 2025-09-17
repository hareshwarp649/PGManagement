
using bca.api.DTOs;
using System.ComponentModel;
using System.Reflection;

namespace bca.api.Helpers
{
    public static class EnumHelper
    {
        public static List<EnumValueDTO> GetEnumValuesWithDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new EnumValueDTO
            {
                IntValue = Convert.ToInt32(e),
                Value = e.ToString(),
                Description = e.GetType().GetField(e.ToString())
                    .GetCustomAttribute<DescriptionAttribute>()?.Description ?? e.ToString()
            }).ToList();
        }

        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
