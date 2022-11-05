using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public string[] tips;

    public Animator animator;
    public GameObject loadingScreen;
    public Slider progressBar;
    public Text progressText;

    public float progressBarLerpSpeed;

    public Text tooltipText;

    [HideInInspector]
    public float currentProgress;
    bool isDoneLoading;
    public void LoadLevel(int sceneIndex, int seed)
    {
        //Debug.Log("check");
        StartCoroutine(LoadAsyncronously(sceneIndex, seed));
    }


    IEnumerator LoadAsyncronously(int sceneIndex, int _seed)
    {
        isDoneLoading = false;
        StartCoroutine(ProgressBarLerp());
        progressText.text = "Building Scene...";
        loadingScreen.SetActive(true);

        int randomInt = Random.Range(0, tips.Length - 1);

        tooltipText.text = tips[randomInt];
        animator.SetBool("isLoading", !isDoneLoading);
        yield return new WaitForSecondsRealtime(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);


        while (!operation.isDone)
        {
            yield return null;
        }

        while(GameManager.instance == null)
        {
            yield return null;
        }

        currentProgress = 0.2f;
        yield return new WaitForSecondsRealtime(1f);
        progressText.text = "Generating Terrain...";

        GameManager.instance.levelLoader = this;

        GameManager.instance.mapPreview.seed = _seed;

        GameManager.instance.BeginGenerationSequence();
    }

    IEnumerator ProgressBarLerp()
    {
        while (!isDoneLoading)
        {
            progressBar.value = Mathf.Lerp(progressBar.value, currentProgress, Time.deltaTime * progressBarLerpSpeed);

            yield return null;
        }
    }
}
