using System;
using static game.TypeWriter;

namespace game
{
    class Text
    {

        public Text( string text, ConsoleColor foregroundColor = ConsoleColor.White, Speed speed = Speed.List, ConsoleColor backgroundColour = ConsoleColor.Black)
        {
            this.text = text;
            this.ForegroundColor = foregroundColor;
            this.speed = speed;
            this.backgroundColour = backgroundColour;
        }

        public string text { get; }

        public ConsoleColor ForegroundColor {get;}

        public Speed speed { get; }
        
        public ConsoleColor backgroundColour { get; }
    }
}
