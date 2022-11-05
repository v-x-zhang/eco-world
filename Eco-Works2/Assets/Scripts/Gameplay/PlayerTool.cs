using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    public PlayerBuilding buildingScript;
    public PlayerGathering gatheringScript;
    public PlayerDemolish demolishScript;

    public Animator buildPanelAnimator;

    public void BuildOnClick()
    {
        buildingScript.enabled = !buildingScript.enabled;
        buildPanelAnimator.SetBool("isOpen", buildingScript.enabled);

        ToggleBuildingOutlines(false);
        ToggleEntityOutlines(false);

        //Disable Other Tools
        gatheringScript.enabled = false;
        demolishScript.enabled = false;
    }

    public void GatheringOnClick()
    {
        gatheringScript.enabled = !gatheringScript.enabled;

        ToggleBuildingOutlines(false);
        ToggleEntityOutlines(gatheringScript.enabled);

        //Disable Other Tools
        buildingScript.enabled = false;
        demolishScript.enabled = false;
        buildPanelAnimator.SetBool("isOpen", buildingScript.enabled);
    }

    public void DemolishOnClick()
    {
        demolishScript.enabled = !demolishScript.enabled;

        ToggleBuildingOutlines(demolishScript.enabled);
        ToggleEntityOutlines(false);

        //Disable Other Tools
        gatheringScript.enabled = false;
        buildingScript.enabled = false;
        buildPanelAnimator.SetBool("isOpen", buildingScript.enabled);
    }

    void ToggleBuildingOutlines(bool on)
    {
        foreach (Outline outline in GameManager.instance.buildingOutlines)
        {
            if (outline.enabled == !on)
            {
                outline.enabled = on;
            }
        }
    }

    void ToggleEntityOutlines(bool on)
    {
        foreach(Outline outline in GameManager.instance.entityOutlines)
        {
            if (outline.enabled == !on)
            {
                outline.enabled = on;
            }
        }
    }
}
