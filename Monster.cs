using System;

namespace game
{
    class Monster
    {
        public Monster(string spices, int attackPointsMin, int attackPointsMax, int healthPoints)
        {
            this.spices = spices;
            this.attackPointsMin = attackPointsMin;
            this.attackPointsMax = attackPointsMax;
            this.healthPoints = healthPoints;
        }

        public string spices { get; }

        internal void TakeAttack(Attack attack)
        {
            healthPoints -= attack.damage;
            TypeWriter.WriteLine($"The monster now has {healthPoints}");
        }

        public readonly int attackPointsMin;
        public readonly int attackPointsMax;
        public int healthPoints { get; set; }
        public int reaction { get; internal set; }
    } 
}
