using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// you need to change the class name to your code file name

public class CameraFollow : MonoBehaviour
{
    public Transform tragetObject;

    public Vector3 cameraOffset;

    public float smoothFactor = 0.5f;

    public bool lookAtTraget = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - tragetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = tragetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtTraget)
        {
            transform.LookAt(tragetObject);
        }
    }
}