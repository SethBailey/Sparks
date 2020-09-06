using System;
using System.Collections.Generic;

namespace game
{

internal class Fight
{
    internal bool Combat(Player player, Monster monster)
    {
        TypeWriter.WriteLine($"You run into a {monster.spices}");
        if (player.reaction >= monster.reaction)
        {
            //player go's first
            var attack = GetPlayerAttack(player);
            monster.TakeAttack(attack);
        }

        return true;
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
    }

}