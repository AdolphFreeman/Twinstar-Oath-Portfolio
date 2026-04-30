using System;
using UnityEngine;

public class PlayerCore : Core
{
    public bool showGizmos;
    [SerializeField]private Vector2 centerOffset;

    public Vector3 Center()
    {
        return transform.position + (Vector3)centerOffset;
    }
    
    //===
    private void OnDrawGizmos()
    {
        if(!showGizmos) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Center(), 0.25f);
    }
}