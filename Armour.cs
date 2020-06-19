namespace game
{
    class Armour : Item
    {
        public Armour( string name, int price, int protection, string description)
        {
            this.name = new Text(name, Colours.Armour);
            this.price = price;
            this.protection = protection;
            this.description = description;

            this.ItemDescription = new Text($"{name} " ,Colours.Protection);
        }

        public int price { get; }
        public int protection { get; }
        public string description { get; }
    }
}
