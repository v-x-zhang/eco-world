using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public PlayerBuilding buildingScript;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered");
        buildingScript.previewInBuilding = true;

    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exited");
        buildingScript.previewInBuilding = false;

    }
}
