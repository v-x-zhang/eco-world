using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBuilding : MonoBehaviour
{
    public Building currentBuilding;

    public LayerMask buildingLayers;
    public LayerMask groundLayers;

    public Transform checkCollider;

    public Transform checkContainer;
    public Transform holdingContainer;
    public Transform buildingContainer;
    public Transform currentHeldBuilding;

    public bool holdExists;


    public bool onGround;
    public bool previewInBuilding;
    public bool previewCanPlace; //Which kind of preview we are currently displaying.

    Quaternion newRotationAngle;
    public float rotationLerpSpeed;

    public AudioSource rotateSFX;

    // Update is called once per frame
    void Update()
    {
        
        if (currentBuilding != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            PreviewCheck();
            DisplayBuilding();

            if(Input.GetMouseButtonDown(0))
            {
                Place();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateSFX.PlayOneShot(rotateSFX.clip);
                newRotationAngle = Quaternion.Euler(0, newRotationAngle.eulerAngles.y + 90,0);
            }
        }

        if(currentHeldBuilding != null)
        {
            if (!CanPlace() && previewCanPlace)
            {
                GameObject objToDes = currentHeldBuilding.gameObject;

                GameObject newOBJ = Instantiate(currentBuilding.buildingPreviewUnable, holdingContainer);
                newOBJ.transform.rotation = objToDes.transform.rotation;

                currentHeldBuilding = newOBJ.transform;

                Destroy(objToDes);
                previewCanPlace = false;
            }
            else if(CanPlace() && !previewCanPlace)
            {
                GameObject objToDes = currentHeldBuilding.gameObject;

                GameObject newOBJ = Instantiate(currentBuilding.buildingPreviewPrefab, holdingContainer);
                newOBJ.transform.rotation = objToDes.transform.rotation;

                currentHeldBuilding = newOBJ.transform;

                Destroy(objToDes);
                previewCanPlace = true;
            }

            holdingContainer.rotation = Quaternion.Lerp(holdingContainer.rotation, newRotationAngle, rotationLerpSpeed * Time.deltaTime);
            checkContainer.rotation = Quaternion.Lerp(checkContainer.rotation, newRotationAngle, rotationLerpSpeed * Time.deltaTime);
        }
    }

    bool CanPlace()
    {
        bool tempBool = true;

        if (!onGround)
        {
            tempBool = false;
        }

        if(currentBuilding.cost > GameManager.instance.material)
        {
            tempBool = false;
        }

        if (previewInBuilding)
        {
            tempBool = false;
        }

        return tempBool;
    }

    void PreviewCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100, groundLayers))
        {

            if (checkCollider == null)
            {
                GameObject checkPreview = Instantiate(currentBuilding.checkPrefab, checkContainer);
                checkPreview.GetComponentInChildren<BuildingPreview>().buildingScript = this;
                checkCollider = checkPreview.transform;

                checkCollider.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(holdingContainer.eulerAngles);

                checkCollider.position = hit.point;

                previewCanPlace = true;
            }
            else if (checkCollider != null)
            {
                checkCollider.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(holdingContainer.eulerAngles);

                checkCollider.position = hit.point;
            }

        }

    }
    void DisplayBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100, groundLayers))
        {
            if (hit.collider.tag == "Ground")
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }

            if (currentHeldBuilding == null)
            {
                GameObject buildingPreview = Instantiate(currentBuilding.buildingPreviewPrefab, holdingContainer);
                currentHeldBuilding = buildingPreview.transform;

                currentHeldBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(holdingContainer.eulerAngles);

                holdingContainer.position = hit.point;

                previewCanPlace = true;
            }
            else if(currentHeldBuilding != null)
            {
                currentHeldBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(holdingContainer.eulerAngles);

                holdingContainer.position = hit.point;
            }

        }
        else
        {
            onGround = false;
            foreach (Transform child in holdingContainer)
            {
                currentHeldBuilding = null;
                Destroy(child.gameObject);
            }
        }

        

    }

    private void OnDrawGizmosSelected()
    {
        //if(currentHeldBuilding != null)
        //{
        //    Vector3 newPos = currentBuilding.center + currentHeldBuilding.position;
        //    Vector3 size = currentBuilding.size;

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawCube(newPos, size);
        //}
    }

    void Place()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, groundLayers))
        {
            if (currentHeldBuilding != null && CanPlace())
            {
                GameManager.instance.AddMaterial(-currentBuilding.cost);

                GameObject placedBuilding = Instantiate(currentBuilding.buildingPrefab);

                placedBuilding.transform.position = hit.point;
                placedBuilding.transform.eulerAngles = currentHeldBuilding.eulerAngles;
            }

        }
    }

    private void OnDisable()
    {
        if (currentHeldBuilding != null)
        {
            Destroy(currentHeldBuilding.gameObject);
            currentHeldBuilding = null;
        }
        if(checkCollider != null)
        {
            Destroy(checkCollider.gameObject);
            checkCollider = null;
        }

        Deselect();

    }

    #region UI Events

    [Header("Buttons")]
    public Color selectedColor;
    public Color defaultColor;

    public Image factoryButton;
    public Image windTurbineButton;
    public Image solarPanelButton;

    public enum Buildings
    {
        Factory,
        WindTurbine,
        SolarPanel
    }

    public void BuildingSelect(Building newBuilding)
    {
        Deselect();

        currentBuilding = newBuilding;
        if(currentHeldBuilding != null)
        {
            Destroy(currentHeldBuilding.gameObject);
            currentHeldBuilding = null;
        }
        if(checkCollider != null)
        {
            Destroy(checkCollider.gameObject);
            checkCollider = null;
        }

        switch (newBuilding.buildingName)
        {
            case Buildings.Factory:
                factoryButton.color = selectedColor;
                break;
            case Buildings.SolarPanel:
                solarPanelButton.color = selectedColor;
                break;
            case Buildings.WindTurbine:
                windTurbineButton.color = selectedColor;
                break;
        }
    }

    void Deselect()
    {
        factoryButton.color = defaultColor;
        windTurbineButton.color = defaultColor;
        solarPanelButton.color = defaultColor;

        currentBuilding = null;
    }
    #endregion
}
