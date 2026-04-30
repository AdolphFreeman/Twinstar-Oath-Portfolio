using System;
using UnityEngine;

namespace Boss_01
{
    public class AttackTypeEffect_00 : MonoBehaviour
    {
        public float delayTime = 5;
        private float _currentTime;
        
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= delayTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
