using System;
using UnityEngine;

namespace Character_00.State
{
    public class AttackState : CraneFSM.Core.State
    {
        [Header("State Info")] 
        public float scale;
        public GameObject bulletPrefab;
        
        public override void Enter()
        {
            base.Enter();

            Vector2 dir = sm.GetVector2("faceDirection").normalized;

            Vector3 position = GetComponentInParent<PlayerCore>().Center()+ (Vector3)dir * scale;
            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

            float angle = Vector2.SignedAngle(Vector2.up, sm.GetVector2("faceDirection"));
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector2 dir1 = sm.GetVector2("faceDirection").normalized;
            bullet.GetComponent<BulletMovement>().moveDirection = dir;

            sm.SetFloat("attackCoolTime", 1f);
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
