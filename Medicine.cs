using System;
using System.Linq;

namespace game
{
    class Medicine : Item
    {
        public Medicine( string name, int price, int healing, string description, string firstDescription ="" )
        {
            this.name = new Text(name, Colours.Medicine);
            this.price = price;
            this.healing = healing;
            this.ItemDescription = new Text(description);
            this.firstDescription = new Text(firstDescription);
            
        }

        public int price { get; }
        public int healing { get; }

        internal override bool DoVerb(string verb, TheGame game)
        {
            if( isSynonymFor(verb,"eat") )  
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine(new Text($"You {verb} the "),
                                     name,
                                     new Text(" and it feels good"));
                game.playerHP += healing;
                return true;
            }

            return false;
        }
    }
}
