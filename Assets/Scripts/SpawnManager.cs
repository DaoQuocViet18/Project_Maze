using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private GameObject pointPrefab;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask itemLayerMask; // Layer chứa Star và Coin
    [SerializeField] private LayerMask obstacleLayerMask; // Layer chứa obstacles

    private void Start()
    {
        SpawnPointsNearWalls();
    }

    private void SpawnPointsNearWalls()
    {
        List<Vector3Int> validPositions = new List<Vector3Int>();
        BoundsInt bounds = floorTilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 worldPosition = floorTilemap.GetCellCenterWorld(tilePosition);

                // Kiểm tra floor, wall, items và obstacles
                if (floorTilemap.HasTile(tilePosition) &&
                    HasAdjacentWall(tilePosition) &&
                    !HasItemAtPosition(worldPosition) &&
                    !HasObstacleAtPosition(worldPosition))
                {
                    validPositions.Add(tilePosition);
                }
            }
        }
        ShuffleList(validPositions);

        // Spawn Points
        foreach (Vector3Int position in validPositions)
        {
            SpawnPoint(position);
        }
    }

    private bool HasItemAtPosition(Vector3 position)
    {
        float checkRadius = 0.4f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, checkRadius, itemLayerMask);
        return colliders.Length > 0;
    }

    private bool HasObstacleAtPosition(Vector3 position)
    {
        float checkRadius = 0.4f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, checkRadius, obstacleLayerMask);
        return colliders.Length > 0;
    }

    private void SpawnPoint(Vector3Int tilePosition)
    {
        Vector3 worldPosition = floorTilemap.GetCellCenterWorld(tilePosition);
        Instantiate(pointPrefab, worldPosition, Quaternion.identity);
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private bool HasAdjacentWall(Vector3Int position)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(-1, 0, 0),  // Trái
            new Vector3Int(1, 0, 0),   // Phải
            new Vector3Int(0, 1, 0),   // Trên
            new Vector3Int(0, -1, 0)   // Dưới
        };

        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighborPos = position + dir;
            if (wallTilemap.HasTile(neighborPos))
            {
                return true;
            }
        }

        return false;
    }
}