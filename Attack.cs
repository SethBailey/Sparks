using System;
using System.Collections.Generic;
using game;

internal class Attack
{
    public enum AttackResult
    {
        Null,
        Dead,
        Damaged,
        Deflected    
    } 

    public Attack(string attackName, int damage)
    {
        this.name = attackName;
        this.damage = damage;
    }

    public string name { get; internal set; }
    public int damage { get; internal set; }
}