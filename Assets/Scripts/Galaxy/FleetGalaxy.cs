using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetGalaxy
{

    public List<ShipGalaxy> ships;
    public string name;
    public string faction;
    public int health;

    public FleetGalaxy(){
        ships = new List<ShipGalaxy>();
    }

    public void AddShip(ShipGalaxy newShip){
        ships.Add(newShip);
    }

    public void CombineFleets(FleetGalaxy fleet){
        ships.AddRange(fleet.ships);
        fleet = null;
    }

    public void ApplyDamage(int damage){
        if(ships.Count != 0){
            ShipGalaxy ship = ships[0];
            int health = ship.health - damage;
            if(health > 0){
                ship.health = health;
            } else {
                this.ships.RemoveAt(0);
                int new_damage = -1 * health;
                this.ApplyDamage(new_damage);
            }
        }
    }

    public int GetDamage(){
        int damage = 0;
        foreach(ShipGalaxy ship in this.ships){
            damage += ship.damage_per_second;
        }
        return damage;
    }

    public int Count(){
        return this.ships.Count;
    }
}
