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
    public int number_of_buildings;
    public int shipyard;
    public int startStation;

    public bool is_shipyard(){
        return shipyard == 1;
    }
}