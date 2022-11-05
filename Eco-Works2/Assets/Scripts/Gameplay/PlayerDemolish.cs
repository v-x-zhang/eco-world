using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolish : MonoBehaviour
{
    public LayerMask buildingLayers;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, buildingLayers))
            {
                Placeable hitObj = hit.collider.GetComponent<Placeable>();
                if(hitObj != null)
                {
                    hitObj.DestroySelf();
                }
            }
        }
    }
}
