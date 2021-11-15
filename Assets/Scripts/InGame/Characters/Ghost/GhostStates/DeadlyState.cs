using Pacmania.Utilities.StateMachines;
using System;
using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public abstract class DeadlyState : BaseState
    {
        public override Type Update(GameObject forGameObject)
        {
            Level level = forGameObject.GetComponent<GhostController>().Level;
            CharacterMovement pacmanMovement = level.Pacman.GetComponent<CharacterMovement>();
            CharacterMovement ghostMovement = forGameObject.GetComponent<CharacterMovement>();

            bool pacmanAboveUs = IsPacmanAboveUs(pacmanMovement, ghostMovement);
            UpdatEyes(pacmanAboveUs, ghostMovement);

            // If pacman is above us then go into confused state (if not already).
            if (pacmanAboveUs == true)
            {
                return typeof(ConfusedState);
            }
            return GetType();
        }

        private bool IsPacmanAboveUs(CharacterMovement pacmanMovement, CharacterMovement ghostMovement)
        {
            // Pacman is considered above us if the longest axis on a vector between us is the z axis.
            Vector3 ghostToPacman = pacmanMovement.ArenaPosition - ghostMovement.ArenaPosition;
            float zAxisLength = Math.Abs(ghostToPacman.z);
            if (zAxisLength > Math.Abs(ghostToPacman.x) && zAxisLength > Math.Abs(ghostToPacman.y))
            {
                return true;
            }
            return false;
        }

        private void UpdatEyes(bool isPacmanAboveUs, CharacterMovement ghostMovement)
        {
            if (isPacmanAboveUs == true)
            {
                ghostMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.LookingUp);
            }
            else
            {
                ghostMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Normal);
            }
        }
    }
}