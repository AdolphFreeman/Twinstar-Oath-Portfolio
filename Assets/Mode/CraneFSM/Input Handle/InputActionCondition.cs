using System.Collections.Generic;
using UnityEngine;

namespace CraneFSM.Core
{
    [System.Serializable]
    public class InputActionCondition
    {
        public enum ConditionType
        {
            And, Or
        }
        
        public string inputActionName;
        public  ConditionType conditionType;
        public List<State> allowExecuteStates = new List<State>();
        public List<ConditionGroup> conditionGroup;

        public bool ConditionEvaluate()
        {
            if(conditionGroup.Count == 0) return true;
            
            List<bool> evaluates = new List<bool>();
            foreach (ConditionGroup conditionGroup in conditionGroup)
            {
                evaluates.Add(conditionGroup.Evaluate());
            }

            switch (conditionType)
            {
                case ConditionType.And:
                    if (evaluates.Contains(false))
                        return false;
                    return true;
                case ConditionType.Or:
                    if (evaluates.Contains(true))
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }

    [System.Serializable]
    public class ConditionGroup
    {
        public enum ConditionType
        {
            And, Or
        }
        
        public  ConditionType conditionType;
        public List<TransitionCondition> conditions = new List<TransitionCondition>();

        public bool Evaluate()
        {
            if(conditions.Count == 0) return true;
            
            List<bool> subConditionEvaluates = new List<bool>();
            foreach (TransitionCondition condition in conditions)
            {
                subConditionEvaluates.Add(condition.Evaluate());
            }
            
            switch (conditionType)
            {
                case ConditionType.And:
                    if(subConditionEvaluates.Contains(false)) 
                        return false;
                    return true;
                case ConditionType.Or:
                    if(subConditionEvaluates.Contains(true)) 
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}
