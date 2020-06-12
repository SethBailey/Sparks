namespace game
{
    class Weapon : Item
    {
        public Weapon( string name, int price, int damage, string discription)
        {
            this.name = name;
            this.price = price;
            this.damage = damage;
            this.discription = discription;

            this.ItemDescription = new Text($"{name} ",Colours.Attack);
        }

        public int price {get;}
        public int damage {get;}
        public string discription { get; }


    }
}
