using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingController : MonoBehaviour
{
    FallingObstacleSpawner fallSpawner;

    [Header("Obstacles")]
    public Vector2 direction;
    public float fallingSpeed = 5f;

    public float yPos = 6f;
    public float delaySpawnTimer;
    float timer;
    bool enableTimer;

    public float maxSpeed;

    private void Start()
    {
        //maxSpeed = maxFallingSpeedPercentage * 0.01f * fallingSpeed;
    }
    private void OnEnable()
    {
        enableTimer = true;
        timer = delaySpawnTimer;
    }
    private void Update()
    {
        if (!enableTimer) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enableTimer = false;
            Obstacle();
            gameObject.SetActive(false);
        }

    }
    public void Obstacle()
    {
        GameObject obs = fallSpawner.GetObstacle();
        obs.transform.up = direction;
        obs.GetComponent<Obstacle>().SetMoveSpeed(fallingSpeed);
        obs.GetComponent<Obstacle>().SetYLimits(-yPos);
        obs.transform.position = transform.position + Vector3.up * yPos;
        obs.SetActive(true);
        //return obs;
    }

    public void SetProperties(FallingObstacleSpawner fall,float fallSpeed)
    {
        this.fallSpawner = fall;
        fallingSpeed = fallSpeed;
    }
}

