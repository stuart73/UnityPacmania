using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class ClydeAI : GhostAI
    {
        public override Vector2Int ScatterPosition() 
        {
            return characterMovement.Arena.BottomRightTile;
        }
        
        public override Vector2Int ChasePosition()
        {
            Vector2Int pacmanTile = pacmanCharacterMovement.GetTileIn();
            Vector2Int ourTile = characterMovement.GetTileIn();

            // If within 8 tiles of pacman (i.e. 8*8 = 64) then use scatter target.
            if ((pacmanTile - ourTile).sqrMagnitude < 64)
            {
                return ScatterPosition();
            }

            // Else use pacman as target.
            return pacmanTile;
        }
    }
}
