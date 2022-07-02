using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Planet : MonoBehaviour
{

    public string planetName;
    public string faction;
    public string description;
    public int numberOfBuildings;

    public GameObject fleet_1, fleet_2, spacestation_slot;
    private MeshRenderer fleet_1_renderer, fleet_2_renderer;
    public FleetGalaxy[] fleets;

    public GameObject GalaxyUI;
    private GalaxyUI galaxyUIScript;

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


    public Stack<ShipTypeModel> productionStackSpace;
    public ShipTypeModel currentShip;
    public float shipCountdown;

    public int baseIncome;

    public TMP_Text planetNameText;

    private Color32 rebel_col;
    private Color32 empire_col;


    public Stack<BuildingModel> productionStackBuildings;
    private BuildingModel currentBuilding;
    public float buildingCountdown;
    // Start is called before the first frame update

    void Awake(){
        placeableBuildings = new List<BuildingModel>();
        productionStackBuildings = new Stack<BuildingModel>();
        productionStackSpace = new Stack<ShipTypeModel>();
        connectingPlanets = new List<GameObject>();
        fleet_1_renderer = fleet_1.transform.GetComponent<MeshRenderer>();
        fleet_2_renderer = fleet_2.transform.GetComponent<MeshRenderer>();

        this.empire_col = new Color32(15, 98, 230, 255);
        this.rebel_col = new Color32(222, 41, 22, 255);
    }

    void Start()
    {
        buildings = new BuildingGalaxy[10];
        fleets = new FleetGalaxy[2];
        currentShip = null;
        this.galaxyUIScript = this.GalaxyUI.transform.GetComponent<GalaxyUI>();

        if(this.stationLevel > 0){
            GameObject spacestation = Instantiate(spacestation_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            spacestation.transform.parent = this.transform;
            spacestation.transform.position = spacestation_slot.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.check_building_stack();
        this.check_space_stack();
        this.UpdateFleetSpot();
        this.UpdateText();

    }

    private void UpdateText(){
        if(this.planetNameText.text != this.planetName){
            this.planetNameText.text = this.planetName;
        }
        if(this.faction == "Empire" && this.planetNameText.color != this.empire_col){
            this.planetNameText.color = this.empire_col;
        } else if(this.faction == "Rebel" && this.planetNameText.color != this.rebel_col){
            this.planetNameText.color = this.rebel_col;
        }
    }

    private void UpdateFleetSpot(){
        if(fleets[0] != null && fleet_1_renderer.enabled == false){
            fleet_1_renderer.enabled = true;
            fleet_1.transform.GetComponent<PlanetFleetSpot>().fleet_script = fleets[0];
        } else if(fleets[0] == null && fleet_1_renderer.enabled == true){
            fleet_1_renderer.enabled = false;
            fleet_1.transform.GetComponent<PlanetFleetSpot>().fleet_script = null;
        }
        
        if(fleets[1] != null && fleet_2_renderer.enabled == false){
            fleet_2_renderer.enabled = true;
            fleet_2.transform.GetComponent<PlanetFleetSpot>().fleet_script = fleets[1];
        } else if(fleets[1] == null && fleet_2_renderer.enabled == true){
            fleet_2_renderer.enabled = false;
            fleet_2.transform.GetComponent<PlanetFleetSpot>().fleet_script = null;
        }
    }

    private void check_space_stack(){
        if(currentShip == null && productionStackSpace.Count == 0){
            return;
        }
        if(currentShip == null && productionStackSpace.Count != 0){
            currentShip = productionStackSpace.Pop();
            shipCountdown = (float)currentShip.build_time_in_seconds;
            return;
        }
        if(shipCountdown > 0){
            shipCountdown -= Time.deltaTime;
            return;
        } else if (currentShip != null){
            Debug.Log("Ship finished: " + currentShip.id);
            ShipGalaxy newShip = new ShipGalaxy(currentShip.id, currentShip.tactical_health, currentShip.description, currentShip.damage_per_second);
            for(int i = 0; i < 2; i++){
                if(fleets[i] != null){
                    fleets[i].AddShip(newShip);
                    break;
                }
                if(i == 1){
                    FleetGalaxy newFleet = new FleetGalaxy();
                    newFleet.faction = this.faction;
                    newFleet.AddShip(newShip);
                    fleets[0] = newFleet;
                }
            }
            currentShip = null;
            galaxyUIScript.ReloadBars();
        }
    }

    private void check_building_stack(){
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
            galaxyUIScript.ReloadBars();
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
        income += this.baseIncome;
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
        FleetGalaxy fleet_script = fleet.transform.GetComponent<PlanetFleetSpot>().fleet_script;
        if(fleet_script.faction != this.faction){
            this.FightAutomatedBattle(fleet_script);
            return;
        }
        if(fleets[0] == null){
            fleets[0] = fleet_script;
        } else if(fleets[1] == null){
            fleets[1] = fleet_script;
        } else {
            fleets[0].CombineFleets(fleet_script);
        }
    }

    private void FightAutomatedBattle(FleetGalaxy fleet_script){
        Debug.Log("Battle");
    }

    public void SetSpaceStation(int level){
        this.stationLevel = level;
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
        if(fleet.transform.GetComponent<PlanetFleetSpot>().fleet_script == fleets[0]){
            fleets[0] = null;
            return;
        }
        if(fleet.transform.GetComponent<PlanetFleetSpot>().fleet_script == fleets[1]){
            fleets[1] = null;
            return;
        }
    }

    public void AddShipProduction(string ship_name, bool applyCost = true){
        ShipTypeModel newShip = this.producableShips.Find(x => x.id == ship_name);
        if(applyCost == false){
            Debug.Log(ship_name);
            this.productionStackSpace.Push(newShip);
            return;
        }
        if(galaxyUIScript.ApplyCost(newShip.cost)){
            this.productionStackSpace.Push(newShip);
            Debug.Log("Space Stack: " + productionStackSpace.Count);
        }
    }

    public void AddBuilding(string building){
        BuildingModel newBuilding = this.placeableBuildings.Find(x => x.building_name == building);
        if(galaxyUIScript.ApplyCost(newBuilding.cost)){
            this.productionStackBuildings.Push(newBuilding);
        }
    }

    public static int GetTimestamp()
    {
        return Int32.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff"));
    }
}