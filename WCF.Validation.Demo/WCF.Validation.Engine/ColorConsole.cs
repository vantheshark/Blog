using System;

namespace WCF.Validation.Engine
{
    public static class ColorConsole
    {
        public static void WriteLine(ConsoleColor color, string text, params object[] args)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, args);
            Console.ForegroundColor = currentColor;
        }
    }
}
