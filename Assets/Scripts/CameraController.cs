using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int x, y;

    public float ScrollSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0f, 25f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
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
}
