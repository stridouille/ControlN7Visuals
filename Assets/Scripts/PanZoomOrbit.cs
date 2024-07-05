using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PanZoomOrbitCenter : MonoBehaviour
{
    [SerializeField] private GameObject parentModel;
    private float rotationSpeed = 300.0f;
    private Vector3 mouseWorldPosStart;
    private float zoomScale = 0.5f;
    private float zoomMin = 0.05f;
    private float zoomMax = 2f;
    private float defaultOrthoSize = 1f;
    private bool dragBegunOnSpheres = false;
    Camera m_Camera;

    void Awake()
    {
       m_Camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        FitToScreen();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {          //check for beginning of orbit
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.transform.parent.gameObject == parentModel) {
                    dragBegunOnSpheres = true;
                }
            }
        }

        if (Input.GetMouseButton(0)) { //Check for orbit
            if (dragBegunOnSpheres) {
                CamOrbit();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            // TODO: reduce the speed and continue camorbit to give a sense of movement
            dragBegunOnSpheres = false;
        }
        ZoomRelative(Input.GetAxis("Mouse ScrollWheel"));
    }


    public void CamOrbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }

    public void FitToScreen() {
        m_Camera.orthographicSize = defaultOrthoSize;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    // Zoom and adjust camera by simple linear transformation
    public void ZoomNaive(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            mouseWorldPosStart = GetPerspectivePos();
            m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
            gameObject.transform.position += mouseWorldPosStart - GetPerspectivePos();
        }
    }


    //Zoom and adjust camera angle by rotating by the angle difference between the point on mouse position before and after zoom
    void ZoomRelative(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Get the pointed pixel transform in world space before zoom
                Vector3 mouseWorldPosBefore = hit.point;
                // Adjust the screen box
                m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
                ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // Get the pointed pixel transform in world space before zoom
                    Vector3 mouseWorldPosAfter = hit.point;
                    DiffSpherical(mouseWorldPosAfter, mouseWorldPosBefore, 0.5f, out float diffX, out float diffY);
                    gameObject.transform.Rotate(-diffX, diffY, 0);
                }
            } else {
                m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
            }
        }
    }

    // Zoom and adjust camera angle by computing the right absolute angle
    void ZoomAbsolute(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mouseScreenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("pos screen mouse : " + mouseScreenPos);
                // Get the pointed pixel transform in world space before zoom
                Vector3 mouseWorldPos = hit.point;
                Debug.Log("pos world mouse : " + mouseWorldPos);

                // Adjust the screen box
                m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
                //float s = Mathf.Clamp(m_Camera.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
                
                // Define the problem's parameters
                int h = Screen.height;
                int w = Screen.width;
                float s = m_Camera.orthographicSize;
                float r = 0.5f;
                float phi = Mathf.Asin(mouseWorldPos.y/r) * Mathf.Rad2Deg;
                if (mouseWorldPos.z > 0)
                {
                    phi = 180 - phi;
                }
                Debug.Log("phi : " + phi);
                float psi = Mathf.Asin(mouseWorldPos.x/r) * Mathf.Rad2Deg;
                if (mouseWorldPos.z > 0)
                {
                    psi = 180 - psi;
                }
                Debug.Log("psi : " + psi);
                float alpha = (float)h/w;
                
                float theta = - Mathf.Asin(2*s*(mouseScreenPos.y/h - 0.5f)/r) * Mathf.Rad2Deg;
                if (mouseWorldPos.z > 0)
                {
                    theta = 180 - theta;
                }
                Debug.Log("theta : " + theta);
                float gamma = - Mathf.Asin(2*s*(mouseScreenPos.x/w - 0.5f)/(alpha*r)) * Mathf.Rad2Deg;
                if (mouseWorldPos.z > 0)
                {
                    gamma = 180 - gamma;
                }
                Debug.Log("gamma : " + gamma);

                // Apply the adjustment
                gameObject.transform.rotation = Quaternion.Euler(theta+phi, -gamma-psi, 0);             
            }
        }
    }


    // Get world space position of point under mouse cursor
    public Vector3 GetPerspectivePos()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.forward, 0.0f);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void DiffSpherical(Vector3 cartCoordsA, Vector3 cartCoordsB, float r, out float diffX, out float diffY){
        diffX = (Mathf.Asin(cartCoordsA.y/r) - Mathf.Asin(cartCoordsB.y/r)) * Mathf.Rad2Deg;
        diffY = (Mathf.Asin(cartCoordsA.x/r) - Mathf.Asin(cartCoordsB.x/r)) * Mathf.Rad2Deg;
    }

}