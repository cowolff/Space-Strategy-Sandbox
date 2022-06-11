using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFleetSpot : MonoBehaviour
{
    public GameObject fleet;

    public void add_Fleet(GameObject fleet)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
        fleet.transform.position = transform.position;
        this.fleet = fleet;
    }

    public GameObject get_Fleet()
    {
        return this.fleet;
    }

    public GameObject remove_Fleet()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = true;
        GameObject fleet = this.fleet;
        this.fleet = null;
        return fleet;
    }

    public bool has_Fleet()
    {
        return this.fleet != null;
    }

    public void merge_Fleet(GameObject fleet)
    {
        this.fleet.GetComponent<FleetGalaxy>().CombineFleets(fleet.GetComponent<FleetGalaxy>());
        Destroy(fleet);
    }
}