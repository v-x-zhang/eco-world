using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectSettings : UpdatableData
{
    public Object[] spawnableObjects;
    public Object[] grassObjects;
    public float slopeHeightMax;


    public float nearbyObjectCheckRadius;
    public LayerMask nearbyObjectLayerMask;
    public LayerMask groundMask;

    [System.Serializable]
    public class Object
    {
        public GameObject objectPrefab;
        public string objectName;

        public float startHeight;
        public float endHeight;

        [Range(0,100)]
        public int spawnChance;


        [Header("Randomization")]
        public bool randomizePosition;
        public Vector3 randomizePositionProperties;
        [Space(10)]
        public bool alignToGround;
        public bool randomizeRotation;
        public Vector3 randomizeRotationProperties;
        [Space(10)]
        public bool randomizeScale;
        public Vector3 randomizeScaleProperties;

        [Header("Offset")]
        public Vector3 offsetPosition;
        public Vector3 offsetRotation;
        public Vector3 offsetScale;

        //public bool checkOnlyForSameObject;
    }
}
