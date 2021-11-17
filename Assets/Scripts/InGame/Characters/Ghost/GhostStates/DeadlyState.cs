using Pacmania.Utilities.StateMachines;
using System;
using UnityEngine;
using Pacmania.InGame.Characters.Ghost.AI;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public abstract class DeadlyState : BaseState
    {
        protected CharacterMovement ghostCharacterMovement;
        protected GhostController ghostController;
        protected Level level;
        protected CharacterMovement pacmanCharacterMovement;
        protected GhostAI ghostAI;

        public override void OnStateEnter(GameObject forGameObject)
        {
            ghostCharacterMovement = forGameObject.GetComponent<CharacterMovement>();
            ghostController = forGameObject.GetComponent<GhostController>();
            ghostAI = forGameObject.GetComponent<GhostAI>();
            level = ghostCharacterMovement.Level;
            pacmanCharacterMovement = level.Pacman.GetComponent<CharacterMovement>();
        }

        public override Type Update(GameObject forGameObject)
        {
            bool pacmanAboveUs = IsPacmanAboveUs(pacmanCharacterMovement, ghostCharacterMovement);
            UpdatEyes(pacmanAboveUs, ghostCharacterMovement);

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