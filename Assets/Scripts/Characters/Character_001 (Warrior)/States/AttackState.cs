using UnityEngine;

namespace Character_001.State
{
    public class AttackState : CraneFSM.Core.State
    {
        public AttackType[] attackTypes = new AttackType[3];
        public float attackCoolTime = 1f;
        
        public override void Enter()
        {
            base.Enter();

            if (sm.GetFloat("attackRestTime") <= 0)
                sm.SetInt("attackType", 0);   
                
            int attackType = sm.GetInt("attackType");
            
            attackTypes[attackType].Execute();
            
            attackType++;
            if (attackType > 2)
                attackType = 0;
            
            sm.SetInt("attackType", attackType);
            sm.SetFloat("attackCoolTime", attackCoolTime);
            sm.SetBool("isAttacking", true);
        }
    }
}
