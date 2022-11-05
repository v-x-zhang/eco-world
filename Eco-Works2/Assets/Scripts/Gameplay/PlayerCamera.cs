using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region Singleton
    public static PlayerCamera currentCamera;

    private void Awake()
    {
        currentCamera = this;
    }
    #endregion 

    public Transform followTransform;
    public Transform cameraTransform;

    public LayerMask collisionLayers;

    //Movement
    [Header("Movement")]
    public Vector2 xBounds;
    public Vector2 zBounds;

    public float normalSpeed;
    public float fastSpeed;
    public float moveLerpSpeed;

    float currentSpeed;
    Vector3 newPosition;

    //Rotation
    [Header("Rotation")]
    public float rotationSpeed;
    public float rotationLerpSpeed;
    
    Quaternion newRotation;

    //Zooming
    [Header("Zooming")]
    public Vector2 zoomBounds;
    public Vector3 zoomSpeed;
    public float zoomLerpSpeed;

    Vector3 newZoom;

    //Mouse
    [Header("Mouse")]
    public float mouseRotateDampen;
    public Vector3 mouseZoomSpeed;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;

    public float minHeight;

    [Header("Tool Scripts")]
    public PlayerBuilding buildingScript;
    public PlayerGathering gatherScript;
    public PlayerDemolish demolishScript;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(followTransform == null)
        {
            MovementInput();
            RotationInput();
            ZoomInput();
            MouseInput();

            UpdateCamera();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    private void LateUpdate()
    {
        float newHeight = 0;

        Vector3 highPos = new Vector3(transform.position.x, 100, transform.position.z);

        RaycastHit hit;
        if (Physics.Raycast(highPos, Vector3.down, out hit,100, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float height = hit.point.y;
            newHeight = height + minHeight;

        }
        Vector3 heightAdjustment = new Vector3(transform.position.x, newHeight, transform.position.z);

        transform.position = heightAdjustment;
    }


    void UpdateCamera()
    {
        //Clamp Movement
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y), transform.position.y, Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y));
        //Do Movement
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveLerpSpeed);

        //Do Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationLerpSpeed);


        //Clamp Zoom
        newZoom = new Vector3(newZoom.x, Mathf.Clamp(newZoom.y, zoomBounds.x, zoomBounds.y), -Mathf.Clamp(-newZoom.z, zoomBounds.x, zoomBounds.y));
        //DO Zooming
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * zoomLerpSpeed);
    }

    void MouseInput()
    {
        if (GameManager.instance.lockMovement && GameManager.instance != null) return;

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * mouseZoomSpeed;
        }


        if (!buildingScript.enabled && !gatherScript.enabled && !demolishScript.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
        }
       

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / mouseRotateDampen));
        }

        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                newPosition = ray.GetPoint(entry);
            }
        }
    }

    void ZoomInput()
    {
        if (GameManager.instance.lockMovement || GameManager.instance == null) return;

        if (Input.GetKey(KeyCode.Z))
        {
            newZoom += zoomSpeed;

        }
        if (Input.GetKey(KeyCode.X))
        {
            newZoom -= zoomSpeed;
        }
    }

    void RotationInput()
    {
        if (GameManager.instance.lockMovement || GameManager.instance == null) return;

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }
    }

    void MovementInput()
    {
        if (GameManager.instance.lockMovement || GameManager.instance == null) return;


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = fastSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }


        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        if (xMov != 0)
        {
            newPosition += transform.right * currentSpeed * xMov;
        }
        if(yMov != 0)
        {
            newPosition += transform.forward * currentSpeed * yMov;
        }
    }
}
