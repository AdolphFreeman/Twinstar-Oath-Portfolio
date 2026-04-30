using System;
using System.Collections;
using System.Collections.Generic;
using CraneFSM.Core;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boss_01
{
    public class AttackType_02 : AttackType
    {
        public GameObject prefab;
        
        [Header("Parameters")]
        public FloatParameter damage;
        public FloatParameter knockbackForce;

        [Header("Info")]
        public int bulletNum = 2;
        public float attackRestTime;
        public float startRadius;
        
        [Header("Expend State ")]
        public float expendRadius;
        public float expendSpeed;
        public float expendRotateSpeed;

        [Header("Revolve State")] 
        public float revolveTime;
        public float revolveSpeed;
        
        [Header("Contract State")]
        public float contractRadius;
        public float contractSpeed;
        public float contractRotateSpeed;
        
        public List<BulletInfo> bullets = new List<BulletInfo>();
        public List<HitPlayerInfo> hitPlayers = new List<HitPlayerInfo>();

        [System.Serializable]
        public class BulletInfo
        {
            public float angle;
            public GameObject origin;
            
            public BulletInfo(float angle, GameObject origin)
            {
                this.angle = angle;
                this.origin = origin;
            }
        }

        [System.Serializable]
        public class HitPlayerInfo
        {
            public GameObject origin;
            public float restTime;

            public HitPlayerInfo(GameObject origin, float restTime)
            {
                this.origin = origin;
                this.restTime = restTime;
            }
        }

        private void Update()
        {
            foreach (HitPlayerInfo playerInfo in new List<HitPlayerInfo>(hitPlayers))
            {
                playerInfo.restTime -= Time.deltaTime;
                if (playerInfo.restTime <= 0)
                    hitPlayers.Remove(playerInfo);
            }
        }

        public override void Execute()
        {
            base.Execute();
            
            float randomAngleDeg = Random.Range(0f, 180f) + (int)Random.Range(0, 90);
            float radians = Mathf.Deg2Rad * randomAngleDeg;
            
            AddBullet(radians);
            AddBullet(Mathf.Deg2Rad * (randomAngleDeg + 180f));

            StartCoroutine(ExecuteAnimation());
        }

        void AddBullet(float radians)
        {
            Vector3 direction = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians)) * startRadius;
            
            GameObject bullet = Instantiate(prefab, transform.position + direction, Quaternion.Euler(0, 0, radians * Mathf.Rad2Deg));
            bullets.Add(new BulletInfo(radians, bullet));
        }

        void CircleMovement(float rotateAngle, float radius)
        {
            foreach (BulletInfo bullet in bullets)
            {
                bullet.angle += rotateAngle;

                radius += Time.deltaTime * 1;
                    
                float x = Mathf.Sin(bullet.angle) * radius;
                float y = Mathf.Cos(bullet.angle) * radius;
                
                Vector3 position = transform.position +  new Vector3(x, y);
                
                bullet.origin.transform.position = position;
                    
                float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                bullet.origin.transform.rotation = Quaternion.Euler(0, 0, angle);
                
                ApplyDamage((Vector2)position, bullet.origin.transform.localScale, bullet.angle);
            }
        }

        void ApplyDamage(Vector2 center, Vector2 size, float angle)
        {
            LayerMask mask = LayerMask.GetMask("Player");
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, mask);

            foreach (Collider2D hit in hits)
            {
                if (!IsHitList(hit.gameObject))
                {
                    hitPlayers.Add(new HitPlayerInfo(hit.gameObject, attackRestTime));
                    PlayerCore core = hit.GetComponent<PlayerCore>();
                    
                    core.GetDamage(damage.value);
                    
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                    Vector2 knockbackDirection = (center - (Vector2)transform.position).normalized;
                    rb.AddForce(knockbackDirection * knockbackForce.value, ForceMode2D.Impulse);
                }   
            }
        }
        
        //===
        bool IsHitList(GameObject player)
        {
            foreach (HitPlayerInfo playerInfo in hitPlayers)
            {
                if (playerInfo.origin == player)
                    return true;
            }

            return false;
        }
        
        //===
        IEnumerator ExecuteAnimation()
        {
            float radius = 1;
            
            while (radius <= expendRadius)
            {
                radius += Time.deltaTime * expendSpeed;
                CircleMovement(expendRotateSpeed * Time.deltaTime, radius);
                
                yield return new WaitForSeconds(Time.deltaTime);
            }

            float state2Time = 0;
            while (state2Time < revolveTime)
            {
                CircleMovement(revolveSpeed * Time.deltaTime, radius);
                
                state2Time += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            while (radius > contractRadius)
            {
                radius -= Time.deltaTime * contractSpeed;
                CircleMovement(contractRotateSpeed * Time.deltaTime, radius);
                
                yield return new  WaitForSeconds(Time.deltaTime);
            }

            foreach (BulletInfo bullet in bullets)
            {
                Destroy(bullet.origin);
            }
            bullets.Clear();
            
            
            CraneFSM.StateMachine sm = GetComponent<CraneFSM.Core.State>().sm;
            if(sm)
                GetComponent<CraneFSM.Core.State>().sm.SetTrigger("doneAttack");
        }
    }
}