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
                TypeWriter.WriteLine();
                game.playerStats();
                return true;
            }

            return false;
        }

        internal override bool ItemPriceDecisions(TheGame game)
        {
            if (game.playerGold < price)
            {
                TypeWriter.WriteLine("Sorry but you don't have the required gold");
                return false;
            }
            else
            {
                game.playerGold -= price;
                return true;
            }
        }
    }
}
