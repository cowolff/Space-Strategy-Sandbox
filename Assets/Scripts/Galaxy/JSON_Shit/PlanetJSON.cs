using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetJSON
{
    public string planet_name;
    public string description;
    public string x_coordinate;
    public string y_coordinate;
    public string connecting;
    public int number_of_buildings;

    public string[] GetConnectingPlanets(){
        return this.connecting.Split(',');
    }
}