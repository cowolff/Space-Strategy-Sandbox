using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet : MonoBehaviour
{

    public string planetName;

    public int ownerId;

    public string description;

    public int numberOfBuildings;

    public GameObject fleet_1, fleet_2, spacestation_slot;

    public GameObject line_prefab;

    public GameObject spacestation_prefab;

    public GameObject spacestation;

    public BuildingGalaxy[] buildings;

    public List<GameObject> connectingPlanets;

    public List<GameObject> nearPlanets;

    public int stationLevel;

    // TO-DO: Building type model implementieren
    // public GameObject[] placeableBuildings;
    public List<ShipTypeModel> producableShips;

    public List<BuildingModel> placeableBuildings;

    public FleetGalaxy[] fleets;
    public Stack<ShipGalaxy> productionStackSpace;


    public Stack<BuildingModel> productionStackBuildings;
    private BuildingModel currentBuilding;
    public float buildingCountdown;
    // Start is called before the first frame update

    void Awake(){
        placeableBuildings = new List<BuildingModel>();
        productionStackBuildings = new Stack<BuildingModel>();
    }

    void Start()
    {
        buildings = new BuildingGalaxy[10];
        fleets = new FleetGalaxy[3];
        productionStackSpace = new Stack<ShipGalaxy>();
        connectingPlanets = new List<GameObject>();

        if(this.stationLevel > 0){
            GameObject spacestation = Instantiate(spacestation_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            spacestation.transform.parent = this.transform;
            spacestation.transform.position = spacestation_slot.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBuilding == null && productionStackBuildings.Count == 0){
            return;
        }
        if(currentBuilding == null && productionStackBuildings.Count != 0){
            currentBuilding = productionStackBuildings.Pop();
            buildingCountdown = (float)currentBuilding.production_time;
            return;
        }
        if(buildingCountdown > 0){
            buildingCountdown -= Time.deltaTime;
            return;
        } 
        else if(currentBuilding != null) {
            Debug.Log("Building finished: " + currentBuilding.building_name);
            BuildingGalaxy newBuilding = new BuildingGalaxy();
            newBuilding.building_name = currentBuilding.building_name;
            newBuilding.income = currentBuilding.income;
            this.__PlaceBuilding(newBuilding);
            currentBuilding = null;
            return;
        }
    }

    public BuildingGalaxy[] GetBuildings(){
        return this.buildings;
    }

    public List<ShipTypeModel> GetProducableShips(){
        List<ShipTypeModel> models = this.producableShips.FindAll(x => x.req_space_station_level <= this.stationLevel);
        return models;
    }

    private void __PlaceBuilding(BuildingGalaxy building){
        for(int i = 0; i < 10; i++){
            Debug.Log(i);
            if(this.buildings[i] == null){
                this.buildings[i] = building;
                break;
            }
        }
    }

    public void removeBuilding(int pos){
        buildings[pos] = null;
    }

    public int GetIncome(){
        int income = 0;
        for(int i = 0; i < 10; i++){
            if(buildings[i] != null){
                income += buildings[i].income;
            }
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

    public void SetSpaceStation(int level){
        this.stationLevel = level;
        Debug.Log("Station level: " + level);
        if(this.stationLevel > 0){
            GameObject spacestation = Instantiate(spacestation_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            spacestation.transform.parent = this.transform;
            spacestation.transform.position = spacestation_slot.transform.position;
            spacestation.transform.localScale -= new Vector3(0.99f, 0.99f, 0.99f);
            this.spacestation = spacestation;
        } else {
            Destroy(this.spacestation);
        }
    }

    public void RemoveFleet(GameObject fleet){

    }

    public void AddShipProduction(string ship_name){
        
    }

    public void AddBuilding(string building){
        BuildingModel newBuilding = this.placeableBuildings.Find(x => x.building_name == building);
        productionStackBuildings.Push(newBuilding);
    }

    public static int GetTimestamp()
    {
        return Int32.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff"));
    }
}