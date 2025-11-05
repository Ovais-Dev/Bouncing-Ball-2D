using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Movement Control")]
    [SerializeField] float moveSpeed;

    float yLimit;
    private void Update()
    {
        if (transform.position.y > yLimit)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
       
    }
    public void SetYLimits( float yLimit)
    {
        this.yLimit = yLimit;
    }
}
