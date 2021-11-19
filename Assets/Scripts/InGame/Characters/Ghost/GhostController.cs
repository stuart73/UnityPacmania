using System.Collections.Generic;
using UnityEngine;
using Pacmania.Utilities.StateMachines;
using Pacmania.InGame.Characters.Ghost.GhostStates;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.ScoreSprites;
using Pacmania.InGame.Arenas;
using Pacmania.Audio;
using Pacmania.GameManagement;
using UnityEditor;

namespace Pacmania.InGame.Characters.Ghost
{
    public class GhostController : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 1.0f)] private float frightenCoEfficient = 0.6f;
        public float FrightenCoEfficient
        {
            get { return frightenCoEfficient; }
        }

        [SerializeField] private bool drawTarget = true;

        public Vector2Int DesiredDirection { get; set; } = Vector2Int.down;
        public Level Level { get; private set; }

        public StateMachine FSM { get; private set; }
        public Vector2Int TargetTile { get; set; }

        protected CharacterMovement pacManMovement;
        protected CharacterMovement characterMovement;

        private Vector2Int lastTile = Vector2Int.zero;
        public Vector2Int LastTile
        {
            set => lastTile = value;
        }
        private string currentStateString = ""; // Used for debug purposes, so we can view the ghost state in the Unity editor

        protected virtual void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            Level = FindObjectOfType<Level>();
            pacManMovement = FindObjectOfType<PacmanController>().GetComponent<CharacterMovement>();
        }

        protected virtual void Start()
        {
            Vector3 startPosition = characterMovement.ArenaPosition;
            Arena arena = characterMovement.Arena;

            if (arena != null)
            {
                startPosition = characterMovement.Arena.GetArenaPositionForTileCenter(characterMovement.Arena.NestTile);
            }

            characterMovement.SetInitialPosition(startPosition);

            // Create our ghost state machine, and start in scatter state.
            List<BaseState> states = new List<BaseState>();
            states.Add(new ScatterState());
            states.Add(new EatenState());
            states.Add(new FrightenState());
            states.Add(new ChassingState());
            states.Add(new RegenerateState());
            states.Add(new ConfusedState());
            FSM = new StateMachine(gameObject, states);
        }

        public void ResetState()
        {
            // Just reset to scatter. If the scatter chase timer is now in chase, then this will change over on the next state update.
            FSM.SetState(typeof(ScatterState));
        }

        public void Fright()
        {
            if (IsDeadly() == true)
            {
                ReverseDirection();
                FSM.SetState(typeof(FrightenState));
            }
            else if (FSM.CurrrentState is FrightenState)
            {
                // Already in frighten sate, then re-enter frighten state to reset counters.
                FSM.SetState(typeof(FrightenState));
            }
        }

        public void ReverseDirectionDueToChase()
        {
            if (Level.GhostManager.GhostsChangeDirectionOnChase == true)
            {
                ReverseDirection();
            }
        }

        private void ReverseDirection()
        {
            DesiredDirection = new Vector2Int(-DesiredDirection.x, -DesiredDirection.y);

            // Needs to be reset when reversing as will end up on same tile again, but still may need to change direction.
            lastTile = Vector2Int.zero;
        }

        public void Eaten()
        {
            FSM.SetState(typeof(EatenState));
            int score = Level.GhostManager.CalculateGhostEatenScore();
            Game.Instance.CurrentSession.AddScore(Level, score);
            SpawnScore(score);
            Level.AudioManager.Play(SoundType.EatGhost);
        }

        private void SpawnScore(int score)
        {
            Color scoreColour = new Color();
            if (TryGetComponent(out ScoreColour colourComponent) == true)
            {
                scoreColour = colourComponent.ScoreColor;
            }

            Level.ScoreSpawner.Spawn(score, gameObject, scoreColour, true);       
        }

        public bool IsDeadly()
        {
            return FSM.CurrrentState is DeadlyState;
        }

        private void CheckForChangeDirection()
        {
            Vector2Int tile = characterMovement.GetTileIn();

            if (characterMovement.IsInTileCenter() && tile != lastTile)
            {
                lastTile = tile;
                List<Vector2Int> newDirections = new List<Vector2Int>();

                bool allowedToGoBackIntoNest = false;

                // We are allowed to consider going back into the ghost start nest position if we are in an eaten state.
                if (FSM.CurrrentState is EatenState)
                {
                    allowedToGoBackIntoNest = true;
                }

                if (DesiredDirection.y <= 0 && characterMovement.Arena.IsCharacterAllowedinTile(tile.x, tile.y - 1, allowedToGoBackIntoNest))
                {
                    newDirections.Add(Vector2Int.down);
                }
                if (DesiredDirection.y >= 0 && characterMovement.Arena.IsCharacterAllowedinTile(tile.x, tile.y + 1,allowedToGoBackIntoNest))
                {
                    newDirections.Add(Vector2Int.up);
                }
                if (DesiredDirection.x <= 0 && characterMovement.Arena.IsCharacterAllowedinTile(tile.x - 1, tile.y, allowedToGoBackIntoNest))
                {
                    newDirections.Add(Vector2Int.left);
                }
                if (DesiredDirection.x >= 0 && characterMovement.Arena.IsCharacterAllowedinTile(tile.x + 1, tile.y, allowedToGoBackIntoNest))
                {
                    newDirections.Add(Vector2Int.right);
                }
                DesiredDirection = GetShortestPath(newDirections, tile);
            }
        }

        private Vector2Int GetShortestPath(List<Vector2Int> newDirections, Vector2Int tile)
        {
            if (newDirections.Count == 0)
            {
                return Vector2Int.down;
            }
            else if (newDirections.Count == 1)
            {
                return newDirections[0];
            }

            Vector2Int candinateDirection = newDirections[0];
            float shortestDist = float.MaxValue;
            foreach (Vector2Int dir in newDirections)
            {
                Vector2Int line = (tile + dir) - TargetTile;
                float distSqr = line.sqrMagnitude;
                if (distSqr < shortestDist)
                {
                    candinateDirection = dir;
                    shortestDist = distSqr;
                }
            }
            return candinateDirection;
        }

        private void FixedUpdate()
        {
            if (characterMovement.Paused == true)
            {
                return;
            }
            FSM.Update();

            // debug purposes
            currentStateString = FSM.CurrrentState.GetType().Name;

            if (DesiredDirection.x != 0 || DesiredDirection.y != 0)
            {
                CheckForChangeDirection();
                characterMovement.Move(DesiredDirection);
            }
        }

        private void Update()
        {
            // We may need to modify the screen position based on where pac-man is on the screen to take into account that the arena wraps around.
            Arena arena = characterMovement.Arena;
            if (arena != null && arena.ArenaWrapper != null)
            {
                transform.position = arena.ArenaWrapper.GetClosestWrappedPosition(transform.position);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (drawTarget == true && characterMovement != null)
            {
                Arena arena = characterMovement.Arena;
                if (arena != null)
                {
                    Vector3 targetPosition = characterMovement.Arena.GetArenaPositionForTileCenter(TargetTile);
                    Vector3 endPostion = characterMovement.Arena.GetTransformPositionFromArenaPosition(targetPosition);
                    Gizmos.color = GetComponent<ScoreColour>().ScoreColor;
                    Gizmos.DrawLine(transform.position, endPostion);
                    Handles.Label(transform.position, this.FSM.CurrrentState.GetType().Name);
                }
            }
#endif
        }

     

        public void SetVisible(bool value)
        {
            SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = value;
            }
        }
    }
}
