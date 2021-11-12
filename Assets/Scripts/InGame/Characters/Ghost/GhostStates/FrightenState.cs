using System;
using UnityEngine;
using Pacmania.Utilities.StateMachines;

namespace Pacmania.InGame.Characters.Ghost.GhostStates
{
    public class FrightenState : BaseState
    {
        private int count = 0;
        private float secondsInFrightenState = 0;
        private CharacterMovement cm;
        private GhostController ghost;

        public override void OnStateEnter(GameObject forGameObject)
        {
            count = 0;
            ghost = forGameObject.GetComponent<GhostController>();
            cm = forGameObject.GetComponent<CharacterMovement>();
            cm.CharacterAnimator.SetInteger("State", GhostAnimationState.Frigten);     
            cm.SpeedCoefficient = ghost.FrightenCoEfficient;     
            secondsInFrightenState = cm.Level.GhostManager.TimeInFrightenState;
        }

        public override Type Update(GameObject forGameObject)
        {
            count++;
            int x = ghost.Level.RandomStream.Range(0, cm.Arena.Width());
            int y = ghost.Level.RandomStream.Range(0, cm.Arena.Height());

            ghost.TargetTile = new Vector2Int(x, y);
  
            // Make ghost flash near end
            if (count >= (secondsInFrightenState * 0.8f) / Time.fixedDeltaTime)
            {
                if ((count & 0xf) < 8)
                {
                    cm.CharacterAnimator.SetInteger("State", GhostAnimationState.Frigten);
                }
                else
                {
                    cm.CharacterAnimator.SetInteger("State", GhostAnimationState.Normal);
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
            cm.SpeedCoefficient = 1;
        }
    }
}
