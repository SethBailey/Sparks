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

        internal override bool DoVerb(string verb, TheGame game)
        {
            if( isSynonymFor(verb,"equip") )  
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine(new Text($"You {verb} the "),
                                     name,
                                     new Text(" and you feel cool"));
                game.maxDamage += damage;
                return true;
            }
            return false;
        }
    }
}