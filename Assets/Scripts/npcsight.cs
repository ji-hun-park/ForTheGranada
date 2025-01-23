using System.Collections;
using UnityEngine;

public class npcsight : MonoBehaviour
{
    [Range(1, 360)] public float angle = 60f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public MeshFilter viewMeshFilter;
    public int segments = 50;
    public float radius = 6f;

    npccontroller npc_controller;

    public Transform Target { get; private set; }
    public bool DetectPlayer { get; private set; }



    private Mesh viewMesh;

    void Start()
    {
        npc_controller = GetComponent<npccontroller>();
        if (viewMeshFilter == null)
            viewMeshFilter = GetComponentInChildren<MeshFilter>();


        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Update()
    {
        Detect();
        DrawFieldOfView();
    }

    private void Detect()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // �þ� ���� ���ο� �ִ��� Ȯ��
            if (Vector3.Angle(new Vector3(npc_controller.movement.x, npc_controller.movement.y, 0), directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // ��ֹ��� ���� �������� �ʾҴ��� Ȯ��
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    DetectPlayer = true;
                    Target = target;
                    return;

                }
            }
        }

        DetectPlayer = false;
        Target = null;

      }
    private void DrawFieldOfView()
    {
        Vector3 forwardDirection = new Vector3(npc_controller.movement.x, npc_controller.movement.y, 0);
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // �߽���

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle / 2 + (angle / segments) * i;
            Vector3 direction = RotateVector(forwardDirection, currentAngle).normalized;
            vertices[i + 1] = direction * radius;

            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    //npc ���� ���� �þ� ����
    private Vector2 RotateVector(Vector3 direction, float offsetAngle)
    {
        float angleRadius = offsetAngle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRadius);
        float sin = Mathf.Sin(angleRadius);

        return new Vector3(direction.x * cos - direction.y * sin,
                           direction.x * sin + direction.y * cos,
                           0);
    }
}
