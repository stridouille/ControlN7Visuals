using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoomOrbitCenter : MonoBehaviour
{
    private float rotationSpeed = 500.0f;
    private Vector3 mouseWorldPosStart;
    private float zoomScale = 2.5f;
    private float zoomMin = 0.5f;
    private float zoomMax = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Orbit, Pan, Zoom
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse2) && !EventSystem.current.IsPointerOverGameObject()) //Check for orbit
        {
            CamOrbit();
        }
        if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift) && !EventSystem.current.IsPointerOverGameObject()) //Check for Pan
        {
            mouseWorldPosStart = GetPerspectivePos();
        }
        if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift) && !EventSystem.current.IsPointerOverGameObject())
        {
            Pan();
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }


    private void CamOrbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }

    void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            transform.position += mouseWorldPosDiff;
        }
    }

    void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            mouseWorldPosStart = GetPerspectivePos();
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomDiff * zoomScale, zoomMin, zoomMax);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            transform.position += mouseWorldPosDiff;
        }
    }

    public Vector3 GetPerspectivePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.forward, 0.0f);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

}