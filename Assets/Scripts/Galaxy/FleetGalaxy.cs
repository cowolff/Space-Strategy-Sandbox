using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetGalaxy
{

    public List<ShipGalaxy> ships;
    public string name;
    public string faction;

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
}
