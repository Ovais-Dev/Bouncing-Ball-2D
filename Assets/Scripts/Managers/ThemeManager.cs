using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{

    private static ThemeManager _instance;
    public static ThemeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ThemeManager>();
            }
            return _instance;
        }
    }

    [System.Serializable]
    class MaterialDatas
    {
        public string name;
        public Material mat;
    }

    [SerializeField] private List<MaterialDatas> allMaterial; 

    [SerializeField]private  List<ThemeData> themes;
    private ThemeData currentTheme;

    private void Start()
    {
        string theme = PlayerPrefs.GetString("Theme", "Blue Mountain");
        ApplyTheme(theme);
    }

    public void ApplyTheme(string themeName = "Blue Mountain")
    {
        currentTheme = GetTheme(themeName);
       
        if (currentTheme == null) { Debug.LogError($"The Theme Doesn't Exist In Themes Data. Ensure it is present with {themeName}"); return; }
        allMaterial.Find(e => e.name == "Layer1").mat.color = currentTheme.layer1Color;
        allMaterial.Find(e => e.name == "Layer2").mat.color = currentTheme.layer2Color;
        allMaterial.Find(e => e.name == "Layer3").mat.color = currentTheme.layer3Color;
        allMaterial.Find(e => e.name == "Layer4").mat.color = currentTheme.layer4Color;
        allMaterial.Find(e => e.name == "Layer5").mat.color = currentTheme.layer5Color;
        allMaterial.Find(e => e.name == "Layer6").mat.color = currentTheme.layer6Color;
        allMaterial.Find(e => e.name == "Layer7").mat.color = currentTheme.layer7Color;

        allMaterial.Find(e => e.name == "Ball").mat.color = currentTheme.ballColor;
        allMaterial.Find(e => e.name == "Trail1").mat.color = currentTheme.trail1Color;
        allMaterial.Find(e => e.name == "Trail2").mat.color = currentTheme.trail2Color;
        allMaterial.Find(e => e.name == "Shoot").mat.color = currentTheme.shootEffectColor;
        allMaterial.Find(e => e.name == "Hit").mat.color = currentTheme.hitEffetColor;

        allMaterial.Find(e => e.name == "UIs").mat.color = currentTheme.uiColor;
        allMaterial.Find(e=>e.name == "MainBGUI").mat.color = currentTheme.mainBGColor;
        allMaterial.Find(e => e.name == "MainBG1UI").mat.color = currentTheme.uiBg1Color;
        allMaterial.Find(e => e.name == "MainBG2UI").mat.color = currentTheme.uiBg2Color;
    }
    public ThemeData GetCurrentTheme()
    {
        return currentTheme;
    }
    public ThemeData GetTheme(string themeName)
    {
        return themes.Find(e => e.themeName == themeName);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Theme", currentTheme.themeName);
    }
}
