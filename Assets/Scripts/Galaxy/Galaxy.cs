using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Galaxy : MonoBehaviour
{

    List<GameObject> planets;

    public GameObject planet_prefab;

    public GameObject line_prefab;

    // Start is called before the first frame update
    void Start()
    {
        this.planets = new List<GameObject>();
        LoadPlanets();
        LoadShipTypes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPlanets()
    {
        using (StreamReader r = new StreamReader("Assets/Config/Planets.json"))
        {
            string json = r.ReadToEnd();
            ListPlanets planets = JsonUtility.FromJson<ListPlanets>(json);
            foreach(PlanetJSON planet in planets.planets){
                GameObject planet_object = Instantiate(planet_prefab, new Vector3(float.Parse(planet.x_coordinate), 0, float.Parse(planet.y_coordinate)),  Quaternion.identity);
                Planet script = planet_object.transform.GetComponent<Planet>();
                script.planetName = planet.planet_name;
                script.description = planet.description;
                script.line_prefab = this.line_prefab;
                script.numberOfBuildings = planet.number_of_buildings;
                this.planets.Add(planet_object);
            }
            foreach(TradeRoute route in planets.trade_routes){
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
    }

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

    private void LoadShipTypes()
    {
        using (StreamReader r = new StreamReader("Assets/Config/Ships.json"))
        {
            string json = r.ReadToEnd();
            Debug.Log(json);
            ShipList ships = JsonUtility.FromJson<ShipList>(json);
            foreach (ShipTypeModel shipType in ships.ship_types)
            {
                Debug.Log(shipType.id);
            }
        }
    }

    private void LoadBuildings(){
        using (StreamReader r = new StreamReader("Assets/Config/Buildings.json"))
        {
            string json = r.ReadToEnd();
            Debug.Log(json);
            ListBuildings buildings = JsonUtility.FromJson<ListBuildings>(json);
            foreach (BuildingModel building in buildings.buildings)
            {
                Debug.Log(building.building_name);
            }
        }
    }
}