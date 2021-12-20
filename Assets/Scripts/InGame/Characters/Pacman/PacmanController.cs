using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Pacmania.Audio;
using Pacmania.Utilities.Record;
using Pacmania.GameManagement;
using Pacmania.InGame.Arenas;

namespace Pacmania.InGame.Characters.Pacman
{
    [RequireComponent(typeof(CharacterMovement), typeof(PacmanCollision))]
    public class PacmanController : MonoBehaviour
    {
        private CharacterMovement characterMovement;
        private Animator animator;

        private PacmanCollision pacmanCollision;
        private Vector2 desiredDirection;
        private bool selectedJump = false;
        private Level level;
        private RandomMovement randomMovement;  // used for demo scenes

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            animator = GetComponent<Animator>();
            pacmanCollision = GetComponent<PacmanCollision>();
            level = FindObjectOfType<Level>();
            randomMovement = GetComponent<RandomMovement>();

            if (Game.Instance.CurrentSession is DemoGameSession && randomMovement != null)
            {
                randomMovement.enabled = true;
            }
        }

        public void StartSpinAnimation()
        {
            animator.SetBool(CharacterAnimatorParameterNames.Dying, true);
            animator.SetFloat(CharacterAnimatorParameterNames.Horizontal, 0);
            animator.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);
            SetShadowVisible(false);
            level.AudioManager.Play(SoundType.DieSpin);
        }

        public void StopSpinAnimation()
        {
            animator.SetBool(CharacterAnimatorParameterNames.Dying, false);
            SetShadowVisible(true);
        }

        private void Start()
        {
            Vector3 startPosition = characterMovement.ArenaPosition;
            Arena arena = characterMovement.Arena;

            if (arena != null)
            {
                startPosition = characterMovement.Arena.GetArenaPositionForTileCenter(characterMovement.Arena.PacmanStartTile);
            }

            characterMovement.SetInitialPosition(startPosition);
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (!value.started) return;

            GameSession currentSession = Game.Instance.CurrentSession;
            if (currentSession is DemoGameSession demoGameSession)
            {
                demoGameSession.StartPlayerGame();
                return;
            }
            selectedJump = true;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (!value.performed) return;
            //keyboard and joystick movement (input is digital un-normalised)
            Vector2 inputMovement = value.ReadValue<Vector2>();
            desiredDirection = new Vector2Int((int)inputMovement.x, -(int)inputMovement.y); // y axis is reversed
        }

        public void OnMovementSwipe(InputAction.CallbackContext value)
        {
            // Touchscreen (input is analogue)
            Vector2 inputMovement = value.ReadValue<Vector2>();

            if (inputMovement.y == 0 && inputMovement.x == 0)
            {
                return;
            }

            if (Math.Abs(inputMovement.x) > Math.Abs(inputMovement.y))
            {
                inputMovement.y = 0;
            }
            else
            {
                inputMovement.x = 0;
            }

            inputMovement.Normalize();
            desiredDirection = new Vector2Int((int)inputMovement.x, -(int)inputMovement.y); // y axis is reversed
        }

        private void FixedUpdate()
        { 
            if (randomMovement != null && randomMovement.enabled == true)
            {
                randomMovement.Next(characterMovement.CurrentDirection, out bool jump, out Vector2 movement);

                selectedJump = jump;
                desiredDirection = movement;
            }

            if (selectedJump == true)
            {
                GetComponent<Jumping>().Jump();

                selectedJump = false;
            }

            characterMovement.Move(desiredDirection);

            if (characterMovement.Paused == false)
            {
                pacmanCollision.CheckCollisions();
            }
        }

        public void SetShadowVisible(bool value)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = value;
        }
    }
}
