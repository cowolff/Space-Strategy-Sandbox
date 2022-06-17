using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGalaxy
{
    public int income { get; set; }

    public string building_name { get; set; }

    public List<UnitGalaxy> producable;

    // Start is called before the first frame update
    public BuildingGalaxy()
    {
        producable = new List<UnitGalaxy>();
    }
}
