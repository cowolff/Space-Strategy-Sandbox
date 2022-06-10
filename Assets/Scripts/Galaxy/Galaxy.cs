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

    public void LoadPlanets()
    {
        using (StreamReader r = new StreamReader("Assets/Scripts/Galaxy/Planets.json"))
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
                script.connectingPlanets = planet.GetConnectingPlanets();
                this.planets.Add(planet_object);
            }
        }
        foreach(GameObject planet in this.planets){
            Planet script = planet.transform.GetComponent<Planet>();
            script.CreateConnection(this.planets);
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
}