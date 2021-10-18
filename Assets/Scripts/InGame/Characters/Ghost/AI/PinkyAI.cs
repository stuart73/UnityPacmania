using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class PinkyAI : GhostAI
    { 
        public override Vector2Int ScatterPosition()
        {
            return characterMovement.Arena.TopleftTile;  
        }

        public override Vector2Int ChasePosition()
        {
            // Chase position is 4 tiles in front of pacman.
            Vector3 currentPacManDirection = pacmanCharacterMovement.CurrentDirection;
            Vector2Int direction = new Vector2Int((int)currentPacManDirection.x, (int)currentPacManDirection.y);
            Vector2Int fourTilesInFrontOfPacman = pacmanCharacterMovement.GetTileIn() + (direction * 4);

            return fourTilesInFrontOfPacman;
        }
    }
}
