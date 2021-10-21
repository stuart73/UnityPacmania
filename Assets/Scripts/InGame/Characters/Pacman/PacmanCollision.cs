using System;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.InGame.ScoreSprites;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Characters.Ghost.GhostStates;
using Pacmania.InGame.Pickups;

namespace Pacmania.InGame.Characters.Pacman
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PacmanCollision : MonoBehaviour
    {
        [SerializeField] private bool invincible = false;  // for debug
        public bool Invincible
        {
            get => invincible;
            set => invincible = value;
        }

        private CharacterMovement characterMovement;
        private Level level;
        // The distance between the center of pacman and something else which is considered touching. We square this number for efficiency.
        private const float touchingDistanceSqr = 8 * 8;  // 8 pixels

        public event Action Dying = delegate { };    
        public event Action EatenPellete = delegate { };
  
        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            level = FindObjectOfType<Level>();
        }

        private void Start()
        {
        }

        // Called from pacman controller
        public void CheckCollisions()
        {
            if (enabled == false) return;

            CheckIfTouchedPickup();
            CheckIfTouchedGhost();
        }

        private void CheckIfTouchedPickup()
        {
            Vector2Int pacmanTile = characterMovement.GetTileIn();
            Pickup pickup = level.Arena.GetTilePickUp(pacmanTile);

            if (pickup == null)  return;

            if (IsTouchingPacman(level.Arena.GetArenaPositionForTileCenter(pacmanTile)) == true)
            {
                Destroy(pickup.gameObject);
                level.Arena.SetTilePickUp(pacmanTile, null);
                pickup.OnPickedUp();

                if (pickup is Pellet || pickup is PowerPellet)
                {
                    EatenPellete();
                } 
            }
        }

        private void CheckIfTouchedGhost()
        {
            GhostController[] ghosts = level.GhostManager.Ghosts;
            foreach (GhostController ghost in ghosts)
            {
                if (IsTouchingPacman(ghost.GetComponent<CharacterMovement>().ArenaPosition) == true)
                {
                    OnTouchedGhost(ghost, out bool actionOccured);

                    // If some action occured because we touched a ghost then break out now.
                    if (actionOccured == true)
                    {
                        break;
                    }
                }
            }
        }

        private void OnTouchedGhost(GhostController ghost, out bool actionOccured)
        {
            actionOccured = false;
            Type state = ghost.FSM.CurrrentState.GetType();

            if (state == typeof(FrightenState))
            {
                ghost.Eaten();            
                actionOccured = true;
            }
            else if (ghost.IsDeadly() == true && invincible == false)
            {
                Dying.Invoke();
                actionOccured = true;
            }
        }

        private bool IsTouchingPacman(Vector3 itemPos)
        {
            return (itemPos - characterMovement.ArenaPosition).sqrMagnitude < touchingDistanceSqr;
        }

    }
}
