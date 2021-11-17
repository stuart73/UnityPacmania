using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class EatenState : BaseState
    {
        private const float speedIncreaseForEatenEyes = 2.0f;
        private CharacterMovement ghostCharacterMovement;
        private GhostController ghostController;
        private SpriteRenderer spriteRenderer;

        public override void OnStateEnter(GameObject forGameObject)
        {
            ghostCharacterMovement = forGameObject.GetComponent<CharacterMovement>();
            ghostCharacterMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Eaten);

            // Set our TargetPostion back to nest
            ghostController = forGameObject.GetComponent<GhostController>();

            ghostController.TargetTile = ghostCharacterMovement.Arena.NestEntranceTile;
            ghostCharacterMovement.SpeedCoefficient = speedIncreaseForEatenEyes; 

            // Hide the eyes until next update called (i.e. there should be a short pause from the level state).
            spriteRenderer = forGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

            SetShadowToEyes(forGameObject, true);
        }

        public override Type Update(GameObject forGameObject)
        {
            spriteRenderer.enabled = true;

            if (ghostCharacterMovement.IsInTileCenter() == false) return GetType();

            Vector2Int tile = ghostCharacterMovement.GetTileIn();
            Vector2Int nestTile = ghostCharacterMovement.Arena.NestTile;

            if (tile == ghostCharacterMovement.Arena.NestEntranceTile)
            {
                ghostController.TargetTile = nestTile;
            }

            if (tile == nestTile)
            {              
                return typeof(RegenerateState);
            }

            return GetType();
        }

        public override void OnStateLeave(GameObject forGameObject)
        {
            ghostCharacterMovement.SpeedCoefficient = 1;
            SetShadowToEyes(forGameObject, false);
        }

        private void SetShadowToEyes(GameObject forGameObject, bool value)
        {
            Shadow shadow = forGameObject.GetComponentInChildren<Shadow>();
            if (shadow)
            {
                shadow.UseEyesShadow = value;
            }
        }
    }
}
