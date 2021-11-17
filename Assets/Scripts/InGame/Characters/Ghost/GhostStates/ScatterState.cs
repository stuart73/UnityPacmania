using System;
using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ScatterState : DeadlyState
    {
        public override Type Update(GameObject forGameObject)
        {
            Type nextState = GetType();
            ghostController.TargetTile = ghostAI.GetScatterTile();

            Level level = ghostController.Level;
           
            if (level.GhostManager.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.chase)
            {
                ghostController.ReverseDirectionDueToChase();
                nextState = typeof(ChassingState);
            }

            Type newType = base.Update(forGameObject);
            if (newType == typeof(ConfusedState) )
            {
                nextState = newType;
            }

            return nextState;
        }
    }
}
