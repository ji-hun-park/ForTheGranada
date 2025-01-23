using UnityEngine;

public class DashPathIndicator : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform boss;
    public Transform dashTarget;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        dashTarget = GameManager.Instance.player.transform;
        // LineRenderer 기본 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f); // 반투명 빨강
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
    }

    public void ShowDashPath(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void HideDashPath()
    {
        lineRenderer.positionCount = 0;
    }

    public void UpdatePath()
    {
        // 테스트: 보스 위치에서 타겟까지 경로 표시
        ShowDashPath(boss.position, dashTarget.position);
    }
}
