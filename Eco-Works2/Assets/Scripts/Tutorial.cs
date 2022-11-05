using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public int currentPane;

    public Image image;
    public Sprite[] imagePanes;

    public Image[] circleButtons;

    public Color selectedColor;
    public Color nonSelectedColor;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Tutorial", 1) == 1)
        {
            GameManager.instance.lockMovement = true;
            UpdateGFX();
        }
        else
        {
            gameObject.SetActive(false);
            GameManager.instance.lockMovement = false;
        }

    }
    
    public void OpenTutorial()
    {
        GameManager.instance.lockMovement = true;
        gameObject.SetActive(true);   
    }

    public void LeftArrowClick()
    {
        currentPane--;
        currentPane = Mathf.Clamp(currentPane, 0, imagePanes.Length-1);

        UpdateGFX();
    }

    public void RightArrowClick()
    {
        currentPane++;
        currentPane = Mathf.Clamp(currentPane, 1, imagePanes.Length-1);

        UpdateGFX();
    }

    public void SetPane(int paneToSet)
    {
        currentPane = paneToSet;
        UpdateGFX();
    }

    public void CloseOnClick()
    {
        gameObject.SetActive(false);
        GameManager.instance.lockMovement = false;
    }

    void UpdateGFX()
    {
        image.sprite = imagePanes[currentPane];
        
        foreach(Image image in circleButtons)
        {
            image.color = nonSelectedColor;
        }

        circleButtons[currentPane].color = selectedColor;
    }
}
