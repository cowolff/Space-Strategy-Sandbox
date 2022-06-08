using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour
{
    public Text slot1, slot2, slot3;

    public PlanetScript selectedPlanet;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selectedPlanet = SelectPlanet();
        UpdateTexts();
    }

    public PlanetScript SelectPlanet(){
        if(Input.GetMouseButtonDown(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Planet");
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)){
                return hitInfo.collider.gameObject.GetComponent<PlanetScript>();
            }

        }
        return selectedPlanet;
    }

    public void UpdateTexts(){
        if(selectedPlanet.fleets[0] == null){
            slot1.text="No fleet";
        } else {
            slot1.text=selectedPlanet.fleets[0].name;
        }
        if(selectedPlanet.fleets[1] == null){
            slot2.text="No fleet";
        } else {
            slot2.text=selectedPlanet.fleets[1].name;
        }
        if(selectedPlanet.fleets[2] == null){
            slot3.text="No fleet";
        } else {
            slot3.text=selectedPlanet.fleets[2].name;
        }
    }
}
