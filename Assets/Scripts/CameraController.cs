using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int x, y;

    public float ScrollSpeed = 2;

    private GameObject selectedFleet, oldPlanet;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0f, 25f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
        DetectMouse();
    }

    // Moves camera if mouse courser gets close to screen border
    private void MoveCamera(){
        if(Input.mousePosition.y >= Screen.height * 0.95f){
            transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
        } else if (Input.mousePosition.y <= Screen.height * 0.05f){
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
        } 
        if (Input.mousePosition.x <= Screen.width * 0.05f){
            transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
        } else if (Input.mousePosition.x >= Screen.width * 0.95f){
            transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
        }
    }

    private void DetectMouse(){
        if(Input.GetMouseButton(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = LayerMask.GetMask("Fleet");
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && this.selectedFleet == null){
                this.selectedFleet = hit.collider.gameObject.transform.parent.gameObject;
            }

            int layerMask2 = LayerMask.GetMask("Transport");
            RaycastHit hit2;
            if(Physics.Raycast(ray, out hit2, Mathf.Infinity, layerMask2)){
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
