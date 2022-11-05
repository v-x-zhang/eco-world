using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField]
    private Transform rotateTransform;
    [SerializeField]
    private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {   
        transform.RotateAround(rotateTransform.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
