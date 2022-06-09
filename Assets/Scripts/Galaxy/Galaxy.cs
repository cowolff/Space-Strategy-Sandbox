using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Galaxy : MonoBehaviour
{

    List<GameObject> planets;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlanets();
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
            Debug.Log(json);
            ListPlanets planets = JsonUtility.FromJson<ListPlanets>(json);
            foreach(PlanetJSON planet in planets.planets){
                Debug.Log(planet.planet_name);
            }
        }
    }
}