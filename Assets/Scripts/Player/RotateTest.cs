using System;
using Spine.Unity;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private void Update()
    {
        float axisX = Input.GetAxis("Horizontal");
        
        if(axisX != 0)
            skeletonAnimation.skeleton.ScaleX = Mathf.Sign(axisX);
    }
}
