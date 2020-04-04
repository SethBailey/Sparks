namespace game
{
    class Weapon
    {
        public Weapon( string name, int price, int damage, string discription)
        {
            this.name = name;
            this.price = price;
            this.damage = damage;
            this.discription = discription;
        }

        public int price {get;}
        public int damage {get;}
        public string discription { get; }
        public string name {get;}
    }
}
