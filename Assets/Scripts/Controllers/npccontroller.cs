using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public bool isChasing = false;
    public GameObject[] points; // 순찰 포인트
    public Vector2 movement = Vector2.zero;
    public float currentSpeed;
    public float moveSpeed;
    public float chaseSpeed;


    private int nextPoint = 0;
    private float distToPoint;

    private bool returnDefault = false;

    void Start()
    {
        moveSpeed = GameManager.Instance.originspeed * 0.75f;
        chaseSpeed = GameManager.Instance.diff == 1 ? GameManager.Instance.originspeed * 0.85f : GameManager.Instance.originspeed * 1f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            UpdateAnimation();
            // 추격 중일 때 다른 동작 중단
            return;
        }

        if (returnDefault == true)
        {
            // 순찰 복귀 중에는 순찰 로직 실행 중단
            ReturnDefault();
            return;
        }

        // 기본 순찰 동작
        NPCMoveDefault();
    }


    void NPCMoveDefault()
    {
        if (points.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned.");
            return;
        }

        // 현재 포인트와의 거리 계산
        distToPoint = Vector2.Distance(transform.position, points[nextPoint].transform.position);
        currentSpeed = moveSpeed;
        // 현재 포인트로 이동
        transform.position = Vector2.MoveTowards(
            transform.position,
            points[nextPoint].transform.position,
            currentSpeed * Time.deltaTime
        );

        // 이동 방향 계산
        movement = (points[nextPoint].transform.position - transform.position).normalized;

        // 현재 포인트에 도달했는지 확인
        if (distToPoint < 0.2f)
        {
            // 다음 포인트로 이동
            nextPoint = (nextPoint + 1) % points.Length;
        }

        UpdateAnimation();
    }

    public void StartChasing()
    {
        //Debug.Log("NPC started chasing.");
        isChasing = true;
        currentSpeed = chaseSpeed;
        returnDefault = false;
        UpdateAnimation();
    }

    public void StopChasing()
    {
        if (returnDefault == true) // 이미 순찰 복귀 중이라면 호출 중단
        {
            Debug.Log("StopChasing() ignored. NPC is already returning to patrol.");
            return;
        }

        Debug.Log("NPC stopped chasing. Returning to patrol.");
        isChasing = false;
        currentSpeed = moveSpeed;
        returnDefault = true; // 순찰 복귀 상태 활성화
        UpdateAnimation();

    }


    void ReturnDefault()
    {
        if (points.Length == 0) return;

        // 현재 위치에서 가장 가까운 포인트 찾기
        int closestPoint = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, points[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = i;
            }
        }

        // 가까운 포인트로 이동
        distToPoint = Vector2.Distance(transform.position, points[closestPoint].transform.position);
        transform.position = Vector2.MoveTowards(
            transform.position,
            points[closestPoint].transform.position,
            currentSpeed * Time.deltaTime
        );

        movement = (points[closestPoint].transform.position - transform.position).normalized;

        // 가까운 포인트에 도달하면 순찰 시작
        if (distToPoint < 0.2f)
        {
            Debug.Log($"Reached closest point {closestPoint}. Resuming patrol.");
            nextPoint = (closestPoint + 1) % points.Length; // 다음 순찰 포인트 설정
            returnDefault = false;
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        // 애니메이션 업데이트
        animator.SetInteger("npc_x", Mathf.RoundToInt(movement.x));
        animator.SetInteger("npc_y", Mathf.RoundToInt(movement.y));
    }


}
