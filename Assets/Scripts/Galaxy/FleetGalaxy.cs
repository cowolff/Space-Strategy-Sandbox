using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetGalaxy : MonoBehaviour
{

    public List<UnitGalaxy> ships;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShip(UnitGalaxy newShip){
        ships.Add(newShip);
    }

    public void CombineFleets(FleetGalaxy fleet){
        ships.AddRange(fleet.ships);
    }
}