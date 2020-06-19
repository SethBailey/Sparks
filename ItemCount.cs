using System;
using System.Collections.Generic;

namespace game
{
    internal class ItemCount
    {
        public ItemCount(Item item)
        {
            this.item = item;

        }

        public Item item {get;} 
        private int count = 1;
        internal Text name {get { return item.name ;} }

        internal Text ItemDescription { 
            get{

                 return item.ItemDescription;

               } }

        internal int Count()
        {
            return count;
        }

        internal void Increment()
        {
            count++;
        }

        internal void Decrement()
        {
            count--;
        }
    }
}