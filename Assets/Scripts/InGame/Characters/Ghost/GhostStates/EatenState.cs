using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class EatenState : BaseState
    {
        private const float speedIncreaseForEatenEyes = 2.0f;
        public override void OnStateEnter(GameObject forGameObject)
        {
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            cm.Animator.SetInteger("State", GhostAnimationState.Eaten);

            // Set our TargetPostion back to nest
            GhostController ghost = forGameObject.GetComponent<GhostController>();
            Vector2Int nestEntranceTile = cm.Arena.NestEntranceTile;

            ghost.TargetPosition = new Vector2Int(nestEntranceTile.x, nestEntranceTile.y);
            cm.SpeedCoefficient = speedIncreaseForEatenEyes; 

            // Hide the eyes until next update called (i.e. there should be a short pause from the level state).
            SpriteRenderer spriteRenderer = forGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

            SetShadowToEyes(forGameObject, true);
        }

        public override Type Update(GameObject forGameObject)
        {
            SpriteRenderer spriteRenderer = forGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;

            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            if (cm.IsInTileCenter() == false) return GetType();

            Vector2Int tile = cm.GetTileIn();
            Vector2Int nestTile = cm.Arena.NestTile;

            if (tile == cm.Arena.NestEntranceTile)
            {
                forGameObject.GetComponent<GhostController>().TargetPosition = nestTile;
            }

            if (tile == nestTile)
            {              
                return typeof(RegenerateState);
            }

            return GetType();
        }

        public override void OnStateLeave(GameObject forGameObject)
        {
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            cm.SpeedCoefficient = 1;
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
