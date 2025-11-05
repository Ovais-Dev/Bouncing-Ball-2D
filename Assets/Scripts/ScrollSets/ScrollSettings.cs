using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollSettings : MonoBehaviour
{
    [System.Serializable]
    public struct ThemeIndexing
    {
        public string themeName;
        public int index;
        public float scollValue;
        public GameObject disabledTheme;
    }

    [SerializeField] Scrollbar scrollBar;
    [SerializeField] Button nextButton, backButton, selectButton;
    [SerializeField] List<ThemeIndexing> themeValues;

    int currentThemeIndex;
    
    private void Start()
    {
        UIManager.Instance.extrasAction += UpdateInitialThemeSettings;
    }
    void UpdateInitialThemeSettings()
    {
        var theme = ThemeManager.Instance.GetCurrentTheme();
        var selectedTheme = themeValues.Find(e => e.themeName == theme.themeName);
        currentThemeIndex = selectedTheme.index;
       // scrollBar.value = selectedTheme.scollValue;
        StartCoroutine(LerpScrollValue(selectedTheme.scollValue));
        SelectButtonUpdation(themeValues[currentThemeIndex]);
        UpdateButtonSettings();
    }

    public void Next()
    {
        if (currentThemeIndex >= themeValues.Count - 1) return;
        currentThemeIndex++;
        var selectedTheme = themeValues.Find(e => e.index == currentThemeIndex);
        //scrollBar.value = selectedTheme.scollValue;
        StartCoroutine(LerpScrollValue(selectedTheme.scollValue));
        SelectButtonUpdation(themeValues[currentThemeIndex]);
        UpdateButtonSettings();
    }
    public void Back()
    {
        if (currentThemeIndex <= 0) return;
        currentThemeIndex--;
        var selectedTheme = themeValues.Find(e => e.index == currentThemeIndex);
        //scrollBar.value = selectedTheme.scollValue;
        StartCoroutine(LerpScrollValue(selectedTheme.scollValue));
        SelectButtonUpdation(themeValues[currentThemeIndex]);
        UpdateButtonSettings();
    }
    void UpdateButtonSettings()
    {
        if (currentThemeIndex >0)
        {
            backButton.interactable = true;
        }
        else
        {
            backButton.interactable = false;
        }
        if (currentThemeIndex < themeValues.Count - 1)
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
        
    }
    public void SelectButtonUpdation(ThemeIndexing themeValue)
    {
        ThemeData overTheme = ThemeManager.Instance.GetTheme(themeValue.themeName);
        if (overTheme.unlocked)
        {
            selectButton.interactable = true;
            GameObject dTheme = themeValue.disabledTheme;
            dTheme.SetActive(false);
        }
        else
        {
            selectButton.interactable = false;
            GameObject dTheme = themeValue.disabledTheme;
            dTheme.SetActive(true);
        }
    }
    public void SelectTheme()
    {
        SelectTheme(currentThemeIndex);
    }
    
    void SelectTheme(int index)
    {
        var currentThemeName = themeValues.Find(e => e.index == index).themeName;
        ThemeManager.Instance.ApplyTheme(currentThemeName);
    }
    IEnumerator LerpScrollValue(float value)
    {
        float elapsedTime = 0;
        float startVolume = scrollBar.value;
        float duration = 0.5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            scrollBar.value = Mathf.Lerp(startVolume, value, elapsedTime / duration);
            yield return null;
        }
        scrollBar.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
