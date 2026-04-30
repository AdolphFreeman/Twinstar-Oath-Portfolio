using System.Collections.Generic;
using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    
    [System.Serializable]
    public class TransitionConditionGroup
    {
        public float exitTime;
        public TransitionLogic logic;
        public TransitionCondition[] conditions;

        public bool Evaluate()
        {
            if (conditions == null) return false;
            
            List<bool> evaluates = new List<bool>();
            foreach (TransitionCondition condition in conditions)
            {
                evaluates.Add(condition.Evaluate());
            }
            
            switch (logic)
            {
                case TransitionLogic.And:
                    if (evaluates.Contains(false))
                        return false;
                    return true;
                case TransitionLogic.Or:
                    if (evaluates.Contains(true))
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }

    public enum TransitionLogic
    {
        And, Or
    }
}