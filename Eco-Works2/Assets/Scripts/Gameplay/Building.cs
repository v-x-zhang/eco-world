using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public PlayerBuilding.Buildings buildingName;

    public int cost;

    public GameObject checkPrefab;

    public GameObject buildingPreviewPrefab;
    public GameObject buildingPreviewUnable;
    public GameObject buildingPrefab;
}
