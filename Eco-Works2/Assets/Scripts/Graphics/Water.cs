using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float waveSpeed;

    float timer;
    bool rising;
    private void Update()
    {
        if (rising)
        {
            timer += Time.deltaTime * waveSpeed;
        }
        else
        {
            timer -= Time.deltaTime * waveSpeed;
        }

        if(timer > 1)
        {
            rising = false;
        }
        else if(timer < 0)
        {
            rising = true;
        }

        transform.position = new Vector3(0,animationCurve.Evaluate(timer), 0);
    }
}
