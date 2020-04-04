namespace game
{
    class Medicine
    {
        public Medicine( string name, int price, int healing, string discription)
        {
            this.name = name;
            this.price = price;
            this.healing = healing;
            this.discription = discription;
        }

        public string name { get; }
        public int price { get; }
        public int healing { get; }
        public string discription { get; }
    }
}
