using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PanZoomOrbitCenter : MonoBehaviour
{
    [SerializeField] private GameObject parentModel;
    private float rotationSpeed = 500.0f;
    private Vector3 mouseWorldPosStart;
    private float zoomScale = 10f;
    private float zoomMin = 0.5f;
    private float zoomMax = 100.0f;
    private float defaultFieldOfView = 60.0f;
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
                if (hit.collider.gameObject == parentModel) {
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
            // TODO: faire diminuer la vitesse et continuer camorbit pour donner sensation de mouvement
            dragBegunOnSpheres = false;
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) //Check for Pan
        {
            mouseWorldPosStart = GetPerspectivePos();
        }
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Pan();
        }
        Zoom1(Input.GetAxis("Mouse ScrollWheel"));
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

    void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            m_Camera.transform.position += mouseWorldPosDiff;
        }
    }

    private Bounds GetBound(GameObject parentGameObject) {
        Bounds bound = new Bounds(parentGameObject.transform.position, Vector3.zero);
        var rList = parentGameObject.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rList) {
            bound.Encapsulate(r.bounds);
        }
        return bound;
    }

    public void FitToScreen() {
        m_Camera.fieldOfView = defaultFieldOfView;
        Bounds bound = GetBound(parentModel);
        Vector3 boundSize = bound.size;
        float boundDiagonal = Mathf.Sqrt(Mathf.Pow(boundSize.x, 2) + Mathf.Pow(boundSize.y, 2) + Mathf.Pow(boundSize.z, 2));
        float camDistanceToBoundCenter = boundDiagonal / 2 / (Mathf.Tan(m_Camera.fieldOfView / 2 * Mathf.Deg2Rad));
        float camDistanceToBoundWithOffset = camDistanceToBoundCenter + boundDiagonal / 2.0f - (m_Camera.transform.position - transform.position).magnitude;
        m_Camera.transform.position = bound.center - transform.forward * camDistanceToBoundWithOffset;
        camDistanceToBoundWithOffset = camDistanceToBoundCenter + boundDiagonal / 2.0f - (m_Camera.transform.position - transform.position).magnitude;
        m_Camera.transform.position = bound.center - transform.forward * camDistanceToBoundWithOffset;
    }

    //Zoom with linear difference between mouse position on plane containing the sphere's center before and after 
    //fieldofview change as camera adjustment
    void Zoom1(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            mouseWorldPosStart = GetPerspectivePos();
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomDiff * zoomScale, zoomMin, zoomMax);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            Camera.main.transform.position += mouseWorldPosDiff;
        }
    }

    //Zoom with linear difference between mouse position the sphere center before and after 
    //fieldofview change as camera adjustment (for now doesn't work when zooming while pointing outside the sphere)
    void Zoom2(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            // Calculate the mouse position in world space before zoom
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 mouseWorldPosBefore = hit.point;

                // Adjust the field of view
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomDiff * zoomScale, zoomMin, zoomMax);

                // Calculate the mouse position in world space after zoom
                ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 mouseWorldPosAfter = hit.point;

                    // Calculate the difference and adjust the camera position
                    Vector3 mouseWorldPosDiff = mouseWorldPosBefore - mouseWorldPosAfter;
                    Camera.main.transform.position += mouseWorldPosDiff;
                    //or rotate this.transform by diffspherical?
                }
                else
                {
                    Debug.LogWarning("No hit detected after adjusting field of view during zoom.");
                }
            }
            else
            {
                Debug.LogWarning("No hit detected before adjusting field of view during zoom.");
            }
        }
    }

    public Vector3 GetPerspectivePos()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.forward, 0.0f);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    // Use this for rotation adjustment ?
    private void DiffSpherical(Vector3 cartCoordsA, Vector3 cartCoordsB, out float diffPolar, out float diffElevation){
        diffPolar = Mathf.Acos(cartCoordsA.x*cartCoordsB.x + cartCoordsA.y*cartCoordsB.y);
        diffElevation = Mathf.Acos(cartCoordsA.x*cartCoordsB.x + cartCoordsA.z*cartCoordsB.z);
    }

}