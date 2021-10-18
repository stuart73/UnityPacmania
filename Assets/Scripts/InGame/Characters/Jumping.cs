using System;
using UnityEngine;
using Pacmania.Audio;

namespace Pacmania.InGame.Characters
{
    public class Jumping : MonoBehaviour
    {
        [SerializeField] private float jumpPower = 1;
        public float JumpPower
        {
            get { return jumpPower; }
            set { jumpPower = value; }
        }

        static readonly float[] jump = { 3, 6, 9, 12, 14, 16, 18, 19, 20, 21, 22, 23, 24, 24, 25, 25, 25, 26, 26, 26, 26, 27, 27, 27, 27, 27, 26, 26, 26, 26, 25, 25, 25, 24, 24, 23, 22, 21, 20, 19, 18, 16, 14, 12, 9, 6, 3, 0, 2, 4, 5, 6, 6, 7, 7, 7, 7, 6, 6, 5, 4, 2, 0 };

        public event Action Jumped = delegate { };
        private int jumpIndex = 0;
        private const int BounceIndex = 48;
        private bool jumpping = false;
        private bool reJump = false;
        private Level level;
        private CharacterMovement characterMovementComponent;

        private void Awake()
        {
            level = FindObjectOfType<Level>();
            characterMovementComponent = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            // Unity Update() method left here so we can enable/disbale component from editor.
        }

        public void ResetJump()
        {
            jumpping = false;
            reJump = false;
            jumpIndex = 0;
        }

        public void Jump()
        {
            if (enabled == false || characterMovementComponent.Paused == true || jumpPower <= 0 )
            {
                return;
            }

            if (jumpping == false)
            {
                jumpping = true;
                Jumped?.Invoke(); 
            }

            if (jumpIndex > 30 && reJump == false)
            {
                reJump = true;
                Jumped?.Invoke();  
            }
        }

        // This method should be called from character movement.
        public void UpdateJumping()
        {
            if (jumpping == false) return;

            if (jumpIndex == 0)
            {
                level.AudioManager.Play(SoundType.Jump1);
            }

            Vector3 newPosition = characterMovementComponent.ArenaPosition;
            newPosition.z = jump[jumpIndex++] * jumpPower;
            characterMovementComponent.ArenaPosition = newPosition;
            if (jumpIndex >= jump.Length)
            {
                if (reJump == false)
                {
                    jumpping = false;
                    level.AudioManager.Play(SoundType.Jump3);

                }
                reJump = false;
                jumpIndex = 0;
            }

            if (jumpIndex == BounceIndex)
            {
                if (reJump == true)
                {
                    reJump = false;
                    jumpIndex = 0;
                }
                else
                {
                    level.AudioManager.Play(SoundType.Jump2);
                }
            }
        }
    }
}
