﻿using System;
using System.Collections.Generic;

namespace game
{
    class TheGame
    {
       int playerHP = 100;
       string userName;
       public int playerGold = 0;
       public string cottonUserName;
       bool cottonIntro = false;
       int protection;
       bool canPlayerRun = false;
       bool isPlayerWithMage = false;
       public bool isPlayerNew = true;
       
       Weapon playerWeapon;
       Armour playerArmour;
       private int healing;

        List<Armour> bunchOfArmour = new List<Armour>();
        List<Weapon> shopWeapons = new List<Weapon>();

        public void Begin()
        {
           

             if (isPlayerNew)
             {
                //Do player introductions
                TypeWriter.WriteLine("Enter your name:",TypeWriter.Speed.Talk); 
                userName = Console.ReadLine();
                 List<Text> startList = new List<Text>();
                startList.Add( new Text($"Nice to meet you {userName} my name is ", ConsoleColor.White, TypeWriter.Speed.Talk));
                startList.Add( new Text("Sparks", ConsoleColor.DarkBlue, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine(startList);

                TypeWriter.WriteLine($"You {userName} are a knight and princess Kafe has been taken hostage by the high dark mage",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("however his prices are steep, and he is demanding 2000 gold coins for her freedom",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("You have two options: collect the gold and pay the mage or",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("Slay him, take back princess Kafe and rid the land of his vile existance",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine("But there is a reason he is still alive, are you up for the challange? time to find out.",TypeWriter.Speed.Talk);
                TypeWriter.WriteLine();
                playerStats();
                FillShopWithArmour();
                FillShopWithWeapons();
             }
             else
             {
                 TypeWriter.WriteLine();
                 TypeWriter.WriteLine($"Welcome back {userName}",TypeWriter.Speed.Talk);
                 TypeWriter.WriteLine("Lets just get on with it",TypeWriter.Speed.Talk);
                 TypeWriter.WriteLine();
                 playerStats();
             }           
        }

        private void FillShopWithArmour()
        {
            //Create our set of armour
            bunchOfArmour.Add(new Armour("shinGuard", 100, 25, "Achillyes would be proud"));
            bunchOfArmour.Add(new Armour("KneeGuard", 200, 45, "Because all the best warriors need one ;)"));
            bunchOfArmour.Add(new Armour("helmet", 250, 65, "Keeps your head safe and works as a hat"));
            bunchOfArmour.Add(new Armour("brestplate", 340, 80, "To keep them pecks from geting any unwanted scars"));
            bunchOfArmour.Add(new Armour("shield", 400, 100, "You should never leave home without one"));
            bunchOfArmour.Add(new Armour("ToothGuard", 500, 130, "Mmmifffmm mmmfmmm immmmfmm"));
        }

        private void FillShopWithWeapons()
        {
            //Create our set of weapons
            shopWeapons.Add(new Weapon("knuckle duster", 150, 20, "Not recomended for real dusting"));
            shopWeapons.Add(new Weapon("sword", 250, 35, "Very standed sword but still good to have in a fight"));
            shopWeapons.Add(new Weapon("nunchuck", 300, 45, "it's a nunnnnnchuuuuuckk!!!"));
            shopWeapons.Add(new Weapon("axe", 400, 60, "A bit to heavy for me but the bigger thay are the harder they fall"));
            shopWeapons.Add(new Weapon("spear", 700, 100, "Got this one stright out of a Chinese myth"));
            shopWeapons.Add(new Weapon("revolver", 1700, 250, "The newest type of weapon, but it's very very loud"));
        }

        public void playerStats()
        {
            List<Text> messsage = new List<Text>();
            messsage.Add( new Text("You have "));
            messsage.Add( new Text($"{playerHP} HP ", ConsoleColor.Red));
            messsage.Add( new Text($"and "));
            messsage.Add( new Text($"{playerGold} Gold", ConsoleColor.DarkYellow));
            
            //string playerStatus = $"You have {playerHP} HP and {playerGold} Gold";
            if (playerWeapon != null )
            {
                messsage.Add( new Text($" and a "));
                messsage.Add( new Text($"{playerWeapon.name} ", ConsoleColor.Cyan));
            }
            if ( playerArmour != null)
            {
                messsage.Add( new Text($" and a {playerArmour.name}" , ConsoleColor.Blue));
            }

            TypeWriter.WriteLine(messsage);

        } 

        public void Fight( Monster monster )
        {
            TypeWriter.WriteLine($"You run into a {monster.spices}",TypeWriter.Speed.List);
            TypeWriter.WriteLine($"The {monster.spices} has {monster.healthPoints} HP",TypeWriter.Speed.List);
            playerStats();
            
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
                            TypeWriter.WriteLine($"{userName} you ran from the fight and make it back to safety",TypeWriter.Speed.List);
                            TypeWriter.WriteLine($"But you lost {runCost} Gold by paying the monster to let you go",TypeWriter.Speed.List);
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
                    TypeWriter.WriteLine($"You strike a blow and deal {playerAP} damage",TypeWriter.Speed.List);
                    monster.healthPoints -= playerAP;
                    if (monster.healthPoints < 0)
                    {
                        monster.healthPoints = 0;
                    }
                    TypeWriter.WriteLine($"The {monster.spices} has {monster.healthPoints} HP",TypeWriter.Speed.List);
                    playerStats();
                    TypeWriter.WriteLine();
                }
                else
                {
                    int damage = monster.attackPoints - protection;
                    if ( damage < 0 )
                    {
                        damage = 0;
                        TypeWriter.WriteLine($"Your {playerArmour.name} deflected the attack",TypeWriter.Speed.List);
                    }
                     TypeWriter.WriteLine($"The {monster.spices} strike's a blow and deals {damage} damage",TypeWriter.Speed.List);

                    playerHP -= damage;
                    if (playerHP < 0)
                    {
                        playerHP = 0;
                    }
                    TypeWriter.WriteLine($"The monster has {monster.healthPoints} HP",TypeWriter.Speed.List);
                    playerStats();
                    TypeWriter.WriteLine();
                }
            }

            if (playerHP == 0)
            {
                throw new Exception("You Died");
            }
            else
            {
                if (isPlayerWithMage == false)
                {
                    
                    int goldReward = new Random().Next(1,101); 

                    List<Text> winMesssage = new List<Text>();
                    winMesssage.Add( new Text($"{userName} won the fight and got "));
                    winMesssage.Add( new Text($"{goldReward} Gold coins", ConsoleColor.DarkYellow));
                    playerGold += goldReward;
                    TypeWriter.WriteLine(winMesssage);
                    AwardMedicine();         
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

        public static string showPlayerOptions()
        {
            TypeWriter.WriteLine();
            TypeWriter.WriteLine("Where would you like to go: north, south, east, west, the (sh)shop or to the (bd)black dungeon:", TypeWriter.Speed.List);
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
            TypeWriter.WriteLine($"You found {foundGold} gold coins on the road", TypeWriter.Speed.List);
            playerGold += foundGold;
            List<Text> goldList = new List<Text>();
            goldList.Add( new Text("You now have "));
            goldList.Add( new Text($"{playerGold} gold coins", ConsoleColor.DarkYellow, TypeWriter.Speed.Talk));
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
            Fight(new Monster("Dark high mage", 100, 300, 300));
        }

        private void blackDungeonPay()
        {
            if (playerGold >= 2000)
            {
                playerGold -= 2000;
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
            TypeWriter.WriteLine($"If you were a chess piece you would be a queen, my most powerfull piece",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Oh and well done for rescuing Kafe as well",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Why are you still here, you can leve now you have no futher use",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Check Mate",TypeWriter.Speed.Talk);
        }

        private void endMessagePay()
        {
            TypeWriter.WriteLine("Congrats you rescued Kafe, all's well and...",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Oh dear! It seems she's been kidnapped again by the mage",TypeWriter.Speed.Talk);
            List<Text> endList = new List<Text>();
            endList.Add( new Text("Next time you should just ", ConsoleColor.White, TypeWriter.Speed.List));
            endList.Add( new Text("kill him! ", ConsoleColor.DarkRed, TypeWriter.Speed.Talk));
            endList.Add( new Text("and then you won't have this problem", ConsoleColor.White, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(endList);
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
                medMessage.Add( new Text($"{medicine} HP", ConsoleColor.DarkGreen));
                playerHP += medicine;
                TypeWriter.WriteLine(medMessage);
        }

        public Monster PickMonsterForEveryDayFight()
        {
            List<Monster> monsters = new List<Monster>();
            monsters.Add( new Monster("Goblin", 1, 101, new Random().Next(1,101)));
            monsters.Add( new Monster("Hobgoblin", 20, 151, new Random().Next(50,151)));
            monsters.Add( new Monster("Skeleton", 1, 101, new Random().Next(1,171)));
            monsters.Add( new Monster("Giant Spider", 1, 101, new Random().Next(10,121)));
            monsters.Add( new Monster("Troll", 40, 201, new Random().Next(50, 301)));
            monsters.Add( new Monster("starving villager", 1, 81, new Random().Next(1, 101)));
            monsters.Add( new Monster("Giant", 60, 301, new Random().Next(80, 401)));
            monsters.Add( new Monster("Dark knight", 20, 100, new Random().Next(50, 150)));
            monsters.Add( new Monster("Zombie", 1, 50, new Random().Next(1, 50)));
            monsters.Add( new Monster("Lesser dragon", 50, 150, new Random().Next(1, 100)));
            monsters.Add( new Monster("Dragon", 50, 200, new Random().Next(100, 150)));
            monsters.Add( new Monster("Fairy", 1, 50, new Random().Next(1, 50)));
            monsters.Add( new Monster("Knight", 1, 100, new Random().Next(1, 120)));

            int monsterPick = new Random().Next(0, monsters.Count);
            return monsters[monsterPick];
            
        }

        private void DispalyArmour(Armour armour, int index)
        {
            TypeWriter.WriteLine($"[{index}] {armour.name} - {armour.price} Gold - {armour.protection} protection - {armour.discription}",TypeWriter.Speed.List);
        }

        public void itemShop()
        {
            TypeWriter.WriteLine();
            if (cottonIntro == false)
            {
                List<Text> cottonList = new List<Text>();
                cottonList.Add( new Text("hello traveller, I'm ", ConsoleColor.White, TypeWriter.Speed.Talk));
                cottonList.Add( new Text("Cotton ", ConsoleColor.Magenta, TypeWriter.Speed.Talk));
                cottonList.Add( new Text("and this is my shop, whats your name?", ConsoleColor.White, TypeWriter.Speed.Talk));
                TypeWriter.WriteLine(cottonList);
                cottonUserName = Console.ReadLine();
                TypeWriter.WriteLine($"{cottonUserName}, hmm ... nice!");
                cottonIntro = true;
            }
            
            TypeWriter.WriteLine($"Hello {cottonUserName} are you looking for (w)weapons, (a)armour or (m)medicine :)",TypeWriter.Speed.Talk);
            TypeWriter.WriteLine("Or (l) to leave",TypeWriter.Speed.Talk);
            
            string playerItemType = Console.ReadLine();
            
            switch (playerItemType.ToLower())
            {
                case "w" : BuyWeapon(); break;
                case "a" : BuyArmour(); break;
                case "m" : BuyMedicine(); break;
                case "l" : TypeWriter.WriteLine($"Bye {cottonUserName} :)",TypeWriter.Speed.Talk); break;
                default: TypeWriter.WriteLine("Sorry but I don't understand", TypeWriter.Speed.Talk); itemShop(); break;
            }
            
        }

        private void BuyMedicine()
        {
            Console.Clear();
            playerStats();

            List<Medicine> shopMedicine = new List<Medicine>();
            shopMedicine.Add(new Medicine("plaster", 30, 10, "Perfect for all flesh wounds"));
            shopMedicine.Add(new Medicine("bandade", 70, 50, "Sould stop the bleeding from most cuts"));
            shopMedicine.Add(new Medicine("chicken soup", 100, 70, "Made it my self"));
            shopMedicine.Add(new Medicine("Green Liquid", 300, 150, "It's green, so it's ether very good for you or very bad for you"));


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
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk,1000);
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
            
            TypeWriter.WriteLine($"[{i}] {medicine.name} - {medicine.price} Gold - {medicine.healing} healing - {medicine.discription}",TypeWriter.Speed.List);
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
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk,1000);
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
                TypeWriter.WriteLine("Sorry but I couldn't hear you over the sound of my world collapsing because of an error",TypeWriter.Speed.Talk,1000);
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
               TypeWriter.WriteLine($"Sorry {cottonUserName} you don't have enough gold", TypeWriter.Speed.Talk, 1000);
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
            TypeWriter.WriteLine($"[{index}] {weapon.name} - {weapon.price} Gold - {weapon.damage} Extra Damage - {weapon.discription}",TypeWriter.Speed.List);
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
            playerGold = 0;
            playerHP = 100;
            isPlayerNew = false;
            isPlayerWithMage = false;
        }

        internal bool End()
        {
            TypeWriter.WriteLine();
            TypeWriter.WriteLine($"Final score: {playerGold}");
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
                    TypeWriter.WriteLine("bye for now :)",TypeWriter.Speed.Talk);
                    return false;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {   
            /*
            List<Text> list = new List<Text>();
            list.Add( new Text("hello", ConsoleColor.DarkBlue, TypeWriter.Speed.List));
            list.Add( new Text(" World", ConsoleColor.DarkRed, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(list);
            return;
*/

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
                        TypeWriter.WriteLine("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ",TypeWriter.Speed.List);
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
