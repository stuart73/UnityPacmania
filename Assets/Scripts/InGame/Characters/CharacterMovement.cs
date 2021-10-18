using UnityEngine;
using Pacmania.InGame.Arenas;

namespace Pacmania.InGame.Characters
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed = 1.4f;
        public float DefaultSpeed
        {
            get { return defaultSpeed; }
            set { defaultSpeed = value; }
        }

        [SerializeField] private float speedIncreasePerLevel = 0.025f;
        [SerializeField] private bool allowedInGhostNest = false;
        [SerializeField] private Vector3 currentDirection;
        public Vector3 CurrentDirection
        {
            get { return currentDirection; }
        }

        public float SpeedCoefficient { get; set; } = 1;
        public Arena Arena { get; private set; }
        public Animator Animator { get; private set; }
        public bool Paused { get; set; } = true;

        public Vector3 ArenaPosition
        {
            get { return arenaPosition; }
            set { arenaPosition = value; }

        }
        private Vector3 arenaPosition;
        private Vector3 initialArenaPosition;
        private Vector2 initialDirection;
        private Jumping jumpingComponent;
        private const float maxSpeed = 2.0f;

        void Awake()
        {
            Arena = FindObjectOfType<Arena>();
            Level level = FindObjectOfType<Level>();
            Animator = GetComponent<Animator>();
            jumpingComponent = GetComponent<Jumping>();

            if (level != null)
            {
                defaultSpeed += (speedIncreasePerLevel * (level.LevelNumber - 1));
            }
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
            Animator.SetFloat("Horizontal", currentDirection.x);
            Animator.SetFloat("Vertical", -currentDirection.y);

            if (Arena != null)
            {
                GetComponent<SpriteRenderer>().sortingOrder = Arena.GetOrder(this.GetTileIn());
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

        public void Move(Vector2Int desiredDirection)
        {
            if (Paused == true || enabled == false)
            {
                return;
            }

            Vector3 oldArenaPosition = arenaPosition; // needed for teleportation test later

            // 180 change of direction.
            if (currentDirection.x < 0 && desiredDirection.x > 0)
            {
                currentDirection.x = 1;
            }
            else if (currentDirection.x > 0 && desiredDirection.x < 0)
            {
                currentDirection.x = -1;
            }
            else if (currentDirection.y < 0 && desiredDirection.y > 0)
            {
                currentDirection.y = 1;
            }
            else if (currentDirection.y > 0 && desiredDirection.y < 0)
            {
                currentDirection.y = -1;
            }

            Vector2Int currentTile = Arena.GetTileForArenaPosition(arenaPosition);
            Vector3 arenaPositionForTileCenter = Arena.GetArenaPositionForTileCenter(currentTile);

            // Change direction if in tile center?
            if (IsInTileCenter())
            {
                if (desiredDirection.y > 0 && Arena.IsCharacterAllowedinTile(currentTile.x, currentTile.y + 1, allowedInGhostNest) == true)
                {
                    currentDirection = new Vector3(0, 1, 0);
                    arenaPosition.x = arenaPositionForTileCenter.x;
                }
                else if (desiredDirection.y < 0 && Arena.IsCharacterAllowedinTile(currentTile.x, currentTile.y - 1, allowedInGhostNest) == true)
                {
                    currentDirection = new Vector3(0, -1, 0);
                    arenaPosition.x = arenaPositionForTileCenter.x;
                }
                else if (desiredDirection.x < 0 && Arena.IsCharacterAllowedinTile(currentTile.x - 1, currentTile.y, allowedInGhostNest) == true)
                {
                    currentDirection = new Vector3(-1, 0, 0);
                    arenaPosition.y = arenaPositionForTileCenter.y;
                }
                else if (desiredDirection.x > 0 && Arena.IsCharacterAllowedinTile(currentTile.x + 1, currentTile.y, allowedInGhostNest) == true)
                {
                    currentDirection = new Vector3(1, 0, 0);
                    arenaPosition.y = arenaPositionForTileCenter.y;
                }
            }

            arenaPosition += currentDirection * CurrentSpeed();

            // Collision test with walls.
            if ((currentDirection.x < 0 && Arena.IsCharacterAllowedInTile(new Vector3(arenaPosition.x - Arena.TileHalfWidthPixels, arenaPosition.y, 0), allowedInGhostNest) == false) ||
                (currentDirection.x > 0 && Arena.IsCharacterAllowedInTile(new Vector3(arenaPosition.x + Arena.TileHalfWidthPixels, arenaPosition.y, 0), allowedInGhostNest) == false) ||
                (currentDirection.y > 0 && Arena.IsCharacterAllowedInTile(new Vector3(arenaPosition.x, arenaPosition.y + Arena.TileHalfHeightPixels, 0), allowedInGhostNest) == false) ||
                (currentDirection.y < 0 && Arena.IsCharacterAllowedInTile(new Vector3(arenaPosition.x, arenaPosition.y - Arena.TileHalfHeightPixels, 0), allowedInGhostNest) == false))
            {
                // Partly inside a wall, so move us back to the center of the current tile.
                arenaPosition = arenaPositionForTileCenter;
            }

            if (jumpingComponent != null)
            {
                jumpingComponent.UpdateJumping();
            }

            TestForTeleportation(oldArenaPosition, ref arenaPosition);

            transform.position = Arena.GetTransformPositionFromArenaPosition(arenaPosition);

            SetAnimationAndSpriteOrder();

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
