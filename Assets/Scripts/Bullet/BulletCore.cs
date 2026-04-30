using System;
using UnityEngine;

public class BulletCore : MonoBehaviour
{
    public float lifeTime;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
            Destroy(gameObject);
    }
}
