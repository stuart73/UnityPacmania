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
        private Vector2Int desiredDirection;
        private bool selectedJump = false;

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            animator = GetComponent<Animator>();
            pacmanCollision = GetComponent<PacmanCollision>();

            if (Game.Instance.CurrentSession is DemoGameSession)
            {
                this.gameObject.AddComponent<PlaybackKeyboard>();
            }
        }

        public void StartSpinAnimation()
        {
            animator.SetBool("Dying", true);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            SetShadowVisible(false);
            FindObjectOfType<AudioManager>().Play(SoundType.DieSpin);

            RecordKeyboard record = GetComponent<RecordKeyboard>();
            if (record != null)
            {
                record.Print();
            }
        }
        public void StopSpinAnimation()
        {
            animator.SetBool("Dying", false);
            SetShadowVisible(true);
        }

        private void Start()
        {
            Vector3 startPosition = characterMovement.ArenaPosition;
            Arena arena = characterMovement.Arena;

            if (arena != null)
            {
                startPosition = characterMovement.Arena.GetArenaPositionForTileCenter(characterMovement.Arena.CenterTile);
            }

            characterMovement.SetInitialPosition(startPosition);
        }

        void Update()
        {
            if (Game.Instance.CurrentSession is DemoGameSession)
            {
                if (Input.GetKeyDown("space"))
                {
                    (Game.Instance.CurrentSession as DemoGameSession)?.StartPlayerGame();
                }

                return;
            }

            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");

            desiredDirection = new Vector2Int(0, 0);

            Vector3 currentMovement = characterMovement.CurrentDirection;

            if (inputHorizontal > 0 && currentMovement.x <= 0) desiredDirection.x = 1;
            else if (inputHorizontal < 0 && currentMovement.x >= 0) desiredDirection.x = -1;

            // Note y axis is reverse to direction in our game. i.e. up button is +y but in game this means head -y direction. 
            if (inputVertical > 0 && currentMovement.y >= 0) desiredDirection.y = -1;
            else if (inputVertical < 0 && currentMovement.y <= 0) desiredDirection.y = 1;

            if (Input.GetKeyDown(KeyCode.Space))
            {
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
