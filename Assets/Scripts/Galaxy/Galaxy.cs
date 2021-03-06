using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Galaxy : MonoBehaviour
{

    public List<GameObject> planets;

    List<BuildingModel> buildings;

    public GameObject planet_prefab;

    public GameObject line_prefab;

    public GameObject galaxyUI;

    public int userId;

    public string faction;

    public string path;

    // Start is called before the first frame update
    void Start()
    {
        this.faction = StaticInformation.faction;
        this.path = StaticInformation.path;

        this.planets = new List<GameObject>();
        LoadBuildings();
        LoadPlanets();
        LoadShipTypes();
        AddShipsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadBuildings()
    {
        using(StreamReader r = new StreamReader(this.path + "Buildings.json")){
            string json = r.ReadToEnd();
            ListBuildings buildings = JsonUtility.FromJson<ListBuildings>(json);
            this.buildings = new List<BuildingModel>(buildings.buildings);
        }
    }

    // Loading the planets from the JSON File and adding them to the scene
    private void LoadPlanets()
    {
        using (StreamReader r = new StreamReader(this.path + "Planets.json"))
        {
            string json = r.ReadToEnd();
            ListPlanets planets = JsonUtility.FromJson<ListPlanets>(json);
            foreach(PlanetJSON planet in planets.planets){
                GameObject planet_object = Instantiate(planet_prefab, new Vector3(float.Parse(planet.x_coordinate), 0, float.Parse(planet.y_coordinate)),  Quaternion.identity);
                Planet script = planet_object.transform.GetComponent<Planet>();
                script.GalaxyUI = this.galaxyUI;
                script.planetName = planet.planet_name;
                script.description = planet.description;
                script.line_prefab = this.line_prefab;
                script.baseIncome = planet.baseIncome;
                script.numberOfBuildings = planet.number_of_buildings;
                script.faction = planet.faction;
                script.Galaxy = this.transform.gameObject;
                foreach(string building in planet.buildings_placable){
                    BuildingModel building_model = this.buildings.Find(x => x.building_name == building);
                    script.placeableBuildings.Add(building_model);
                }
                script.SetSpaceStation(planet.startStation);
                this.planets.Add(planet_object);
            }
            CreateLines(planets.trade_routes);
            PlanetVicinity();
        }
    }

    // Loading the respective starting ships for each planet from the JSON File and adding them to the production queue
    private void AddShipsOnStart(){
        using (StreamReader r = new StreamReader(this.path + "Planets.json")){
            string json = r.ReadToEnd();
            ListPlanets json_planets = JsonUtility.FromJson<ListPlanets>(json);
            foreach(PlanetJSON planet in json_planets.planets){
                GameObject planet_object = this.planets.Find(x => x.transform.GetComponent<Planet>().planetName == planet.planet_name);
                Planet script = planet_object.transform.GetComponent<Planet>();
                foreach(string ship in planet.startShips){
                    script.AddShipProduction(ship, false);
                }
            }
        }
    }

    // Display the lines for the trade routes, though they at this point don't serve any specific purpose
    private void CreateLines(TradeRoute[] trade_routes){
        foreach(TradeRoute route in trade_routes){
            for(int i = 0; i < route.planets.Length; i++){
                GameObject planet_object = this.planets.Find(x => x.transform.GetComponent<Planet>().planetName == route.planets[i]);
                Planet script = planet_object.transform.GetComponent<Planet>();
                if(i > 0){
                    GameObject connectingPlanet = this.planets.Find(x => x.transform.GetComponent<Planet>().planetName == route.planets[i-1]);
                    script.connectingPlanets.Add(connectingPlanet);
                }
                if(i < route.planets.Length - 1){
                    GameObject connectingPlanet = this.planets.Find(x => x.transform.GetComponent<Planet>().planetName == route.planets[i+1]);
                    script.connectingPlanets.Add(connectingPlanet);
                    GameObject line = Instantiate(line_prefab);
                    LineRenderer line_renderer = line.transform.GetChild(0).transform.GetComponent<LineRenderer>();
                    line_renderer.SetPosition(0, planet_object.transform.position);
                    line_renderer.SetPosition(1, connectingPlanet.transform.position);
                }
            }
        }
    }

    // Calculate planets in the vicinity. Also not important at this point yet
    private void PlanetVicinity(){
        foreach(GameObject planet in this.planets){
            foreach(GameObject secondPlanet in this.planets){
                if(planet != secondPlanet){
                    float distance = Vector3.Distance(planet.transform.position, secondPlanet.transform.position);
                    if(distance < 20){
                        planet.transform.GetComponent<Planet>().nearPlanets.Add(secondPlanet);
                    }
                }
            }
        }
    }

    // Loading the different ship types for each faction into the current scene and to each planet
    private void LoadShipTypes()
    {
        using (StreamReader r = new StreamReader(this.path + "Ships.json"))
        {
            string json = r.ReadToEnd();
            ShipList ships = JsonUtility.FromJson<ShipList>(json);

            foreach (ShipTypeModel shipType in ships.ship_types)
            {
                if(shipType.planet_restriction.Length == 0){
                    foreach(GameObject planet in this.planets){
                        Planet script = planet.transform.GetComponent<Planet>();
                        script.producableShips.Add(shipType);
                    }
                } 
                else if(shipType.planet_restriction.Length == 1){
                    foreach(string planet in shipType.planet_restriction){
                        GameObject planet_object = this.planets.Find(x => x.transform.GetComponent<Planet>().planetName == planet);
                        planet_object.transform.GetComponent<Planet>().producableShips.Add(shipType);
                    }
                }
            }
        }
    }
}