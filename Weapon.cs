using System;

namespace game
{
    class Weapon : Item
    {
        public Weapon( string name, int price, int damage, string description, string firstDescription = "")
        {
            this.name = new Text(name, Colours.Damage);
            this.price = price;
            this.damage = damage;
            this.description = description;
            this.ItemDescription = new Text(description);
            this.firstDescription = new Text(firstDescription);
        }

        public int price {get;}
        public int damage {get;}
        public string description { get; }

        internal override bool ItemPriceDecisions(TheGame game)
        {
            if (game.player.gold < price)
            {
                TypeWriter.WriteLine("Sorry but you don't have the required gold");
                return false;
            }
            else
            {
                game.player.gold -= price;
                return true;
            }
        }

        internal Attack getAttack()
        {
            return new Attack(this.name.text,this.damage);
        }
    }
}