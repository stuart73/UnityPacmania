using UnityEngine;
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

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            animator = GetComponent<Animator>();
            pacmanCollision = GetComponent<PacmanCollision>();
            level = FindObjectOfType<Level>();

            if (Game.Instance.CurrentSession is DemoGameSession)
            {
                this.gameObject.AddComponent<PlaybackKeyboard>();
            }
        }

        public void StartSpinAnimation()
        {
            animator.SetBool(CharacterAnimatorParameterNames.Dying, true);
            animator.SetFloat(CharacterAnimatorParameterNames.Horizontal, 0);
            animator.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);
            SetShadowVisible(false);
            level.AudioManager.Play(SoundType.DieSpin);

            RecordKeyboard record = GetComponent<RecordKeyboard>();
            if (record != null)
            {
                record.Print();
            }
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

        void Update()
        {
            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");

            desiredDirection = new Vector2(inputHorizontal, -inputVertical);  // y axis is reversed

            if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) )
            {
                GameSession currentSession = Game.Instance.CurrentSession;
                if (currentSession is DemoGameSession demoGameSession)
                {
                    demoGameSession.StartPlayerGame();
                    return;
                }
                selectedJump = true;
            }
        }

        private void FixedUpdate()
        { 
            PlaybackKeyboard playback = GetComponent<PlaybackKeyboard>();
            if (playback != null && playback.enabled == true)
            {
                RecordKeyboard.KeyboardSnapshot snapshot = playback.GetNextFixedUpdateSnapshot();

                characterMovement.Move(snapshot.direction);
                if (snapshot.jump == true)
                {
                    GetComponent<Jumping>().Jump();
                }
            }
            else
            {
                if (selectedJump == true)
                {
                    GetComponent<Jumping>().Jump();
                }

                characterMovement.Move(desiredDirection);

                RecordKeyboard record = GetComponent<RecordKeyboard>();
                if (record != null)
                {
                    record.RecordFixedUpdate(desiredDirection, selectedJump);
                }

                selectedJump = false;
            }

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
