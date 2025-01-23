using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // �÷��̾� Transform
    [SerializeField] private scanner scanner;  // Scanner ����
    private float cameraZ = -10f;

    private void Start()
    {
        scanner = GameManager.Instance.sc;
        player = GameManager.Instance.player;

        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }
    }

    private void LateUpdate()
    {
        // �÷��̾� ��ġ �������� ī�޶� ��ǥ ��ġ ����
        Vector3 targetPosition = player.position;
        targetPosition.z = cameraZ;

        // ī�޶� �� ũ�� ���
        if (scanner != null && scanner.HasValidBorders())
        {
            Vector2 mapMin = scanner.GetMapMin();
            Vector2 mapMax = scanner.GetMapMax();

            // ī�޶��� ���� �� ũ�� ���
            float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect; // ���� �� ũ��
            float cameraHalfHeight = Camera.main.orthographicSize;                     // ���� �� ũ��

            // ī�޶� ��ġ ���� (�� ũ�� ���)
            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMin.x + cameraHalfWidth, mapMax.x - cameraHalfWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMin.y + cameraHalfHeight, mapMax.y - cameraHalfHeight);
        }
        else
        {
            Debug.LogWarning("No valid borders detected. Camera will freely follow the player.");
        }

        // ī�޶� ��ġ ����
        transform.position = targetPosition;
    }
}
