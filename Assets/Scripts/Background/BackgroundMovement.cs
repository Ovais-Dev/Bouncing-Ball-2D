using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] Transform bg1, bg2;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float maxSpeedPercentage = 300f;
    [SerializeField] float yOffsets;
    Transform bgSelected, bgNext;
    Camera cam;
    float resHeight;

    float maxSpeed;
    float initialSpeed;
    private void Start()
    {
        cam = Camera.main;
        resHeight = cam.orthographicSize;
        bgSelected = bg1;
        bgNext = bg2;

        bgSelected.localPosition = Vector3.zero + Vector3.up * yOffsets;
        bgNext.localPosition = bgSelected.localPosition + Vector3.up * 2f * resHeight;

        maxSpeed = moveSpeed * 0.01f * maxSpeedPercentage;
        //for resetting
        initialSpeed = moveSpeed;
    }
    private void Update()
    {
        bg1.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
        bg2.localPosition += Vector3.down * moveSpeed * Time.deltaTime;

        if (bgSelected.localPosition.y <= -2 * resHeight)
        {
            bgSelected.localPosition = bgNext.localPosition + Vector3.up * 2f * resHeight;
            Transform tempTrans = bgSelected;
            bgSelected = bgNext;
            bgNext = tempTrans;
        }
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void SetMoveSpeed(float speed)
    {
        if (speed >= maxSpeed)
        {
            return;
        }
        moveSpeed = speed;
    }
    public void ResetSpeed()
    {
        moveSpeed = initialSpeed;
    }
}
