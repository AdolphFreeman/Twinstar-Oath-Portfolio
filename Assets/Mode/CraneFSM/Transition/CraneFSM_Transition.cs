using System;
using System.Collections.Generic;
using CraneFSM.Core;
using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    public class Transition : MonoBehaviour
    {
        public string transitionName;
        public State transitionState;
        public TransitionLogic logic;
        public TransitionConditionGroup[] transitionConditionGroups;

        public bool Evaluate()
        {
            if (transitionConditionGroups == null) return false;
            
            List<bool> evaluates = new List<bool>();
            foreach (TransitionConditionGroup conditionGroup in transitionConditionGroups)
            {
                if (conditionGroup != null)
                {
                    bool evaluate = conditionGroup.Evaluate() && 
                        GetComponentInParent<StateMachine>().GetFloat("runningTime") >= conditionGroup.exitTime;
                    evaluates.Add(evaluate);
                }
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

        public void OnTransition()
        {
            if (transitionConditionGroups == null) return;

            foreach (TransitionConditionGroup group in transitionConditionGroups)
            {
                if (group == null) continue;
                
                // Only reset triggers for groups that actually contributed to the transition
                if (!group.Evaluate())
                    continue;

                if (group.conditions == null) continue;

                foreach (TransitionCondition condition in group.conditions)
                {
                    if (condition == null) continue;

                    //print(condition.parameter.Value.GetType());
                    if (condition.parameter.Value is TriggerClass triggerClass)
                    {
                        triggerClass.Reset();
                    }
                }
            }
        }
    }   
}