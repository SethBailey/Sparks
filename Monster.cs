using System;
using System.Collections.Generic;

namespace game
{
    
    class Monster : Character
    {
        List<Attack> monsterAttacks = new List<Attack>(); 

        public Monster(string name, int HpMin, int HpMax, int reaction, /*string strength,  string weakness, */ int gold,/*, string spices*/ int xp) 
            : base(name, new Random().Next(HpMin,HpMax+1), reaction, /*strength, weakness, */gold)
        {
            //this.spices = spices;
            this.xp = xp;
        }

        //public string spices { get; }
        public int xp { get; private set; }

        internal List<Attack> GetAttacks()
        {
            var monsterAttacks = new List<Attack>();

            var lines = Utils.LoadCSVFile($"./Config/MonsterAttacks.csv");            
            foreach (var values in lines)
            {
                var monsterName = values[0];
                var attackName = values[1];
                var damage = int.Parse(values[2]);

               if (monsterName == name)
               {
                    monsterAttacks.Add(new Attack(attackName, damage));
               }
            }   

            return monsterAttacks;
        }

        internal void AddAttack(Attack attack)
        {
            monsterAttacks.Add(attack);
        }
        
        internal override Attack displayAttack()
        {
            List<Attack> monsterAttacks = GetAttacks();
            int count = monsterAttacks.Count;

            if (count == 0)
            {
                TypeWriter.WriteLine(new Text($"The {name} uses the hurl insult attack"));
                return new Attack("hurl insult",2);
            }

            //Let the monster Choose
            Random rnd = new Random();
            int monsterChoice = rnd.Next(0, count);
            Attack attack = monsterAttacks[monsterChoice];

            TypeWriter.WriteLine(new Text($"The {name} {attack.name}"));

            //return his choice
            return attack;
        }
    } 
}
