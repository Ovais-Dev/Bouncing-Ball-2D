using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    //[System.Serializable]
    public enum GameState { Menu = -1, Play=0, Paused = 1};
    public GameState gmState;

    public event Action playAction;
    public event Action gameOverAction;
    public event Action gamePauseEvent;
    public event Action gameResumeEvent;
    public event Action menuAction;

   

    Ball ball;
    //Transform obsHolder;
    // Start is called before the first frame update
    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        //obsHolder = GameObject.Find("ObstacleHolder").transform;

        playAction += () => { gmState = GameState.Play; BallInitialization(); };
        gameOverAction += () => {  gmState = GameState.Menu;ball.gameObject.SetActive(false);StartCoroutine(SlowEffectGameOver()); };
        gamePauseEvent += () => {  gmState = GameState.Paused; Time.timeScale = 0f; };
        gameResumeEvent += () => { gmState = GameState.Play; Time.timeScale = 1f; };
        menuAction += () => { gmState = GameState.Menu; };
    }
    void Start()
    {

        InitialSettings();
       // menuAction += () => { gmState = GameState.Menu; };
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (gmState == GameState.Play)
                Pause();
            else if (gmState == GameState.Menu) UIManager.Instance.OnExit();
            
        }
    }
    void InitialSettings()
    {
        Time.timeScale = 1f;
        ball.gameObject.SetActive(false);
        Menu();
    }

    void BallInitialization()
    {
        ball.ResetPosition();
        ball.gameObject.SetActive(true);
    }
    public void Play() => playAction?.Invoke();
    public void Pause() => gamePauseEvent?.Invoke(); // for restart too
    public void Resume() => gameResumeEvent?.Invoke();

    public void GameOver() => gameOverAction?.Invoke();

    public void Menu() => menuAction?.Invoke(); // for all menu where we have to disable the their panel

   
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SlowEffectGameOver()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 1f;
    }
}
