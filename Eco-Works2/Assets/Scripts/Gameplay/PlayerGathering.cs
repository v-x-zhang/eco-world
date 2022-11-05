using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGathering : MonoBehaviour
{
    public LayerMask materialLayers;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, materialLayers))
            {
                Entity hitObj = hit.collider.GetComponent<Entity>();
                if (hitObj != null)
                {
                    hitObj.DestroySelf();
                }
            }
        }
    }
}
