using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GalaxyUI : MonoBehaviour
{

    [SerializeField]
    private UIDocument m_UIDocument;

    [SerializeField]
    private GameObject m_Galaxy;
    private Galaxy m_GalaxyScript;

    private GameObject selected_planet;

    private GameObject selectedFleet, oldPlanet;
    private Vector3 oldPlanetCoor;

    Button spaceButton, groundButton;

    List<Button> currentButtons;
    VisualElement first_row;
    VisualElement second_row;
    VisualElement progress;
    Label income_label;
    Label asset_label;
    Label faction_label;
    Label time_label;

    public StyleSheet styleSheet;

    int income;
    int assets;
    float time_left;
    string current_menue;

    ProgressBar spaceProgress;
    ProgressBar groundProgress;

    void Start()
    {
        var rootElement = m_UIDocument.rootVisualElement;
        currentButtons = new List<Button>();

        this.assets = 1000;
        this.time_left = 20f;
        this.income = 0;

        m_GalaxyScript = m_Galaxy.GetComponent<Galaxy>();

        first_row = rootElement.Q<VisualElement>("FirstRow");
        second_row = rootElement.Q<VisualElement>("SecondRow");
        income_label = rootElement.Q<Label>("IncomeLabel");
        asset_label = rootElement.Q<Label>("AssetLabel");
        spaceButton = rootElement.Q<Button>("SpaceButton");
        groundButton = rootElement.Q<Button>("GroundButton");
        faction_label = rootElement.Q<Label>("FactionLabel");
        time_label = rootElement.Q<Label>("TimeLabel");
        progress = rootElement.Q<VisualElement>("Progress");

        spaceButton.clickable.clicked += this.SpaceButtonClicked;
        groundButton.clickable.clicked += this.GroundButtonClicked;

        rootElement.styleSheets.Add(styleSheet);
    }

    void Update()
    {
        this.DetectMouse();
        this.UpdateIncome();
        this.UpdateAsset();
        this.UpdateProgress();
        faction_label.text = m_GalaxyScript.faction;
    }

    private void SpaceButtonClicked(){
        this.ClickedOnPlanet();
    }

    private void UpdateProgress(){
        progress.Clear();
        if(this.selected_planet != null){
            Planet script = this.selected_planet.transform.GetComponent<Planet>();
            if(script.currentBuilding != null && this.current_menue == "Ground"){
                BuildingModel currentBuilding = script.currentBuilding;
                VisualElement new_element = new VisualElement();
                new_element.AddToClassList("FirstRowGround");
                Label new_label = new Label() {text = currentBuilding.building_name + "  " + (int)script.buildingCountdown};
                new_element.Add(new_label);
                progress.Add(new_element);

                foreach(BuildingModel model in script.productionStackBuildings){
                    VisualElement new_queue = new VisualElement();
                    new_queue.AddToClassList("FirstRowGround");
                    Label new_queue_label = new Label() {text = model.building_name};
                    new_queue.Add(new_queue_label);
                    progress.Add(new_queue);
                }
            } else if(script.currentShip != null && this.current_menue == "Space"){
                ShipTypeModel currentShip = script.currentShip;
                VisualElement new_element = new VisualElement();
                new_element.AddToClassList("FirstRowSpace");
                Label new_label = new Label() {text = currentShip.id + "  " + (int)script.shipCountdown};
                new_element.Add(new_label);
                progress.Add(new_element);

                foreach(ShipTypeModel model in script.productionStackSpace){
                    VisualElement new_queue = new VisualElement();
                    new_queue.AddToClassList("FirstRowSpace");
                    Label new_queue_label = new Label() {text = model.id};
                    new_queue.Add(new_queue_label);
                    progress.Add(new_queue);
                }
            }
        }
    }

    private void GroundButtonClicked(){
        second_row.Clear();
        first_row.Clear();
        this.current_menue = "Ground";
        Planet currentPlanet = selected_planet.transform.GetComponent<Planet>();
        if(currentPlanet.faction == m_GalaxyScript.faction){
            for(int i = 0; i<currentPlanet.placeableBuildings.Count; i++){
                Button button = new Button() { text = currentPlanet.placeableBuildings[i].building_name + "\nPrice: " + currentPlanet.placeableBuildings[i].cost};
                button.AddToClassList("GroundButton");
                button.clickable.clicked += () => {
                    string text = button.text.Split("\n")[0];
                    currentPlanet.transform.GetComponent<Planet>().AddBuilding(text);
                };
                second_row.Add(button);
            }
        

            BuildingGalaxy[] buildings = currentPlanet.GetBuildings();
            Debug.Log(buildings.Length);
            for(int i = 0; i < buildings.Length; i++){
                if(buildings[i] != null){
                    VisualElement new_element = new VisualElement();
                    new_element.AddToClassList("FirstRowGround");
                    Label new_label = new Label() {text = buildings[i].building_name};
                    new_element.Add(new_label);
                    first_row.Add(new_element);
                }
            }
        }
    }

    void ClickedOnPlanet(){
        second_row.Clear();
        first_row.Clear();
        this.current_menue = "Space";
        if(this.selected_planet != null){
            Planet currentPlanet = selected_planet.transform.GetComponent<Planet>();
            if(currentPlanet.faction == m_GalaxyScript.faction){
                List<ShipTypeModel> ships = currentPlanet.GetProducableShips();
                for(int i = 0; i< ships.Count; i++){
                    if(ships[i].affiliation == m_GalaxyScript.faction){
                        Button button = new Button() { text = ships[i].id + "\nPrice: " + ships[i].cost};
                        button.AddToClassList("SpaceButton");
                        button.clickable.clicked += () => {
                            string text = button.text.Split("\n")[0];
                            currentPlanet.transform.GetComponent<Planet>().AddShipProduction(text);
                        };
                        second_row.Add(button);
                    }
                }
            }
        }
    }

    private void UpdateIncome()
    {
        this.income = 0;
        foreach (GameObject planet in m_GalaxyScript.planets)
        {
            Planet script = planet.transform.GetComponent<Planet>();
            income = income + script.GetIncome();
        }
        income_label.text = "Income: " + income;
    }

    private void UpdateAsset(){
        this.time_left -= Time.deltaTime;
        time_label.text = "Time left: -" + (int)(20 - (20 - this.time_left));
        if(this.time_left < 0){
            this.time_left = 20f;
            this.assets = this.assets + this.income;
        }
        this.asset_label.text = "Assets: " + this.assets;
    }

    private void DetectMouse(){
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Planet_Collider");
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
                this.selected_planet = hit.collider.transform.parent.gameObject;
                Debug.Log(this.selected_planet.transform.GetComponent<Planet>().planetName);
                this.ClickedOnPlanet();
            }
        }
        if(Input.GetMouseButton(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = LayerMask.GetMask("Fleet");
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && this.selectedFleet == null){
                if(hit.collider.gameObject.transform.GetComponent<PlanetFleetSpot>().fleet_script != null){
                    this.selectedFleet = hit.collider.gameObject;
                    this.oldPlanetCoor = hit.collider.transform.position;
                }
            }

            int layerMask2 = LayerMask.GetMask("Transport");
            RaycastHit hit2;
            if(Physics.Raycast(ray, out hit2, Mathf.Infinity, layerMask2) && this.selectedFleet != null){
                this.selectedFleet.transform.position = hit2.point + new Vector3(0, (this.selectedFleet.transform.GetComponent<Collider>().bounds.size.y / 2), 0);
            }

            int layerMask3 = LayerMask.GetMask("Planet_Collider");
            RaycastHit hit3;
            if(Physics.Raycast(ray, out hit3, Mathf.Infinity, layerMask3) && this.oldPlanet == null && this.selectedFleet != null){
                Debug.Log("Something is happening");
                this.oldPlanet = hit3.collider.gameObject.transform.parent.gameObject;
            }
        }
        else if(this.selectedFleet != null){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = LayerMask.GetMask("Planet_Collider");
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
                Debug.Log("Colission happened");
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<Planet>().AddFleet(this.selectedFleet);
                Debug.Log(this.oldPlanet);
                this.oldPlanet.transform.GetComponent<Planet>().RemoveFleet(this.selectedFleet);
                this.selectedFleet.transform.position = this.oldPlanetCoor;
                this.selectedFleet = null;
                this.oldPlanet = null;
            } else {
                this.selectedFleet.transform.position = this.oldPlanetCoor;
                this.selectedFleet = null;
                this.oldPlanet = null;
            }
        }
    }

    public void ReloadBars(){
        if(this.current_menue == "Ground"){
            this.GroundButtonClicked();
        } else {
            this.ClickedOnPlanet();
        }
    }

    public bool ApplyCost(int cost){
        if(this.assets >= cost){
            this.assets -= cost;
            return true;
        } else {
            return false;
        }
    }
}