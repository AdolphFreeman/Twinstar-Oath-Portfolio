using System;
using CraneFSM;
using UnityEngine;

public class Core : MonoBehaviour
{
    public StateMachine sm;

    public virtual void Awake()
    {
        sm = GetComponentInChildren<StateMachine>();
    }

    public virtual void GetDamage(float damage)
    {
        if(damage <= 0) return;

        float tempHP = sm.GetFloat("hp");
        sm.SetFloat("hp", tempHP - damage);
    }
}
