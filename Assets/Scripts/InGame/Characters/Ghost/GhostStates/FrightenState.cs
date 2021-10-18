using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class FrightenState : BaseState
    {
        private int count = 0;
        private float secondsInFrightenState = 0;

        public override void OnStateEnter(GameObject forGameObject)
        {
            count = 0;
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            cm.Animator.SetInteger("State", GhostAnimationState.Frigten);
            GhostController ghost = forGameObject.GetComponent<GhostController>();
            cm.SpeedCoefficient = ghost.FrightenCoEfficient;

            secondsInFrightenState = UnityEngine.Object.FindObjectOfType<GhostManager>().TimeInFrightenState;
        }

        public override Type Update(GameObject forGameObject)
        {
            count++;

            // Set random target position
            GhostController ghost = forGameObject.GetComponent<GhostController>();
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();

            int x = ghost.Level.RandomStream.Range(0, cm.Arena.Width());
            int y = ghost.Level.RandomStream.Range(0, cm.Arena.Height());

            ghost.TargetPosition = new Vector2Int(x, y);
  
            // Make ghost flash near end
            if (count >= (secondsInFrightenState * 0.8f) / Time.fixedDeltaTime)
            {
                if ((count & 0xf) < 8)
                {
                    cm.Animator.SetInteger("State", GhostAnimationState.Frigten);
                }
                else
                {
                    cm.Animator.SetInteger("State", GhostAnimationState.Normal);
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
            CharacterMovement cm = forGameObject.GetComponent<CharacterMovement>();
            cm.SpeedCoefficient = 1;
        }
    }
}
