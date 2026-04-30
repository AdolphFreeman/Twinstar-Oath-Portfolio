using System.Collections;
using CraneFSM;
using UnityEngine;

namespace Character_001
{
    public class AttackType02 : AttackType
    {
        public StateMachine sm;
        public GameObject sword;
        public float distance = 2.5f;
        public float moveTime = 5f;
        
        public override void Execute()
        {
            sword.transform.localPosition = Vector3.zero;
            Vector2 faceDirection = sm.GetVector2("faceDirection");

            float startAngle = Vector2.SignedAngle(Vector2.up, faceDirection);
            StartCoroutine(ExecuteAnimation(startAngle, faceDirection, sword.transform));
        }

        IEnumerator ExecuteAnimation(float startAngle, Vector2 faceDirection, Transform target)
        {
            PlayerCore playerCore = sm.GetComponentInParent<PlayerCore>();
            
            target.gameObject.SetActive(true);
            target.rotation = Quaternion.Euler(0f, 0f, startAngle);

            Vector3 startPosition = playerCore.Center();
            Vector3 endPosition = startPosition + (Vector3)faceDirection * distance;

            float duration = moveTime; // 👉 攻擊速度（可以調）
            float t = 0;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;

                endPosition = playerCore.Center() + (Vector3)faceDirection * distance;
                target.position = Vector3.Lerp(startPosition, endPosition, t);

                yield return null; // 👉 正確寫法
            }

            // 👉 保證最後位置準確
            target.position = endPosition;

            yield return new WaitForSeconds(0.05f); // 可選：停一下
            
            target.position = startPosition;
            target.gameObject.SetActive(false);

            sm.SetFloat("attackRestTime", 5);
            sm.SetBool("isAttacking", false);
        }
    }   
}
