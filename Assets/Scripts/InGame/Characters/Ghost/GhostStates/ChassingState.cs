using System;
using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ChassingState : DeadlyState
    {
        public override Type Update(GameObject forGameObject)
        {
            Type nextState = GetType();

            ghostController.TargetTile = ghostAI.GetChaseTile();

            if (level.GhostManager.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.scatter)
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
