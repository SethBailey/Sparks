using System;
using System.Collections.Generic;

namespace game
{
    public class Location
    {
        public Location(string name)
        {
            string[] lines = System.IO.File.ReadAllLines($"./Config/locations/{name}.txt");

            foreach (string line in lines)
            {
                var values = line.Split("=");
                if (values.Length < 2 )
                {
                    continue;
                } 
                var key = values[0].Trim();
                var value = values[1].Trim();

                switch (key )
                {
                    case "northMessage" : northMessage = value; break;
                    case "southMessage" : southMessage = value; break;
                    case "eastMessage" : eastMessage = value; break;
                    case "westMessage" : westMessage = value; break;
                    case "northLocation" : northLocation = value; break;
                    case "southLocation" : southLocation = value; break;
                    case "eastLocation" : eastLocation = value; break;
                    case "westLocation" : westLocation = value; break;              
                    case "d" : descriptions.Add(value); break;      
                }

            }
        }

        internal void displayDescription()
        {
            foreach ( var line in descriptions)
            {
                TypeWriter.WriteLine(line);
            }
        }

        private List<string> descriptions = new List<string>();
        private string northMessage;
        private string southMessage;
        private string westMessage;
        private string eastMessage;
        
        private string eastLocation;
        private string westLocation;
        private string southLocation;
        private string northLocation;

        internal Location GetNextLocation(string playerDirection)
        {
            string message;
            switch( playerDirection)
            {
                case "north": 
                {
                    if (northLocation != "")
                    {
                        return new Location(northLocation);
                    }     
                    message = northMessage; break;
                }

                case "south": 
                {
                    if (southLocation != "")
                    {
                        return new Location(southLocation);
                    }     
                    message = southMessage; break;
                }

                case "east": 
                {
                    if (eastLocation != "")
                    {
                        return new Location(eastLocation);
                    }     
                    message = eastMessage; break;
                }

                case "west": 
                {
                    if (westLocation != "")
                    {
                        return new Location(westLocation);
                    }     
                    message = westMessage; break;
                }
 
                default : message = "you can't go there"; break;
            }

            TypeWriter.WriteLine(message);
            return this;
        }
    }
}