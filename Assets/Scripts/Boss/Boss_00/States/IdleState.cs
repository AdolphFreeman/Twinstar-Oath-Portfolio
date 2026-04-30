using UnityEngine;
using Random = System.Random;

namespace Boss_01.State
{
    public class IdleState : CraneFSM.Core.State
    {
        override public void Enter()
        {
            base.Enter();

            sm.SetFloat("switchTime", new Random().Next(5, 10));
            sm.SetInt("attackType", new Random().Next(2));  
        }

        public override void Execute()
        {
            base.Execute();
            
            float currentSwitchTime = sm.GetFloat("switchTime") - Time.deltaTime;
            sm.SetFloat("switchTime", currentSwitchTime);
        }

        override public void Exit()
        {
            base.Exit();
        }
    }
   
}