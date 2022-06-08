using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{

    public string PlanetName;

    public string Description;

    public int numberOfBuildings;

    public BuildingGalaxy[] buildings;

    public FleetGalaxy[] fleets;
    public Stack<ShipGalaxy> productionStackSpace;
    public Stack productionStackGround;
    // Start is called before the first frame update
    void Start()
    {
        buildings = new BuildingGalaxy[numberOfBuildings];
        fleets = new FleetGalaxy[3];
        productionStackSpace = new Stack<ShipGalaxy>();
        productionStackGround = new Stack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BuildingGalaxy[] GetBuildings(){
        return this.buildings;
    }

    public void addBuilding(BuildingGalaxy newBuilding, int pos){
        if(buildings[pos] == null){
            buildings[pos] = newBuilding;
        }
    }

    public void removeBuilding(int pos){
        buildings[pos] = null;
    }

    public int getIncome(){
        int income = 0;
        for(int i = 0; i < numberOfBuildings; i++){
            income = income + buildings[i].income;
        }
        return income;
    }

    public List<UnitGalaxy> getProducableUnits(){
        List<UnitGalaxy> producable = new List<UnitGalaxy>();
        for(int i = 0; i < numberOfBuildings; i++){
            if(buildings[i] != null){
                producable.AddRange(buildings[i].producable);
            }
        }
        return producable;
    }

    public void AddFleet(FleetGalaxy fleet, int index){
        if(fleets[index] == null){
            fleets[index] = fleet;
        } else {
            fleets[index].CombineFleets(fleet);
        }
    }

    public void AddShipProduction(){
        
    }
}