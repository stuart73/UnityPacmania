using System;
using UnityEngine;
using Pacmania.InGame.Characters.Ghost.AI;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ScatterState : DeadlyState
    {
        public override Type Update(GameObject forGameObject)
        {
            Type nextState = GetType();

            GhostAI ghostAI = forGameObject.GetComponent<GhostAI>();
            GhostController ghostController = forGameObject.GetComponent<GhostController>();

            ghostController.TargetTile = ghostAI.GetScatterTile();

            Level level = forGameObject.GetComponent<GhostController>().Level;
           
            if (level.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.chase)
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
