using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pacmania.Utilities.StateMachines
{
    public class StateMachine
    {
        private List<BaseState> states = new List<BaseState>();
        private GameObject gameObject;

        public BaseState CurrrentState { get; private set; }

        public StateMachine(GameObject forgameObject, List<BaseState> states)
        {
            gameObject = forgameObject;
            this.states = states;
            if (states.Count > 0)
            {
                CurrrentState = states[0];
                CurrrentState.OnStateEnter(gameObject);
            }       
        }

        public void Update()
        {
            if (CurrrentState != null)
            {
                Type newStateType = CurrrentState.Update(gameObject);

                if (newStateType != CurrrentState.GetType())
                {
                    SetState(newStateType);
                }
            }
        }

        public void SetState(Type newStateType)
        {
            foreach (BaseState state in states)
            {
                if (state.GetType() == newStateType)
                {
                    if (CurrrentState.GetType() == newStateType)
                    {
                        CurrrentState.OnStateEnter(gameObject);
                    }
                    else
                    {
                        CurrrentState.OnStateLeave(gameObject);
                        CurrrentState = state;
                        CurrrentState.OnStateEnter(gameObject);
                    }
                    return;
                }
            }

           Debug.LogError("State not in state machine " + newStateType.ToString());
        }
    }
}
