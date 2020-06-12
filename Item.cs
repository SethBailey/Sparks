namespace game
{
    using System;
    
    internal class Item : IEquatable<Item>
    {
        virtual internal Text ItemDescription { get; set; }
        internal string name {get; set;}

        public bool Equals(Item other)
        {
            return other.name == this.name;
        }
    }
}