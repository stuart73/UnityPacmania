using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class RegenerateState : BaseState
    {
        private GhostManager ghostManager;
        private CharacterMovement characterMovement;
        private GhostController ghostController;

        public override void OnStateEnter(GameObject forGameObject)
        {     
            characterMovement = forGameObject.GetComponent<CharacterMovement>();

            ghostManager = characterMovement.Level.GhostManager;
            characterMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Regenerating);

            ghostController = forGameObject.GetComponent<GhostController>();
            ghostController.DesiredDirection = new Vector2Int(0, 0);     
        }

        public override Type Update(GameObject forGameObject)
        {
            if (characterMovement.CharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) // range 0...1, so 1 = finished.
            {
                if (ghostManager.ScatterChaseTimer.CurrentAction == ScatterChaseTimer.currentGhostAction.scatter)
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
            // Move out of nest.
            ghostController.DesiredDirection = new Vector2Int(0, -1);
            ghostController.LastTile = new Vector2Int(0, 0);
        }
    }
}
