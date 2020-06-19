using System;
using System.Linq;

namespace game
{
    class Medicine : Item
    {
        public Medicine( string name, int price, int healing, string description )
        {
            this.name = new Text(name, Colours.Medicine);
            this.price = price;
            this.healing = healing;
            this.description = description;
        }

        public int price { get; }
        public int healing { get; }
        public string description { get; }

        internal override bool DoVerb(string verb, TheGame game)
        {
            if( isSynonymFor(verb,"eat") )  
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine(new Text($"You consume the "),
                                     name,
                                     new Text(" and it feels good"));
                game.playerHP += healing;
                return true;
            }

            return false;
        }
    }
}
