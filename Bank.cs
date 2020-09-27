using System;
using System.Collections.Generic;
using System.Threading;

namespace game
{
    internal class Bank
    { 
        int balance = 0;
        private TheGame theGame;

        public Bank(TheGame theGame)
        {
            this.theGame = theGame;
        }

        public void Start()
        {
            bool leaveBank = false;
            while (leaveBank == false)
            {
                BankIntro();

                var Awnser = theGame.GetLowerReply();
                switch (Awnser)
                {
                    case "d":
                    case "deposit":
                        BankDeposit();
                        break;

                    case "w":
                    case "withdrawal":
                        BankWithdrawal();
                        break;

                    case "l":
                        leaveBank = true;
                        break;

                    default:
                        TypeWriter.WriteLine("Sorry, but I don't understand");
                        break;
                }        
            }
        }

        private void BankWithdrawal()
        {
            bool leave = false;

            while (leave == false)
            {
                int withdrawNumber;

                if (balance > 0)
                {
                    TypeWriter.WriteLine(new Text("You have "),
                                         new Text($"{balance} gold ", Colours.Gold),
                                         new Text("in your account"));
                    TypeWriter.WriteLine();
                }

                TypeWriter.WriteLine("How much gold would you like to withdraw?");
                TypeWriter.WriteLine("or l to leave");
                var withdrawAmount = theGame.GetLowerReply();
                if (withdrawAmount == "l")
                {
                    leave = true;
                    TypeWriter.WriteLine();
                    continue;
                }

                try 
                {
                    withdrawNumber = int.Parse(withdrawAmount);
                }
                catch (Exception)
                {
                    TypeWriter.WriteLine("Sorry but only numbers are valid",TypeWriter.Speed.Talk);
                    TypeWriter.WriteLine();
                    Thread.Sleep(1000);
                    continue;
                }

                if (balance >= withdrawNumber)
                {
                    balance -= withdrawNumber;
                    theGame.player.gold += withdrawNumber;

                    TypeWriter.WriteLine($"Your withdrawal of {withdrawNumber} gold has been made");
                    TypeWriter.WriteLine();
                }
                else
                {
                    TypeWriter.WriteLine("Sorry, but you don't have that amount in your account");
                    TypeWriter.WriteLine();
                }
            }
        }

        private void BankDeposit()
        {
            bool leave = false;

            while (leave == false)
            {
                int depositNumber;

                if (balance > 0)
                {
                    TypeWriter.WriteLine(new Text("You have "),
                                         new Text($"{balance} gold ", Colours.Gold),
                                         new Text("in your account"));
                    TypeWriter.WriteLine();
                }

                TypeWriter.WriteLine("How much gold would you like to deposit?");
                TypeWriter.WriteLine("or l to leave");
                var depositAmount = theGame.GetLowerReply();
                if (depositAmount == "l")
                {
                    leave = true;
                    TypeWriter.WriteLine();
                    continue;
                }

                try 
                {
                    depositNumber = int.Parse(depositAmount);
                }
                catch (Exception)
                {
                    TypeWriter.WriteLine("Sorry but only numbers are valid",TypeWriter.Speed.Talk);
                    TypeWriter.WriteLine();
                    Thread.Sleep(1000);
                    continue;
                }

                if (theGame.player.gold < depositNumber)
                {
                    TypeWriter.WriteLine("Sorry, but you don't have that amount of gold");
                    TypeWriter.WriteLine();
                }
                else
                {
                    balance =+ depositNumber;
                    theGame.player.gold -= depositNumber;

                    TypeWriter.WriteLine($"Your deposit of {depositNumber} gold has been made");
                    TypeWriter.WriteLine();
                }
            }
        }

        private void BankIntro()
        {
            TypeWriter.WriteLine(new Text("Hello would you like to make a ", Colours.Speech, TypeWriter.Speed.Talk),
                                 new Text("(d)deposit ", Colours.Deposit, TypeWriter.Speed.Talk),
                                 new Text("or make a ", Colours.Speech, TypeWriter.Speed.Talk),
                                 new Text("(w)withdrawal", Colours.Withdrawal, TypeWriter.Speed.Talk));
            TypeWriter.WriteLine(new Text("or l to leave", Colours.Speech, TypeWriter.Speed.Talk));
        }   
    }
}