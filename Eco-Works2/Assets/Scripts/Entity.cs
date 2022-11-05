using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public int materialAmount;

    [Header("GFX")]
    public Color defaultColor;
    public Color hoverColor;

    public Outline outline;

    public GameObject destroyEffectPrefab;

    [Header("Audio")]
    public AudioSource destroySFX;
    public AudioSource hoverSFX;

    private void Start()
    {
        destroySFX = AudioManager.instance.treeDestroySFX;

        GameManager.instance.entityOutlines.Add(GetComponent<Outline>());
    }

    public void DestroySelf()
    {
        destroySFX.PlayOneShot(destroySFX.clip);

        GameManager.instance.AddMaterial(Random.Range(-5, 5) + materialAmount);

        GameManager.instance.entityOutlines.Remove(GetComponent<Outline>());

        GameObject destroyFX = Instantiate(destroyEffectPrefab);
        destroyFX.transform.position = transform.position;

        Destroy(destroyFX, 5f);

        Destroy(gameObject);
    }

    void OnMouseOver()
    {
        if (outline.enabled)
        {
            outline.OutlineColor = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (outline.enabled)
        {
            outline.OutlineColor = defaultColor;
        }
    }
}
