using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform playerStart;
    [SerializeField] private Transform goal;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask itemLayerMask; // Layer chứa Star và Coin
    [SerializeField] private LayerMask obstacleLayerMask; // Layer chứa obstacles

    [Header("Debug")]
    [SerializeField] private bool showDebugPath = true;
    [SerializeField] private Color pathColor = Color.yellow;
    [SerializeField] private Color pointColor = Color.green;
    private List<Vector3> debugPath = new List<Vector3>();

    private void Start()
    {
        if (!playerStart || !goal)
        {
            Debug.LogError("Missing playerStart or goal reference!");
            return;
        }

        Vector3Int startPos = floorTilemap.WorldToCell(playerStart.position);
        Vector3Int goalPos = floorTilemap.WorldToCell(goal.position);
        
        List<Vector3Int> path = FindPath(startPos, goalPos);
        if (path != null && path.Count > 0)
        {
            // Lưu đường đi để vẽ trong OnDrawGizmos
            debugPath = path.ConvertAll(p => floorTilemap.GetCellCenterWorld(p));
            SpawnPointsAlongPath(path);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugPath) return;

        // Vẽ vị trí start và goal
        if (playerStart && goal)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerStart.position, 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(goal.position, 0.3f);
        }

        // Vẽ đường đi
        if (debugPath != null && debugPath.Count > 0)
        {
            Gizmos.color = pathColor;
            for (int i = 0; i < debugPath.Count - 1; i++)
            {
                Gizmos.DrawLine(debugPath[i], debugPath[i + 1]);
                Gizmos.DrawWireSphere(debugPath[i], 0.1f);
            }
            Gizmos.DrawWireSphere(debugPath[debugPath.Count - 1], 0.1f);
        }
    }

    private List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        HashSet<Vector3Int> visitedPositions = new HashSet<Vector3Int>();
        Vector3Int currentPos = startPos;
        Vector3Int currentDirection = GetInitialDirection(startPos, targetPos);
        int maxAttempts = 100;
        int attempts = 0;

        path.Add(currentPos);
        visitedPositions.Add(currentPos);

        while (currentPos != targetPos && attempts < maxAttempts)
        {
            attempts++;
            bool moved = false;

            // Thử các hướng theo thứ tự: phía trước -> trái -> phải
            Vector3Int[] directionsToTry = GetDirectionsToTry(currentDirection);
            
            foreach (Vector3Int dir in directionsToTry)
            {
                List<Vector3Int> line = MoveInDirectionUntilWall(currentPos, dir);
                if (line.Count > 0)
                {
                    foreach (var pos in line)
                    {
                        if (!visitedPositions.Contains(pos))
                        {
                            path.Add(pos);
                            visitedPositions.Add(pos);
                        }
                    }
                    currentPos = line[line.Count - 1];
                    currentDirection = dir; // Cập nhật hướng hiện tại
                    moved = true;
                    break;
                }
            }

            if (!moved)
            {
                Debug.LogWarning($"Stuck at {currentPos} after {attempts} attempts");
                break;
            }

            Debug.Log($"Current: {currentPos}, Direction: {currentDirection}, Target: {targetPos}");
        }

        return path;
    }

    private Vector3Int GetInitialDirection(Vector3Int start, Vector3Int target)
    {
        // Ưu tiên di chuyển theo X trước
        if (target.x != start.x)
        {
            return new Vector3Int(target.x > start.x ? 1 : -1, 0, 0);
        }
        // Nếu cùng X thì di chuyển theo Y
        return new Vector3Int(0, target.y > start.y ? 1 : -1, 0);
    }

    private Vector3Int[] GetDirectionsToTry(Vector3Int currentDir)
    {
        // Tạo mảng các hướng theo thứ tự: phía trước -> trái -> phải
        Vector3Int[] directions = new Vector3Int[3];
        
        // Hướng phía trước
        directions[0] = currentDir;
        
        // Hướng bên trái
        directions[1] = new Vector3Int(-currentDir.y, currentDir.x, 0);
        
        // Hướng bên phải
        directions[2] = new Vector3Int(currentDir.y, -currentDir.x, 0);

        if (showDebugPath)
        {
            Debug.Log($"Current Direction: {currentDir}");
            Debug.Log($"Left Direction: {directions[1]}");
            Debug.Log($"Right Direction: {directions[2]}");
        }

        return directions;
    }

    private List<Vector3Int> MoveInDirectionUntilWall(Vector3Int start, Vector3Int direction)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        Vector3Int current = start;
        int maxSteps = 50;
        int steps = 0;

        while (steps < maxSteps)
        {
            steps++;
            Vector3Int next = current + direction;
            if (!IsWalkable(next))
            {
                if (showDebugPath)
                {
                    Debug.DrawLine(
                        floorTilemap.GetCellCenterWorld(current),
                        floorTilemap.GetCellCenterWorld(next),
                        Color.red,
                        60f
                    );
                }
                break;
            }
            positions.Add(next);
            current = next;

            if (showDebugPath)
            {
                Debug.DrawLine(
                    floorTilemap.GetCellCenterWorld(current),
                    floorTilemap.GetCellCenterWorld(next),
                    Color.blue,
                    60f
                );
            }
        }

        return positions;
    }

    private bool IsWalkable(Vector3Int position)
    {
        return floorTilemap.HasTile(position) && 
               !wallTilemap.HasTile(position) && 
               !HasObstacleAtPosition(floorTilemap.GetCellCenterWorld(position));
    }

    private void SpawnPointsAlongPath(List<Vector3Int> path)
    {
        foreach (Vector3Int pos in path)
        {
            Vector3 worldPos = floorTilemap.GetCellCenterWorld(pos);
            if (!HasItemAtPosition(worldPos))
            {
                SpawnPoint(pos);
                if (showDebugPath)
                {
                    Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, pointColor, 60f);
                }
            }
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
}