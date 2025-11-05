using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllLevelHard : MonoBehaviour
{

    private static ControllLevelHard _instance;
    public static ControllLevelHard Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ControllLevelHard>();
            }
            return _instance;
        }
    }

    [Header("Levels Challenges Manager")]
    BackgroundMovement[] backgrounds;
    //[SerializeField] float maxLevelPercentage = 100f;

    Spawner spawner;
    FallingObstacleSpawner fallSpawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        fallSpawner = FindObjectOfType<FallingObstacleSpawner>();
        backgrounds = FindObjectsOfType<BackgroundMovement>();
    }

    public void UpdateLevelChallenge(int level) // start from zero
    {
        
        float percentage;
        switch (level)
        {
            case 0:
                spawner.ResetSpawnTime();
                spawner.ResetMoveSpeed();
                ResetEnvironmentSpeed();
                fallSpawner.levelReached = false;
                fallSpawner.ResetFallSpeed();
                fallSpawner.ResetSpawnTime();
                break;
            case 1:
                percentage = -10f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                break;
            case 2:
                
                percentage = -20f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                break;
            case 3:
                percentage = -10f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                break;
            case 4:
                percentage = -10f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                fallSpawner.levelReached = true;
                break;
            case 5:
                // use dropping arrows
                percentage = -10f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                fallSpawner.IncreaseFallSpeed(10f);
                break;
            case 6:
                percentage = -10f;
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                EnvironemntSpeedIncreaser(5f);
                fallSpawner.TimeDecrease(-10f);
                fallSpawner.IncreaseFallSpeed(10f);
                break;
            case 7:
            case 8:
                percentage = 5f;
                spawner.SetSpawnTime(percentage);
                fallSpawner.TimeDecrease(-20f);
                fallSpawner.IncreaseFallSpeed(10f);
                ThemeManager.Instance.GetTheme("Red Mountain").unlocked = true;
                break;
            case 9:
                percentage = 5f;
                spawner.SetSpawnTime(percentage);
                fallSpawner.TimeDecrease(-10f);
                fallSpawner.IncreaseFallSpeed(10f);
                break;
            case 10:
            case 11:
            case 12:
                fallSpawner.IncreaseFallSpeed(10f);
                break;
            case 13:
                ThemeManager.Instance.GetTheme("Green Forest").unlocked = true;
                break;
            default:
                percentage = -10f;
                EnvironemntSpeedIncreaser(5f);
                fallSpawner.TimeDecrease(-10f);
                fallSpawner.IncreaseFallSpeed(10f);
                spawner.SetSpawnTime(percentage);
                spawner.SetObsMoveSpeed(5f);
                break;
        }
    }
    //float GetFinalValueWithPercentageIncrease(float value, float percentage)
    //{
    //    return value + value * percentage * 0.01f;
    //}    //float GetFinalValueWithPercentageIncrease(float value, float percentage)
    //{
    //    return value + value * percentage * 0.01f;
    //}
    void EnvironemntSpeedIncreaser(float percentage)
    {
        foreach (var back in backgrounds)
        {
            float value = back.GetMoveSpeed();
            value += IncreaseValueByPercentage(value, percentage);
            back.SetMoveSpeed(value);
        }
    }
    void ResetEnvironmentSpeed()
    {
        foreach (var back in backgrounds)
        {
            back.ResetSpeed();
        }
    }
    float IncreaseValueByPercentage(float value , float percentage)
    {
        return value * 0.01f * percentage;
    }
}
