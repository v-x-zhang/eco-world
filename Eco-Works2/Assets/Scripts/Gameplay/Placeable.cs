using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public Building building;

    [Tooltip("How Much to Increase/Decrease the Value by Every Second.")]
    public float effectRate = 1f;
    public bool affectCO2;

    [Header("Power")]
    public bool usePower;
    public bool providePower;
    public int usageRate;
    public int powerProvisionRate;


    [Header("GFX")]
    public Color defaultColor;
    public Color hoverColor;

    public Outline outline;

    public GameObject placeEffectPrefab;
    public GameObject destroyEffectPrefab;


    [Header("Audio")]
    public AudioSource placeSFX;
    public AudioSource destroySFX;

    private void Start()
    {
        placeSFX = AudioManager.instance.buildingPlaceSFX;
        destroySFX = AudioManager.instance.buildingDemolishSFX;

        placeSFX.PlayOneShot(placeSFX.clip);

        GameObject placeFX = Instantiate(placeEffectPrefab);
        placeFX.transform.position = transform.position;

        GameManager.instance.currentPlacedBuildings.Add(this); //Might be unnecessary
        GameManager.instance.UpdateRate(affectCO2, effectRate, providePower, powerProvisionRate);

        GameManager.instance.buildingOutlines.Add(GetComponent<Outline>());

        Destroy(placeFX, 5f);

    }

    void OnMouseOver()
    {
        if (outline.enabled)
        {
            outline.OutlineColor = hoverColor;
        }
    }

    public void DestroySelf()
    {
        destroySFX.PlayOneShot(destroySFX.clip);

        GameManager.instance.UpdateRate(affectCO2, -effectRate, providePower, -powerProvisionRate);
        GameManager.instance.buildingOutlines.Remove(GetComponent<Outline>());
        GameManager.instance.currentPlacedBuildings.Remove(this);

        GameManager.instance.AddMaterial(building.cost / 2);

        GameObject destroyFX = Instantiate(destroyEffectPrefab);
        destroyFX.transform.position = transform.position;

        Destroy(destroyFX, 5f);

        Destroy(gameObject);
    }

    void OnMouseExit()
    {
        if (outline.enabled)
        {
            outline.OutlineColor = defaultColor;
        }
    }

}
