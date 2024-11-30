using UnityEngine;
using System.Collections.Generic;

public class npcchase : MonoBehaviour
{
    public Vector2Int gridSize; // ���� ũ��
    public float cellSize = 1f; // ������ ũ��
    public LayerMask obstacleLayer; // ��ֹ� ���̾�

    npcsight npc_sight;
    npccontroller npc_controller;

    private Stack<Vector2> dfsStack = new Stack<Vector2>(); // DFS ����
    private HashSet<Vector2> visited = new HashSet<Vector2>(); // �湮�� ���

    private bool isSearching = false; // DFS Ž�� ����
    private Vector2 currentTarget; // ���� ��ǥ ��ġ

    void Start()
    {
        npc_sight = GetComponent<npcsight>();
        npc_controller = GetComponent<npccontroller>();
    }

    void Update()
    {
        if (npc_sight.DetectPlayer && npc_sight.Target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, npc_sight.Target.position);

            if (!isSearching)
            {
                npc_controller.StartChasing(); // �߰� ����
                PerformDFS(); // DFS Ž�� ����
            }

            // ��ǥ�� �̵� �Ǵ� ����
            if (distanceToPlayer > 0.5f)
            {
                MoveTo(currentTarget); // DFS Ž�� ����� �̵�
            }
            else
            {
                StopMoving(); // ����
            }
        }
        else
        {
            if (isSearching)
            {
                isSearching = false; // Ž�� ����
                npc_controller.StopChasing(); // �߰� �ߴ� �� ������ ����
            }
        }
    }

    void PerformDFS()
    {
        if (npc_sight.Target == null || isSearching) return;

        // DFS �ʱ�ȭ
        dfsStack.Clear();
        visited.Clear();

        Vector2 start = AlignToGrid(transform.position);
        Vector2 goal = AlignToGrid(npc_sight.Target.position);

        dfsStack.Push(start);
        visited.Add(start);

        isSearching = true;

        int maxIterations = 1000; // Ž�� ����
        int iterations = 0;

        while (dfsStack.Count > 0)
        {
            if (iterations++ > maxIterations)
            {
                Debug.LogWarning($"DFS exceeded iteration limit! Stack size: {dfsStack.Count}, Visited nodes: {visited.Count}");
                isSearching = false;
                return; // Ž�� ���� ����
            }

            Vector2 current = dfsStack.Pop();

            // ��ǥ ��ġ�� ������ ���
            if (Vector2.Distance(current, goal) < cellSize / 2)
            {
                Debug.Log($"Target reached at {current}. Iterations: {iterations}");
                currentTarget = goal; // ��ǥ ����
                return;
            }

            // ���� ��ġ���� Ž�� ������ �̿� ��� �߰�
            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && IsValidPosition(neighbor))
                {
                    dfsStack.Push(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        Debug.LogWarning("Target not reachable. Exploration terminated.");
        isSearching = false;
    }

    void MoveTo(Vector2 position)
    {
        float distanceToTarget = Vector2.Distance(transform.position, position);

        if (distanceToTarget > 0.5f)
        {
            // ��ǥ ��ġ�� �̵�
            transform.position = Vector2.MoveTowards(
                transform.position,
                position,
                npc_controller.currentSpeed * Time.deltaTime
            );

            // �̵� ���� ���
            npc_controller.movement = (position - (Vector2)transform.position).normalized;

        }
    }

    void StopMoving()
    {
        // ���� ���·� ����
        npc_controller.movement = Vector2.zero;

    }

    Vector2 AlignToGrid(Vector2 position)
    {
        // ���ڿ� ���ĵ� ��ġ ��ȯ
        return new Vector2(
            Mathf.Floor(position.x / cellSize) * cellSize,
            Mathf.Floor(position.y / cellSize) * cellSize
        );
    }

    List<Vector2> GetNeighbors(Vector2 current)
    {
        List<Vector2> neighbors = new List<Vector2>
        {
            current + Vector2.up * cellSize,
            current + Vector2.down * cellSize,
            current + Vector2.left * cellSize,
            current + Vector2.right * cellSize
        };

        return neighbors;
    }

    bool IsValidPosition(Vector2 position)
    {
        // �� ��� Ȯ��
        if (position.x < 0 || position.x >= gridSize.x * cellSize ||
            position.y < 0 || position.y >= gridSize.y * cellSize)
        {
            return false;
        }

        // ��ֹ� Ȯ��
        if (Physics2D.OverlapCircle(position, cellSize / 2, obstacleLayer))
        {
            return false;
        }

        return true;
    }
}
