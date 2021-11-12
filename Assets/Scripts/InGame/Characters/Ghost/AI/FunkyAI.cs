﻿
using UnityEngine;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class FunkyAI : GhostAI
    {
        public override Vector2Int GetScatterTile()
        {
            return GetChaseTile();
        }

        public override Vector2Int GetChaseTile()
        {
            return pacmanCharacterMovement.GetTileIn();
        }
    }

      
}
