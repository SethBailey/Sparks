namespace game
{
    class Armour : Item
    {
        public Armour( string name, int price, int protection, string discription)
        {
            this.name = name;
            this.price = price;
            this.protection = protection;
            this.discription = discription;

            this.ItemDescription = new Text($"{name} " ,Colours.Protection);
        }

        public int price { get; }
        public int protection { get; }
        public string discription { get; }
    }
}
