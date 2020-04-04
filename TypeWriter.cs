using System;
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

        static public void WriteLine(string message, Speed speed = Speed.List, int wait = 0 )
        {
            if (speed == Speed.Immediate)
            {
                Console.WriteLine(message);
                return;
            }

            foreach ( var c in message)
            {
                Console.Write(c);
                Thread.Sleep((int)speed);
            }
            Console.WriteLine();
            Thread.Sleep(wait);
        }
    }
}
