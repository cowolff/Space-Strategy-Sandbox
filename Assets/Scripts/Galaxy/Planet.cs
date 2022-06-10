using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public string planetName;

    public string description;

    public int numberOfBuildings;

    public GameObject line_prefab;

    public BuildingGalaxy[] buildings;

    public string[] connectingPlanets;

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

    public void CreateConnection(List<GameObject> planets){
        foreach(string planet in this.connectingPlanets){
            Debug.Log(planet);
            GameObject line = Instantiate(line_prefab);
            GameObject planet_obj = planets.Find(x => x.transform.GetComponent<Planet>().planetName == planet);
            LineRenderer line_renderer = line.transform.GetChild(0).transform.GetComponent<LineRenderer>();
            line_renderer.SetPosition(0, this.transform.position);
            line_renderer.SetPosition(1, planet_obj.transform.position);
        }
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