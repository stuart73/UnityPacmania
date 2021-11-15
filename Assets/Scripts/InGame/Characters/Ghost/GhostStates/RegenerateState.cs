using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class RegenerateState : BaseState
    {
        public override void OnStateEnter(GameObject forGameObject)
        {
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            cm.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Regenerating);

            GhostController ghost = forGameObject.GetComponent<GhostController>();
            ghost.DesiredDirection = new Vector2Int(0, 0);
      
        }

        public override Type Update(GameObject forGameObject)
        {
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            if (cm.CharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) // range 0...1, so 1 = finished.
            {
                Level level = forGameObject.GetComponent<GhostController>().Level;

                if (level.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.scatter)
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

        public override void OnStateLeave(GameObject forGameObject)
        {
            // move out of nest
            GhostController ghost = forGameObject.GetComponent<GhostController>();
            ghost.DesiredDirection = new Vector2Int(0, -1);
            ghost.LastTile = new Vector2Int(0, 0);
        }
    }
}
