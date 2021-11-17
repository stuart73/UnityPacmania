using System;
using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ConfusedState : DeadlyState
    {
        private int count =0;
        private ScatterChaseTimer.currentGhostAction revivedAction;
        private const float secondsInConfusedState = 5.0f;
        private GhostManager ghostManager;

        public override void OnStateEnter(GameObject forGameObject)
        {    
            base.OnStateEnter(forGameObject);
            count = 0;
            ghostManager = level.GhostManager;
            revivedAction = ghostManager.ScatterChaseTimer.CurrentAction;
        }

        public override Type Update(GameObject forGameObject)
        {        
            base.Update(forGameObject);
            count++;

            // Set random target position.
            int x = level.RandomStream.Range(0, ghostCharacterMovement.Arena.Width());
            int y = level.RandomStream.Range(0, ghostCharacterMovement.Arena.Height());

            ScatterChaseTimer.currentGhostAction currentAction = ghostManager.ScatterChaseTimer.CurrentAction;

            if (currentAction != revivedAction)
            {
                revivedAction = currentAction;
                if (revivedAction == ScatterChaseTimer.currentGhostAction.chase)
                {
                    ghostController.ReverseDirectionDueToChase();
                }
            }

            ghostController.TargetTile = new Vector2Int(x, y);
   
            if (count >= Game.FramesPerSecond * secondsInConfusedState)
            {   
                if (revivedAction == ScatterChaseTimer.currentGhostAction.scatter)
                {
                    return typeof(ScatterState);
                }
                else
                {
                    return typeof(ChassingState);
                }         
            }

            return GetType();
        }
    }
}
