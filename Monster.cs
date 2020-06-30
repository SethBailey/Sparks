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

        public readonly int attackPointsMin;
        public readonly int attackPointsMax;

        // public int attackPoints 
        // { 
            // get {
                // return new Random().Next(attackPointsMin,attackPointsMax);
            // } 
        // }
        public int healthPoints { get; set; }
    } 
}
