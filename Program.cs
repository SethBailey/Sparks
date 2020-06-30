using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;

namespace game
{
    class TheGame
    {
       public int playerHP = 100;
       int maxHealthpoints = 500;
       string userName = "";
       private const int startingGold = 300;
       public int playerGold = startingGold;
       public string cottonUserName = "";
       bool cottonIntro = false;
       public int protection = 0;
       bool canPlayerRun = true;
       bool isPlayerWithMage = false;
       public bool isPlayerNew = true;
       string knightOrMage = "";
       string isPlayerKnight = "";
       bool hasPlayerBeenInShop = false;
       bool playerHasSeenMorse = false;
       int magePayGold = 2000;
       public int killXP = 0;
       int cottonXP = 0;
       bool morseXPAwarded = false;
       public int daysTillEnd = 70;
       bool isGameCorrupted = false;
       int priceForWatch = 200;
       bool playerHasWatch = false;
       int XPNeededForWatch = 300;
       bool hasPlayerSeenBill = false;
       string billUserName = "";
       bool isPlayerWithBill = false;
       bool doesPlayerHavePrivateKey = false;
       bool isBillDead = false;
       public int maxDamage = 100;
       int MinDamage = 1;
       bool hasPlayerBeenForcedOut = false;
       bool isPlayerWithDummy = false;
       bool hasPlayerReceivedKey = false;
       
       List<ItemCount> inventory = new List<ItemCount>();

       Weapon? playerWeapon;
       Armour? playerArmour;
       private int healing;

        List<Armour> bunchOfArmour = new List<Armour>();
        List<Weapon> shopWeapons = new List<Weapon>();

      
        public TheGame()
        {
        }

        public void Begin()
        {
            playerType();
            FillShopWithArmour();
            FillShopWithWeapons();
 
            if (isPlayerNew)
            {
                //Do player introductions
                TypeWriter.WriteLine("Enter your name:", TypeWriter.Speed.Talk);
                userName = Console.ReadLine();
                TypeWriter.WriteLine(new Text($"Nice to meet you {userName} my name is ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text("Sparks",Colours.Sparks, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine($"You {userName} are a {isPlayerKnight} and princess Kafe has been taken hostage by the high dark mage", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine(new Text("however his prices are steep, and he is demanding ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"{magePayGold} gold coins ",Colours.GoldReward, TypeWriter.Speed.Talk),
                                     new Text("for her freedom", Colours.Speech, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("You have two options: collect the gold and pay the mage or", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("Slay him, take back princess Kafe and rid the land of his vile existence", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("But there is a reason he is still alive, are you up for the challenge? time to find out.", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine(new Text("I will give you ",Colours.Speech , TypeWriter.Speed.Talk),
                                     new Text($"{startingGold} gold coins ",Colours.GoldReward, TypeWriter.Speed.Talk),
                                     new Text("to start you off with",Colours.Speech, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("And a muffin");                    
                AddToInventory(new Medicine("muffin", 0, 20, "muffin",""));                    
            }
            else
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine($"Welcome back {userName}",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("Lets just get on with it",TypeWriter.Speed.Talk);
            }           
            
            TypeWriter.WriteLine();
            playerStats();
        }

        public void playerType()
        {
            TypeWriter.WriteLine("Are you a (k)Knight or a (m)Mage", TypeWriter.Speed.Talk);
            knightOrMage = GetLowerReply();

            switch (knightOrMage)
            {
                case "k":
                    isPlayerKnight = "knight";
                    break;
                case "m":
                    isPlayerKnight = "mage";
                    break;
                default:
                    TypeWriter.WriteLine("That was not an option", TypeWriter.Speed.Talk);
                    playerType();
                    break;
            }
        }

        private void FillShopWithArmour()
        {
            if (isPlayerKnight == "knight")
            {
               //Create our set of armour
               LoadArmour("./Config/Armour-Knight.csv");
            }
            else 
            {
               LoadArmour("./Config/Armour-Mage.csv");
            }    
            
        }

        private void FillShopWithWeapons()
        {
            if (isPlayerKnight == "knight")
            {
                LoadWeapons("./Config/Weapons-Knight.csv");
            }
            else 
            {
                LoadWeapons("./Config/Weapons-Mage.csv");
            }
            
        }

        private List<string[]> LoadCSVFile(string file)
        {
            var fileData = new List<string[]>();
            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    fileData.Add( currentLine.Split(","));
                }
            }
            return fileData;
        }

        private void LoadWeapons(string file)
        {
            var lines = LoadCSVFile(file);
            foreach ( var values in lines)
            {
                var name = values[0];
                var price = int.Parse(values[1]);
                var damage = int.Parse(values[2]);
                var description = values[3];
                shopWeapons.Add(new Weapon(name, price, damage, description));
            }
        }

        private void LoadArmour(string file)
        {
            var lines = LoadCSVFile(file);
            foreach( var values in lines)
            {
                var name = values[0];
                var price = int.Parse(values[1]);
                var protection = int.Parse(values[2]);
                var description = values[3];
                bunchOfArmour.Add(new Armour(name, price, protection, description));
            }
        }

        private List<Medicine> LoadMedicine(string file)
        {
            List<Medicine> medicines = new List<Medicine>();
            var lines = LoadCSVFile(file);
            foreach (var values in lines)
            {
                var name = values[0];
                var price = int.Parse(values[1]);
                var healing = int.Parse(values[2]);
                var description = values[3];
                medicines.Add(new Medicine(name, price, healing, description));
            }
            return medicines;
        }

        public void playerStats()
        {
            List<Text> messsage = new List<Text>();
            messsage.Add( new Text("You have "));
            messsage.Add( new Text($"{playerHP} HP ",Colours.Health));
            messsage.Add( new Text("and "));
            messsage.Add( new Text($"{playerGold} Gold ",Colours.GoldReward));
            messsage.Add( new Text("and "));
            var totalXP = killXP + cottonXP;
            messsage.Add( new Text($"{totalXP} XP ", Colours.XP));
        
            if (playerWeapon != null )
            {
                messsage.Add( new Text("and a "));
                messsage.Add( playerWeapon.name);
            }
            if (playerArmour != null)
            {
                messsage.Add( new Text("and a "));
                messsage.Add( playerArmour.name);
            }

            TypeWriter.WriteLine(messsage);

        } 

        private void inventoryPage()
        {
            TypeWriter.WriteLine("Inventory page");
            TypeWriter.WriteLine();

            foreach ( var item in inventory)
            {  
                TypeWriter.WriteLine(item.name, new Text($" x {item.Count().ToString()}, "), item.ItemDescription);
                TypeWriter.WriteLine();
            }

            string[] playerOptions = Console.ReadLine().Split(" ");
            if ( playerOptions.Count() < 2 )
            {
                TypeWriter.WriteLine( "Must be a verb - noun action here");
                return;
            }

            string verb = playerOptions[0];
            string itemName = playerOptions[1];
            
            if (inventory.Exists(e => e.name.text == itemName))
            {
                var item = inventory.Find(e => e.name.text == itemName);
                
                if (Item.isSynonymFor(verb, "drop"))
                {
                   RemoveItem(item);
                   TypeWriter.WriteLine();
                   TypeWriter.WriteLine($"You drop the {itemName}");
                   return;
                }

                if (!item.item.DoVerb(verb, this))
                {
                    TypeWriter.WriteLine("Sorry but I don't understand");
                    return;
                }

                RemoveItem(item);
            }
            TypeWriter.WriteLine();
        }

        private void RemoveItem(ItemCount item)
        {
            item.Decrement();
            if (item.Count() <= 0)
            {
                inventory.Remove(item);
            }
        }

        public void Fight( Monster monster )
        {
            TypeWriter.WriteLine($"You run into a {monster.spices}",TypeWriter.Speed.List);
            TypeWriter.WriteLine(new Text($"The {monster.spices} has "),
                                 new Text($"{monster.healthPoints} HP",Colours.Monsterhealth));
            playerStats();
            var xpWin = CalcXPWin(monster);

            while (playerHP > 0 && monster.healthPoints > 0)
            {
                int playerAP = new Random().Next(GetPlayerMinDamage(),GetPlayerMaxDamage());
                int attackPoints = new Random().Next(monster.attackPointsMin,monster.attackPointsMax);
                int whoStrikes = new Random().Next(1,3);
                int runCost = new Random().Next(1, 201);
                GetPlayerProtection();
                
                if (canPlayerRun == true)
                {  TypeWriter.WriteLine($"To flee will cost you {runCost} Gold, will you fight?: yes / no",TypeWriter.Speed.List);
                   string playerFlee = GetLowerReply();
                    if (playerFlee == "no")
                    {
                        if (playerGold < runCost)
                        {
                            TypeWriter.WriteLine("You don't have sufficient funds to refuse",TypeWriter.Speed.List);
                        }
                        else
                        {
                            TypeWriter.WriteLine($"{userName} you ran from the fight and make it back to safety ",TypeWriter.Speed.List);
                            TypeWriter.WriteLine(new Text("But you lost "),
                                                 new Text($"{runCost} Gold ", Colours.Gold),
                                                 new Text("by paying the monster to let you go"));
                            playerGold -= runCost;
                            playerStats();
                            return;
                        }
                    }
                }   
                else
                {
                    TypeWriter.WriteLine("There is no running",TypeWriter.Speed.Talk);
                    GetLowerReply();
                }
              

                if (whoStrikes == 1)
                {
                    TypeWriter.WriteLine( new Text("You strike a blow and deal "),
                                          new Text($"{playerAP} damage",Colours.FightDamage));

                    monster.healthPoints -= playerAP;
                    if (monster.healthPoints < 0)
                    {
                        monster.healthPoints = 0;
                    }
                    TypeWriter.WriteLine( new Text($"The {monster.spices} has "),
                                          new Text($"{monster.healthPoints} HP",Colours.Monsterhealth));
                    playerStats();                      
                    TypeWriter.WriteLine();
                }
                else
                {
                    int damage = attackPoints - protection;
                    if ( damage < 0 )
                    {
                        if (playerArmour != null )
                        {
                            damage = 0;
                            TypeWriter.WriteLine(new Text($"Your "),
                                                 playerArmour.name,
                                                 new Text(" deflected the attack", Colours.Speech, TypeWriter.Speed.List));
                        }                  
                        
                    }
                    TypeWriter.WriteLine(new Text($"The {monster.spices} strike's a blow and deals "),
                                          new Text($"{damage} damage",Colours.FightDamage));

                    playerHP -= damage;
                    if (playerHP < 0)
                    {
                        playerHP = 0;
                    }
                    TypeWriter.WriteLine(new Text($"The {monster.spices} has "),
                                         new Text($"{monster.healthPoints} HP", Colours.Monsterhealth));
                    playerStats();
                    TypeWriter.WriteLine();
                }
            }

            if (playerHP == 0)
            {
                fightDescriptionDie(monster);
                TypeWriter.WriteLine("");
                throw new Exception("You Died");
            }
            else
            {
                killXP += xpWin;

                if (isPlayerWithMage == false)
                {
                    if (isPlayerWithBill == true || isPlayerWithDummy == true)
                    {
                        canPlayerRun = true;
                        XPMessage(xpWin);
                        return;
                    }    
                        int goldReward = new Random().Next(1, 101);

                        fightDescriptionWin(monster);
                        TypeWriter.WriteLine("");

                        List<Text> winMesssage = new List<Text>();
                        winMesssage.Add(new Text($"{userName} won the fight and got "));
                        winMesssage.Add(new Text($"{goldReward} Gold coins ", Colours.GoldReward));
                        playerGold += goldReward;
                        TypeWriter.WriteLine(winMesssage);
                        
                        XPMessage(xpWin);
                        AwardMedicine();
                        playerStats();
                        Console.WriteLine();
                        
                }
                else
                {
                    TypeWriter.WriteLine();
                    endMessageKill();
                    throw new Exception("");
                }
               
            }
        }

        private int CalcXPWin(Monster monster)
        {
            return monster.healthPoints;
        }

        private void XPMessage(int xpWin)
        {
            List<Text> XPMessage = new List<Text>();
            XPMessage.Add(new Text("You learned something from the experience and gained "));
            XPMessage.Add(new Text($"{xpWin} XP", Colours.XP));
            TypeWriter.WriteLine(XPMessage);
        }

        private int GetPlayerMaxDamage()
        {
            if ( playerWeapon != null )
            {
                maxDamage += playerWeapon.damage;
            }    
            return maxDamage;
        }

        private int GetPlayerMinDamage()
        {
            if ( playerWeapon != null)
            {
                MinDamage += playerWeapon.damage;
            }
            return MinDamage;
        }

        private int GetPlayerProtection()
        {
            protection = 0;
            if ( playerArmour != null)
            {
                protection += playerArmour.protection;
            }
            return protection;
        }

        private void fightDescriptionDie( Monster monster )
        {
            var lines = LoadCSVFile("./Config/DieDescriptions.csv");
            int pick = new Random().Next(0,lines.Count);
            var values = lines[pick];

            var dieIntroduction = values[0].Replace("{monster.spices}",$"{monster.spices}");
            var dieWinWay = values[1].Replace("{monster.spices}",$"{monster.spices}");
            var dieEnd = values[2].Replace("{monster.spices}",$"{monster.spices}");

            TypeWriter.WriteLine( new Text(dieIntroduction),
                                  new Text(dieWinWay,Colours.Damage),
                                  new Text(dieEnd));  
        }

        private void fightDescriptionWin( Monster monster )
        {
            if (knightOrMage == "m")
            {
                var lines = LoadCSVFile("./Config/WinDescriptions-Mage.csv");
                int pick = new Random().Next(0,lines.Count);
                var values = lines[pick];

                var mageIntroduction = values[0].Replace("{monster.spices}",$"{monster.spices}");
                var mageWinWay = values[1].Replace("{monster.spices}",$"{monster.spices}");
                var mageEnd = values[2].Replace("{monster.spices}",$"{monster.spices}");

                TypeWriter.WriteLine( new Text(mageIntroduction),
                                      new Text(mageWinWay,Colours.Attack),
                                      new Text(mageEnd));
            }
            else if (knightOrMage == "k")
            {
                var lines = LoadCSVFile("./Config/WinDescriptions-Knight.csv");
                int pick = new Random().Next(0,lines.Count);
                var values = lines[pick];

                var knightIntroduction = values[0].Replace("{monster.spices}",$"{monster.spices}");
                var knightWinWay = values[1].Replace("{monster.spices}",$"{monster.spices}");
                var knightEnd = values[2].Replace("{monster.spices}",$"{monster.spices}");

                TypeWriter.WriteLine( new Text(knightIntroduction),
                                      new Text(knightWinWay,Colours.Attack),
                                      new Text(knightEnd));
            }                                                                   
        }

        public static string showPlayerOptions()
        {
            TypeWriter.WriteLine();
            TypeWriter.WriteLine(new Text("Where would you like to go: (n)north, (s)south, (e)east, (w)west, the "),
                                 new Text("(sh)shop, ",Colours.Cotton),
                                 new Text("the "),
                                 new Text("(do)dojo ", Colours.Bill),
                                 new Text("or to the "),
                                 new Text("(bd)black dungeon",Colours.BlackDungeon,TypeWriter.Speed.List));
            TypeWriter.WriteLine($"Or i for the inventory page");                     
            string playerDirection = Console.ReadLine();
            return playerDirection;
        }

        public Location GoToLocation( Location location)
        {
            TypeWriter.WriteLine();
            location.displayDescription();
            location.displayItems();
            TypeWriter.WriteLine();

            bool didFight = false;

            if (location.chance > 0)
            {
                var isMonster = new Random().Next(location.chance , location.outOf);
                if (isMonster <= location.chance)
                {
                    Fight(PickMonsterForEveryDayFight());
                    didFight = true;
                }
            }

            if (!didFight)
            {
                int monsterGoldOrNothing = new Random().Next(1, 4);
                switch (monsterGoldOrNothing)
                {
                    case 1: FoundNothing(); break;
                    case 2: FoundGold(); break;
                    default: AwardMedicine(); break;
                }
            }
            

            string[] playerOptions = showPlayerOptions().ToLower().Split(" ");
            string playerOption = playerOptions[0];

            Console.Clear();
            playerStats();
            TypeWriter.WriteLine(new Text("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ",Colours.Speech, TypeWriter.Speed.List));
                                 new Text("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _",Colours.Speech, TypeWriter.Speed.List);
            TypeWriter.WriteLine();

            endGameCheck();

            //adjust user short cut to full string
            switch (playerOption)
            {
                case "n": playerOption = "north"; break;
                case "s": playerOption = "south"; break;
                case "e": playerOption = "east"; break;
                case "w": playerOption = "west"; break;
                case "bd": playerOption = "black dungeon"; break;
                case "sh": playerOption = "shop"; break;
                case "do": playerOption = "dojo"; break;
                case "take": playerOption = "take"; break;

                case "i": inventoryPage();
                    return location;  
                      
                default: TypeWriter.WriteLine("I don't understand"); 
                    return location;
            }

            switch (playerOption)
            {
                case "black dungeon": blackDungeon(); break;
                case "shop": itemShop(); break;
                case "dojo": dojo(); break;
                case "take" : tryTakeItem(location, playerOptions); break;
            }

            switch (playerOption)
            {
                case "north": 
                case "south": 
                case "east": 
                case "west": 
                {   
                    location = location.GetNextLocation( playerOption );
                    break;
                }
            }

            EndTime();

            return location;
        }

        private void tryTakeItem(Location location, string[] playerOptions)
        {
            if (playerOptions.Count() < 2)
            {
                TypeWriter.WriteLine("You need to pick something if you want to take it.");
                return;
            }

            string itemName = playerOptions[1];
            Item? item = location.tryTakeItem(new Text(itemName));
            if (item != null)
            {
                AddToInventory(item);
                ItemsTaken(item);
            }
            else
            {
                TypeWriter.WriteLine($"There is no {itemName} here ?");
            }
        }

        public static bool ItemsTaken(Item item)
        {
            File.AppendAllText(@".\Data\Item memory.txt", "\r\n" + item.name.text);

            return HasItemBeenTaken(item);
        }

        public static bool HasItemBeenTaken(Item item)
        {
            var stuffInFile = File.ReadAllLines(@".\Data\Item memory.txt").ToList();

            if (stuffInFile.Exists(e => e == item.name.text))
            {
                return true;
            }
            return false;
        }

        private void AddToInventory(Item item)
        {
            TypeWriter.WriteLine();
            TypeWriter.WriteLine(new Text("You picked up a "), item.name);
            TypeWriter.WriteLine();
            if (inventory.Exists(e => e.name == item.name))
            {
                inventory.Find(e => e.name == item.name).Increment();
            }
            else
            {
                inventory.Add(new ItemCount(item));
            }
        }

        private void EndTime()
        {
            daysTillEnd -= 1;

            if (daysTillEnd == 0)
            {
                isGameCorrupted = true;
                throw new Exception("Game has been corrupted");
            }
        }

        private static void FoundNothing()
        {
            int nothingHappend = new Random().Next(1, 4);

            switch (nothingHappend)
            {
                case 1: TypeWriter.WriteLine("Nothing of consequence happend"); break;
                case 2: TypeWriter.WriteLine("Nothing important happend"); break;
                case 3: TypeWriter.WriteLine("Nothing interesting occurred"); break; 
            }

        }

        private void FoundGold()
        {
            int foundGold = new Random().Next(1, 81);
            TypeWriter.WriteLine(new Text("You found "),
                                 new Text($"{foundGold} gold coins",Colours.Gold));
                                 
            playerGold += foundGold;
            List<Text> goldList = new List<Text>();
            goldList.Add( new Text("You now have "));
            goldList.Add( new Text($"{playerGold} gold coins", Colours.Gold, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(goldList);
        }

        private void dojo()
        {
            HasBillBeenKilled();
            if ( DojoIntro() )
            {
                dojoCommonPlace();
            }
        }

        private void dojoCommonPlace()
        {
            bool loop = true;
            while (loop)
            {            
                TypeWriter.WriteLine(new Text("Do you want to train in ", Colours.Speech , TypeWriter.Speed.Talk),
                                    new Text("(a)attack ", Colours.Attack, TypeWriter.Speed.Talk),
                                    new Text("or ", Colours.Speech , TypeWriter.Speed.Talk),
                                    new Text("(d)defence", Colours.Protection, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("or (l) to leave", TypeWriter.Speed.Talk);
                string dojoTrainingAwnser = GetLowerReply();

                switch (dojoTrainingAwnser)
                {
                    case "a": attackTraining(); break;
                    case "d": defenceTraining(); break;
                    case "l": 
                        TypeWriter.WriteLine($"bye {billUserName}", TypeWriter.Speed.Talk);
                        TypeWriter.WriteLine();
                        loop = false;
                        break;
                    default : 
                        TypeWriter.WriteLine("sorry but I don't understand", TypeWriter.Speed.Talk);
                        TypeWriter.WriteLine();
                        break;        
                }
            }
        }

        private void attackTraining()
        {
            int level1Price = 50;
            int level2Price = 100;
            int level3Price = 150;

            bool loop = true;
            while (loop)
            {
                isPlayerWithDummy = true;
                canPlayerRun = false;

                Console.Clear();
                playerStats();

                TypeWriter.WriteLine(new Text("Do you want to train at level 1, ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level1Price} Gold) ", Colours.Gold),
                                     new Text("2 ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level2Price} Gold) ", Colours.Gold),
                                     new Text("or ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text("3 ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level3Price} Gold)", Colours.Gold, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("Or (l to leave)", TypeWriter.Speed.Talk);                     
                string answer = GetLowerReply();
                switch (answer)
                {
                    case "1": 
                        if (playerGold >= level1Price)
                        {
                            Fight(new Monster("Dummy", 0, 0, 50));
                            playerGold -= 50; 
                            maxDamage += 5;
                            MinDamage += 5;
                            TypeWriter.WriteLine($"You now deal between {MinDamage} and {maxDamage} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break;
                    case "2":
                        if (playerGold >= level2Price)
                        {
                            Fight(new Monster("Dummy", 1, 20, 200));
                            playerGold -= 100;
                            maxDamage += 10;
                            MinDamage += 10;
                            TypeWriter.WriteLine($"You now deal between {MinDamage} and {maxDamage} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break;
                    case "3":
                        if (playerGold >= level3Price)
                        {
                            Fight(new Monster("Dummy", 1, 50, 400));
                            playerGold -= 150;
                            maxDamage += 15;
                            MinDamage += 15;
                            TypeWriter.WriteLine($"You now deal between {MinDamage} and {maxDamage} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break;  
                    case "l":
                        loop = false;
                        canPlayerRun = true;
                        break;
                    default:
                        TypeWriter.WriteLine("sorry but I don't understand", TypeWriter.Speed.Talk);
                        TypeWriter.WriteLine();
                        break;              
                }
            }
        }

        private void defenceTraining()
        {
            int level1Price = 50;
            int level2Price = 100;
            int level3Price = 150;

            bool loop = true;
            while(loop)
            {
                isPlayerWithDummy = true;
                canPlayerRun = false;

                Console.Clear();
                playerStats();

                TypeWriter.WriteLine();
                TypeWriter.WriteLine(new Text("Do you want to train at level 1, ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level1Price} Gold) ", Colours.Gold),
                                     new Text("2 ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level2Price} Gold) ", Colours.Gold),
                                     new Text("or ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text("3 ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text($"({level3Price} Gold)", Colours.Gold, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("Or (l to leave)");                     
                string answer = GetLowerReply();
                switch (answer)
                {
                    case "1": 
                        if (playerGold >= level1Price)
                        {
                            Fight(new Monster("Dummy", 1, 10, 100));
                            playerGold -= 50;
                            protection += 10;
                            TypeWriter.WriteLine($"You can now block {protection} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break;
                    case "2":
                        if (playerGold >= level2Price)
                        {
                            Fight(new Monster("Dummy", 1, 50, 200));
                            playerGold -= 100;
                            protection += 20;
                            TypeWriter.WriteLine($"You can now block {protection} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break;
                    case "3":
                        if (playerGold >= level3Price)
                        {
                            Fight(new Monster("Dummy", 1, 150, 300));
                            playerGold -= 150;
                            protection += 30;
                            TypeWriter.WriteLine($"You can now block {protection} damage", TypeWriter.Speed.Talk);
                            TypeWriter.WriteLine();
                        }
                        else
                        {
                            TypeWriter.WriteLine("Sorry but you don't have enough Gold");
                            TypeWriter.WriteLine();
                        }
                        break; 
                    case "l":
                        loop = false;
                        canPlayerRun = true;
                        break;  
                    default:
                        TypeWriter.WriteLine("sorry but I don't understand", TypeWriter.Speed.Talk);
                        TypeWriter.WriteLine();
                        break;             
                }
            }
        }

        private void HasBillBeenKilled()
        {
            if (isBillDead == true)
            {
                TypeWriter.WriteLine($"{userName} William is dead don't linger on the past", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine();
            }
        }

        private bool DojoIntro()
        {
            int totalXP = killXP + cottonXP; 
            if (hasPlayerSeenBill == false)
            {
                if (hasPlayerBeenForcedOut == true)
                {
                    forcedOutEntry();
                }
                DisplayDescription("./Config/DojoDescription.csv");
                
                TypeWriter.WriteLine("yes of course I'll try, I know, I know... got to go", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine(new Text("Hey there my name is ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text("William ", Colours.Bill, TypeWriter.Speed.Talk),
                                     new Text("and I run the Dojo", Colours.Speech, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine("And whats your name?", TypeWriter.Speed.Talk);
                billUserName = Console.ReadLine();
                hasPlayerSeenBill = true;
                TypeWriter.WriteLine($"Nice, well {billUserName} before we start you must take an oath OK: yes/no", TypeWriter.Speed.Talk);
                string answer = GetLowerReply();
                if (answer == "yes")
                {
                   theOath();
                   return false; 
                }
                else
                {
                    hasPlayerBeenForcedOut = true;
                    forcedOutOfDojo();
                    return false;
                }
            }
            else
            {
                if (hasPlayerBeenForcedOut == true)
                {
                    forcedOutEntry();
                    return false;
                }
                TypeWriter.WriteLine($"Wellcome back {billUserName}", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine();

                if (totalXP <= 400)
                {
                    if (hasPlayerReceivedKey == false)
                    {
                        TypeWriter.WriteLine();
                        TypeWriter.WriteLine(new Text("By the way there is something I would like you to have ", Colours.Bill, TypeWriter.Speed.Talk));
                        TypeWriter.WriteLine(new Text("let's just keep this between you and me ", Colours.Bill, TypeWriter.Speed.Talk));
                        TypeWriter.WriteLine(new Text("William hands you a key with a tag that says ", Colours.Speech, TypeWriter.Speed.Talk),
                        new Text("private", Colours.Bill, TypeWriter.Speed.Talk));
                        TypeWriter.WriteLine();
                        GiveKey();
                    }
                    hasPlayerReceivedKey = true;
                }
            }
            return true;
        }

        private void GiveKey()
        {
            if (doesPlayerHavePrivateKey == false)
            {
                doesPlayerHavePrivateKey = true;
                AddToInventory(new Item {name = new Text("private Key"), ItemDescription = new Text("private Key ", Colours.Bill) });
            }
        }

        private void forcedOutEntry()
        {
            bool loop = true;
            while (loop)
            {
                TypeWriter.WriteLine();
                TypeWriter.WriteLine("will you take the oath now: yes/no", TypeWriter.Speed.Talk);
                string oathAwnser = GetLowerReply();
                if (oathAwnser == "yes")
                {
                    theOath();
                    loop = false;
                }
                else if (oathAwnser == "no")
                {
                    forcedOutOfDojo();
                    loop = false;
                }
                else
                {
                    TypeWriter.WriteLine("sorry I don't understand", TypeWriter.Speed.Talk);
                }
            }
        }

        private void theOath()
        {
            TypeWriter.WriteLine("1) I will only use these skills in self-defence", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("2) I will allways seek the most peaceful solution", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("3) I will allways do what I feel is right", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("yes/no");

            string oathAwnser = GetLowerReply();
            if (oathAwnser == "yes")
            {
                TypeWriter.WriteLine("Great Well then lets begin", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine();
                dojoCommonPlace();
            }
            else
            {
                TypeWriter.WriteLine("Then I have no choice!", TypeWriter.Speed.Talk);
                canPlayerRun = false;
                isPlayerWithBill = true;
                Fight(new Monster("William", 100, 200, 300));
                killBillMessage();
            }
        }

        private void forcedOutOfDojo()
        {
            TypeWriter.WriteLine("Then I cant help you", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("You are forced out of the dojo", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine();
        }

        private void killBillMessage()
        {
            TypeWriter.WriteLine(new Text("Sorry", Colours.Bill, TypeWriter.Speed.Talk ));
            TypeWriter.WriteLine(new Text("Please keep this safe", Colours.Bill, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(new Text("William hands you a key with a tag that says ", Colours.Speech ,TypeWriter.Speed.Talk),
                                 new Text("private", Colours.Bill, TypeWriter.Speed.Talk));
            GiveKey();
            isBillDead = true;
            TypeWriter.WriteLine("You leave the dojo", TypeWriter.Speed.Talk);
            TypeWriter.WriteLine();
        }

        private void blackDungeon()
        {
            canPlayerRun = true;

            DisplayDescription("./Config/BlackDungeonDescription.csv");

            TypeWriter.WriteLine("the gates close behind you, there is no runing",TypeWriter.Speed.Talk);

            TypeWriter.WriteLine($"{userName} the time has come, will you kill the mage or pay him?: kill / pay",TypeWriter.Speed.Talk);
            string killOrPay = GetLowerReply();
            switch (killOrPay)
            {
                case "kill":
                    isPlayerWithMage = true;
                    TypeWriter.WriteLine("At last, here we go",TypeWriter.Speed.Talk);  
                    TypeWriter.WriteLine();   
                    blackDungeonKill();
                    break;  

                case "pay":
                    {
                       blackDungeonPay();
                       TypeWriter.WriteLine($"Very well {userName}",TypeWriter.Speed.Talk);
                       TypeWriter.WriteLine();
                       TypeWriter.WriteLine("You pay the mage",TypeWriter.Speed.Talk);
                       TypeWriter.WriteLine();
                       TypeWriter.WriteLine(new Text("User this program may have been infected please stand down", Colours.Hidden, TypeWriter.Speed.Talk)); 
                       playerStats();
                       endMessagePay();
                       throw new Exception("");
                    }

                default:
                    TypeWriter.WriteLine("That was not a option",TypeWriter.Speed.Talk);
                    blackDungeon();
                    break;
            }
        }
        private void blackDungeonKill()
        {
            TypeWriter.WriteLine(new Text("User this program may have been infected please stand down", Colours.Hidden, TypeWriter.Speed.Talk));       
            Fight(new Monster("Dark high mage", 100, 500, 1000));
        }

        private void blackDungeonPay()
        {
            if (playerGold >= magePayGold)
            {
                playerGold -= magePayGold;
            }
            else
            {
                TypeWriter.WriteLine("You do not have the required funds, try that again", TypeWriter.Speed.Talk);
                blackDungeon();
            }
        }  

        private void endMessageKill()
        {
            TypeWriter.WriteLine($"Very well done {userName}, hats off to you and so on and so forth",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("If you were a chess piece you would be a queen, my most powerfull piece",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine(new Text("Downloading... downloading...", Colours.Hidden, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine("Oh and well done for rescuing Kafe as well",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine(new Text("Files found... corrupting...", Colours.Hidden, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine("Why are you still here, you can leave now you have no futher use",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine(new Text("Check Mate",Colours.Damage,TypeWriter.Speed.Talk));
        }

        private void endMessagePay()
        {
            TypeWriter.WriteLine("Congrats you rescued Kafe, all's well and...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Oh dear! It seems she's been kidnapped again by the mage",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine(new Text("Downloading... downloading...", Colours.Hidden, TypeWriter.Speed.Talk));
            List<Text> endList = new List<Text>();
            endList.Add( new Text("Next time you should just ", Colours.Speech, TypeWriter.Speed.Talk));
            endList.Add( new Text("kill him! ", Colours.Damage, TypeWriter.Speed.Talk));
            endList.Add( new Text("and then you won't have this problem", Colours.Speech, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(endList);
            TypeWriter.WriteLine(new Text("Files could not be reached... anti-mallware sill in place", Colours.Hidden, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine("Until we meet again",TypeWriter.Speed.Talk);
        }

        public void endGameCheck()
        {
            if (playerGold >= 2000)
            {

                TypeWriter.WriteLine($"{userName} you have the required funds to confront the high dark mage",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("Are you ready: yes / no",TypeWriter.Speed.Talk);
                string playerAwnser = GetLowerReply(); 
                if (playerAwnser == "yes")
                {
                    TypeWriter.WriteLine("He is located in the black dungeon",TypeWriter.Speed.Talk);
                }
                else
                {
                    TypeWriter.WriteLine("OK",TypeWriter.Speed.Talk);
                    TypeWriter.WriteLine($"but if you change your mind {userName} he is located in the black dungeon",TypeWriter.Speed.Talk);
                }
            }
        }
        public void AwardMedicine()
        {
            int medicine = new Random().Next(1, 30);
            List<Text> medMessage = new List<Text>();
            medMessage.Add(new Text("You have found some medicine and you gain "));
            medMessage.Add(new Text($"{medicine} HP", Colours.Medicine));
            playerHP += medicine;
            TypeWriter.WriteLine(medMessage);
            MaxHealthEnforcer();
        }

        private void MaxHealthEnforcer()
        {
            if (playerHP > maxHealthpoints)
            {
                playerHP = maxHealthpoints;
                TypeWriter.WriteLine(new Text("Max HP reached", Colours.Health, TypeWriter.Speed.Talk));
            }
        }

        public Monster PickMonsterForEveryDayFight()
        {
            List<Monster> monsters = new List<Monster>();
           
            var lines = LoadCSVFile("./Config/Monsters.csv");
            foreach (var values in lines)
            {
                var name = values[0];
                var MinDamage = int.Parse(values[1]);
                var MaxDamage = int.Parse(values[2]);
                var MinHealthPoints = int.Parse(values[3]);
                var MaxHealthpoints = int.Parse(values[4]);
                monsters.Add(new Monster(name, MinDamage, MaxDamage, new Random().Next(MinHealthPoints, MaxHealthpoints)));
            }

            int monsterPick = new Random().Next(0, monsters.Count);
            return monsters[monsterPick];
            
        }

        private void DispalyArmour(Armour armour, int index)
        {
            TypeWriter.WriteLine(new Text($"[{index}] "),
                                 armour.name,
                                 new Text($" - "),
                                 new Text($"{armour.price} Gold " , Colours.Gold),
                                 new Text($"- "),
                                 new Text($"{armour.protection} protection " ,Colours.Protection),
                                 new Text("- "),
                                armour.ItemDescription);
        }

        private string GetLowerReply()
        {
            return Console.ReadLine().ToLower();
        }

        private void DisplayDescription(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                TypeWriter.WriteLine("\t" + line, TypeWriter.Speed.Talk);
            }

            TypeWriter.WriteLine();
        }

        public void itemShop()
        {
            int totalXP = TotalXP();

            if (cottonIntro == false)
            {
                TypeWriter.WriteLine();
                DisplayDescription("./Config/ShopDescription.csv");
                List<Text> cottonList = new List<Text>();
                cottonList.Add(new Text("hello traveller, I'm ", Colours.Speech, TypeWriter.Speed.Talk));
                cottonList.Add(new Text("Cotton ", Colours.Cotton, TypeWriter.Speed.Talk));
                cottonList.Add(new Text("and this is my shop, whats your name?", Colours.Speech, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine(cottonList);
                cottonUserName = GetLowerReply();
                TypeWriter.WriteLine($"{cottonUserName}, hmm ... nice!");
                cottonIntro = true;
            }
            if (hasPlayerBeenInShop == true)
            {
                somethingCool();
                if (playerHasWatch == false && playerGold >= priceForWatch)
                {
                    GetWatch(totalXP);
                }
                shopCommonPlace();
            }
            else
            {
                shopCommonPlace();
                hasPlayerBeenInShop = true;
            }

        }

        private int TotalXP()
        {
            return killXP + cottonXP;
        }

        private void GetWatch(int totalXP)
        {
            if (totalXP >= XPNeededForWatch)
            {
                TypeWriter.WriteLine(new Text($"Hey {cottonUserName} do you want to trade ", Colours.Speech, TypeWriter.Speed.Talk),
                                     new Text("{priceForWatch} Gold ", Colours.Gold, TypeWriter.Speed.Talk),
                                     new Text("for my pocket watch: yes/no",Colours.Speech ,TypeWriter.Speed.Talk));
                string Awnser = GetLowerReply();
                if (Awnser == "yes")
                {
                    TypeWriter.WriteLine();
                    TypeWriter.WriteLine("Great you've got a pocket watch :)", TypeWriter.Speed.Talk);
                    TypeWriter.WriteLine();
                    Thread.Sleep(1000);
                    playerGold -= 150;
                }
                else
                {
                    TypeWriter.WriteLine();
                    TypeWriter.WriteLine("Fine just take it");
                    TypeWriter.WriteLine();
                    Thread.Sleep(1000);
                }

                if (playerHasWatch == false)
                {
                    playerHasWatch = true;
                    AddToInventory( new PocketWatch( this ) );
                }
            }
        }

        private void shopCommonPlace()
        {
            bool leaveShop = false;
            while (leaveShop != true )
            {

                TypeWriter.WriteLine(new Text($"Hello {cottonUserName} are you looking for "),
                                    new Text("(w)weapons, ", Colours.Attack, TypeWriter.Speed.Talk),
                                    new Text("(a)armour ", Colours.Protection, TypeWriter.Speed.Talk),
                                    new Text("or ", Colours.Speech, TypeWriter.Speed.Talk),
                                    new Text("(m)medicine ", Colours.Health, TypeWriter.Speed.Talk),
                                    new Text(":)", Colours.Speech, TypeWriter.Speed.Talk));
                if (hasPlayerBeenInShop == true)
                {
                    if (playerHasSeenMorse == true)
                    {
                        TypeWriter.WriteLine("(h) to hear it again or (l) to leave", TypeWriter.Speed.Talk);
                    }
                    else if (playerHasSeenMorse == false)
                    {
                        TypeWriter.WriteLine("(h) to hear something cool or (l) to leave", TypeWriter.Speed.Talk);
                    }
                }
                else
                {
                    TypeWriter.WriteLine("or (l) to leave", TypeWriter.Speed.Talk);  
                }

                string playerItemType = GetLowerReply();

                switch (playerItemType)
                {
                    case "w": BuyWeapon(); break;
                    case "a": BuyArmour(); break;
                    case "m": BuyMedicine(); break;
                    case "l": 
                        TypeWriter.WriteLine($"Bye {cottonUserName} :)", TypeWriter.Speed.Talk); 
                        TypeWriter.WriteLine();
                        leaveShop = true;
                    break;
                    case "h": TypeWriter.WriteLine("Sure thing");
                            morseCodeMessage();
                    break;

                    default: 
                        TypeWriter.WriteLine("Sorry but I don't understand", TypeWriter.Speed.Talk);
                        break;
                }
            }
        }

        private void somethingCool()
        {
            if (playerHasSeenMorse == false)
            {
                TypeWriter.WriteLine(new Text($"Hello {cottonUserName}, you want to hear something cool: yes/no"));
                string somthingCoolAwnser = GetLowerReply();

                switch (somthingCoolAwnser)
                {
                    case "yes":
                        playerHasSeenMorse = true;
                        TypeWriter.WriteLine(new Text("Some guy came the other day and gave me this tape (make sure you can hear it :)", Colours.Speech, TypeWriter.Speed.Talk));
                        morseCodeMessage();
                        Thread.Sleep(1000);
                        TypeWriter.WriteLine("strange huh?... but anyway");
                        TypeWriter.WriteLine();
                        break;

                    case "no":
                        TypeWriter.WriteLine($"O.K {cottonUserName} ");
                        TypeWriter.WriteLine();
                        break;

                    default:
                        TypeWriter.WriteLine("Sorry but I don't understand");
                        TypeWriter.WriteLine();
                        break;
                }
            }            
        }

        private void morseCodeMessage()
        {
            if (morseXPAwarded == false)
            {
                int award = 30;
                cottonXP += award;
                TypeWriter.WriteLine(new Text($"Cotton awards you {award} XP", Colours.Cotton));       
                morseXPAwarded = true; 
            }   

            var sequence = Enumerable.Range(0, 3).ToList();
            
            foreach (var e in sequence)
            {
                Console.Beep(650, 100);
                Console.Write(".");
                Thread.Sleep(100);
                
            }
            
            foreach (var e in sequence)
            {
                Console.Beep(650, 400);
                Console.Write("-");
                Thread.Sleep(400);
            }
            
            foreach (var e in sequence)
            {
                Console.Beep(650, 100);
                Thread.Sleep(100);
                Console.Write(".");
            }
            Console.WriteLine();
            Thread.Sleep(500);
        }

        private void BuyMedicine()
        {
            bool leave = false;
            while ( leave != true )
            {
                Console.Clear();
                playerStats();
                List<Medicine> shopMedicine = LoadMedicine("./Config/Medicine.csv");

                for (int i = 0; i < shopMedicine.Count; i++)
                {
                    DisplayMedicine(shopMedicine[i], i);
                }
                TypeWriter.WriteLine($"[{shopMedicine.Count}] Exit this part of shop",TypeWriter.Speed.List);

                string userMedicineChoice = GetLowerReply();
                //making sure that the user inputs a valid answer
                int purchaseChoice = 0;
                try 
                {
                    purchaseChoice = Int32.Parse(userMedicineChoice);
                }
                catch (Exception)
                {
                    TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                    Thread.Sleep(1000);
                    continue;
                }

                if (purchaseChoice >= shopMedicine.Count)
                {
                    leave = true;
                    continue;
                }
                if (playerGold >= shopMedicine[purchaseChoice].price)
                {
                    MedicineChosen(shopMedicine[purchaseChoice]);
                }
                else
                {
                    TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold",TypeWriter.Speed.Talk);
                }
            }
        }

        public void MedicineChosen(Medicine medicine)
        {
            TypeWriter.WriteLine("Good choice, you use " + medicine.name);
            healing = medicine.healing;
            playerHP += healing;
            playerGold -= medicine.price;
        }

        private void DisplayMedicine(Medicine medicine, int i)
        {
            
            TypeWriter.WriteLine(new Text($"[{i}] "),
                                 medicine.name,
                                 new Text($" - "),
                                 new Text($"{medicine.price} Gold ", Colours.Gold),
                                 new Text($"- "),
                                 new Text($"{medicine.healing} healing ",Colours.Health),
                                 new Text($"- "),
                                 medicine.ItemDescription);
        }



        private void BuyArmour()
        {
            bool leave = false;
            while (leave != true)
            {   
                Console.Clear();
                playerStats();

                for (int i = 0; i < bunchOfArmour.Count; i++)
                {
                    DispalyArmour(bunchOfArmour[i], i);
                }
                TypeWriter.WriteLine($"[{bunchOfArmour.Count}] Exit this part of shop",TypeWriter.Speed.List);

                string userArmorChoice = GetLowerReply();

                int purchaseChoice = 0;
                try 
                {
                    purchaseChoice = Int32.Parse(userArmorChoice);
                }
                catch (Exception)
                {
                    TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                    Thread.Sleep(1000);
                    continue;
                }
                
                if (purchaseChoice >= bunchOfArmour.Count)
                {
                    leave = true;
                    continue;
                }

                if (playerGold < bunchOfArmour[purchaseChoice].price)
                {
                    TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold",TypeWriter.Speed.Talk);
                    Thread.Sleep(1000);
                    continue;
                }

            
                //first return any item you may already have
                if (playerArmour != null)
                {
                   // bunchOfArmour.Add(playerArmour);
                }
                ArmourChosen(bunchOfArmour[purchaseChoice]);
                bunchOfArmour.RemoveAt(purchaseChoice);
            }
        }

        private void ArmourChosen(Armour armour)
        {
            if (playerArmour != null)
            {
               // inventory.Remove(playerArmour);
            }
            playerArmour = armour;
            AddToInventory(playerArmour);
            playerGold -= playerArmour.price;
            TypeWriter.WriteLine($"Good choice, you've got your self a " + playerArmour.name);    
        }

        private void BuyWeapon()
        {
            bool leave = false;
            while (leave != true)
            {
                Console.Clear();
                playerStats();

                for (int i = 0; i < shopWeapons.Count; i++)
                {
                    DisplayWeapons(shopWeapons[i], i);
                }
                TypeWriter.WriteLine($"[{shopWeapons.Count}] Exit this part of shop",TypeWriter.Speed.List);

                string playerChoiceString = GetLowerReply();

                int playerChoice = 0;
                try 
                {
                    playerChoice = Int32.Parse(playerChoiceString);
                }
                catch (Exception)
                {
                    TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                    Thread.Sleep(1000);
                    continue;
                }

                if (playerChoice >= shopWeapons.Count)
                {
                    leave = true;
                    continue;
                }
                else
                {
                    WeaponChosen(shopWeapons, playerChoice);
                }
            }
        }

        private void WeaponChosen(List<Weapon> shopWeapons, int playerChoice)
        {
            if (playerGold < shopWeapons[playerChoice].price)
            {
               TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold", TypeWriter.Speed.Talk);
               Thread.Sleep(1000);
            }
            else
            {
                 //first return any item you may already have
                if (playerWeapon != null)
                {
                   //shopWeapons.Add(playerWeapon); 
                }
                chosenWeapon(shopWeapons[playerChoice]);
                shopWeapons.RemoveAt(playerChoice);
            }
        }

        private void chosenWeapon(Weapon weapon)
        {
            if (playerWeapon != null)
            {
                //inventory.Remove( playerWeapon);
            }
            playerWeapon = weapon;
            AddToInventory( playerWeapon );
            playerGold -= playerWeapon.price;
            TypeWriter.WriteLine($"Good choice you've got your self a " + playerWeapon.name);
        }

        private static void DisplayWeapons(Weapon weapon, int index) 
        {
            TypeWriter.WriteLine(new Text($"[{index}] "),
                                 weapon.name, 
                                 new Text(" - "),
                                 new Text($"{weapon.price} Gold ", Colours.Gold), 
                                 new Text("- "),
                                 new Text($"{weapon.damage} Extra Damage ", Colours.Damage),
                                 weapon.ItemDescription);
        }

        public void playerReset()
        {
            if (playerArmour != null)
            {
                bunchOfArmour.Add(playerArmour);
            }
            playerArmour = null;
            if (playerWeapon != null)
            {
                shopWeapons.Add(playerWeapon); 
            }
            playerWeapon = null;
            playerGold = startingGold;
            playerHP = 100;
            isPlayerNew = false;
            isPlayerWithMage = false;
            killXP = 0;
            playerHasWatch = false;
            maxDamage = 100;
            MinDamage = 1; 
            canPlayerRun = true;

            bunchOfArmour.Clear();
            shopWeapons.Clear();
                     
        }

        internal bool End()
        {
            var totalXP = TotalXP();
            TypeWriter.WriteLine();
            TypeWriter.WriteLine($"Final score: {totalXP}");
            TypeWriter.WriteLine();
            TypeWriter.WriteLine("Game Over",TypeWriter.Speed.Talk);

            if (isGameCorrupted == false)
            {   
                TypeWriter.WriteLine($"psst {cottonUserName} do you want to try again: yes / no",TypeWriter.Speed.Talk);
                
                string tryAgain = GetLowerReply();
                switch(tryAgain)
                {
                    case "yes":
                        playerReset();
                        return true;

                    default:
                        TypeWriter.WriteLine("bye for now :)", TypeWriter.Speed.Talk);
                        TypeWriter.WriteLine();
                        UpdateLeaderBoard(totalXP);
                        return false;
                }
            }
            else
            {
                TypeWriter.WriteLine();
                UpdateLeaderBoard(totalXP);
                return false;
            }   
        }

        private void UpdateLeaderBoard(int totalXP)
        {
            //read the leaderboard
            var fileName = @".\Data\LeaderBoard.txt";
            var rawLeaderBoard = new List<string>();
            Directory.CreateDirectory(@".\Data");
            if (File.Exists(fileName))
            {
                rawLeaderBoard = File.ReadAllLines(fileName).ToList();
            }
            var leaderBoard = new List<LeaderBoardEntry>();
            
            //add to the leaderboard
            var currentGameEntry = new LeaderBoardEntry(userName, totalXP);
            leaderBoard.Add( currentGameEntry );

            //fill the leaderboard will the raw leaderboard
            foreach( var rawEntry in rawLeaderBoard)
            {
                leaderBoard.Add( new LeaderBoardEntry( rawEntry ) );
            }
            
            leaderBoard.Sort();
            leaderBoard.Reverse();

            if (leaderBoard.Count > 9)
            {
                leaderBoard.RemoveAt(leaderBoard.Count - 1); 
            }
            
            //write to file
            rawLeaderBoard.Clear();
            foreach (var entry in leaderBoard)
            {
                rawLeaderBoard.Add(entry.GetRawString());
            }
            File.WriteAllLines(@".\Data\LeaderBoard.txt", rawLeaderBoard.ToArray());

            //write to screen
            TypeWriter.WriteLine("Leaderboard");
            TypeWriter.WriteLine("");
            int position = 1;
            foreach (var entry in leaderBoard)
            {
                TypeWriter.WriteLine($"{position}: {entry.GetRawString()} XP");
                position++;
            }
            TypeWriter.WriteLine();
        }
    }

    

    class Program
    {

        static void Main(string[] args)
        {   
            var app = new CommandLineApplication();

            app.HelpOption();
            var startingGold = app.Option<int>("-g|--gold <VALUE>", "Staring Gold", CommandOptionType.SingleValue);
            var totalXP = app.Option<int>("-x|--xp <VALUE>", "total XP", CommandOptionType.SingleValue);
            
            app.OnExecute(() =>
            {
                TheGame theGame = new TheGame();
                if ( startingGold.HasValue() )
                {
                    theGame.playerGold = startingGold.ParsedValue;
                }
                if ( totalXP.HasValue())
                {
                    theGame.killXP = totalXP.ParsedValue;
                }
             
                bool replay = true;

                //replay loop
                while ( replay )
                {
                    Console.Clear();
                    File.WriteAllLines(@".\Data\Item memory.txt",Array.Empty<string>());
                    theGame.Begin(); 
                    try
                    {
                        //game loop
                        var location = new Location("Spawn point");
                        while (true)
                        {
                            location = theGame.GoToLocation(location);
                        }
                    }
                    catch ( Exception e )
                    {
                        TypeWriter.WriteLine( e.Message );
                    }
                    replay = theGame.End();
                }

            });

            app.Execute(args);
        }

    }
}
