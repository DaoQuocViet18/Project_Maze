using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;

    [Header("Prefabs")]
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject starPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int maxStars = 3;
    [SerializeField] private int maxCoins = 5;

    private void Start()
    {
        SpawnItemsNearWalls();
    }

    private void SpawnItemsNearWalls()
    {
        // Lấy tất cả các vị trí floor cạnh wall
        List<Vector3Int> validPositions = new List<Vector3Int>();
        BoundsInt bounds = floorTilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (floorTilemap.HasTile(tilePosition) && HasAdjacentWall(tilePosition))
                {
                    validPositions.Add(tilePosition);
                }
            }
        }

        // Xáo trộn danh sách vị trí để spawn ngẫu nhiên
        ShuffleList(validPositions);

        int positionIndex = 0;

        // Spawn Stars
        for (int i = 0; i < maxStars && positionIndex < validPositions.Count; i++)
        {
            SpawnItem(starPrefab, validPositions[positionIndex]);
            positionIndex++;
        }

        // Spawn Coins
        for (int i = 0; i < maxCoins && positionIndex < validPositions.Count; i++)
        {
            SpawnItem(coinPrefab, validPositions[positionIndex]);
            positionIndex++;
        }

        // Spawn Points ở các vị trí còn lại
        while (positionIndex < validPositions.Count)
        {
            SpawnItem(pointPrefab, validPositions[positionIndex]);
            positionIndex++;
        }
    }

    private void SpawnItem(GameObject prefab, Vector3Int tilePosition)
    {
        Vector3 worldPosition = floorTilemap.GetCellCenterWorld(tilePosition);
        Instantiate(prefab, worldPosition, Quaternion.identity);
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
        // Mảng các hướng để kiểm tra (trái, phải, trên, dưới)
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