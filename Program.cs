﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace game
{
    class TheGame
    {
       int playerHP = 100;
       string userName;
       private const int startingGold = 300;
       public int playerGold = startingGold;
       public string cottonUserName;
       bool cottonIntro = false;
       int protection;
       bool canPlayerRun = false;
       bool isPlayerWithMage = false;
       public bool isPlayerNew = true;
       string knightOrMage;
       string isPlayerKnight;
       bool hasPlayerBeenInShop = false;
       bool playerHasSeenMorse = false;
       int magePayGold = 2000;
       int killXP = 0;
       int cottonXP = 0;
       bool morseXPAwarded = false;
       

       Weapon playerWeapon;
       Armour playerArmour;
       private int healing;

        List<Armour> bunchOfArmour = new List<Armour>();
        List<Weapon> shopWeapons = new List<Weapon>();

      

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
                TypeWriter.WriteLine("Slay him, take back princess Kafe and rid the land of his vile existance", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("But there is a reason he is still alive, are you up for the challange? time to find out.", TypeWriter.Speed.Talk);
                TypeWriter.WriteLine(new Text("I will give you "),
                                     new Text($"{startingGold} gold coins ",Colours.GoldReward),
                                     new Text("to start you off with"));
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
            knightOrMage = Console.ReadLine();

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
                messsage.Add( new Text($"{playerWeapon.name} ",Colours.Attack));
            }
            if ( playerArmour != null)
            {
                messsage.Add( new Text("and a "));
                messsage.Add( new Text($"{playerArmour.name}" ,Colours.Protection));
            }

            TypeWriter.WriteLine(messsage);

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
                int whoStrikes = new Random().Next(1,3);
                int runCost = new Random().Next(1, 201);
                GetPlayerProtection();
                
                if (canPlayerRun == false)
                {  TypeWriter.WriteLine($"To flee will cost you {runCost} Gold, will you fight?: yes / no",TypeWriter.Speed.List);
                   string playerFlee = Console.ReadLine();
                    if (playerFlee.ToLower() == "no")
                    {
                        if (playerGold < runCost)
                        {
                            TypeWriter.WriteLine("You don't have sufficent funds to refuse",TypeWriter.Speed.List);
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
                    Console.ReadLine();
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
                    int damage = monster.attackPoints - protection;
                    if ( damage < 0 )
                    {
                        damage = 0;
                        TypeWriter.WriteLine(new Text($"Your "),
                                             new Text($"{playerArmour.name} ",Colours.Protection),
                                             new Text("deflected the attack", Colours.Speech, TypeWriter.Speed.List));
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
            int maxDamage = 100;
            if ( playerWeapon != null )
            {
                maxDamage += playerWeapon.damage;
            }    
            return maxDamage;
        }

        private int GetPlayerMinDamage()
        {
            int MinDamage = 1;
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

                var kinghtIntroduction = values[0].Replace("{monster.spices}",$"{monster.spices}");
                var kinghtWinWay = values[1].Replace("{monster.spices}",$"{monster.spices}");
                var knightEnd = values[2].Replace("{monster.spices}",$"{monster.spices}");

                TypeWriter.WriteLine( new Text(kinghtIntroduction),
                                      new Text(kinghtWinWay,Colours.Attack),
                                      new Text(knightEnd));
            }                                                                   
        }

        public static string showPlayerOptions()
        {
            TypeWriter.WriteLine();
            TypeWriter.WriteLine(new Text("Where would you like to go: north, south, east, west, the "),
                                 new Text("(sh)shop ",Colours.Cotton),
                                 new Text("or to the "),
                                 new Text("(bd)black dungeon:",Colours.BlackDungeon,TypeWriter.Speed.List));
            string playerDirection = Console.ReadLine();
            return playerDirection;
        }

        public void Move(string playerDirection)
        {
            endGameCheck();

            //adjust user short cut to full string
            switch( playerDirection )
            {
                case "bd" : playerDirection = "black dungeon"; break;
                case "sh" : playerDirection = "shop"; break;
            }    

            TypeWriter.WriteLine($"You have moved to the {playerDirection}", TypeWriter.Speed.List);
            switch( playerDirection )
            {
                case "black dungeon" : blackDungeon(); break;
                case "shop" : itemShop(); break;
            }    

            int monsterGoldOrNothing = new Random().Next(1, 5);
            switch (monsterGoldOrNothing)
            {
                case 1: Fight(PickMonsterForEveryDayFight());  break;
                case 2: FoundNothing(); break;
                case 3: FoundGold(); break;
                default: AwardMedicine(); break;
            }
        }
        private static void FoundNothing()
        {
            TypeWriter.WriteLine("Nothing happend", TypeWriter.Speed.List);
        }

        private void FoundGold()
        {
            int foundGold = new Random().Next(1, 81);
            TypeWriter.WriteLine(new Text("You found "),
                                 new Text($"{foundGold} gold coins ",Colours.Gold),
                                 new Text("on the road", Colours.Speech,TypeWriter.Speed.List));
            playerGold += foundGold;
            List<Text> goldList = new List<Text>();
            goldList.Add( new Text("You now have "));
            goldList.Add( new Text($"{playerGold} gold coins", Colours.Gold, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(goldList);
        }

        private void blackDungeon()
        {
            canPlayerRun = true;

            TypeWriter.WriteLine("the gates close behind you, there is no runing",TypeWriter.Speed.Talk);

            TypeWriter.WriteLine($"{userName} the time has come, will you kill the mage or pay him?: kill / pay",TypeWriter.Speed.Talk);
            string killOrPay = Console.ReadLine();
            switch (killOrPay.ToLower())
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
            TypeWriter.WriteLine("Why are you still here, you can leve now you have no futher use",TypeWriter.Speed.Talk);
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
            TypeWriter.WriteLine("Untill we meet again",TypeWriter.Speed.Talk);
        }

        public void endGameCheck()
        {
            if (playerGold >= 2000)
            {

                TypeWriter.WriteLine($"{userName} you have the required funds to confront the high dark mage",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("Are you ready: yes / no",TypeWriter.Speed.Talk);
                string playerAwnser = Console.ReadLine(); 
                if (playerAwnser.ToLower() == "yes")
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
                medMessage.Add( new Text("You have found some medicine and you gain "));
                medMessage.Add( new Text($"{medicine} HP",Colours.Medicine));
                playerHP += medicine;
                TypeWriter.WriteLine(medMessage);
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
                                 new Text($"{armour.name} " ,Colours.Armour),
                                 new Text($"- "),
                                 new Text($"{armour.price} Gold " , Colours.Gold),
                                 new Text($"- "),
                                 new Text($"{armour.protection} protection " ,Colours.Protection),
                                 new Text("- "),
                                 new Text($"{armour.discription}"));
        }

        public void itemShop()
        {
            TypeWriter.WriteLine();
            if (cottonIntro == false)
            {
                List<Text> cottonList = new List<Text>();
                cottonList.Add(new Text("hello traveller, I'm ", Colours.Speech, TypeWriter.Speed.Talk));
                cottonList.Add(new Text("Cotton ", Colours.Cotton, TypeWriter.Speed.Talk));
                cottonList.Add(new Text("and this is my shop, whats your name?", Colours.Speech, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine(cottonList);
                cottonUserName = Console.ReadLine();
                TypeWriter.WriteLine($"{cottonUserName}, hmm ... nice!");
                cottonIntro = true;
            }
            if (hasPlayerBeenInShop == true)
            {
                somethingCool();
            }
            else
            {   
                TypeWriter.WriteLine($"Hello {cottonUserName} ");
                shopCommonPlace();
                hasPlayerBeenInShop = true;
            }

        }

        private void shopCommonPlace()
        {
            TypeWriter.WriteLine(new Text("Are you looking for "),
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

            string playerItemType = Console.ReadLine();

            switch (playerItemType.ToLower())
            {
                case "w": BuyWeapon(); break;
                case "a": BuyArmour(); break;
                case "m": BuyMedicine(); break;
                case "l": TypeWriter.WriteLine($"Bye {cottonUserName} :)", TypeWriter.Speed.Talk); break;
                case "h": TypeWriter.WriteLine("Sure thing");
                          morseCodeMessage();
                          shopCommonPlace();
                break;

                default: TypeWriter.WriteLine("Sorry but I don't understand", TypeWriter.Speed.Talk);
                         itemShop(); 
                         break;
            }
        }

        private void somethingCool()
        {
            if (playerHasSeenMorse == false)
            {
                TypeWriter.WriteLine(new Text($"Hello {cottonUserName}, you want to hear somthing cool: yes/no"));
                string somthingCoolAwnser = Console.ReadLine();

                switch (somthingCoolAwnser)
                {
                    case "yes":
                        playerHasSeenMorse = true;
                        TypeWriter.WriteLine(new Text("Some guy came the other day and gave me this tape (make shure you can hear it :)", Colours.Speech, TypeWriter.Speed.Talk));
                        morseCodeMessage();
                        Thread.Sleep(1000);
                        TypeWriter.WriteLine("strange huh?... but anyway");
                        shopCommonPlace(); 
                        break;

                    case "no":
                        TypeWriter.WriteLine($"O.K {cottonUserName} ");
                        shopCommonPlace();  
                        break;

                    default:
                        TypeWriter.WriteLine("Sorry but I don't understand");
                        shopCommonPlace();  
                        break;
                }
            }
            else
            {
                TypeWriter.WriteLine($"Hello {cottonUserName}");
                shopCommonPlace();
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
            Console.Clear();
            playerStats();
            List<Medicine> shopMedicine = LoadMedicine("./Config/Medicine.csv");

            for (int i = 0; i < shopMedicine.Count; i++)
            {
                DisplayMedicine(shopMedicine[i], i);
            }
            TypeWriter.WriteLine($"[{shopMedicine.Count}] Exit this part of shop",TypeWriter.Speed.List);

            string userMedicineChoice = Console.ReadLine();
            //making shure that the user inputs a valid awnswer
            int purchaseChoice = 0;
            try 
            {
                purchaseChoice = Int32.Parse(userMedicineChoice);
            }
            catch (Exception)
            {
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                Thread.Sleep(1000);
                BuyMedicine();
                return;
            }

            if (purchaseChoice >= shopMedicine.Count)
            {
                itemShop();
                return;
            }
            if (playerGold >= shopMedicine[purchaseChoice].price)
            {
                MedicineChosen(shopMedicine[purchaseChoice]);
            }
            else
            {
                TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold",TypeWriter.Speed.Talk);
                BuyMedicine();
            }

        }

        private void MedicineChosen(Medicine medicine)
        {
            TypeWriter.WriteLine($"Good choice, you've got yourself a {medicine.name}",TypeWriter.Speed.Talk);
            healing = medicine.healing;
            playerHP += healing;
            playerGold -= medicine.price;
            BuyMedicine();
        }

        private void DisplayMedicine(Medicine medicine, int i)
        {
            
            TypeWriter.WriteLine(new Text($"[{i}] "),
                                 new Text($"{medicine.name} ",Colours.Medicine),
                                 new Text($"- "),
                                 new Text($"{medicine.price} Gold ", Colours.Gold),
                                 new Text($"- "),
                                 new Text($"{medicine.healing} healing ",Colours.Health),
                                 new Text($"- "),
                                 new Text($"{medicine.discription}"));
        }



        private void BuyArmour()
        {
            Console.Clear();
            playerStats();

            for (int i = 0; i < bunchOfArmour.Count; i++)
            {
                DispalyArmour(bunchOfArmour[i], i);
            }
            TypeWriter.WriteLine($"[{bunchOfArmour.Count}] Exit this part of shop",TypeWriter.Speed.List);

            string userArmorChoice = Console.ReadLine();

            int purchaseChoice = 0;
            try 
            {
                purchaseChoice = Int32.Parse(userArmorChoice);
            }
            catch (Exception)
            {
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                Thread.Sleep(1000);
                BuyArmour();
                return;
            }
            
            if (purchaseChoice >= bunchOfArmour.Count)
            {
                itemShop();
                return;
            }

            if (playerGold < bunchOfArmour[purchaseChoice].price)
            {
                TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold",TypeWriter.Speed.Talk);
                BuyArmour();
                return;
            }

          
            //first return any itme you may already have
            if (playerArmour != null)
            {
                bunchOfArmour.Add(playerArmour);
            }
            ArmourChosen(bunchOfArmour[purchaseChoice]);
            bunchOfArmour.RemoveAt(purchaseChoice);
            TypeWriter.WriteLine($"Good chioce, you've got your self a {playerArmour.name}",TypeWriter.Speed.Talk);
            BuyArmour(); 
        }

        private void ArmourChosen(Armour armour)
        {
            playerArmour = armour;
            playerGold -= playerArmour.price;
            
        }

        private void BuyWeapon()
        {
            Console.Clear();
            playerStats();

            for (int i = 0; i < shopWeapons.Count; i++)
            {
                DisplayWeapons(shopWeapons[i], i);
            }
            TypeWriter.WriteLine($"[{shopWeapons.Count}] Exit this part of shop",TypeWriter.Speed.List);

            string playerChoiceString = Console.ReadLine();

               int playerChoice = 0;
            try 
            {
                playerChoice = Int32.Parse(playerChoiceString);
            }
            catch (Exception)
            {
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk);
                Thread.Sleep(1000);
                BuyWeapon();
                return;
            }

            if (playerChoice >= shopWeapons.Count)
            {
                itemShop();
                return;
            }

            WeaponChosen(shopWeapons, playerChoice);
        }

        private void WeaponChosen(List<Weapon> shopWeapons, int playerChoice)
        {
            if (playerGold < shopWeapons[playerChoice].price)
            {
               TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold", TypeWriter.Speed.Talk);
               Thread.Sleep(1000);
               BuyWeapon();
            }
            else
            {
                 //first return any itme you may already have
                if (playerWeapon != null)
                {
                   shopWeapons.Add(playerWeapon); 
                }
                chosenWeapon(shopWeapons[playerChoice]);
                shopWeapons.RemoveAt(playerChoice);
                TypeWriter.WriteLine($"Good chioce you've got your self a {playerWeapon.name}", TypeWriter.Speed.Talk);
                BuyWeapon();
            }
        }

        private void chosenWeapon(Weapon weapon)
        {
            playerWeapon = weapon;
            playerGold -= playerWeapon.price;
        }

        private static void DisplayWeapons(Weapon weapon, int index) 
        {
            TypeWriter.WriteLine(new Text($"[{index}] "),
                                 new Text($"{weapon.name} ",Colours.Attack), 
                                 new Text("- "),
                                 new Text($"{weapon.price} Gold ", Colours.Gold), 
                                 new Text("- "),
                                 new Text($"{weapon.damage} Extra Damage ", Colours.Damage),
                                 new Text($"- {weapon.discription}"));
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

            bunchOfArmour.Clear();
            shopWeapons.Clear();           
        }

        internal bool End()
        {
            var totalXP = killXP + cottonXP;
            TypeWriter.WriteLine();
            TypeWriter.WriteLine($"Final score: {totalXP}");
            TypeWriter.WriteLine();
            TypeWriter.WriteLine("Game Over",TypeWriter.Speed.Talk);    
            TypeWriter.WriteLine($"psst {cottonUserName} do you want to try again: yes / no",TypeWriter.Speed.Talk);
            
            string tryAgain = Console.ReadLine();
            switch(tryAgain.ToLower())
            {
                case "yes":
                    playerReset();
                    return true;

                default:
                    TypeWriter.WriteLine("bye for now :)", TypeWriter.Speed.Talk);
                    TypeWriter.WriteLine("");
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
            leaderBoard.Add( currentGameEntry);

            //fill the leaderboard will the raw leaderboard
            foreach( var rawEntry in rawLeaderBoard)
            {
                leaderBoard.Add( new LeaderBoardEntry( rawEntry ) );
            }
            
            leaderBoard.Sort();
            leaderBoard.Reverse();

            if (leaderBoard.Count > 9)
            {
                leaderBoard.RemoveAt(10); 
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
            TheGame theGame = new TheGame();
             
            bool replay = true;

            //replay loop
            while ( replay )
            {
                Console.Clear();
                theGame.Begin(); 
                try
                {
                    //game loop
                    while (true)
                    {
                        string direction = TheGame.showPlayerOptions();
                        Console.Clear();
                        theGame.playerStats();
                        TypeWriter.WriteLine("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _",TypeWriter.Speed.List);
                        TypeWriter.WriteLine();
                        theGame.Move( direction.ToLower() );
                    }
                }
                catch ( Exception e )
                {
                    TypeWriter.WriteLine( e.Message );
                }
                replay = theGame.End();
            }
           
        }

    }
}
