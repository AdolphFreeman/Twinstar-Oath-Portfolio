using CraneFSM;
using UnityEngine;

namespace Character_001
{
    public class AnimationHandle : MonoBehaviour
    {
        public Animator animator;
        public StateMachine stateMachine;
        
        public void DoneAttack()
        {
            animator.SetLayerWeight(1, 0);
            stateMachine.SetBool("isAttacking", false);
        }
    }
}
