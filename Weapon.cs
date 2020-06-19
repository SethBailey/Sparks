namespace game
{
    class Weapon : Item
    {
        public Weapon( string name, int price, int damage, string description)
        {
            this.name = new Text(name, Colours.Attack);
            this.price = price;
            this.damage = damage;
            this.description = description;

            this.ItemDescription = new Text($"{name} ",Colours.Attack);
        }

        public int price {get;}
        public int damage {get;}
        public string description { get; }


    }
}
