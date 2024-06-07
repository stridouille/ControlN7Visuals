using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSpeed = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        // Key inputs.
        if (Input.GetKey("w")){
            this.transform.Translate(Vector3.forward *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("s")){
            this.transform.Translate(-Vector3.forward *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("a")){
            this.transform.Translate(-Vector3.right *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("d")){
            this.transform.Translate(Vector3.right *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("space")){
            this.transform.Translate(Vector3.up *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("left shift")){
            this.transform.Translate(-Vector3.up *
             speed * Time.deltaTime);
        }

        if (Input.GetKey("y")){
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshot.png");
        }

        // Camera rotation.
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
        {
             
            transform.eulerAngles += mouseSpeed * new Vector3( -Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"),0) ; 

            
        }

    }
}
