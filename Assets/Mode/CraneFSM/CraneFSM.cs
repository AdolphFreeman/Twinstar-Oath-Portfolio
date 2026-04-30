using System;
using System.Collections.Generic;
using System.Linq;
using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]public State currentState;

        public Transform origin;
        public State initialState;

        private Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        public Transition[] AnyStateTransitions;

        private void Awake()
        {
            foreach (Parameter parameter in GetComponentsInChildren<Parameter>())
            {
                if (!_parameters.ContainsKey(parameter.parameterName))
                {
                    _parameters.Add(parameter.parameterName, parameter);
                }
            }

            GameObject anyState = transform.GetChild(1).gameObject;
            AnyStateTransitions = anyState.GetComponentsInChildren<Transition>();

            foreach (Transition transition in AnyStateTransitions)
            {
                transition.transitionName = $"Any State => {transition.transitionState.name}";
            }
        }

        private void Start()
        {
            if (initialState != null)
            {
                SwitchState(initialState);
            }
        }

        private void Update()
        {
            foreach (Transition transition in AnyStateTransitions)
            {
                transition.name = $"{transition.transitionName}: {transition.Evaluate()}";
                if (transition.Evaluate())
                {
                    if(currentState == transition.transitionState)
                        continue;
    
                    transition.OnTransition();
                    SwitchState(transition.transitionState);    
                    break;
                }
            }
            
            
            if (_parameters.ContainsKey("runningTime"))
            {
                float currentRunningTime = GetFloat("runningTime");
                currentRunningTime += Time.deltaTime;
                SetFloat("runningTime", currentRunningTime);
            }
            
            currentState?.Execute();
        }

        //===---- 
        public void SwitchState(State newState)
        {
            if (newState == null) return;

            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            SetFloat("runningTime", 0);
            
            currentState.Enter();
        }
        
        //===---- Parameter ---===
        public int GetInt(string parameterName)
        {
            if(_parameters.ContainsKey(parameterName))
                return (int)_parameters[parameterName].Value;
            Debug.LogError("Parameter " + parameterName + " not found");
            return 0;
        }
        
        public float GetFloat(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName))
                return (float)_parameters[parameterName].Value;
            Debug.LogError("Parameter " + parameterName + " not found");
            return 0;
        }

        public bool GetBool(string parameterName)
        {
            if(_parameters.ContainsKey(parameterName))
                return (bool) _parameters[parameterName].Value;
            Debug.LogError("Parameter " + parameterName + " not found");
            return false;
        }

        public Vector2 GetVector2(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName))
                return  (Vector2)_parameters[parameterName].Value;
            Debug.LogError("Parameter " + parameterName + " not found");
            return Vector2.zero;
        }

        public Vector3 GetVector3(string parameterName)
        {
            if(_parameters.ContainsKey(parameterName))
                return (Vector3)_parameters[parameterName].Value;
            Debug.LogError("Parameter " + parameterName + " not found");
            return  Vector3.zero;
        }

        public bool GetTrigger(string parameterName)
        {
            if(_parameters.ContainsKey(parameterName))
                return ((TriggerClass)_parameters[parameterName].Value).isTrigger;
            Debug.LogError("Parameter " + parameterName + " not found");
            return false;
        }

        public void SetInt(string parameterName, int value)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) && parameter is IntegerParameter intParameter)
            {
                intParameter.value = value;
                intParameter.RefreshValue();
                return;
            }

            Debug.LogError("Integer parameter " + parameterName + " not found");
        }
        
        public void SetFloat(string parameterName, float value)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) && parameter is FloatParameter floatParameter)
            {
                floatParameter.value = value;
                floatParameter.RefreshValue();
                return;
            }

            Debug.LogError("Float parameter " + parameterName + " not found");
        }

        public void SetBool(string parameterName, bool value)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) && parameter is BoolParameter boolParameter)
            {
                boolParameter.value = value;
                boolParameter.RefreshValue();
                return;
            }

            Debug.LogError("Bool parsameter " + parameterName + " not found");
        }

        public void SetVector2(string parameterName, Vector2 value)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) && parameter is Vector2Parameter vector2Parameter)
            {
                vector2Parameter.value = value;
                vector2Parameter.RefreshValue();
                return;
            }

            Debug.LogError("Vector2 parameter " + parameterName + " not found");
        }

        public void SetVector3(string parameterName, Vector3 value)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) && parameter is Vector3Parameter vector3Parameter)
            {
                vector3Parameter.value = value;
                vector3Parameter.RefreshValue();
                return;
            }

            Debug.LogError("Vector3 parameter " + parameterName + " not found");
        }

        public void SetTrigger(string parameterName)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) &&
                parameter is TriggerParameter triggerParameter)
            {
                triggerParameter.value.Set();
                return;
            }
            
            Debug.LogError("Trigger parameter " + parameterName + " not found");
        }
        
        public void ResetTrigger(string parameterName)
        {
            if (_parameters.TryGetValue(parameterName, out Parameter parameter) &&
                parameter is TriggerParameter triggerParameter)
            {
                triggerParameter.value.Reset();
                return;
            }
            
            Debug.LogError("Trigger parameter " + parameterName + " not found");
        }
    }
}
