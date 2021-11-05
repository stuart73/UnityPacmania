﻿using UnityEngine;

namespace Pacmania.InGame.Arenas
{
    public class PacmanPark : Arena
    {
        // Start is called before the first frame update
        private static readonly int[,] tiles = {
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 2, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 2, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 4, 1, 1, 4, 1, 1, 4, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 4, 0, 0, 0, 0, 0, 0, 4, 4, 1, 4, 0, 0, 0, 0, 0, 4, 1, 4, 4, 0, 0, 0, 0, 0, 4 },
                     { 4, 4, 4, 4, 4, 4, 0, 4, 4, 1, 4, 0, 4, 0, 4, 0, 4, 1, 4, 4, 0, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 0, 0, 0, 1, 0, 0, 4, 4, 4, 0, 0, 1, 0, 0, 0, 4, 4, 4, 4, 4 },
                     { 4, 0, 0, 0, 0, 0, 0, 4, 4, 1, 4, 0, 0, 0, 0, 0, 4, 1, 4, 4, 0, 0, 0, 0, 0, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 0, 4, 4, 4, 0, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 4, 4, 1, 4, 4, 1, 4, 1, 4, 4, 1, 4, 4, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 2, 1, 4, 1, 1, 1, 1, 0, 1, 1, 1, 1, 4, 1, 2, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 1, 4, 1, 4, 1, 4, 4, 4, 1, 4, 1, 4, 1, 4, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 4, 1, 1, 4, 1, 1, 4, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 1, 4, 1, 4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4 },
                     { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 } };

        
        private static readonly int[,] order= {
                     { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 },
                     { 100, 100, 100, 100, 100, 100, 110, 110, 110, 110, 110, 100, 100, 690, 695, 695, 695, 695, 695, 695, 695, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 660, 670, 670, 675, 680, 680, 685, 690, 695, 700, 700, 705, 710, 710, 775, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 660, 670, 670, 675, 680, 680, 685, 690, 695, 700, 700, 705, 710, 710, 775, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 715, 715, 715, 715, 715, 715, 715, 715, 695, 720, 705, 705, 775, 775, 775, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 715, 720, 720, 725, 735, 736, 737, 737, 737, 755, 758, 775, 780, 780, 785, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 725, 725, 725, 725, 735, 738, 738, 750, 755, 755, 758, 785, 785, 785, 785, 790, 790, 790, 790, 790 },
                     { 100, 100, 100, 100, 100, 100, 730, 730, 730, 734, 740, 740, 745, 750, 755, 758, 758, 775, 800, 800, 800, 800, 100, 100, 100, 100 },
                     { 100, 735, 735, 725, 725, 725, 735, 730, 730, 734, 740, 745, 745, 755, 759, 759, 770, 775, 800, 800, 825, 825, 825, 825, 825, 100 },
                     { 100, 733, 733, 733, 733, 733, 735, 730, 730, 735, 740, 755, 760, 765, 760, 765, 770, 775, 800, 800, 880, 880, 880, 880, 880, 880 },
                     { 100, 733, 733, 733, 733, 733, 737, 737, 737, 735, 745, 745, 760, 760, 760, 765, 775, 775, 801, 801, 801, 880, 880, 880, 880, 880 },
                     { 100, 745, 745, 745, 745, 745, 745, 810, 810, 815, 820, 825, 825, 825, 825, 825, 870, 875, 802, 802, 803, 803, 803, 803, 803, 803 },
                     { 800, 800, 800, 800, 800, 800, 810, 810, 810, 815, 820, 825, 835, 835, 835, 865, 870, 875, 880, 880, 885, 875, 875, 875, 875, 875 },
                     { 800, 800, 800, 800, 800, 800, 815, 815, 815, 825, 835, 845, 845, 860, 865, 875, 875, 875, 935, 935, 935, 950, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 825, 826, 826, 845, 850, 850, 855, 860, 865, 890, 890, 935, 940, 940, 945, 950, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 827, 827, 840, 845, 855, 855, 865, 865, 895, 895, 895, 895, 940, 945, 945, 950, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 830, 835, 840, 845, 900, 905, 908, 908, 908, 925, 930, 935, 940, 955, 960, 960, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 905, 895, 895, 895, 900, 909, 909, 920, 925, 925, 930, 945, 945, 965, 965, 980, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 905, 910, 910, 910, 910, 910, 915, 920, 925, 970, 970, 970, 970, 970, 974, 980, 990, 990, 990, 990 },
                     { 800, 800, 800, 800, 800, 800, 905, 975, 975, 975, 975, 975, 975, 975, 975, 975, 975, 975, 975, 975, 975, 980, 990, 990, 990, 990 },
                     { 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990 } };


        /*
        int[,] order3 = {
                     { ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., ..., ..., ..., 690, ..., ..., ..., ..., ..., ..., ..., 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, 200, 670, 670, ..., 680, 680, ..., 690, ..., 700, 700, ..., 710, 710, 200, 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., 670, 670, ..., 680, 680, ..., 690, ..., 700, 700, ..., 710, 710, ..., 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., 720, 720, ..., 735, ..., 737, 737, 737, ..., 758, ..., 780, 780, ..., 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., 735, ..., ..., 750, ..., ..., 758, ..., ..., ..., ..., 790, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, 730, 730, 730, ..., 740, 740, ..., 750, ..., 758, 758, ..., 800, 800, 800, 800, ###, ###, ###, ### },
                     { ###, 000, 000, 000, 000, 000, 000, 730, 730, ..., 740, 000, 000, 000, 000, 000, 770, ..., 800, 800, 000, 000, 000, 000, 000, ### },
                     { ###, 733, 733, 733, 733, 733, 000, 730, 730, ..., 740, 000, 760, 000, 760, 000, 770, ..., 800, 800, 000, 880, 880, 880, 880, 880 },
                     { ###, 733, 733, 733, 733, 733, 000, 000, 000, ..., 000, 000, 760, 760, 760, 000, 000, ..., 875, 875, 000, 880, 880, 880, 880, 880 },
                     { ###, 000, 000, 000, 000, 000, 000, 810, 810, ..., 820, 000, 000, 000, 000, 000, 870, ..., 880, 880, 885, 885, 885, 885, 885, ### },
                     { ###, ###, ###, ###, ###, ###, 810, 810, 810, ..., 820, 000, 835, 835, 835, 000, 870, ..., 880, 880, 885, 875, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., ..., ..., ..., 860, ..., ..., ..., ..., ..., ..., ..., 950, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., 826, 826, ..., 850, 850, ..., 860, ..., 890, 890, ..., 940, 940, ..., 950, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, 200, ..., 840, ..., ..., ..., ..., 000, ..., ..., ..., ..., 940, ..., 200, 950, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, 830, ..., 840, ..., 900, ..., 908, 908, 908, ..., 930, ..., 940, ..., 960, 960, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., 900, ..., ..., 920, ..., ..., 930, ..., ..., ..., ..., 980, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., 910, 910, 910, 910, 910, ..., 920, ..., 970, 970, 970, 970, 970, ..., 980, ###, ###, ###, ### },
                     { ###, ###, ###, ###, ###, ###, ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., ..., 980, ###, ###, ###, ### },
                     { 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990, 990 } };

        */

        protected override void Awake()
        {
            PacmanStartTile = new Vector2Int(13, 15);
            BonusTile = new Vector2Int(13, 11);
            NestTile = new Vector2Int(13, 9);
            NestEntranceTile = new Vector2Int(13, 8);

            TileWidthPixels = 24;
            TileHeightPixels = 28;
            pixelArtTileAspect = 1.0f;
            base.Awake();

        }

        protected override int[,] TileTypeMap
        {
            get { return tiles; }
        }

        protected override int[,] TileOrderMap
        {
            get {  return order;  }
        }
    }
}
