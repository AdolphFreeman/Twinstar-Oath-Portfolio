using System;
using CraneFSM.Core;
using UnityEngine;

public class CraneFSM_SyncAnimatorParameter : MonoBehaviour
{
    public enum SyncDirection
    {
        AnimatorToParameter,
        ParameterToAnimator
    }

    public Animator animator;
    public Parameter targetParameter;
    public SyncDirection syncDirection;

    private void Awake()
    {
        targetParameter = GetComponent<Parameter>();
    }

    private void Update()
    {
        if (animator == null || targetParameter == null)
            return;

        switch (syncDirection)
        {
            case SyncDirection.AnimatorToParameter:
                SyncFromAnimator();
                break;
            case  SyncDirection.ParameterToAnimator:
                SyncToAnimator();
                break;
        }
    }

    private void SyncFromAnimator()
    {
        string parameterName = targetParameter.parameterName;
        switch (targetParameter)
        {
            case IntegerParameter intParam:
                intParam.value = animator.GetInteger(parameterName);
                break;
            case  FloatParameter floatParam:
                floatParam.value = animator.GetFloat(parameterName);
                break;
            case BoolParameter boolParam:
                boolParam.value = animator.GetBool(parameterName);
                break;
            case Vector2Parameter vector2Param:
                Vector2 vector2Value = new Vector2(animator.GetFloat(parameterName + "X"), 
                    animator.GetFloat(parameterName + "Y"));
                vector2Param.value = vector2Value;
                break;
            case  Vector3Parameter vector3Param:
                Vector3 vector3Value = new Vector3(animator.GetFloat(parameterName + "X"), 
                    animator.GetFloat(parameterName + "Y"),
                    animator.GetFloat(parameterName + "Z"));
                vector3Param.value = vector3Value;
                break;
        }
    }

    private void SyncToAnimator()
    {
        string parameterName = targetParameter.parameterName;
        switch (targetParameter)
        {
            case IntegerParameter intParam:
                animator.SetInteger(parameterName, intParam.value);
                break;
            case  FloatParameter floatParam:
                animator.SetFloat(parameterName, floatParam.value);
                break;
            case BoolParameter boolParam:
                animator.SetBool(parameterName, boolParam.value);
                break;
            case Vector2Parameter vector2Param:
                animator.SetFloat(parameterName + "X", vector2Param.value.x);
                animator.SetFloat(parameterName + "Y", vector2Param.value.y);
                break;
            case  Vector3Parameter vector3Param:
                animator.SetFloat(parameterName + "X", vector3Param.value.x);
                animator.SetFloat(parameterName + "Y", vector3Param.value.y);
                animator.SetFloat(parameterName + "Z", vector3Param.value.z);
                break;
            case TriggerParameter triggerParam:
                if(triggerParam.value.isTrigger)
                {
                    animator.SetTrigger(parameterName);
                }
                else
                {
                    animator.ResetTrigger(parameterName);
                }
               break;
        }
    }
}
