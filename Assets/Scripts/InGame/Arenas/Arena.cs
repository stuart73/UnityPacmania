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
        public Vector2Int PacmanStartTile { get; protected set; }
        public Vector2Int BonusTile { get; protected set; }
        public Vector2Int NestTile { get; protected set; }
        public Vector2Int NestEntranceTile { get; protected set; }
        public Vector2Int TopleftTile { get { return new Vector2Int(0, 0); } }
        public Vector2Int TopRightTile { get { return new Vector2Int(Width() - 1, 0); } }
        public Vector2Int BottomLeftTile { get { return new Vector2Int(0, Height() - 1); } }
        public Vector2Int BottomRightTile { get { return new Vector2Int(Width() - 1, Height() - 1); } }
        public int TileWidthPixels { get; protected set; }
        public int TileHalfWidthPixels { get; private set; }
        public int TileHeightPixels { get; protected set; }
        public int TileHalfHeightPixels { get; private set; }   
        public ArenaWrapper ArenaWrapper { get; private set; }
        public int InitialNumberOfPellets { get; private set; }

        public const float spritePixelPerUnit = 100.0f;
        protected float pixelArtTileAspect;
        private float isometricYScale = 0;

        protected abstract int[,] TileTypeMap { get; }
        protected abstract int[,] TileOrderMap { get; }
        private Pickup[,] tilePickupsMap = null;

        private const float arenaScreenWidth = 5.12f;

        public int Width() => TileTypeMap.GetLength(1);
        public int Height() => TileTypeMap.GetLength(0);
        private enum TileType
        {
            empty = 0,
            pellet = 1,
            powerPellet = 2,
            wall = 4
        }
        protected virtual void Awake()
        {
            TileHalfWidthPixels = TileWidthPixels / 2;
            TileHalfHeightPixels = TileHeightPixels / 2;

            // Calculate the ratio between the isometric pixel art tile's dimensions ratio against our actual tile dimensions ratio. 
            float tileAspect = TileWidthPixels / (float)TileHeightPixels;
            isometricYScale = pixelArtTileAspect / tileAspect;

            tilePickupsMap = new Pickup[Height(), Width()];
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
                    TileType tileContent = GetTileType(x, y);
                    GameObject localPellet = null;

                    if (tileContent == TileType.pellet)
                    {
                        localPellet = InstantiatePellet(pellet.gameObject, x, y, gameObject.transform);
                    }
                    else if (tileContent == TileType.powerPellet)
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
            tilePickupsMap[y, x] = pellet.GetComponent<Pickup>();
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
                    if (tilePickupsMap[y, x] != null)
                    {
                        SpriteRenderer[] children = tilePickupsMap[y, x].GetComponentsInChildren<SpriteRenderer>();
                        foreach (var spriteRenderer in children)
                        {
                            spriteRenderer.enabled = false;
                        }
                    }
                }
            }
        }

        private TileType GetTileType(int x, int y)
        {
            if (Width() < x || x < 0 || Height() < y || y < 0)
            {
                return TileType.empty;
            }
            return (TileType)TileTypeMap[y, x];
        }

        public bool ContainsPickup<T>() where T : Pickup
        {
            for (int y = 0; y < Height(); y++)
            {
                for (int x = 0; x < Width(); x++)
                {
                    if (tilePickupsMap[y, x] != null && tilePickupsMap[y, x].GetType() == typeof(T))
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

        private bool IsTileValid(Vector2Int tile) => tile.x < Width() && tile.x >= 0 && tile.y < Height() && tile.y >= 0;

        public Pickup GetTilePickUp(Vector2Int tile)
        {
            if (IsTileValid(tile) == false)
            {
                return null;
            }
            return tilePickupsMap[tile.y, tile.x];
        }

        public void SetTilePickUp(Vector2Int tile, Pickup pickup)
        {
            if (IsTileValid(tile) == false)
            {
                return;
            }

            // This there is a pickup already there, then destory it.
            if (tilePickupsMap[tile.y, tile.x] != null && pickup != null)
            {
                Destroy(tilePickupsMap[tile.y, tile.x].gameObject);
            }
            tilePickupsMap[tile.y, tile.x] = pickup;
        }

   
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

            return TileTypeMap[tile.y, tile.x] != (int)TileType.wall;
        }

        float CalculateZOrder(float x, float y)
        {
            float w = Width() * TileWidthPixels;  // w = total number of pixels horizontally.
            float z = -0.00001f * ((y * w) + x);
            return z;
        }

        public Vector3 GetTransformPositionFromArenaPosition(Vector3 arenaPosition)
        {
            if (isometric == true)
            {
                float z = CalculateZOrder(arenaPosition.x, arenaPosition.y);

                arenaPosition.x -= (PacmanStartTile.x * TileWidthPixels + TileHalfWidthPixels);
                arenaPosition.y -= (PacmanStartTile.y * TileHeightPixels + TileHalfHeightPixels);
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
