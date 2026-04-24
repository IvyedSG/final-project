using UnityEngine;
using UnityEngine.Tilemaps;

namespace MetalSlugPE.World
{
    public class InfiniteGroundTilemap : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase groundTile;
        [SerializeField] private Transform target;
        [SerializeField] private int groundY = -9;
        [SerializeField] private int halfWidthInTiles = 60;

        private bool initialized;
        private int generatedMinX;
        private int generatedMaxX;

        private void Awake()
        {
            if (tilemap == null)
            {
                tilemap = GetComponent<Tilemap>();
            }
        }

        private void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }

            if (groundTile == null)
            {
                groundTile = tilemap.GetTile(new Vector3Int(0, groundY, 0));
            }

            if (tilemap == null || groundTile == null)
            {
                enabled = false;
                return;
            }

            EnsureGroundAround(GetTargetCellX());
        }

        private void Update()
        {
            if (!enabled)
            {
                return;
            }

            EnsureGroundAround(GetTargetCellX());
        }

        private int GetTargetCellX()
        {
            if (target == null)
            {
                return 0;
            }

            return tilemap.WorldToCell(target.position).x;
        }

        private void EnsureGroundAround(int centerX)
        {
            int desiredMin = centerX - halfWidthInTiles;
            int desiredMax = centerX + halfWidthInTiles;

            if (!initialized)
            {
                FillRange(desiredMin, desiredMax);
                generatedMinX = desiredMin;
                generatedMaxX = desiredMax;
                initialized = true;
                return;
            }

            if (desiredMin < generatedMinX)
            {
                FillRange(desiredMin, generatedMinX - 1);
                generatedMinX = desiredMin;
            }

            if (desiredMax > generatedMaxX)
            {
                FillRange(generatedMaxX + 1, desiredMax);
                generatedMaxX = desiredMax;
            }
        }

        private void FillRange(int minX, int maxX)
        {
            for (int x = minX; x <= maxX; x++)
            {
                tilemap.SetTile(new Vector3Int(x, groundY, 0), groundTile);
            }
        }
    }
}
