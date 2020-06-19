namespace game
{
    internal class PocketWatch : Item
    {
        private readonly TheGame game;

        public PocketWatch(TheGame game)
        {
            this.game = game;
            this.name = new Text("Pocket Watch");
        }

        override internal Text ItemDescription{  
            get {
                int daysTillEnd = game.daysTillEnd; 
                return new Text($"pocket watch that says: {daysTillEnd} days left", Colours.Cotton ); 
            }
        }    
    }
}