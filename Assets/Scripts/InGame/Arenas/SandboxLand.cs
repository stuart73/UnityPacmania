﻿using UnityEngine;

namespace Pacmania.InGame.Arenas
{
    public class SandboxLand : Arena
    {
        // Start is called before the first frame update
        private static readonly int[,] tiles = {
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 4, 4, 4, 1, 1, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 2, 4, 1, 1, 1, 1, 1, 4, 2, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 0, 4, 4, 1, 4, 4, 4, 4 },
                     { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 0, 4, 4, 1, 1, 1, 0, 0 },
                     { 4, 4, 4, 1, 4, 1, 4, 1, 4, 1, 4, 4, 4, 4, 4, 1, 4, 1, 4, 0 },
                     { 4, 4, 4, 1, 1, 1, 4, 1, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1, 4, 0 },
                     { 4, 4, 4, 1, 4, 1, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 0 },
                     { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1, 0, 0 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 0, 1, 1, 1, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 4, 1, 4, 1, 4, 1, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 1, 1, 4, 4, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 4, 1, 1, 1, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 1, 4, 4, 4, 1, 4, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 2, 1, 1, 1, 1, 1, 1, 1, 2, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 1, 4, 4, 4, 1, 4, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 } };


        private static readonly int[,] order = {
                     { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                     { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                     { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
                     { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },
                     { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 },
                     { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 },
                     { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                     { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 },
                     { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 },
                     { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 },
                     { 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13 },
                     { 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14 },
                     { 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 },
                     { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                     { 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17 },
                     { 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18 },
                     { 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19 },
                     { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 } };


        protected override void Awake()
        {
            PacmanStartTile = new Vector2Int(12, 13);
            BonusTile = new Vector2Int(12, 11);
            NestTile = new Vector2Int(12, 6);
            NestEntranceTile = new Vector2Int(12, 4);

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
