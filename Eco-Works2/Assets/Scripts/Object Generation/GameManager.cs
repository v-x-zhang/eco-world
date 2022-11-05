using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        
        instance = this;
        Debug.Log("GameManager Initialized");
    }
    #endregion

    #region Gameplay
    public float maxFog;


    public List<Placeable> currentPlacedBuildings;



    [Header("Power")]
    public float powerGoal;
    public float currentPower;
    public float currentPowerGrowthRate;

    public Slider powerSlider;
    public Text powerPercentageText;


    [Header("CO2")]
    public float currentCO2Rate;
    public float naturalDecayRate;
    public float currentCO2;
    public float maxCO2;

    public Slider CO2Slider;
    public Text CO2PercentageText;

    public Image background;
    public Image fill;

    public Gradient colorGradient;

    void UpdateValues()
    {
        if (lockMovement) return;

        //Power
        currentPower += currentPowerGrowthRate * Time.deltaTime / 4;

        float currentPowerValue = currentPower / powerGoal;

        powerSlider.value = currentPowerValue;
        powerPercentageText.text = (System.Math.Round(Mathf.Clamp(currentPowerValue * 100, 0, 100), 1) + "%");

        currentCO2 += (currentCO2Rate * Time.deltaTime) / 4;
        currentCO2 -= naturalDecayRate * Time.deltaTime;

        currentCO2 = Mathf.Clamp(currentCO2, 0, 100);

        float currentCO2Value = currentCO2 / maxCO2;

        CO2Slider.value = currentCO2Value;
        CO2PercentageText.text = (System.Math.Round(Mathf.Clamp(currentCO2Value * 100, 0, 100), 1) + "%");

        Color newColor = colorGradient.Evaluate(currentCO2Value);

        background.color = newColor;
        fill.color = newColor;

        RenderSettings.fogDensity = maxFog * (currentCO2 / maxCO2);


        if (currentPower >= powerGoal)
        {
            GameWin();
        }

        if(currentCO2 >= maxCO2)
        {
            GameOver();
        }
    }

    #endregion

    #region Terrain Generation

    [Header("Map Generation")]
    public LevelLoader levelLoader;

    public TerrainGenerator terrainGenerator;
    public ObjectGenerator objectGenerator;
    public MapPreview mapPreview;

    public void BeginGenerationSequence()
    {
        StartCoroutine(GenerationSequence());
    }

    public IEnumerator GenerationSequence()
    {
        yield return StartCoroutine(terrainGenerator.GenerateTerrain()); //Generate Base Mesh
        levelLoader.currentProgress = 0.4f;
        yield return new WaitForSecondsRealtime(1f);
        levelLoader.progressText.text = "Placing Objects...";

        yield return StartCoroutine(objectGenerator.GenerateObjects());//Generate Trees, Bushes, Etc.
        levelLoader.currentProgress = 0.6f;
        yield return new WaitForSecondsRealtime(1f);
        levelLoader.progressText.text = "Growing Grass...";

        yield return StartCoroutine(objectGenerator.GenerateGrass());
        levelLoader.currentProgress = 0.8f;
        yield return new WaitForSecondsRealtime(1f);

        levelLoader.progressText.text = "Initializing...";
        levelLoader.currentProgress = 1f;

        yield return new WaitForSecondsRealtime(1f);

        Debug.Log("Done Loading");
        levelLoader.animator.SetBool("isLoading", false);
    }


    public void SetIslandMeshData(MeshData _meshData)
    {
        objectGenerator.meshData = _meshData;
    }
    #endregion

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(GenerateMapTesting());
        //}

        UpdateValues();

        UpdateMoneyText();
    }

    //IEnumerator GenerateMapTesting()
    //{
    //    yield return StartCoroutine(terrainGenerator.GenerateTerrain()); //Generate Base Mesh
    //    yield return new WaitForSecondsRealtime(1f);
    //    yield return StartCoroutine(objectGenerator.GenerateObjects());//Generate Trees, Bushes, Etc.
    //    yield return new WaitForSecondsRealtime(1f);
    //    yield return StartCoroutine(objectGenerator.GenerateGrass());
    //}

    public void UpdateRate(bool affectCO2, float affectRate, bool givePower, int powerProvisionRate)
    {
        if (affectCO2)
        {
            currentCO2Rate += affectRate;

        }


        if (givePower)
        {
            currentPowerGrowthRate += powerProvisionRate;
        }
        
    }

    #region Resources

    [Header("Outlines")]
    public List<Outline> buildingOutlines;

    public List<Outline> entityOutlines;

    [Header("Resources")]
    public float materialLerpSpeed;
    public int material;

    float currentMaterial;

    public Text materialText;

    public Text materialChangeText;
    public CanvasGroup currentCanvasGroup;

    private void Start()
    {
        materialText.text = material.ToString();
    }

    public void AddMaterial(int amount)
    {
        material += amount;

        if(amount < 0)
        {
            materialChangeText.text = amount.ToString();
        }
        else
        {
            materialChangeText.text = "+" + amount.ToString();
        }

        currentCanvasGroup.alpha = 1;
        //materialText.text = material.ToString();
    }

    void UpdateMoneyText()
    {
        currentMaterial = Mathf.Lerp(currentMaterial, material, materialLerpSpeed * Time.deltaTime);

        currentCanvasGroup.alpha = Mathf.Lerp(currentCanvasGroup.alpha, 0, 4 * Time.deltaTime);

        materialText.text = Mathf.RoundToInt(currentMaterial).ToString();
    }

    #endregion


    #region Ending
    [Header("Ending")]
    public bool lockMovement;


    //Lose
    public GameObject lossPanel;
    public Text powerText;

    //Win
    public GameObject winPanel;

    public Text CO2Text;


    public CanvasGroup fadeCanvas;

    public AudioManager audioManager;
    void GameOver()
    {
        audioManager.fadeOut = true;
        lockMovement = true;

        lossPanel.SetActive(true);
        powerText.text = "Your Power Level : " + (System.Math.Round(Mathf.Clamp((currentPower / powerGoal) * 100, 0, 100), 1) + "%");
    }

    void GameWin()
    {
        audioManager.fadeOut = true;
        lockMovement = true;

        winPanel.SetActive(true);
        CO2Text.text = "Your CO2 Level : " + (System.Math.Round(Mathf.Clamp((currentCO2 / maxCO2) * 100, 0, 100), 1) + "%");
    }

    public void BackToMenuOnClick()
    {
        if(levelLoader != null)
        {
            Destroy(levelLoader.gameObject);
        }

        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        fadeCanvas.blocksRaycasts = true;
        while(fadeCanvas.alpha < 0.99)
        {
            fadeCanvas.alpha = Mathf.Lerp(fadeCanvas.alpha, 1, 5 * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void RetryOnClick()
    {
        instance = null;
        levelLoader.LoadLevel(1, mapPreview.seed);
    }

    public void NextOnClick()
    {
        instance = null;

        int randSeed = Random.Range(0, 1000000);
        levelLoader.LoadLevel(1, randSeed);
    }

    #endregion


}
