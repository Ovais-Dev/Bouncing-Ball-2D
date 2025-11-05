using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [Header("Obstacle Object")]
    [SerializeField] GameObject obstacle;
    [SerializeField] float obstacleMoveSpeed;
    [SerializeField] float maxObstacleMoveSpeed = 80f;
    [SerializeField] Transform objHolder;
    List<GameObject> obstacleList;
    float initialObstacleMoveSpeed;
    [Header("Time Settings")]
    [SerializeField] float timeToSpawn;
    [SerializeField] float minSpawnTime = 0.3f;
    [HideInInspector][SerializeField] float negativeTimeVariation;
    [HideInInspector][SerializeField] float positiveTimeVariation;
    float timer;
    //float minSpawnTime;
    float initialSpawnTime;
    [Header("Position Settings")]
    [SerializeField] float yPos;
    [SerializeField] float leftXPos;
    [SerializeField] float rightXPos;

    bool spawning = true;


    private void Awake()
    {   
        obstacleList = new List<GameObject>();
        GameManager.Instance.gameOverAction += () => spawning = false;
        GameManager.Instance.playAction += () => spawning = true;
        GameManager.Instance.menuAction += ()=> { DisableAllObstacle(); spawning = false; };
    }
    private void Start()
    {
        int randVar = Random.Range(0, 2);
        UpdateSpawnTime();
        timer = GetTime(timeToSpawn, (randVar==1?negativeTimeVariation:positiveTimeVariation));
       // minSpawnTime = minSpawnPercentage * 0.01f * timeToSpawn;
        initialSpawnTime = timeToSpawn;
        initialObstacleMoveSpeed = obstacleMoveSpeed;

    }
    public void SetSpawnTime(float percentage)
    {
        float newTime = timeToSpawn + GetValueFromPercentage(timeToSpawn,percentage);
        if (newTime <= minSpawnTime) return;
        timeToSpawn = newTime;
    }
    public void SetObsMoveSpeed(float percentage)
    {
        float newSpeed = obstacleMoveSpeed + GetValueFromPercentage(obstacleMoveSpeed, percentage);
        if (newSpeed >= maxObstacleMoveSpeed) return;
        obstacleMoveSpeed = newSpeed;
    }
    void UpdateSpawnTime()
    {
        negativeTimeVariation = GetValueFromPercentage(timeToSpawn, -30f);
        positiveTimeVariation = GetValueFromPercentage(timeToSpawn, 15f);
    }
    public float GetCurrentSpawnTime() => timeToSpawn;
    float GetValueFromPercentage(float value, float percentage)
    {
        return value * percentage * 0.01f;
    }
    void DisableAllObstacle()
    {
        foreach(var obs in obstacleList)
        {
            if (!obs.activeSelf) continue;
            obs.SetActive(false);
        }
    }
    void Update()
    {
        if (!spawning) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnObstracle();
            int randVar = Random.Range(0, 2);
            timer = GetTime(timeToSpawn, (randVar == 1 ? negativeTimeVariation : positiveTimeVariation));
        }
    }
    void SpawnObstracle()
    {
        GameObject newObs = GetObstracle();
        newObs.SetActive(true);
        newObs.GetComponent<Obstacle>().SetMoveSpeed(obstacleMoveSpeed);
        //set new obs property like speed, it can be once when created means during Instantiation
        int rand = Random.Range(0, 2);
        if (rand == 1) // left
        {
            Vector2 pos = new Vector2(leftXPos, yPos);
            newObs.transform.position = pos;
            newObs.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else
        {
            Vector2 pos = new Vector2(rightXPos, yPos);
            newObs.transform.position = pos;
            newObs.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        // set speed, min y pos to disable
    }
    public GameObject GetObstracle()
    {
        foreach(var obj in obstacleList)
        {
            if (!obj.activeSelf) return obj;
        }
        GameObject obs = Instantiate(obstacle, objHolder);
        obs.GetComponent<Obstacle>().SetYLimits(-yPos);
        obstacleList.Add(obs);
        obs.SetActive(false);
        return obs;
    }
    float GetTime(float timeToSpawn, float timeVariation)
    {
        return timeToSpawn + Random.Range(0, timeVariation);
    }
    public void ResetSpawnTime()
    {
        timeToSpawn = initialSpawnTime;
    }
    public void ResetMoveSpeed()
    {
        obstacleMoveSpeed = initialObstacleMoveSpeed;
    }
}
