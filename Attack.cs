using System;
using System.Collections.Generic;
using game;

internal class Attack
{

    public Attack(string name, int damage)
    {
        this.name = name;
        this.damage = damage;
    }

    public string name { get; internal set; }
    public int damage { get; internal set; }
}