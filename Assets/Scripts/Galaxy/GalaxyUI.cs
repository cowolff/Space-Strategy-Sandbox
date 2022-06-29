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

    Button spaceButton, groundButton;

    List<Button> currentButtons;
    VisualElement first_row;
    VisualElement second_row;
    Label income_label;

    void Start()
    {
        var rootElement = m_UIDocument.rootVisualElement;
        currentButtons = new List<Button>();

        m_GalaxyScript = m_Galaxy.GetComponent<Galaxy>();

        first_row = rootElement.Q<VisualElement>("FirstRow");
        second_row = rootElement.Q<VisualElement>("SecondRow");
        income_label = rootElement.Q<Label>("IncomeLabel");
        spaceButton = rootElement.Q<Button>("SpaceButton");
        groundButton = rootElement.Q<Button>("GroundButton");

        spaceButton.clickable.clicked += this.GroundButtonClicked;
        groundButton.clickable.clicked += this.SpaceButtonClicked;
    }

    void Update()
    {
        this.DetectMouse();
        this.UpdateIncome();
    }

    private void SpaceButtonClicked(){
        this.clickedOnPlanet();
    }

    private void GroundButtonClicked(){
        second_row.Clear();
        Planet currentPlanet = selected_planet.transform.GetComponent<Planet>();
        List<ShipTypeModel> ships = currentPlanet.GetProducableShips();
        for(int i = 0; i< ships.Count; i++){
            Button button = new Button() { text = ships[i].id };
            button.style.width = 160;
            button.style.height = 50;
            button.style.marginLeft = 10;
            button.style.marginTop = 10;
            button.style.marginBottom = 10;
            button.style.marginRight = 10;
            button.clickable.clicked += () => {
                currentPlanet.transform.GetComponent<Planet>().AddShipProduction(button.text);
            };
            second_row.Add(button);
        }
    }

    void clickedOnPlanet(){
        second_row.Clear();
        Planet currentPlanet = selected_planet.transform.GetComponent<Planet>();
        for(int i = 0; i<currentPlanet.placeableBuildings.Count; i++){
            Button button = new Button() { text = currentPlanet.placeableBuildings[i].building_name };
            button.style.width = 160;
            button.style.height = 50;
            button.style.marginLeft = 10;
            button.style.marginTop = 10;
            button.style.marginBottom = 10;
            button.style.marginRight = 10;
            button.clickable.clicked += () => {
                currentPlanet.transform.GetComponent<Planet>().AddBuilding(button.text);
            };
            second_row.Add(button);
        }
    }

    private void UpdateIncome()
    {
        int income = 0;
        foreach (GameObject planet in m_GalaxyScript.planets)
        {
            Planet script = planet.transform.GetComponent<Planet>();
            income = income + script.GetIncome();
        }
        income_label.text = "Income: " + income;
    }

    private void DetectMouse(){
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Planet_Collider");
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
                this.selected_planet = hit.collider.transform.parent.gameObject;
                Debug.Log(this.selected_planet.transform.GetComponent<Planet>().planetName);
                this.clickedOnPlanet();
            }
        }
        if(Input.GetMouseButton(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = LayerMask.GetMask("Fleet");
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && this.selectedFleet == null){
                this.selectedFleet = hit.collider.gameObject.transform.parent.gameObject;
            }

            int layerMask2 = LayerMask.GetMask("Transport");
            RaycastHit hit2;
            if(Physics.Raycast(ray, out hit2, Mathf.Infinity, layerMask2) && this.selectedFleet != null){
                this.selectedFleet.transform.position = hit2.point + new Vector3(0, (this.selectedFleet.transform.GetChild(0).transform.GetComponent<Collider>().bounds.size.y / 2), 0);
            }

            int layerMask3 = LayerMask.GetMask("Planet");
            RaycastHit hit3;
            if(Physics.Raycast(ray, out hit3, Mathf.Infinity, layerMask3) && this.oldPlanet == null && this.selectedFleet != null){
                this.oldPlanet = hit3.collider.gameObject.transform.parent.gameObject;
            }
        }
        else if(this.selectedFleet != null){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = LayerMask.GetMask("Planet");
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<Planet>().AddFleet(this.selectedFleet);
                this.oldPlanet.transform.parent.gameObject.GetComponent<Planet>().RemoveFleet(this.selectedFleet);
                this.selectedFleet = null;
                this.oldPlanet = null;
            } else {
                this.selectedFleet = null;
                this.oldPlanet = null;
            }
        }
    }
}