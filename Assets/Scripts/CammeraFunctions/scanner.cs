using UnityEngine;

public class scanner : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    [SerializeField] private Vector2 mapMin = Vector2.zero; // ���� ��� �ּ� ��ǥ
    [SerializeField] private Vector2 mapMax = Vector2.zero; // ���� ��� �ִ� ��ǥ
    [SerializeField] private bool hasBorders = false;      // ��谪 ��ȿ ����
    //[SerializeField] private Transform player;             // �÷��̾� Transform

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            ResizeBoxCollider();
        }
    }

    private void ResizeBoxCollider()
    {
        // �ػ󵵿� ���� ���� ���
        float widthRatio = Screen.width / 1920f; // ���� �ػ�: 1920
        float heightRatio = Screen.height / 1080f; // ���� �ػ�: 1080
        float offset = (Screen.width <= 1024 || Screen.height <= 768) ? 1.4f : 1f;

        // BoxCollider ũ�� ����
        boxCollider.size = new Vector2(
            boxCollider.size.x * widthRatio * offset,
            boxCollider.size.y * heightRatio * offset
        );
    }

    public void UpdateBorder()
    {
        if (GameManager.Instance.player == null) return;

        // ��� Border �±׸� ���� ��ü �˻�
        GameObject[] borders = GameObject.FindGameObjectsWithTag("border");
        if (borders.Length == 0) return;

        // ���� ����� Border ã��
        float shortestDistance = float.MaxValue;
        GameObject nearestBorder = null;

        foreach (GameObject border in borders)
        {
            float distance = Vector2.Distance(GameManager.Instance.player.position, border.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestBorder = border;
            }
        }

        // ����� Border�� Bounds�� ����Ͽ� ��� ������Ʈ
        if (nearestBorder != null)
        {
            Collider2D borderCollider = nearestBorder.GetComponent<Collider2D>();
            if (borderCollider != null)
            {
                Bounds bounds = borderCollider.bounds;
                mapMin = bounds.min;
                mapMax = bounds.max;
                hasBorders = true;
            }
        }
    }

    // �ּ�/�ִ� ��ǥ ��ȯ �޼���
    public Vector2 GetMapMin() => mapMin;
    public Vector2 GetMapMax() => mapMax;
    public bool HasValidBorders() => hasBorders;

    // �����: ��踦 �ð�ȭ
    private void OnDrawGizmos()
    {
        if (hasBorders)
        {
            Gizmos.color = Color.red;

            // ��� �簢�� �׸���
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMin.y, 0), new Vector3(mapMax.x, mapMin.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMin.y, 0), new Vector3(mapMax.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMax.y, 0), new Vector3(mapMin.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMax.y, 0), new Vector3(mapMin.x, mapMin.y, 0));
        }
    }
}
