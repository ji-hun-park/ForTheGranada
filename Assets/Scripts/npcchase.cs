using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class npcchase : MonoBehaviour
{
    public float cellSize = 1f; // 그리드 크기
    public LayerMask obstacleLayer; // 장애물 레이어

    private Stack<Vector2> dfsStack = new Stack<Vector2>(); // DFS 스택
    private HashSet<Vector2> visited = new HashSet<Vector2>(); // 방문한 노드
    private Stack<Vector2> path = new Stack<Vector2>(); // 경로
    private Vector2 lastPlayerPosition; // 이전 플레이어 위치
    private bool isSearching = false; // 탐색 중인지 여부
    private float lastSearchTime = 0f; // 마지막 탐색 시간
    private float epsilon = 0.2f; // 허용 오차 거리 

    npcsight npc_sight;
    npccontroller npc_controller;
    npcattack npc_attack;


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

            GameManager.Instance.is_detected = true;
            Vector2 currentPlayerPosition = AlignToGrid(npc_sight.Target.position);
            float distanceToPlayer = Vector2.Distance(transform.position, npc_sight.Target.position);

            npc_controller.StartChasing();
                if (!isSearching && Time.time - lastSearchTime >= 0.1f && (currentPlayerPosition != lastPlayerPosition || path.Count == 0))
                {
                    PerformDFS(); // DFS 탐색 시작
                    lastPlayerPosition = currentPlayerPosition;
                    lastSearchTime = Time.time;
                }
                MoveTo(); // DFS 탐색 결과로 이동
            
        }
        else if (!npc_sight.DetectPlayer && npc_controller.isChasing) //추격 중 플레이어 놓치면
        {
            if (isSearching)
            {
                npc_controller.movement = Vector2.zero; //멈춤
                isSearching = false;
                npc_controller.StopChasing(); // 추격 중단 후 순찰로 복귀
            }
            npc_controller.isChasing = false;
            GameManager.Instance.is_detected = false;

        }
    }
    void PerformDFS()
    {

        if (npc_sight.Target == null || isSearching) return;

        dfsStack.Clear();
        visited.Clear();
        path.Clear();

        Vector2 start = AlignToGrid(transform.position);
        Vector2 goal = AlignToGrid(npc_sight.Target.position);

        if (!IsWithinSearchRadius(goal, start))
        {
            Debug.LogWarning($"DFS 중단 - 목표가 탐색 반경을 벗어남: {goal}");
            Debug.Log($"Goal: {goal}, Start: {start}, Distance: {Vector2.Distance(goal, start)}, Radius: {npc_sight.radius}");
            GameManager.Instance.is_detected = false;
            isSearching = false; // DFS 종료
            npc_controller.StopChasing(); // 추격 중단 후 순찰로 복귀
            return;
        }

        dfsStack.Push(start);
        visited.Add(start);

        isSearching = true;

        int maxIterations = 100; //탐색제한
        int iterations = 0;

        while (dfsStack.Count > 0)
        {
            if (iterations++ > maxIterations)
            {
                isSearching = false;
                return; // ?먯깋 媛뺤젣 醫낅즺
            }

            Vector2 current = dfsStack.Pop();

            // 紐⑺몴 ?꾩튂???꾨떖??寃쎌슦
            if (Vector2.Distance(current, goal) < cellSize)
            {
                BuildPath(current);

                //currentTarget = goal; // 紐⑺몴 ?ㅼ젙
                isSearching = false;
                return;
            }

            // ?꾩옱 ?꾩튂?먯꽌 ?먯깋 媛?ν븳 ?댁썐 ?몃뱶 異붽?
            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && IsValidPosition(neighbor) && IsWithinSearchRadius(neighbor, start))
                {
                    dfsStack.Push(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        isSearching = false;
        GameManager.Instance.is_detected = false;
    }

    void BuildPath(Vector2 current)
    {
        path.Clear();

        while (current != AlignToGrid(transform.position))
        {
            path.Push(current);
            current = AlignToGrid(transform.position); // 경로를 NPC 위치 기준으로 정렬
        }
    }

    void MoveTo()
    {
        if (path.Count == 0)
        {
            npc_controller.movement = Vector2.zero; // 경로가 없으면 멈춤
            return;
        }

        Vector2 currentTarget = path.Peek(); // 현재 목표 가져오기
        float distanceToTarget = Vector2.Distance(transform.position, npc_sight.Target.position);

        // 목표 위치에 도달하면 다음 노드로 이동
        if (distanceToTarget < cellSize * 0.1f)
        {
            path.Pop();
            if (path.Count == 0)
            {
                npc_controller.movement = Vector2.zero;
                Debug.Log("NPC: 최종 목표에 도달");
                return;
            }
            currentTarget = path.Peek();
        }
        distanceToTarget = Vector2.Distance(transform.position, currentTarget);

        //if (distanceToTarget > npc_attack.attackRange)
        //{
            // 紐⑺몴 ?꾩튂濡??대룞
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget,
                npc_controller.currentSpeed * Time.deltaTime
            );

            // ?대룞 諛⑺뼢 怨꾩궛
            npc_controller.movement = (currentTarget - (Vector2)transform.position).normalized;

        //}
        /*else if (distanceToTarget <= npc_attack.attackRange)
        {
            npc_controller.movement = Vector2.zero;
            Debug.Log($"distanceToTarget: {distanceToTarget}, attackRange: {npc_attack.attackRange}");
            Debug.Log("NPC: 공격 범위 내 도달, 멈춤");
        }*/

    }




    Vector2 AlignToGrid(Vector2 position)
    {
        // 寃⑹옄???뺣젹???꾩튂 諛섑솚
        return new Vector2(
            Mathf.Round(position.x / cellSize) * cellSize,
            Mathf.Round(position.y / cellSize) * cellSize
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

        // 유효한 좌표만 반환
        neighbors = neighbors.FindAll(neighbor => IsWithinSearchRadius(neighbor, current)); // 탐색 반경 제한);
        return neighbors;
    }
    bool IsWithinSearchRadius(Vector2 position, Vector2 start)
    {
        float distance = Vector2.Distance(position, start);
        bool isWithin = distance <= npc_sight.radius+ epsilon;
        return isWithin;
    }
    bool IsValidPosition(Vector2 position)
    {
        // ?μ븷臾??뺤씤
        if (Physics2D.OverlapCircle(position, cellSize / 2, obstacleLayer))
        {

            return false;
        }
        return true;
    }

}
