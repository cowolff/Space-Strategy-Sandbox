using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int x, y;

    public float ScrollSpeed = 2;

    private float max_X, min_X, max_Z, min_Z;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach(GameObject planet in planets){
            if(planet.transform.position.x < min_X){
                min_X = planet.transform.position.x;
            }
            if(planet.transform.position.x > max_X){
                max_X = planet.transform.position.x;
            }
            if(planet.transform.position.z > max_Z){
                max_Z = planet.transform.position.z;
            }
            if(planet.transform.position.z < min_Z){
                min_Z = planet.transform.position.z;
            }
        }
        max_Z = max_Z - 10;
        min_Z = min_Z - 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
    }

    // Moves camera if mouse courser gets close to screen border
    private void MoveCamera(){
        if(Input.mousePosition.y >= Screen.height * 0.98f && max_Z > transform.position.z){
            transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
        } else if (Input.mousePosition.y <= Screen.height * 0.02f && min_Z < transform.position.z){
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
        } 
        if (Input.mousePosition.x <= Screen.width * 0.02f && min_X < transform.position.x){
            transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
        } else if (Input.mousePosition.x >= Screen.width * 0.98f &&  max_X > transform.position.x){
            transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
        }
    }
}
