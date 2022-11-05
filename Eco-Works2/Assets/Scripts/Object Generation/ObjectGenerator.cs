using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    
    public MeshData meshData;

    public ObjectSettings settings;


    public Transform objectContainer;


    Vector3[] meshVertices;
    Vector3[] meshNormals;
    public IEnumerator GenerateObjects()
    {
        float lastNoiseHeight = 0;

        meshVertices = meshData.GetVertices();
        meshNormals = meshData.CreateMesh().normals;
        for (int i = 0; i < meshVertices.Length; i++)
        {
            Vector3 worldPoint = transform.TransformPoint(meshVertices[i]);
            Vector3 offsetWorldPoint = new Vector3(worldPoint.x - 10.6f, worldPoint.y - 6.35f, worldPoint.z + 106.3f);

            float noiseHeight = offsetWorldPoint.y;
            ObjectSettings.Object spawnObj = settings.spawnableObjects[Random.Range(0, settings.spawnableObjects.Length)];


            if (Mathf.Abs(lastNoiseHeight - worldPoint.y) < settings.slopeHeightMax)
            {
                if (noiseHeight > spawnObj.startHeight)
                {
                    if (noiseHeight < spawnObj.endHeight)
                    {
                        int spawnTicket = Random.Range(0, 101);
                        if (spawnTicket <= spawnObj.spawnChance)
                        {
                            bool alreadyHasObjectNearby = Physics.CheckSphere(offsetWorldPoint, settings.nearbyObjectCheckRadius, settings.nearbyObjectLayerMask);


                            if (!alreadyHasObjectNearby)
                            {
                                GameObject spawnedOBJ = Instantiate(spawnObj.objectPrefab, offsetWorldPoint, Quaternion.identity);
                                spawnedOBJ.transform.parent = objectContainer;
                                if (spawnObj.alignToGround)
                                {
                                    spawnedOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, meshNormals[i]);
                                }
                                Randomize(spawnedOBJ, spawnObj, settings.groundMask);
                            }
                        }
                    }
                }
            }

            lastNoiseHeight = noiseHeight;
        }
        yield return null;
    }

    public IEnumerator GenerateGrass()
    {
        float lastNoiseHeight = 0;
        for (int i = 0; i < meshVertices.Length; i++)
        {
            Vector3 worldPoint = transform.TransformPoint(meshVertices[i]);
            Vector3 offsetWorldPoint = new Vector3(worldPoint.x - 10.6f, worldPoint.y - 6.35f, worldPoint.z + 106.3f);

            float noiseHeight = offsetWorldPoint.y;
            ObjectSettings.Object grassObj = settings.grassObjects[Random.Range(0, settings.grassObjects.Length)];


            if (Mathf.Abs(lastNoiseHeight - worldPoint.y) < settings.slopeHeightMax)
            { 
                if (noiseHeight > grassObj.startHeight && noiseHeight < grassObj.endHeight)
                {

                    int spawnTicket = Random.Range(0, 101);
                    if (spawnTicket <= grassObj.spawnChance)
                    {

                        GameObject spawnedOBJ = Instantiate(grassObj.objectPrefab, offsetWorldPoint, Quaternion.identity);
                        spawnedOBJ.transform.parent = objectContainer;
                        if (grassObj.alignToGround)
                        {
                            spawnedOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, meshNormals[i]);
                        }
                        Randomize(spawnedOBJ, grassObj, settings.groundMask);
                    }


                }
            }

            lastNoiseHeight = noiseHeight;
        }
        yield return null;
    }


    void Randomize(GameObject _gameObject, ObjectSettings.Object settings, LayerMask groundMask)
    {
        Transform obj = _gameObject.transform;

        //Randomize Position
        if (settings.randomizePosition)
        {
            float newX = obj.position.x + Random.Range(-settings.randomizePositionProperties.x,settings.randomizePositionProperties.x);
            float newY = obj.position.y + Random.Range(-settings.randomizePositionProperties.y, settings.randomizePositionProperties.y);
            float newZ = obj.position.z + Random.Range(-settings.randomizePositionProperties.z, settings.randomizePositionProperties.z);
            Vector3 newPosition = new Vector3(newX, newY, newZ);
            obj.localPosition = newPosition;
        }

        //Randomize Rotation
        if (settings.randomizeRotation)
        {
            float newX = obj.localEulerAngles.x + Random.Range(-settings.randomizeRotationProperties.x, settings.randomizeRotationProperties.x);
            float newY = obj.localEulerAngles.y + Random.Range(-settings.randomizeRotationProperties.y, settings.randomizeRotationProperties.y);
            float newZ = obj.localEulerAngles.z + Random.Range(-settings.randomizeRotationProperties.z, settings.randomizeRotationProperties.z);

            Vector3 newRotation = new Vector3(newX, newY, newZ);
            obj.localEulerAngles = newRotation;
        }

        //Randomize Scale
        if (settings.randomizeScale)
        {
            float newX = obj.localScale.x + Random.Range(-settings.randomizeScaleProperties.x, settings.randomizeScaleProperties.x);
            float newY = obj.localScale.y + Random.Range(-settings.randomizeScaleProperties.y, settings.randomizeScaleProperties.y);
            float newZ = obj.localScale.z + Random.Range(-settings.randomizeScaleProperties.z, settings.randomizeScaleProperties.z);

            Vector3 newScale = new Vector3(newX, newY, newZ);
            obj.localScale = newScale;
        }



        //Move straight to terrain
        RaycastHit hit;
        if (Physics.Raycast(obj.position, Vector3.up, out hit,100, groundMask))
        {
            obj.position = hit.point;
        }
        else if(Physics.Raycast(obj.position, -Vector3.up, out hit, 100, groundMask))
        {
            obj.position = hit.point;
        }
        else
        {
            Destroy(obj.gameObject);
        }

        if (obj.position.y < settings.startHeight || obj.position.y > settings.endHeight)
        {
            Destroy(obj.gameObject);
        }

        //Final offset
        obj.position += settings.offsetPosition;
        obj.eulerAngles += settings.offsetRotation;
        obj.localScale += settings.offsetScale;

        
    }
}
