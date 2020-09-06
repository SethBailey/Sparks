using System;
using game;

internal class HealthBar
{
    private int maxHealthpoints;
    private int critical;
    private readonly int danger;

    public HealthBar(int maxHealthpoints, int critical, int danger)
    {
        this.maxHealthpoints = maxHealthpoints;
        this.critical = critical;
        this.danger = danger;
    }

    internal void Display(int playerHealth)
    {
        var previousColour = Console.ForegroundColor;
        var barHealthNumber = 100 * playerHealth / maxHealthpoints;

        Console.ForegroundColor = ConsoleColor.DarkRed;
        for (int i = 0; i < barHealthNumber; i++)
        {
            if (i == critical)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (i == danger)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            TypeWriter.Type("|", TypeWriter.Speed.List);
        }
        Console.ForegroundColor = previousColour;
    }
} 