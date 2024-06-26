﻿using System;
using UnityEngine;
using Pacmania.InGame.Arenas;

namespace Pacmania.InGame.Characters
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] [Range(0.4f, 2.0f)] private float defaultSpeed = 1.4f;
        public float DefaultSpeed
        {
            get => defaultSpeed;
            set => defaultSpeed = value;
        }

        [SerializeField] [Range(0.01f, 0.04f)] private float speedIncreasePerLevel = 0.025f;
        [SerializeField] private bool allowedInGhostNest = false;
        [SerializeField] private Vector3 currentDirection;
        public Vector3 CurrentDirection
        {
            get => currentDirection;
        }

        public float SpeedCoefficient { get; set; } = 1;
        public Arena Arena { get; private set; }
        public Level Level { get; private set; }
        public Animator CharacterAnimator { get; private set; }
        public bool Paused { get; set; } = true;

        private Vector3 arenaPosition;
        public Vector3 ArenaPosition
        {
            get => arenaPosition;
            set => arenaPosition = value;
        }
        
        private Vector3 initialArenaPosition;
        private Vector2 initialDirection;
        private Jumping jumpingComponent;
        private const float maxSpeed = 2.0f;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            Level = FindObjectOfType<Level>();
            Arena = FindObjectOfType<Arena>();

            CharacterAnimator = GetComponent<Animator>();
            jumpingComponent = GetComponent<Jumping>();

            if (Level != null)
            {
                defaultSpeed += (speedIncreasePerLevel * (Level.LevelNumber - 1));
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetInitialPosition(Vector3 position)
        {
            arenaPosition = position;
            initialArenaPosition = arenaPosition;
            initialDirection = currentDirection; ;
            SetAnimationAndSpriteOrder();
        }

        public void ResetToStartPosition()
        {
            arenaPosition = initialArenaPosition;
            currentDirection = initialDirection;
            transform.position = Arena.GetTransformPositionFromArenaPosition(arenaPosition);
            SetAnimationAndSpriteOrder();

            if (jumpingComponent != null)
            {
                jumpingComponent.ResetJump();
            }

            // Make sure speed coefficent is 1. e.g. pacman may be in green pellet mode previously.
            SpeedCoefficient = 1.0f;
        }

        private void SetAnimationAndSpriteOrder()
        {
            CharacterAnimator.SetFloat(CharacterAnimatorParameterNames.Horizontal, currentDirection.x);
            CharacterAnimator.SetFloat(CharacterAnimatorParameterNames.Vertical, -currentDirection.y);

            if (Arena != null)
            {
                spriteRenderer.sortingOrder = Arena.GetOrder(this.GetTileIn());
            }
        }

        private float CurrentSpeed()
        {
            return Mathf.Clamp(defaultSpeed * SpeedCoefficient, 0, maxSpeed);
        }

        public bool IsInTileCenter()
        {
            Vector3 basePosition = arenaPosition;
            basePosition.z = 0;

            if (Vector3.Distance(Arena.GetArenaPositionForTileCenter(basePosition), basePosition) < (CurrentSpeed() / 1.95f))
            {
                return true;
            }
            return false;
        }

        public Vector2Int GetTileIn()
        {
            return Arena.GetTileForArenaPosition(arenaPosition);
        }

        public void Move(Vector2 desiredDirection)
        {
            if (Paused == true || enabled == false)
            {
                return;
            }

            Vector3 oldArenaPosition = arenaPosition; // needed for teleportation test later
            Vector2Int currentTile = Arena.GetTileForArenaPosition(arenaPosition);

            if (IsInTileCenter())
            {
                CheckDirectionChange(desiredDirection, currentTile);
            }
            else
            {
                CheckOppositeDirectionChange(desiredDirection);
            }

            arenaPosition += currentDirection * CurrentSpeed();

            CheckCollisionWithWalls(currentTile);

            if (jumpingComponent != null)
            {
                jumpingComponent.UpdateJumping();
            }

            TestForTeleportation(oldArenaPosition, ref arenaPosition);

            transform.position = Arena.GetTransformPositionFromArenaPosition(arenaPosition);

            SetAnimationAndSpriteOrder();
        }

        private void CheckCollisionWithWalls(Vector2Int currentTile)
        {
            Vector3 testPoint = arenaPosition + currentDirection * (Math.Abs(currentDirection.y) > 0 ? Arena.TileHalfHeightPixels : Arena.TileHalfWidthPixels);

            if (Arena.IsCharacterAllowedInTile(testPoint, allowedInGhostNest) == false)
            {
                arenaPosition = Arena.GetArenaPositionForTileCenter(currentTile);
            }
        }

        private void CheckOppositeDirectionChange(Vector2 desiredDirection)
        {
            if (currentDirection.x < 0 && desiredDirection.x > 0)
            {
                currentDirection = Vector3.right;
            }
            else if (currentDirection.x > 0 && desiredDirection.x < 0)
            {
                currentDirection = Vector3.left;
            }
            else if (currentDirection.y < 0 && desiredDirection.y > 0)
            {
                currentDirection = Vector3.up;
            }
            else if (currentDirection.y > 0 && desiredDirection.y < 0)
            {
                currentDirection = Vector3.down;
            }
        }

        private void CheckDirectionChange(Vector2 desiredDirection, Vector2Int currentTile)
        {
            Vector3 arenaPositionForTileCenter = Arena.GetArenaPositionForTileCenter(currentTile);

            if (desiredDirection.y > 0 && currentDirection.y <=0 && Arena.IsCharacterAllowedinTile(currentTile.x, currentTile.y + 1, allowedInGhostNest) == true)
            {
                currentDirection = Vector3.up;
                arenaPosition.x = arenaPositionForTileCenter.x;
            }
            else if (desiredDirection.y < 0 && currentDirection.y >= 0 && Arena.IsCharacterAllowedinTile(currentTile.x, currentTile.y - 1, allowedInGhostNest) == true)
            {
                currentDirection = Vector3.down;
                arenaPosition.x = arenaPositionForTileCenter.x;
            }
            else if (desiredDirection.x < 0 && currentDirection.x >=0 && Arena.IsCharacterAllowedinTile(currentTile.x - 1, currentTile.y, allowedInGhostNest) == true)
            {
                currentDirection = Vector3.left;
                arenaPosition.y = arenaPositionForTileCenter.y;
            }
            else if (desiredDirection.x > 0 && currentDirection.x <= 0 && Arena.IsCharacterAllowedinTile(currentTile.x + 1, currentTile.y, allowedInGhostNest) == true)
            {
                currentDirection = Vector3.right;
                arenaPosition.y = arenaPositionForTileCenter.y;
            }
        }

        private void TestForTeleportation(Vector3 oldPosition, ref Vector3 newPosition)
        {
            foreach (Teleporter teleporer in Arena.Teleporters)
            {
                if (teleporer.TestForTeleportation(oldPosition, ref newPosition) == true)
                {
                    // Done a teleportaion? just return.
                    return;
                }
            }
        }
    }
}
