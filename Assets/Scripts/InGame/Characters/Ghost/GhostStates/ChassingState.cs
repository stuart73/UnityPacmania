using System;
using UnityEngine;
using Pacmania.InGame.Characters.Ghost.AI;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ChassingState : DeadlyState
    {
        public override Type Update(GameObject forGameObject)
        {
            Type nextState = GetType();

            GhostAI ghostAI = forGameObject.GetComponent<GhostAI>();
            forGameObject.GetComponent<GhostController>().TargetPosition = ghostAI.ChasePosition();

            Level level = forGameObject.GetComponent<GhostController>().Level;

            if (level.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.scatter)
            {        
                nextState = typeof(ScatterState);
            }

            Type newType = base.Update(forGameObject);
            if (newType == typeof(ConfusedState))
            {
                nextState = newType;
            }

            return nextState;
        }
    }
}
