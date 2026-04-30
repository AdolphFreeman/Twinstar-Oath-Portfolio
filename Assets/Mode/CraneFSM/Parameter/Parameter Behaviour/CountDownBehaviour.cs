using System;
using UnityEngine;

namespace CraneFSM.Core.ParameterBehaviour
{
    public class CountDownBehaviour : MonoBehaviour
    {
        public Parameter parameter;
        private float _currentSecond;

        private void Awake()
        {
            parameter = GetComponent<Parameter>();
        }

        private void Update()
        {
            switch (parameter)
            {
                case IntegerParameter integer:
                    if(integer.value <= 0) break;
                    
                    _currentSecond += Time.deltaTime;
                    if(_currentSecond >= 1f)
                    {
                        integer.value -= (int)_currentSecond;
                        _currentSecond = 0f;
                    }
                    break;
                case FloatParameter floatParam:
                    if(floatParam.value <= 0) break;
                    
                    floatParam.value -= Time.deltaTime;
                    break;
            }
        }
    }   
}
