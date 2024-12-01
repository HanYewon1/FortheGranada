using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class npcchase : MonoBehaviour
{
    public Vector2Int gridSize; // 맵의 크기
    public float cellSize = 1f; // 격자의 크기
    public float attackRange = 2f;
    public LayerMask obstacleLayer; // 장애물 레이어

    npcsight npc_sight;
    npccontroller npc_controller;
    npcattack npc_attack;

    private Stack<Vector2> dfsStack = new Stack<Vector2>(); // DFS 스택
    private HashSet<Vector2> visited = new HashSet<Vector2>(); // 방문한 노드

    private bool isSearching = false; // DFS 탐색 상태
    private Vector2 currentTarget; // 현재 목표 위치

    void Start()
    {
        npc_sight = GetComponent<npcsight>();
        npc_controller = GetComponent<npccontroller>();
        npc_attack = GetComponent<npcattack>();
    }

    void Update()
    {
        if (npc_sight.DetectPlayer && npc_sight.Target != null)//�÷��̾� �ν��ϰ� �þ߿� �÷��̾� ������
        {
            float distanceToPlayer = Vector2.Distance(transform.position, npc_sight.Target.position);
<<<<<<< Updated upstream
            if (distanceToPlayer <= attackRange)
            {
                // 공격 범위 안에 있을 경우 멈추고 공격
                npc_controller.movement = Vector2.zero; // 정지
                npc_attack.Attack(); // 공격 수행
            }
            else
            {
                // 추격
                npc_controller.StartChasing();
                if (!isSearching)
                {
                    PerformDFS(); // DFS 탐색 시작
                }
                MoveTo(currentTarget); // 목표로 이동

=======
            npc_controller.StartChasing();
            if (distanceToPlayer <= npc_attack.attackRange)
            {
                // ���� ���� �ȿ����� �߰� �ߴ�
                npc_controller.movement = Vector2.zero; //����

            }
            else
            {
                PerformDFS();
                MoveTo(currentTarget); // DFS Ž�� ����� �̵�
>>>>>>> Stashed changes
            }

        }
        else if (!npc_sight.DetectPlayer && npc_controller.isChasing)
        {
            if (isSearching)
            {
<<<<<<< Updated upstream
                isSearching = false; // 탐색 종료


            }
            npc_controller.StopChasing(); // 추격 중단 후 순찰로 복귀  

=======
                npc_controller.movement = Vector2.zero; //����

                isSearching = false;
                npc_controller.StopChasing(); // �߰� �ߴ� �� ������ ����
            }
            npc_controller.isChasing = false;
>>>>>>> Stashed changes
        }
    }
    void PerformDFS()
    {
<<<<<<< Updated upstream
        if (npc_sight.Target == null || isSearching) return;

        // DFS 초기화
=======
        // DFS �ʱ�ȭ
>>>>>>> Stashed changes
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
                isSearching = false;
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

<<<<<<< Updated upstream
        if (distanceToTarget > attackRange)
=======
        if (distanceToTarget > npc_attack.attackRange)
>>>>>>> Stashed changes
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
        else
        {
<<<<<<< Updated upstream
            // 플레이어가 공격 범위 안에 있을 경우 멈추고 공격
            npc_controller.movement = Vector2.zero;
            npc_attack.Attack(); // 공격 수행
=======
            npc_controller.movement = Vector2.zero;
            Debug.Log("HeyStop");
>>>>>>> Stashed changes
        }

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
