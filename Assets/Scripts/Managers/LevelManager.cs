using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }
            return _instance;
        }
    }

    public int currentLevel = 0;
    public Text levelText;
    public Slider levelSlider;

    public AnimationCurve levelsCurve;

    float currentTime;
    float maxTime;
    // Start is called before the first frame update
    void Start()
    {
        maxTime = levelsCurve.Evaluate(currentLevel);
        levelSlider.minValue = 0;
        levelSlider.maxValue = maxTime;
        levelSlider.value = currentTime;
        levelText.text= $"Level- { currentLevel + 1}";
        
        GameManager.Instance.gameOverAction += () => { PlayerPrefs.SetInt("Level", currentLevel); };
        GameManager.Instance.playAction += LevelRestart;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gmState != GameManager.GameState.Play) return;
        currentTime += Time.deltaTime;
        levelSlider.value = currentTime;
        if (currentTime >= maxTime)
        {
            currentTime = 0;
            currentLevel++;
            //if (currentLevel < levelsCurve.length)
            //{
                maxTime = levelsCurve.Evaluate(currentLevel);
                ControllLevelHard.Instance.UpdateLevelChallenge(currentLevel);
            //}
            UpdateLevelAndValues();
            Debug.Log($"Current Level {currentLevel}, Max time {maxTime}");
            FindObjectOfType<Ball>().UpgradeEffect();
        }
    }
    void UpdateLevelAndValues()
    {
        levelSlider.maxValue = maxTime;
        levelSlider.value = currentTime;
        levelText.text = $"Level- { currentLevel + 1}";
    }
    void LevelRestart()
    {
        currentTime = 0;
        currentLevel = 0;
        maxTime = levelsCurve.Evaluate(currentLevel);
        ControllLevelHard.Instance.UpdateLevelChallenge(currentLevel);
        UpdateLevelAndValues();
    }
    private void LateUpdate()
    {
        levelSlider.value = currentTime;
    }
    private void OnApplicationQuit()
    {
        //PlayerPrefs.SetInt("Level", currentLevel);
    }
}
