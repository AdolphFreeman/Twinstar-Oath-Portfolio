using System.Collections;
using CraneFSM;
using UnityEngine;

namespace Character_001
{
    public class AttackType00 : AttackType
    {
        public StateMachine sm;
        public GameObject sword;
        public float rotateAngle = 25;
        
        public override void Execute()
        {
            sword.transform.localPosition = Vector3.zero;
            Vector2 faceDirection = sm.GetVector2("faceDirection");
            float startAngle = Vector2.SignedAngle(Vector2.up, faceDirection);

            print(startAngle);
            StartCoroutine(ExecuteAnimation(startAngle, sword.transform));
        }

        IEnumerator ExecuteAnimation(float startAngle, Transform target)
        {
            PlayerCore playerCore = sm.GetComponentInParent<PlayerCore>();
            
            float range = rotateAngle;
            float halfRange = range / 2f;

            target.position = playerCore.Center();
            float currentAngle = startAngle - halfRange;
            float endAngle = startAngle + halfRange;

            target.gameObject.SetActive(true);
            target.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            while (currentAngle <= endAngle)
            {
                currentAngle += 3;
                target.position = playerCore.Center();
                target.rotation = Quaternion.Euler(0f, 0f, currentAngle);
                
                yield return new WaitForSeconds(0.01f);
            }
            target.gameObject.SetActive(false);
            
            sm.SetFloat("attackRestTime", 5);
            sm.SetBool("isAttacking", false);
        }
    }   
}
