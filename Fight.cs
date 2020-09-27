using System;
using System.Collections.Generic;

namespace game
{

internal class Fight
{
    internal bool Combat(Player player, Monster monster)
    {
        TypeWriter.WriteLine($"You run into a {monster.name}: {monster.HP} HP");
        Character attacker;
        Character defender;

        if (player.reaction >= monster.reaction)
        {
            attacker = player;
            defender = monster;        
        }
        else
        {
            attacker = monster;
            defender = player;
        }

        Attack.AttackResult result = Attack.AttackResult.Null;
        while ( result != Attack.AttackResult.Dead)
        {
            result = doStrike(attacker,defender);
            if ( result != Attack.AttackResult.Dead)
            {
                //swap attacker and defender
                (attacker, defender) = (defender, attacker);
            }
        }

        if (attacker == player)
        {
            //player wins
            TypeWriter.WriteLine(new Text("congratulations you win", Colours.Speech, TypeWriter.Speed.Talk));
            return true;
        }
        else
        {
            //monsert wins
            TypeWriter.WriteLine("you loose");
        }
        
        return false;
    }

        private Attack GetMonsterAttack(Monster monster)
        {
            List<Attack> monsterAttacks = monster.GetAttacks();
            int count = monsterAttacks.Count;

            //Let the monster Choose
            Random rnd = new Random();
            int monsterChoice = rnd.Next(0, count);

            //return his choice
            return monsterAttacks[monsterChoice];
        }

        private Attack GetPlayerAttack(Player player)
        {
            List<Attack> attacks = player.GetAttacks();
            TypeWriter.WriteLine("Possible attacks");
            int count = 1;
            foreach (var attack in attacks)
            {
                TypeWriter.WriteLine(new Text($"[{count}] {attack.name}"));
                count++;
            }

            //Let the palyer Choose
            var choice = Console.ReadLine();
            int playerChoice = int.Parse(choice);

            //return his choice
            return attacks[playerChoice-1];
        }

        private Attack.AttackResult doStrike(Character attacker, Character defender)
        {
            //player go's first
            var attack = attacker.displayAttack();
            return defender.TakeAttack(attack);
        }
    }
}