using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class SueAI : GhostAI
    {
        public override Vector2Int ScatterPosition()       
        {
            return ChasePosition();
        }

        public override Vector2Int ChasePosition()
        {
            return pacmanCharacterMovement.GetTileIn();
        } 
    }
}