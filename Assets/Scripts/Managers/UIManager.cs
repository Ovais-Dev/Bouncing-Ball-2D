using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
class PanelObject
{
    public string gmName;
    public GameObject gmObject;

}
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public event Action settingsAction;
    public event Action extrasAction;
    public event Action exitAction;
    [Header("Panel Object")]
    [SerializeField] List<PanelObject> panelObjects;

    GameObject gameoverPanel;

    // Start is called before the first frame update
    void Awake()
    {
        //gameoverPanel = panelObjects.Find(e => e.gmName == "GameOver").gmObject;
        exitAction += () =>
        {
            DisablePanel("Exit",true);
            DisablePanel("Menu");
        };
        settingsAction += () =>
        {
            DisablePanel("Setting", true);
            DisablePanel("Menu");


        };
        extrasAction += () =>
        {
            DisablePanel("Extra", true);
            DisablePanel("Menu");
        };

        GameManager.Instance.playAction += () => { DisablePanel("Menu");
            DisablePanel("GameOver");
            DisablePanel("PauseButton", true);
            DisablePanel("LevelDenoter",true);
        };

        GameManager.Instance.gameOverAction += () => { Invoke(nameof(EnableGameOverPanel), 0.5f); DisablePanel("PauseButton"); };

        GameManager.Instance.gamePauseEvent += () => { DisablePanel("Pause", true); DisablePanel("PauseButton"); };

        GameManager.Instance.gameResumeEvent += () => { DisablePanel("Pause", false); DisablePanel("PauseButton",true); };

        GameManager.Instance.menuAction += () => {
            DisablePanel("Menu",true);
            DisablePanel("Extra");
            DisablePanel("Setting");
            DisablePanel("Exit");
            DisablePanel("GameOver");
            DisablePanel("PauseButton");
            DisablePanel("Pause");
            DisablePanel("LevelDenoter");
        };

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EnableGameOverPanel()
    {
       // gameoverPanel.SetActive(true);
        DisablePanel("GameOver", true);
    }

    public void Setting() => settingsAction?.Invoke();
    public void Extras() => extrasAction?.Invoke();
    public void OnExit() => exitAction?.Invoke();

    public void DisablePanel(string name,bool activate = false)
    {
        var gmObj = panelObjects.Find(e => e.gmName == name).gmObject;
        if (gmObj == null) { Debug.LogError($"The panel with {name} name doesn't exit. Please ensure it exist in UIManager PanelObject With Correct Name"); return; }
        gmObj.SetActive(activate);
    }
}
