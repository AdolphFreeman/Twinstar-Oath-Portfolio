using UnityEngine;
using UnityEngine.InputSystem;

namespace Character_01.State
{
    public class IdleState : CraneFSM.Core.State
    {
        //=== Input Action ===
        public void Attack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && isRunning)
            {
                sm.SetTrigger("isAttack");
            }
        }
    }
}