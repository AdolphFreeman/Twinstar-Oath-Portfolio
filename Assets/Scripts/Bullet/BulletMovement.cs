using System;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    public Vector2 moveDirection;

    private void Update()
    {
        Vector3 position = transform.position;
        position += (Vector3)moveDirection * Time.deltaTime * speed;
        transform.position = position;
    }
}
