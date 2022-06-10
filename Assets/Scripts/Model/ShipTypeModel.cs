

using System.Collections.Generic;

[System.Serializable]
public class ShipTypeModel
{
    public string id;
    public string description;
    public string[] planet_restriction;
    public string ship_class; 
    public string affiliation;
    public int damage_per_second;
    public int tactical_health;
    public int shield_points;
    public int shield_refresh_rate_per_second;
    public int cost;
    public int build_time_in_seconds;
    public float speed;
    public float hyperspace_capable;
    public float sublight_speed;
    public int population_slots;
    public int req_space_station_level;
    public string[] strong_against;
    public string[]  weak_against;
    public SquadronCapacity[] squadrons_capacity;
    public int num_per_squadron;
    public string source;
}