using UnityEngine;

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

        public override Vector2Int GetScatterTile() 
        {
            return characterMovement.Arena.BottomLeftTile;    
        }

        public override Vector2Int GetChaseTile()
        {
            Vector2Int pacmanTile = pacmanCharacterMovement.GetTileIn();

            if (blinkyMovement == null)
            {
                // No blinky? just return pacman tile.
                return pacmanTile;
            }

            // Chase position is a vector two tiles in front of pacman to blinky position and then rotating this vector 
            // from pacman by 180 degrees. 
            // Basically we get a target position that is near opposite to blinky compared to pacman. This has Inky
            // trying to cut pacman's escape path off.

            Vector3 currentPacManDirection = pacmanCharacterMovement.CurrentDirection;
            Vector2Int direction = new Vector2Int((int)currentPacManDirection.x, (int)currentPacManDirection.y);
            Vector2Int twoTilesInFrontOfPacman = pacmanTile + (direction * 2);

            Vector2Int blinkyTile = blinkyMovement.GetTileIn();
            Vector2Int vectorToBlinky = blinkyTile - twoTilesInFrontOfPacman;
            Vector2Int chasePosition = twoTilesInFrontOfPacman - vectorToBlinky;

            return chasePosition;
        }
    }
}
