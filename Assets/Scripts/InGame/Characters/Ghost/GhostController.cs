﻿using System.Collections.Generic;
using UnityEngine;
using Pacmania.Utilities.StateMachines;
using Pacmania.InGame.Characters.Ghost.GhostStates;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.ScoreSprites;
using Pacmania.InGame.Arenas;
using UnityEditor;

namespace Pacmania.InGame.Characters.Ghost
{
    public class GhostController : MonoBehaviour
    {
        [SerializeField] private float frightenCoEfficient = 0.6f;
        public float FrightenCoEfficient
        {
            get { return frightenCoEfficient; }
        }

        [SerializeField] private bool drawTarget = true;

        public Vector2Int DesiredDirection { get; set; } = new Vector2Int(0, -1);
        public Level Level { get; private set; }

        public StateMachine fsm { get; private set; }
        public Vector2Int TargetPosition { get; set; }

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
            fsm = new StateMachine(gameObject, states);
        }

        public void ResetState()
        {
            // Just reset to scatter. If the scatter chase timer is now in chase, then this will change over on the next state update.
            fsm.SetState(typeof(ScatterState));
        }

        public void Fright()
        {
            if (IsDeadly() == true)
            {
                ReverseDirection();
                fsm.SetState(typeof(FrightenState));
            }
            else if (fsm.CurrrentState is FrightenState)
            {
                // Already in frighten sate, then re-enter frighten state to reset counters.
                fsm.SetState(typeof(FrightenState));
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
            fsm.SetState(typeof(EatenState));
        }

        public bool IsDeadly()
        {
            return fsm.CurrrentState is DeadlyState;
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
                if (fsm.CurrrentState is EatenState)
                {
                    allowedToGoBackIntoNest = true;
                }

                bool canGoUp = characterMovement.Arena.IsCharacterAllowedinTile(tile.x, tile.y - 1, allowedToGoBackIntoNest);
                if (canGoUp && DesiredDirection.y <= 0)
                {
                    newDirections.Add(new Vector2Int(0, -1));
                }
                bool canGoDown = characterMovement.Arena.IsCharacterAllowedinTile(tile.x, tile.y + 1, allowedToGoBackIntoNest);
                if (canGoDown && DesiredDirection.y >= 0)
                {
                    newDirections.Add(new Vector2Int(0, 1));
                }

                bool canGoLeft = characterMovement.Arena.IsCharacterAllowedinTile(tile.x - 1, tile.y, allowedToGoBackIntoNest);
                if (canGoLeft && DesiredDirection.x <= 0)
                {
                    newDirections.Add(new Vector2Int(-1, 0));
                }

                bool canGoRight = characterMovement.Arena.IsCharacterAllowedinTile(tile.x + 1, tile.y, allowedToGoBackIntoNest);
                if (canGoRight && DesiredDirection.x >= 0)
                {
                    newDirections.Add(new Vector2Int(1, 0));
                }
                DesiredDirection = GetShortestPath(newDirections, tile);
            }
        }

        private Vector2Int GetShortestPath(List<Vector2Int> newDirections, Vector2Int tile)
        {
            if (newDirections.Count == 0)
            {
                return new Vector2Int(0, -1);
            }
            else if (newDirections.Count == 1)
            {
                return newDirections[0];
            }

            Vector2Int candinateDirection = newDirections[0];
            float shortestDist = float.MaxValue;
            foreach (Vector2Int dir in newDirections)
            {
                Vector2Int dir2 = new Vector2Int(dir.x, dir.y);
                Vector2Int line = (tile + dir2) - TargetPosition;
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
            fsm.Update();

            // debug purposes
            currentStateString = fsm.CurrrentState.GetType().Name;

            if (DesiredDirection.x != 0 || DesiredDirection.y != 0)
            {
                CheckForChangeDirection();
                characterMovement.Move(DesiredDirection);
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
                    Vector3 targetPosition = characterMovement.Arena.GetArenaPositionForTileCenter(new Vector2Int(TargetPosition.x, TargetPosition.y));
                    Vector3 endPostion = characterMovement.Arena.GetTransformPositionFromArenaPosition(targetPosition);
                    Gizmos.color = GetComponent<ScoreColour>().ScoreColor;
                    Gizmos.DrawLine(transform.position, endPostion);
                    Handles.Label(transform.position, this.fsm.CurrrentState.GetType().Name);
                }
            }
#endif
        }

        private void LateUpdate()
        {
            // We've moved the ghost and calculated the screen position during Update(), but we then may need to modify the screen position
            // further based on where pac-man is on the screen.  This is to take into account that the arena wraps around.
            Arena arena = characterMovement.Arena;
            if (arena != null && arena.ArenaWrapper != null)
            {
                transform.position = characterMovement.Arena.ArenaWrapper.GetClosestWrappedPosition(transform.position);
            }
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
