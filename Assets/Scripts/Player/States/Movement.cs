using CraneFSM.Core;
using Spine.Unity;
using UnityEngine;

public class Movement : State
{
    public SkeletonMecanim skeletonMecanim;
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        Vector2 moveDirection = sm.GetVector2("moveDirection");
        Vector2 faceDirection = sm.GetVector2("faceDirection");
        
        bool canMove = moveDirection != Vector2.zero;
        sm.SetBool("canMove", canMove);
        
        if (canMove && faceDirection != moveDirection)
        {
            sm.SetVector2("faceDirection", moveDirection);
            
            float scaleX = Mathf.Abs(skeletonMecanim.skeleton.ScaleX);
            //float scaleY = Mathf.Abs(skeletonMecanim.skeleton.ScaleY);
            
            scaleX *= Mathf.Sign(moveDirection.x);
            //scaleY *= Mathf.Sign(moveDirection.y);
            
            skeletonMecanim.skeleton.ScaleX = scaleX;
            //skeletonMecanim.skeleton.SetLocalScale(new Vector2(scaleY, scaleX));
        }
        
        Vector3 position = sm.origin.position;
        position += (Vector3)sm.GetVector2("moveDirectionAxis") * sm.GetFloat("moveSpeed") * Time.deltaTime;
        sm.origin.position = position;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
