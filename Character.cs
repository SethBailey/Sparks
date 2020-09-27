using System;

namespace game
{
    internal class Character
    {
        private Armour? armour;
        public string name { get; set; }
        public int HP { get; set; }
        public int reaction { get; set; }
        //public string strength { get; set; }
        //public string weakness { get; set; }
        public int gold { get; set; }

        public Character(string name, int HP, int reaction, /*string strength, string weakness,*/ int gold)
        {
            this.name = name;
            this.HP = HP;
            this.reaction = reaction;
          //  this.strength = strength;
          //  this.weakness = weakness;
            this.gold = gold;
        }

        internal Attack.AttackResult TakeAttack(Attack attack)
        {
            //TODO: add in defence
            var protection = armour?.protection ?? 0;
            var damage = attack.damage - protection;
            damage = Math.Max(damage,0);

            if (protection > 0)
            {
                TypeWriter.WriteLine(
                    new Text($"The "),
                    new Text(armour?.name.text ?? ""),
                    new Text(" deflected the attack", Colours.Speech, TypeWriter.Speed.List)
                );
            }    

            HP -= damage;

            if (HP < 0)
            {
                HP = 0;
            }
            TypeWriter.WriteLine($"{name} now has {HP} HP");
            
            if (HP <= 0)
            {
                return Attack.AttackResult.Dead;
            }
            else if (damage == 0)
            {
                return Attack.AttackResult.Deflected;
            }

            return Attack.AttackResult.Damaged;
        }

        internal virtual Attack displayAttack()
        {
            throw new NotImplementedException();
        }
    }
}