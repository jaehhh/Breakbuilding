using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject followingTarget;

    private void Start()
    {
        followingTarget = GameObject.Find("Player");
    }

    private void LateUpdate()
    {
        transform.position = followingTarget.transform.position + new Vector3(0, 2.3f, -10);
    }
}
