using CraneFSM.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character_000
{ 
    public class InputManager : MonoBehaviour
    {
        public InputActionHandle inputActionHandle;

        public void AttackAction(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && inputActionHandle.GetEvaluate("Attack Action"))
            {
                inputActionHandle.stateMachine.SetTrigger("isAttack");
            }
        }

        public void MoveAction(InputAction.CallbackContext ctx)
        {
            Vector2 moveDirection =  ctx.ReadValue<Vector2>();
        
            inputActionHandle.stateMachine.SetVector2("moveDirectionAxis", moveDirection);
            inputActionHandle.stateMachine.SetVector2("moveDirection", 
                new Vector2(Mathf.Abs(moveDirection.x) <= 0.25f ? 0 : Mathf.Sign(moveDirection.x),
                    Mathf.Abs(moveDirection.y) <= 0.25f ? 0 : Mathf.Sign(moveDirection.y)));
        }
    }
}
