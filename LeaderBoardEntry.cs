using System;

namespace game
{
    internal class LeaderBoardEntry : IComparable<LeaderBoardEntry>
    {
        public string userName {get;}
        public int totalXP {get;}

        public LeaderBoardEntry(string userName, int totalXP)
        {
            this.userName = userName;
            this.totalXP = totalXP;
        }

        public LeaderBoardEntry(string raw)
        {
            //"bob 100"
            //userName = "bob"
            //totalXP = 100
            var parts = raw.Split(" ");
            this.userName = parts[0];
            this.totalXP = int.Parse(parts[1]);
        }

        public int CompareTo(LeaderBoardEntry other)
        {
            return totalXP.CompareTo(other.totalXP);
        }

        internal string GetRawString()
        {
            return $"{userName} {totalXP}";
        }
    }
}