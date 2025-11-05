using UnityEngine;

[CreateAssetMenu(fileName ="Themes", menuName ="Game/Theme")]
public class ThemeData : ScriptableObject
{
    public string themeName;
    public bool unlocked = false;
    [Header("Background Colors")]
    public Color layer1Color;
    public Color layer2Color;
    public Color layer3Color;
    public Color layer4Color;
    public Color layer5Color;
    public Color layer6Color;
    public Color layer7Color;

    [Header("Ball Colors")]
    public Color ballColor;
    public Color trail1Color;
    public Color trail2Color;
    public Color shootEffectColor;
    public Color hitEffetColor;

    [Header("UI Color")]
    public Color uiColor;
    public Color mainBGColor;
    public Color uiBg1Color;
    public Color uiBg2Color;

}
