using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform target;
    public float Speed;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.LookAt(target);
            transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * Speed);
        }
    }
}
