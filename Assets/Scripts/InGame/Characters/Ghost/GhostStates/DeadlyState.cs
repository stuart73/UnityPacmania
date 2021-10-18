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

            CheckIfPacmanAboveUs(pacmanMovement, ghostMovement);

            // If we are on the same tile as pacman, then we go into confused state.
            if (IsGhostOnSameTileAsPacman(pacmanMovement, ghostMovement) == true)
            {
                return typeof(ConfusedState);
            }
            return GetType();
        }

        private bool IsGhostOnSameTileAsPacman(CharacterMovement pacmanMovement, CharacterMovement ghostMovement)
        {
            Vector2Int Ghosttile = ghostMovement.Arena.GetTileForArenaPosition(ghostMovement.ArenaPosition);
            Vector2Int PacmanTile = pacmanMovement.Arena.GetTileForArenaPosition(pacmanMovement.ArenaPosition);
            return Ghosttile == PacmanTile;
        }

        private void CheckIfPacmanAboveUs(CharacterMovement pacmanMovement, CharacterMovement ghostMovement)
        {
            // If pacman is above us, then look up!
            Vector3 ghostToPacman = pacmanMovement.ArenaPosition - ghostMovement.ArenaPosition;
            if (Math.Abs(ghostToPacman.z) > Math.Abs(ghostToPacman.x) && Math.Abs(ghostToPacman.z) > Math.Abs(ghostToPacman.y))
            {
                ghostMovement.Animator.SetInteger("State", GhostAnimationState.LookingUp);
            }
            else
            {
                ghostMovement.Animator.SetInteger("State", GhostAnimationState.Normal);
            }
        }
    }
}