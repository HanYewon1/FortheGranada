using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class npcchase : MonoBehaviour
{
    public Vector2Int gridSize; // 留듭쓽 ?ш린
    public float cellSize = 1f; // 寃⑹옄???ш린
    public LayerMask obstacleLayer; // ?μ븷臾??덉씠??

    npcsight npc_sight;
    npccontroller npc_controller;
    npcattack npc_attack;

    private Stack<Vector2> dfsStack = new Stack<Vector2>(); // DFS ?ㅽ깮
    private HashSet<Vector2> visited = new HashSet<Vector2>(); // 諛⑸Ц???몃뱶

    private bool isSearching = false; // DFS ?먯깋 ?곹깭
    private Vector2 currentTarget; // ?꾩옱 紐⑺몴 ?꾩튂

    void Start()
    {
        npc_sight = GetComponent<npcsight>();
        npc_controller = GetComponent<npccontroller>();
        npc_attack = GetComponent<npcattack>();
    }

    void Update()
    {
        if (npc_sight.DetectPlayer && npc_sight.Target != null)//플레이어 인식하고 시야에 플레이어 있으면
        {
            float distanceToPlayer = Vector2.Distance(transform.position, npc_sight.Target.position);

            npc_controller.StartChasing();
            if (distanceToPlayer <= npc_attack.attackRange)
            {
                // 공격 범위 안에서는 추격 중단
                npc_controller.movement = Vector2.zero; //멈춤

            }
            else
            {
                PerformDFS();
                MoveTo(currentTarget); // DFS 탐색 결과로 이동

            }

        }
        else if (!npc_sight.DetectPlayer && npc_controller.isChasing)
        {
            if (isSearching)
            {

                npc_controller.movement = Vector2.zero; //멈춤

                isSearching = false;
                npc_controller.StopChasing(); // 추격 중단 후 순찰로 복귀
            }
            npc_controller.isChasing = false;

        }
    }
    void PerformDFS()
    {

        if (npc_sight.Target == null || isSearching) return;

        dfsStack.Clear();
        visited.Clear();

        Vector2 start = AlignToGrid(transform.position);
        Vector2 goal = AlignToGrid(npc_sight.Target.position);

        dfsStack.Push(start);
        visited.Add(start);

        isSearching = true;

        int maxIterations = 1000; // ?먯깋 ?쒗븳
        int iterations = 0;

        while (dfsStack.Count > 0)
        {
            if (iterations++ > maxIterations)
            {
                Debug.LogWarning($"DFS exceeded iteration limit! Stack size: {dfsStack.Count}, Visited nodes: {visited.Count}");
                isSearching = false;
                return; // ?먯깋 媛뺤젣 醫낅즺
            }

            Vector2 current = dfsStack.Pop();

            // 紐⑺몴 ?꾩튂???꾨떖??寃쎌슦
            if (Vector2.Distance(current, goal) < cellSize / 2)
            {
                Debug.Log($"Target reached at {current}. Iterations: {iterations}");
                currentTarget = goal; // 紐⑺몴 ?ㅼ젙
                isSearching = false;
                return;
            }

            // ?꾩옱 ?꾩튂?먯꽌 ?먯깋 媛?ν븳 ?댁썐 ?몃뱶 異붽?
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


        if (distanceToTarget > npc_attack.attackRange)
        {
            // 紐⑺몴 ?꾩튂濡??대룞
            transform.position = Vector2.MoveTowards(
                transform.position,
                position,
                npc_controller.currentSpeed * Time.deltaTime
            );

            // ?대룞 諛⑺뼢 怨꾩궛
            npc_controller.movement = (position - (Vector2)transform.position).normalized;

        }
        else
        {

            npc_controller.movement = Vector2.zero;
            Debug.Log("HeyStop");
        }

    }




    Vector2 AlignToGrid(Vector2 position)
    {
        // 寃⑹옄???뺣젹???꾩튂 諛섑솚
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
        // 留?寃쎄퀎 ?뺤씤
        if (position.x < 0 || position.x >= gridSize.x * cellSize ||
            position.y < 0 || position.y >= gridSize.y * cellSize)
        {
            return false;
        }

        // ?μ븷臾??뺤씤
        if (Physics2D.OverlapCircle(position, cellSize / 2, obstacleLayer))
        {
            return false;
        }

        return true;
    }
}
