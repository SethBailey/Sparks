namespace game
{
    class Armour
    {
        public Armour( string name, int price, int protection, string discription)
        {
            this.name = name;
            this.price = price;
            this.protection = protection;
            this.discription = discription;
        }

        public string name { get; }
        public int price { get; }
        public int protection { get; }
        public string discription { get; }
    }
}
