using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Range(0.1f,10f)]
    public float RotationAngle = 1.5f;
    void Update()
    {
        transform.Rotate(Vector3.up, RotationAngle * Time.deltaTime);
    }
}
