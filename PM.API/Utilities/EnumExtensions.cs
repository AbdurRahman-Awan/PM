namespace PM.API.Utilities
{
    public static class EnumExtensions
    {
        public static bool TryParseEnum<T>(this string value, out T enumValue) where T : struct, Enum
        {
            return Enum.TryParse(value, true, out enumValue) && Enum.IsDefined(typeof(T), enumValue);
        }
    }

}
