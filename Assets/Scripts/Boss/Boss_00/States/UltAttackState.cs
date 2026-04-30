using System.Collections;
using UnityEngine;

namespace Boss_01.State
{
    public class UltAttackState : CraneFSM.Core.State
    {
        public int randomTypeNum;
        public AttackType[] attackTypes;
        public EffectManager effectManager;
        
        [Header("Execute Info")] 
        public int round;
        public float delayTime;
        
        public override void Enter()
        {
            base.Enter();

            //randomTypeNum = new System.Random().Next(attackTypes.Length);
            
            //attackTypes[randomTypeNum].Execute();

            round = new System.Random().Next(4, 7);
            StartCoroutine(ExecuteAnimation());
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }

        IEnumerator ExecuteAnimation()
        {   
            int currentRound = 0;

            effectManager.ApplyEffect();
            while (currentRound < round)
            {
                if (sm.GetBool("ultIsRunning"))
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }

                yield return new WaitForSeconds(delayTime);
                print($"{currentRound + 1} / {round}");
                
                currentRound++;
                
                randomTypeNum = new System.Random().Next(attackTypes.Length);
                attackTypes[randomTypeNum].Execute();
                
                sm.SetBool("ultIsRunning", true);
            }

            while (sm.GetBool("ultIsRunning"))
            {
                yield return new WaitForSeconds(1);
            }

            effectManager.type = EffectManager.EffectType.None;
            sm.SetTrigger("doneULT");
        } 
    }

}