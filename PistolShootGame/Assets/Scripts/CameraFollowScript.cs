using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [Header("Target")]
    public GameObject targetToFollow;
    public bool isFollowingTarget;

    [Header("Distance")]
    public Vector3 distanceBetweenTargetAndCamera;
    public Vector3 gameEndLookAboveDistance;

    private void Start()
    {
        SetIsFollow(true);
    }
    private void SetIsFollow(bool a)
    {
        isFollowingTarget = a;
    }


    private void FixedUpdate()
    {
        if (isFollowingTarget)
        {
            FollowTarget();
        }
        else
        {
            LookFromAbove();
        }

    }
    private void FollowTarget()
    {
        transform.position = targetToFollow.transform.position + distanceBetweenTargetAndCamera;
    }
    private void LookFromAbove()
    {
        transform.position = Vector3.Lerp(this.transform.position, gameEndLookAboveDistance, Time.deltaTime);
    }
}
