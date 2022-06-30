using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGalaxy
{
    public string name { get; set; }
    public int health { get; set; }
    public string description { get; set; }
    public int damage_per_second { get; set; }

    public ShipGalaxy(string name, int health, string description, int damage_per_second){
        this.name = name;
        this.health = health;
        this.description = description;
        this.damage_per_second = damage_per_second;
    }
}