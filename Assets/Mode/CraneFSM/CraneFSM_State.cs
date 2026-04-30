using System;
using System.Collections.Generic;
using CraneFSM;
using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    public class State : MonoBehaviour
    {
        public bool isRunning = false;
        public StateMachine sm;
        public string stateName;
        public Transition[] transitions;

        private void Awake()
        {
            gameObject.name = stateName;
        
            sm = GetComponentInParent<StateMachine>();
            transitions = GetComponentsInChildren<Transition>();

            foreach (Transition transition in transitions)
            {
                transition.transitionName = $"{stateName} => {transition.transitionState.stateName}";
            }
        }

        private void Update()
        {
            foreach (Transition transition in transitions)
            {
                transition.name = $"{transition.transitionName}: {transition.Evaluate()}";
            }
        }

        //---
        public virtual void Enter()
        {
            isRunning = true;
            name = stateName + " (🟩🟩Running🟩🟩) ";
        }

        public virtual void Execute()
        {
            ApplyTransit();
        }

        public virtual void Exit()
        {
            isRunning = false;
            name = stateName;
        }

        public void ApplyTransit()
        {
            foreach (Transition transition in transitions)
            {
                if(transition.Evaluate())
                {
                    transition.OnTransition();
                    sm.SwitchState(transition.transitionState);
                    break;
                }
            }
        }
    }

}