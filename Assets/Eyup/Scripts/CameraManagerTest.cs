using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerTest : MonoBehaviour
{

    [SerializeField] Vector3 offset;
    [SerializeField] Transform obj;
    float smoothTime = .125f;

    public void StartFollowing(Transform a,float t)
    {
        
        Vector3 finalPosition = a.position + offset;
        Vector3 smoothPosition = Vector3.Slerp(transform.position, finalPosition, t);
        transform.position = smoothPosition;


    }

    private void LateUpdate()
    {

        StartFollowing(obj, smoothTime);

    }


}
