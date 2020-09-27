using System;
using System.Collections.Generic;

namespace game
{
    internal class Player : Character
    {
        private readonly List<ItemCount> inventory;

        public Player(string name, int HP, int reaction, /*string strength, string weakness, */ int gold, List<ItemCount> inventory) : base(name, HP, reaction, /*strength, weakness,*/ gold)
        {
            this.XP = 0;
            this.inventory = inventory;
        }
        public int XP { get; set; }

        internal List<Attack> GetAttacks()
        {
            //get from inventory
            var attacks = new List<Attack>();
            attacks.Add( new Attack("punch", 10));

            foreach(var itemCount in inventory)
            {
                if (itemCount.item is Weapon)
                {
                    attacks.Add(((Weapon)itemCount.item).getAttack());
                }
            }

            return attacks;
        }


        internal override Attack displayAttack()
        {
            List<Attack> attacks = GetAttacks();
            TypeWriter.WriteLine("Possible attacks");
            int count = 1;
            foreach (var attack in attacks)
            {
                TypeWriter.WriteLine(new Text($"[{count}] "),
                                     new Text($"{attack.name}", Colours.Damage));
                count++;
            }

            //Let the palyer Choose
            var choice = Console.ReadLine();
            int playerChoice = int.Parse(choice);

            //return his choice
            return attacks[playerChoice-1];
        }
    }
}