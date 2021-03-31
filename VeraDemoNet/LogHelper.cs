namespace VeraDemoNet
{
    public static class StringExtensions
    {
        public static string ToLogStatement(this string message)
        {
            var maxLength = 30;

            if (message.Length > maxLength)
            {
                message = message.Substring(0, maxLength);
            }

            return message = message
                .Replace("\r", "-")
                .Replace("\n", "-");
        }
    }
}