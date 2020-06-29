namespace game
{
    class Armour : Item
    {
        public Armour( string name, int price, int protection, string description, string firstDescription = "")
        {
            this.name = new Text(name, Colours.Armour);
            this.price = price;
            this.protection = protection;
            this.ItemDescription = new Text(description);
            this.firstDescription = new Text(firstDescription);
        }

        public int price { get; }
        public int protection { get; }

        internal override bool DoVerb(string verb, TheGame game)
        {
            if( isSynonymFor(verb,"wear") )  
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine(new Text($"You use the "),
                                     name,
                                     new Text(" and you feel safe"));
                game.protection += protection;
                return true;
            }

            return false;
        }
    }
}
