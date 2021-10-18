using UnityEngine;
using Pacmania.InGame.Characters;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class InkyAI : GhostAI
    {
        private CharacterMovement blinkyMovement = null;
        protected override void Awake()
        {
            base.Awake();
            GhostAI blinky = FindObjectOfType<BlinkyAI>();

            if (blinky != null)
            {
                blinkyMovement = blinky.GetComponent<CharacterMovement>();
            }

            if (blinkyMovement == null)
            {
                Debug.LogError("Inky ghost could not find blinky.  Inky AI will not work correctly without blinky.", this);
            }
        }

        public override Vector2Int ScatterPosition() 
        {
            return characterMovement.Arena.BottomLeftTile;    
        }

        public override Vector2Int ChasePosition()
        {
            Vector2Int pacmanTile = pacmanCharacterMovement.GetTileIn();

            Vector2Int direction = new Vector2Int(0, 0);
            if (pacmanCharacterMovement.CurrentDirection.x < 0)
            {
                direction.x = -1;
            }
            else if (pacmanCharacterMovement.CurrentDirection.x > 0)
            {
                direction.x = 1;
            }
            else if (pacmanCharacterMovement.CurrentDirection.y < 0)
            {
                direction.y = -1;
            }
            else if (pacmanCharacterMovement.CurrentDirection.y > 0)
            {
                direction.y = 1;
            }

            Vector2Int twoTilesInFrontOfPacman = pacmanTile + (direction * 2);

            if (blinkyMovement == null)
            {
                return pacmanTile;
            }

            Vector2Int blinkyTile = characterMovement.GetTileIn();
            Vector2Int vectorToBlinky = blinkyTile - twoTilesInFrontOfPacman;
            Vector2Int chasePosition = twoTilesInFrontOfPacman - vectorToBlinky;

            return chasePosition;
        }
    }
}
