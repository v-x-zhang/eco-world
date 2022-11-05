using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    bool isPaused;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            GameManager.instance.lockMovement = isPaused;
        }

        if (isPaused)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, 5 * Time.deltaTime);
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, 5 * Time.deltaTime);
            canvasGroup.blocksRaycasts = false;
        }

    }

    public void ResumeOnClick()
    {
        GameManager.instance.lockMovement = false;
        isPaused = false;
    }
}
