using System;
using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class ConfusedState : DeadlyState
    {
        private int count =0;
        private ScatterChaseTimer.currentGhostAction currentStateAction;
        private const float secondsInConfusedState = 5.0f;

        public override void OnStateEnter(GameObject forGameObject)
        {
            count = 0;
            Level level = forGameObject.GetComponent<GhostController>().Level;
            currentStateAction = level.ScatterChaseTimer.CurrentAction;
        }

        public override Type Update(GameObject forGameObject)
        {        
            base.Update(forGameObject);
            count++;

            // Set random target position.
            GhostController ghost = forGameObject.GetComponent<GhostController>();
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();

            int x = ghost.Level.RandomStream.Range(0, cm.Arena.Width());
            int y = ghost.Level.RandomStream.Range(0, cm.Arena.Height());

            Level level = forGameObject.GetComponent<GhostController>().Level;

            if (level.ScatterChaseTimer.CurrentAction != currentStateAction)
            {
                currentStateAction = level.ScatterChaseTimer.CurrentAction;

                if (currentStateAction == ScatterChaseTimer.currentGhostAction.chase)
                {
                    ghost.ReverseDirectionDueToChase();
                }
            }

            ghost.TargetTile = new Vector2Int(x, y);
   
            if (count >= Game.FramesPerSecond * secondsInConfusedState)
            {   
                if (currentStateAction == ScatterChaseTimer.currentGhostAction.scatter)
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
