namespace game
{
    using System;
    using System.Linq;

    internal class Item : IEquatable<Item>
    {
        public Item()
        {
            this.name = new Text("");
            this.ItemDescription = new Text("");
            this.firstDescription = new Text("");
        }

        virtual internal Text ItemDescription { get; set; }
        internal Text name {get; set;}
        virtual internal Text firstDescription { get; set; }

        public bool Equals(Item other)
        {
            return other.name == this.name;
        }

        internal virtual bool DoVerb(string verb, TheGame game)
        {
            return true;
        }

        internal virtual bool ItemPriceDecisions(TheGame game)
        {
            return true;
        }
        
        public static bool isSynonymFor(string verb, string synonym)
        {
            if (verb == synonym)
            {
                return true;
            }

            string[] lines = System.IO.File.ReadAllLines("synonyms.txt");

            foreach (var line in lines)
            {
                var split = line.Split("=");
                var word = split[0];
                if (word != synonym)
                {
                    continue;
                }
                var synonyms = split[1];
                var synonymsSplit = synonyms.Split(",").ToList();
                return synonymsSplit.Exists( e => e == verb );
            }
            return false;
        }

    }
}