using System.Collections.Generic;
using UnityEngine;
using Pacmania.Audio;
using Pacmania.Utilities.Record;
using Pacmania.InGame.Pickups;

namespace Pacmania.InGame.Arenas
{
    public abstract class Arena : MonoBehaviour
    {
        [SerializeField] private List<Teleporter> teleporters = new List<Teleporter>();
        public List<Teleporter> Teleporters
        {
            get { return teleporters; }
        }

        [SerializeField] private Pellet pellet = default;
        [SerializeField] private PowerPellet powerPellet = default;
        [SerializeField] private SoundType music = default;
        [SerializeField] private bool generateWraparoundPellets = true;
        [SerializeField] private bool isometric = true;
        public SoundType Music
        {
            get { return music; }
        }
     
        public virtual RecordKeyboard.KeyboardSnapshot[] DemoSteps { get { return null; } }
        public Vector2Int CenterTile { get; protected set; }
        public Vector2Int BonusTile { get; protected set; }
        public Vector2Int NestTile { get; protected set; }
        public Vector2Int NestEntranceTile { get; protected set; }
        public Vector2Int TopleftTile { get { return new Vector2Int(0, 0); } }
        public Vector2Int TopRightTile { get { return new Vector2Int(TileMap.GetLength(1) - 1, 0); } }
        public Vector2Int BottomLeftTile { get { return new Vector2Int(0, TileMap.GetLength(0) - 1); } }
        public Vector2Int BottomRightTile { get { return new Vector2Int(TileMap.GetLength(1) - 1, TileMap.GetLength(0) - 1); } }
        public int TileWidthPixels { get; protected set; }
        public int TileHalfWidthPixels { get; protected set; }
        public int TileHeightPixels { get; protected set; }
        public int TileHalfHeightPixels { get; protected set; }   
        public ArenaWrapper ArenaWrapper { get; private set; }
        public int InitialNumberOfPellets { get; private set; } = 0;
        public float PixelArtTileAspect { get; protected set; } = 0;

        protected abstract int[,] TileMap { get; }
        protected abstract int[,] TileOrderMap { get; }

        public const float spritePixelPerUnit = 100.0f;
        private const float arenaScreenWidth = 5.12f;

        private Pickup[,] tilePickups = null;

        private void Awake()
        {
            tilePickups = new Pickup[TileMap.GetLength(0), TileMap.GetLength(1)];
            ArenaWrapper = GetComponent<ArenaWrapper>();
        }

        private void Start()
        {
            GeneratePellets();
        }

        private void GeneratePellets()
        {
            for (int y = 0; y < Height(); y++)
            {
                for (int x = 0; x < Width(); x++)
                {
                    int tileContent = GetTileType(x, y);
                    GameObject localPellet = null;

                    if (tileContent == 1)
                    {
                        localPellet = InstantiatePellet(pellet.gameObject, x, y, gameObject.transform);
                    }
                    else if (tileContent == 2)
                    {
                        localPellet = InstantiatePellet(powerPellet.gameObject, x, y, null);
                        Renderer renderer = localPellet.GetComponent<Renderer>();
                        renderer.sortingOrder = TileOrderMap[y, x];
                    }

                    if (localPellet != null && generateWraparoundPellets == true)
                    {
                        DuplicateObjectForWrapAround(localPellet);
                    }
                }
            }
        }

        private void DuplicateObjectForWrapAround(GameObject gameObject)
        {
            GameObject clone = Instantiate(gameObject, new Vector3(gameObject.transform.position.x + arenaScreenWidth, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            GameObject clone2 = Instantiate(gameObject, new Vector3(gameObject.transform.position.x - arenaScreenWidth, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

            clone2.transform.parent = gameObject.transform;
            clone.transform.parent = gameObject.transform;
        }

        private GameObject InstantiatePellet(GameObject prefab, int x, int y, Transform parent)
        {
            Vector3 arenaPosition = GetArenaPositionForTileCenter(new Vector2Int(x, y));
            arenaPosition.y += 1;
            Vector3 screenPosition = GetTransformPositionFromArenaPosition(arenaPosition);
            GameObject pellet = Instantiate(prefab, screenPosition, Quaternion.identity, parent);
            tilePickups[y, x] = pellet.GetComponent<Pickup>();
            InitialNumberOfPellets++; 
            return pellet;
        }

        public int GetTileSortingOrder(Vector2Int tile) => TileOrderMap[tile.y, tile.x];

        public void HideAllPickups()
        {
            for (int y = 0; y < Height(); y++)
            {
                for (int x = 0; x < Width(); x++)
                {
                    if (tilePickups[y, x] != null)
                    {
                        SpriteRenderer[] children = tilePickups[y, x].GetComponentsInChildren<SpriteRenderer>();
                        foreach (var spriteRenderer in children)
                        {
                            spriteRenderer.enabled = false;
                        }
                    }
                }
            }
        }

        public int GetTileType(int x, int y)
        {
            if (TileMap.GetLength(1) < x || x < 0 || TileMap.GetLength(0) < y || y < 0)
            {
                return -1;
            }
            return TileMap[y, x];
        }

        public bool ContainsPickup<T>() where T : Pickup
        {
            for (int y = 0; y < Height(); y++)
            {
                for (int x = 0; x < Width(); x++)
                {
                    if (tilePickups[y, x] != null && tilePickups[y, x].GetType() == typeof(T))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetOrder(Vector2Int tile)
        {
            if (IsTileValid(tile) == false)
            {
                return -1;
            }
            return TileOrderMap[tile.y, tile.x];
        }

        private bool IsTileValid(Vector2Int tile) => tile.x < tilePickups.GetLength(1) && tile.x >= 0 && tile.y < tilePickups.GetLength(0) && tile.y >= 0;

        public Pickup GetTilePickUp(Vector2Int tile)
        {
            if (IsTileValid(tile) == false)
            {
                return null; ;
            }

            Pickup result;
            try
            {
                result = tilePickups[tile.y, tile.x];
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public void SetTilePickUp(Vector2Int tile, Pickup pickup)
        {
            if (IsTileValid(tile) == false)
            {
                return;
            }

            // This there is a pickup already there, then destory it.
            if (tilePickups[tile.y, tile.x] != null && pickup != null)
            {
                Destroy(tilePickups[tile.y, tile.x].gameObject);
            }
            tilePickups[tile.y, tile.x] = pickup;
        }

        public int Width() => TileMap.GetLength(1);

        public int Height() => TileMap.GetLength(0);

        public bool IsCharacterAllowedInTile(Vector3 position, bool allowedInNestTile)
        {
            Vector2Int tile = GetTileForArenaPosition(position);
            return IsCharacterAllowedinTile(tile, allowedInNestTile);
        }

        public bool IsCharacterAllowedinTile(int x, int y, bool allowedInNestTile) => IsCharacterAllowedinTile(new Vector2Int(x, y), allowedInNestTile);

        public bool IsCharacterAllowedinTile(Vector2Int tile, bool allowedInNestTile)
        {
            if (IsTileValid(tile) == false)
            {
                return false;
            }
            if (allowedInNestTile == false && tile.x == NestTile.x && tile.y == NestTile.y)
            {
                return false;
            }

            return TileMap[tile.y, tile.x] <= 3;
        }

        float CalculateZOrder(float x, float y)
        {
            float w = TileMap.GetLength(1) * TileWidthPixels;  // w = total number of pixels horizontally.
            float z = -0.00001f * ((y * w) + x);
            return z;
        }

        public Vector3 GetTransformPositionFromArenaPosition(Vector3 arenaPosition)
        {
            if (isometric == true)
            {
                float z = CalculateZOrder(arenaPosition.x, arenaPosition.y);

                // y scale is a ratio between the isometric pixel art tile's dimensions ratio against our actual tile dimensions ratio. 
                float isometricYScale = PixelArtTileAspect / (((float)TileWidthPixels) / ((float)TileHeightPixels));

                arenaPosition.x -= (CenterTile.x * TileWidthPixels + TileHalfWidthPixels);
                arenaPosition.y -= (CenterTile.y * TileHeightPixels + TileHalfHeightPixels);
                arenaPosition.y /= isometricYScale;

                arenaPosition.x -= (arenaPosition.y / 2);  // All isometric pixel art angle is a 1:2 ratio. i.e. 1 pixel x for 2 y.
                arenaPosition.y -= (arenaPosition.z);  // Add jump to y, this is literal pixels (i.e same as x-axis).

                float x = arenaPosition.x / spritePixelPerUnit;
                float y = -arenaPosition.y / spritePixelPerUnit;

                return new Vector3(x, y, z);
            }

            return arenaPosition;
        }

        public Vector2Int GetTileForArenaPosition(Vector3 postion)
        {
            int tx = (int)(postion.x / TileWidthPixels);
            int ty = (int)(postion.y / TileHeightPixels);

            return new Vector2Int(tx, ty);
        }

        public Vector3 GetArenaPositionForTileCenter(Vector2Int tile) => new Vector3((tile.x * TileWidthPixels) + TileHalfWidthPixels, (tile.y * TileHeightPixels) + TileHalfHeightPixels, 0);

        public Vector3 GetArenaPositionForTileCenter(Vector3 arenaPostion) => GetArenaPositionForTileCenter(GetTileForArenaPosition(arenaPostion));
    }
}
