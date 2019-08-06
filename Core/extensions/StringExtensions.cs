public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
    {
        return s == null || s.Length == 0;
    }
    public static bool IsNullOrWhiteSpace(this string s)
    {
        return IsNullOrEmpty(s) || s.Trim().Length == 0;
    }
}