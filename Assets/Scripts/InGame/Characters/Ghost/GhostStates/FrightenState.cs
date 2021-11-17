using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class FrightenState : BaseState
    {
        private int count = 0;
        private float secondsInFrightenState = 0;
        private CharacterMovement characterMovement;
        private GhostController ghostController;

        public override void OnStateEnter(GameObject forGameObject)
        {
            count = 0;
            ghostController = forGameObject.GetComponent<GhostController>();
            characterMovement = forGameObject.GetComponent<CharacterMovement>();
            characterMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Frigten);     
            characterMovement.SpeedCoefficient = ghostController.FrightenCoEfficient;     
            secondsInFrightenState = characterMovement.Level.GhostManager.TimeInFrightenState;
        }

        public override Type Update(GameObject forGameObject)
        {
            count++;
            Level level = characterMovement.Level;
            int x = level.RandomStream.Range(0, characterMovement.Arena.Width());
            int y = level.RandomStream.Range(0, characterMovement.Arena.Height());

            ghostController.TargetTile = new Vector2Int(x, y);
  
            // Make ghost flash near end
            if (count >= (secondsInFrightenState * 0.8f) / Time.fixedDeltaTime)
            {
                if ((count & 0xf) < 8)
                {
                    characterMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Frigten);
                }
                else
                {
                    characterMovement.CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Normal);
                }
            }

            if (count >= secondsInFrightenState / Time.fixedDeltaTime)
            {
                return typeof(ScatterState);
            }

            return GetType();
        }

        public override void OnStateLeave(GameObject forGameObject)
        {
            characterMovement.SpeedCoefficient = 1;
        }
    }
}
