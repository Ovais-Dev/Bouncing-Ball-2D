using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ScoreManagers : MonoBehaviour
{
    private static ScoreManagers _instance;

    public static ScoreManagers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManagers>();
            }
            return _instance;
        }
    }


    [SerializeField] Text scoreText;
    [SerializeField] Text[] allScoreTexts;
    [SerializeField] Text[] allHighScoreTexts;


    int score = 0;
    public static int highestScore { get; private set; }


    private void Start()
    {
        GameManager.Instance.playAction += ResetScore;
        GameManager.Instance.gameOverAction += ()=> { SaveScore();UpdateAllScoreText(); scoreText.text = ""; };
        UIManager.Instance.settingsAction += () => { scoreText.text = ""; };
        UIManager.Instance.extrasAction += () => { scoreText.text = ""; };
        UIManager.Instance.exitAction += () => { scoreText.text = ""; };
        GameManager.Instance.gamePauseEvent += () => { scoreText.text = ""; };
        GameManager.Instance.gameResumeEvent += () => { scoreText.text = score.ToString(); };
        GameManager.Instance.menuAction += () => { scoreText.text = highestScore.ToString(); };

        LoadHighestScore();
    }

    void LoadHighestScore()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore");
        scoreText.text = highestScore.ToString();
    }
    void SaveScore()
    {
        if (score >= highestScore)
        {
            PlayerPrefs.SetInt("HighestScore", score);
        }
    }
    void UpdateAllScoreText()
    {
        foreach (var scText in allScoreTexts)
        {
            scText.text = score.ToString();
        }
        foreach(var hsText in allHighScoreTexts)
        {
            hsText.text = highestScore.ToString();
        }
    }
    
    public void IncrementScore(int amt = 1)
    {
        score += amt;
        scoreText.text = $"{score}";
    }
    public void ResetScore()
    {
        this.score = 0;
        scoreText.text = $"{score}";
    }
}
