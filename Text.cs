using System;
using static game.TypeWriter;

namespace game
{
    class Text
    {

        public Text( string text, ConsoleColor color = ConsoleColor.White, Speed speed = Speed.List)
        {
            this.text = text;
            this.ConsoleColor = color;
            this.speed = speed;
        }

        public string text { get; }

        public ConsoleColor ConsoleColor {get;}

        public Speed speed { get; }
    }
}
