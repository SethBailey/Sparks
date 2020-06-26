using System;
using static game.TypeWriter;

namespace game
{
    class Text : IEquatable<Text>
    {

        public Text( string text, ConsoleColor foregroundColor = ConsoleColor.White, Speed speed = Speed.List, ConsoleColor backgroundColour = ConsoleColor.Black)
        {
            this.text = text;
            this.ForegroundColor = foregroundColor;
            this.speed = speed;
            this.backgroundColour = backgroundColour;
        }

        public string text { get; }

        public ConsoleColor ForegroundColor { get; }

        public Speed speed { get; }
        
        public ConsoleColor backgroundColour { get; }

        public static Text operator + ( Text txt, string str)
        {
            return new Text( txt.text + str, txt.ForegroundColor, txt.speed, txt.backgroundColour);
        } 

        public static Text operator + (string str, Text txt)
        {
            return new Text( str + txt.text, txt.ForegroundColor, txt.speed, txt.backgroundColour);
        } 

        public bool Equals(Text other)
        {
            return other.text == this.text;
        }

        // override object.Equals
        public override bool Equals(object obj)
        { 
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return Equals( obj );
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.text.GetHashCode();
        }

        public static bool operator == (Text first, Text second)
        {
            return first.Equals(second);
        }

        public static bool operator != (Text first, Text second)
        {
            return !first.Equals(second);
        }

    }
}
