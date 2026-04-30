using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CraneFSM.Core
{
    public class InputActionHandle : MonoBehaviour
    {
        public StateMachine stateMachine;
        public List<InputActionCondition> inputActionConditions = new List<InputActionCondition>();

        private Dictionary<string, Func<bool>> _conditions = new Dictionary<string, Func<bool>>();
        
        private void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            foreach (InputActionCondition condition in inputActionConditions)
            {
                _conditions.Add(condition.inputActionName, () => Evaluate(condition));
            }
        }

        public bool GetEvaluate(string inputActionName)
        {
            if(_conditions.ContainsKey(inputActionName))
                return _conditions[inputActionName]();
            return false;
        }
        
        public bool Evaluate(InputActionCondition condition)
        {
            return condition.allowExecuteStates.Contains(stateMachine.currentState) &&
                condition.ConditionEvaluate();
        }
    }   
}
