using UnityEngine;

namespace Boss_01.State
{
    public class AttackState : CraneFSM.Core.State
    {
        public GameObject bulletPrefab;
        public AttackType attackType;
        
        override public void Enter()
        {
            base.Enter();

            attackType.Execute();
        }

        public override void Execute()
        {
            base.Execute();
        }

        override public void Exit()
        {
            base.Exit();
        }
    }
}