using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    public float followSpeed;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        Vector3 desideredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desideredPosition, followSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
