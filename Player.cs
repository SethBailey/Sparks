using System;
using System.Collections.Generic;

namespace game
{
    internal class Player
    {
        List<Attack> attacks = new List<Attack>();

        public Player()
        {
        }

        public int reaction { get; internal set; }

        internal List<Attack> GetAttacks()
        {
            return attacks;
        }

        internal void AddAttack(Attack attack)
        {
            attacks.Add(attack);
        }
    }
}