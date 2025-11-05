using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
[System.Serializable]
class Sounds
{
    public string clipName;
    public AudioClip soundClip;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Volume Controls (Mixer)")]
    [Range(-10, 10)] [SerializeField] float maxVolume;
    [SerializeField] AudioMixer musicMixer;
    float initialMainVolume;
    [SerializeField] AudioMixer sfxMixer;

    [Header("UI Controls")]
    public Slider mainVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Sound Clips (with name)")]
    [SerializeField] List<Sounds> sounds;
    
    // smooth volume controls
    bool volumeChange = false;
    float elapsedTime = 0f;
    private void Awake()
    {
        InitialSettings();
        
        GameManager.Instance.playAction +=  ResetMainVolume;
        GameManager.Instance.gameOverAction += () => ChangeMainVolumeUsingPercentage(-80f);
        GameManager.Instance.menuAction += () => ChangeMainVolumeUsingPercentage(-80f);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
        Button[] uiButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach(var uiButton in uiButtons)
        {
            
            uiButton.onClick.AddListener(()=> PlayClip("UIClick"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(volumeChange)
        {
            elapsedTime += Time.unscaledDeltaTime;
            
        }

    }

    void InitialSettings()
    {
        float startMainVolume = PlayerPrefs.GetFloat("MainVolume",maxVolume-10);
        float startSfxVolume = PlayerPrefs.GetFloat("SfxVolume",maxVolume-5);
        MainVolumeControl(startMainVolume);
        SfxVolumeControl(startSfxVolume);
        mainVolumeSlider.maxValue = maxVolume;
        sfxVolumeSlider.maxValue = maxVolume;   
        mainVolumeSlider.value = startMainVolume;
        sfxVolumeSlider.value = startSfxVolume;
        initialMainVolume = 1;
    }
    #region Audio Control
    public void PlayMusic()
    {
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void ChangeMainVolumeUsingPercentage(float percentage)//percentage can be negative or positive
    {
        float newValue = initialMainVolume * (1+ percentage * 0.01f);

        StartCoroutine(FadeVolume(newValue));
    }
    public void ResetMainVolume()
    {
        StartCoroutine(FadeVolume(initialMainVolume));
    }
    IEnumerator FadeVolume(float value)
    {
        float elapsedTime = 0;
        float startVolume = musicSource.volume;
        float duration = 2f;
        while (elapsedTime<duration)
        {
            elapsedTime += Time.unscaledDeltaTime ;
            musicSource.volume = Mathf.Lerp(startVolume, value, elapsedTime/duration);
            yield return null;
        }
        musicSource.volume = value;
    }

    public void PlayClip(string clipName)
    {
        var clip = sounds.Find(a => a.clipName == clipName).soundClip;
        sfxSource.PlayOneShot(clip);
    }
    #endregion

    #region Audio Mixer Controls
    public void MainVolumeControl(float volume)
    {
        musicMixer.SetFloat("Volume", volume);
    }
    public void SfxVolumeControl(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
    }
    public void OnMixerPanelExit() //save and exit
    {
        float mainValue, sfxValue;
        if(musicMixer.GetFloat("Volume",out mainValue))
        {
            PlayerPrefs.SetFloat("MainVolume", mainValue);
        }
        if(sfxMixer.GetFloat("Volume",out sfxValue))
        {
            PlayerPrefs.SetFloat("SfxVolume", sfxValue);
        }
       
        UIManager.Instance.DisablePanel("Setting");
    }
    #endregion
}
