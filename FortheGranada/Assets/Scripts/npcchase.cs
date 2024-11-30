using UnityEngine;
using System.Collections.Generic;

public class npcchase : MonoBehaviour
{
    public Vector2Int gridSize; // 맵의 크기
    public float cellSize = 1f; // 격자의 크기
    public LayerMask obstacleLayer; // 장애물 레이어

    npcsight npc_sight;
    npccontroller npc_controller;

    private Stack<Vector2> dfsStack = new Stack<Vector2>(); // DFS 스택
    private HashSet<Vector2> visited = new HashSet<Vector2>(); // 방문한 노드

    private bool isSearching = false; // DFS 탐색 상태
    private Vector2 currentTarget; // 현재 목표 위치

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
                npc_controller.StartChasing(); // 추격 시작
                PerformDFS(); // DFS 탐색 실행
            }

            // 목표로 이동 또는 멈춤
            if (distanceToPlayer > 0.5f)
            {
                MoveTo(currentTarget); // DFS 탐색 결과로 이동
            }
            else
            {
                StopMoving(); // 멈춤
            }
        }
        else
        {
            if (isSearching)
            {
                isSearching = false; // 탐색 종료
                npc_controller.StopChasing(); // 추격 중단 후 순찰로 복귀
            }
        }
    }

    void PerformDFS()
    {
        if (npc_sight.Target == null || isSearching) return;

        // DFS 초기화
        dfsStack.Clear();
        visited.Clear();

        Vector2 start = AlignToGrid(transform.position);
        Vector2 goal = AlignToGrid(npc_sight.Target.position);

        dfsStack.Push(start);
        visited.Add(start);

        isSearching = true;

        int maxIterations = 1000; // 탐색 제한
        int iterations = 0;

        while (dfsStack.Count > 0)
        {
            if (iterations++ > maxIterations)
            {
                Debug.LogWarning($"DFS exceeded iteration limit! Stack size: {dfsStack.Count}, Visited nodes: {visited.Count}");
                isSearching = false;
                return; // 탐색 강제 종료
            }

            Vector2 current = dfsStack.Pop();

            // 목표 위치에 도달한 경우
            if (Vector2.Distance(current, goal) < cellSize / 2)
            {
                Debug.Log($"Target reached at {current}. Iterations: {iterations}");
                currentTarget = goal; // 목표 설정
                return;
            }

            // 현재 위치에서 탐색 가능한 이웃 노드 추가
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
            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(
                transform.position,
                position,
                npc_controller.currentSpeed * Time.deltaTime
            );

            // 이동 방향 계산
            npc_controller.movement = (position - (Vector2)transform.position).normalized;

        }
    }

    void StopMoving()
    {
        // 멈춤 상태로 설정
        npc_controller.movement = Vector2.zero;

    }

    Vector2 AlignToGrid(Vector2 position)
    {
        // 격자에 정렬된 위치 반환
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
        // 맵 경계 확인
        if (position.x < 0 || position.x >= gridSize.x * cellSize ||
            position.y < 0 || position.y >= gridSize.y * cellSize)
        {
            return false;
        }

        // 장애물 확인
        if (Physics2D.OverlapCircle(position, cellSize / 2, obstacleLayer))
        {
            return false;
        }

        return true;
    }
}
