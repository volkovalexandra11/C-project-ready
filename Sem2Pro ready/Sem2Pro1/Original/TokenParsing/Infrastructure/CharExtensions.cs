namespace TokenParsing.Infrastructure
{
    public static class CharExtensions
    {
        public static bool IsDecimalSign(this char c)
        {
            return c == ',' || c == '.';
        }

        public static bool IsOpeningBracket(this char c)
        {
            return c == '(' || c == '[' || c == '{';
        }

        public static bool IsClosingBracket(this char c)
        {
            return c == ')' || c == ']' || c == '}';
        }
    }
}