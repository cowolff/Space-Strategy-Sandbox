using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingModel
{
    public string building_name;
    public string description;
    public int number_allowed;
    public int building_level;
    public int cost;
    public int income;
    public int production_time;
    public string[] producing_units;
    public int squads;
    public int defensive;
}
