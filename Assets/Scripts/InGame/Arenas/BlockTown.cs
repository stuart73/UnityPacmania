﻿using UnityEngine;
using Pacmania.Utilities.Record;

namespace Pacmania.InGame.Arenas
{
    public class BlockTown : Arena
    {
        private static readonly int[,] tiles = { { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
                     { 4, 1, 4, 4, 1, 4, 1, 4, 1, 4, 4, 1, 4 },
                     { 4, 2, 4, 4, 1, 4, 1, 4, 1, 4, 4, 2, 4 },
                     { 4, 1, 4, 4, 1, 1, 1, 1, 1, 4, 4, 1, 4 },
                     { 4, 1, 1, 1, 1, 4, 0, 4, 1, 1, 1, 1, 4 },
                     { 4, 4, 1, 4, 1, 4, 0, 4, 1, 4, 1, 4, 4 },
                     { 4, 4, 1, 4, 1, 4, 4, 4, 1, 4, 1, 4, 4 },
                     { 4, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 4 },
                     { 4, 1, 4, 4, 1, 4, 1, 4, 1, 4, 4, 1, 4 },
                     { 4, 1, 4, 1, 1, 4, 1, 4, 1, 1, 4, 1, 4 },
                     { 4, 1, 4, 1, 4, 4, 1, 4, 4, 1, 4, 1, 4 },
                     { 4, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 4 },
                     { 4, 1, 4, 4, 1, 4, 1, 4, 1, 4, 4, 1, 4 },
                     { 4, 2, 4, 4, 1, 4, 1, 4, 1, 4, 4, 2, 4 },
                     { 4, 1, 4, 4, 1, 1, 1, 1, 1, 4, 4, 1, 4 },
                     { 4, 1, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 } };

        private static readonly int[,] order = {
                     { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 },
                     { 100, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 900 },
                     { 100, 110, 120, 120, 130, 140, 150, 160, 170, 180, 180, 190, 900 },
                     { 100, 110, 120, 120, 130, 140, 150, 160, 170, 180, 180, 190, 900 },
                     { 100, 110, 120, 120, 130, 140, 170, 170, 170, 180, 180, 190, 900 },
                     { 100, 130, 130, 130, 130, 240, 250, 260, 270, 190, 190, 190, 900 },
                     { 100, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 900 },
                     { 100, 200, 210, 220, 230, 240, 260, 260, 270, 280, 290, 300, 900 },
                     { 100, 210, 230, 230, 270, 270, 270, 270, 270, 300, 310, 310, 900 },
                     { 100, 210, 280, 280, 300, 320, 310, 340, 310, 320, 320, 360, 900 },
                     { 100, 210, 280, 290, 300, 320, 330, 340, 350, 350, 380, 400, 900 },
                     { 100, 210, 280, 290, 320, 320, 330, 360, 360, 370, 380, 400, 900 },
                     { 100, 290, 290, 330, 330, 330, 375, 380, 380, 400, 400, 400, 900 },
                     { 100, 290, 340, 340, 350, 360, 380, 390, 400, 410, 410, 420, 900 },
                     { 100, 290, 340, 340, 350, 360, 380, 390, 400, 410, 410, 420, 900 },
                     { 100, 290, 340, 340, 370, 370, 400, 400, 400, 410, 410, 420, 900 },
                     { 100, 350, 350, 350, 350, 410, 410, 410, 420, 420, 420, 420, 900 },
                     { 100, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900 } };

        protected override void Awake() 
        {
            PacmanStartTile = new Vector2Int(6, 12);
            BonusTile = new Vector2Int(6, 8);
            NestTile = new Vector2Int(6, 6);
            NestEntranceTile = new Vector2Int(6, 4);

            TileWidthPixels = 32;
            TileHeightPixels = 28;
            pixelArtTileAspect = 32.0f / 24.0f;
            base.Awake();

         }

        protected override int[,] TileTypeMap
        {
            get { return tiles; }
        }

        protected override int[,] TileOrderMap
        {
            get { return order; }
        }
    }
}
