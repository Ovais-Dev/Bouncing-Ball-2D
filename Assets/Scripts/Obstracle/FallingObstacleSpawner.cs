using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    [Header ("Objects")]
    [SerializeField] GameObject fallObject; //fallinng controller
    [SerializeField] Transform obstacleHolder;
    [SerializeField] float initialFallingSpeed = 5f;
    [SerializeField] float maxFallSpeed = 100f;
    List<GameObject> fallList;

    [Header("Position Settings")]
    public Vector2 xLength;

    [Header("Time Settings")]
    [SerializeField] float timeToSpawn = 7f;
    [SerializeField] float minTimeToSpawn = 0.3f;
    [HideInInspector] [SerializeField] float negativeTimeVariation;
    [HideInInspector] [SerializeField] float positiveTimeVariation;
   // float minTimeValue;
    float timer;

    bool spawning = true;
    public bool levelReached = false;

    float initialSpawnTime;
    float currentFallSpeed;
    private void Awake()
    {
        fallList = new List<GameObject>();
        GameManager.Instance.gameOverAction += () => spawning = false;
        GameManager.Instance.playAction += () => spawning = true;
        GameManager.Instance.menuAction += () => { DisableAllObstacle(); spawning = false; };

        
    }
    // Start is called before the first frame update
    void Start()
    {
        int randVar = Random.Range(0, 2);
        UpdateSpawnTime(timeToSpawn);
        timer = GetTime(timeToSpawn, (randVar == 1 ? negativeTimeVariation : positiveTimeVariation));
        initialSpawnTime = timeToSpawn;
        currentFallSpeed = initialFallingSpeed;
        //minTimeValue = minTimePercentage * 0.01f * timeToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelReached) return;
        if (!spawning) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnFall();
            int randVar = Random.Range(0, 2);
            timer = GetTime(timeToSpawn, (randVar == 1 ? negativeTimeVariation : positiveTimeVariation));
        }
    }

    void UpdateSpawnTime(float newTime)
    {
        timeToSpawn = newTime;
        negativeTimeVariation = GetValueFromPercentage(timeToSpawn, -30f);
        positiveTimeVariation = GetValueFromPercentage(timeToSpawn, 15f);
    }

    void SpawnFall()
    {
        GameObject newObs = GetObstracle();
        newObs.SetActive(true);

        Vector2 pos = new Vector2(Random.Range(xLength.x,xLength.y),0);
        newObs.transform.position = pos;
    }
    GameObject GetObstracle()
    {
        foreach (var obj in fallList)
        {
            if (!obj.activeSelf) return obj;
        }
        GameObject obs = Instantiate(fallObject, obstacleHolder);
        obs.GetComponent<FallingController>().SetProperties(this,currentFallSpeed);
       // obs.GetComponent<FallingController>().fallingSpeed = currentFallSpeed;
        fallList.Add(obs);
        obs.SetActive(false);
        return obs;
    }

    float GetValueFromPercentage(float value, float percentage)
    {
        return value * percentage * 0.01f;
    }




    void DisableAllObstacle()
    {
        foreach (var obs in fallList)
        {
            if (!obs.activeSelf) continue;
            obs.SetActive(false);
        }
    }

    float GetTime(float timeToSpawn, float timeVariation)
    {
        return timeToSpawn + Random.Range(0, timeVariation);
    }

    public GameObject GetObstacle()
    {
        GameObject obs = spawner.GetObstracle();
        return obs;
    }

    #region Upgrades
    public void TimeDecrease(float percentage)
    {
        float newSpawnTime = timeToSpawn;
        newSpawnTime -= percentage * 0.01f * newSpawnTime;

        if (newSpawnTime <= minTimeToSpawn)
        {
            return;
        }
        UpdateSpawnTime(newSpawnTime);
    }
    public void ResetSpawnTime()
    { 
        UpdateSpawnTime(initialSpawnTime);
    }
    public void IncreaseFallSpeed(float percentage)
    {
        float newSpeed = currentFallSpeed;
        newSpeed += percentage * 0.01f * newSpeed;
        
        if (newSpeed >= maxFallSpeed)
        {
            return;
        }
        foreach (var fallObj in fallList)
        {
            fallObj.GetComponent<FallingController>().fallingSpeed = newSpeed;
        }

        currentFallSpeed = newSpeed;
    }
    public void ResetFallSpeed()
    {
        // float newSpeed = fallList[0].GetComponent<FallingController>().fallingSpeed;
        if (fallList.Count <= 0) return;
        foreach (var fallObj in fallList)
        {
            fallObj.GetComponent<FallingController>().fallingSpeed = initialFallingSpeed;
        }
    }
    #endregion
}
