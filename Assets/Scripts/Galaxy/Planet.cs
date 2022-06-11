using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public string planetName;

    public string description;

    public int numberOfBuildings;

    public GameObject fleet_1, fleet_2;

    public GameObject line_prefab;

    public BuildingGalaxy[] buildings;

    public List<GameObject> connectingPlanets;

    public List<GameObject> nearPlanets;

    // TO-DO: Building type model implementieren
    // public GameObject[] placeableBuildings;
    public List<ShipTypeModel> producableShips;

    public FleetGalaxy[] fleets;
    public Stack<ShipGalaxy> productionStackSpace;
    public Stack productionStackGround;
    // Start is called before the first frame update
    void Start()
    {
        buildings = new BuildingGalaxy[numberOfBuildings];
        fleets = new FleetGalaxy[3];
        productionStackSpace = new Stack<ShipGalaxy>();
        connectingPlanets = new List<GameObject>();
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

    public void AddFleet(GameObject fleet){
        if(!fleet_1.transform.GetComponent<PlanetFleetSpot>().has_Fleet()){
            fleet_1.transform.GetComponent<PlanetFleetSpot>().add_Fleet(fleet);
        } else if(!fleet_2.transform.GetComponent<PlanetFleetSpot>().has_Fleet()){
            fleet_2.transform.GetComponent<PlanetFleetSpot>().add_Fleet(fleet);
        } else {
            fleet_1.transform.GetComponent<PlanetFleetSpot>().merge_Fleet(fleet);
        }
    }

    public void RemoveFleet(GameObject fleet){

    }

    public void AddShipProduction(){
        
    }
}