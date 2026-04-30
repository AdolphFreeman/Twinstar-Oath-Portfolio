using System;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if(!target) return;
        
        Vector3 position = target.position;
        position.z = -10;
        transform.position = position;
    }
}
