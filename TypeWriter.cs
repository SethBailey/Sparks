using System;
using System.Collections.Generic;
using System.Threading;

namespace game
{
    class TypeWriter
    {
        public enum Speed        
        {
            Talk = 20,
            List = 10,
            Immediate        
        };

        static public void WriteLine()
        {
            Console.WriteLine();
        }

        static public void WriteLine(List<Text> texts)
        {
            WriteLine(texts.ToArray());
        }

        public static void WriteLine(params Text[] texts)
        {
            ConsoleColor originalFg = Console.ForegroundColor;
            ConsoleColor originalBg = Console.BackgroundColor;
            foreach (var text in texts)
            {
                Console.ForegroundColor = text.ForegroundColor;
                Console.BackgroundColor = text.backgroundColour;
                Type(text.text, text.speed);
            }            
            Console.ForegroundColor = originalFg;
            Console.BackgroundColor = originalBg;
            Console.WriteLine();
        }

        public static void WriteLine(string text, Speed speed = Speed.List)
        {
            Type(text, speed);
            Console.WriteLine();
        }

        private static void Type(string text, Speed speed)
        {
            foreach (var c in text)
            {
                Console.Write(c);
                Thread.Sleep((int)speed);
            }
        }
    }
}
