using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private scanner scanner;  // Scanner 참조
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
        // 플레이어 위치 기준으로 카메라 목표 위치 설정
        Vector3 targetPosition = player.position;
        targetPosition.z = cameraZ;

        // 카메라 뷰 크기 계산
        if (scanner != null && scanner.HasValidBorders())
        {
            Vector2 mapMin = scanner.GetMapMin();
            Vector2 mapMax = scanner.GetMapMax();

            // 카메라의 절반 뷰 크기 계산
            float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect; // 가로 반 크기
            float cameraHalfHeight = Camera.main.orthographicSize;                     // 세로 반 크기

            // 카메라 위치 제한 (뷰 크기 고려)
            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMin.x + cameraHalfWidth, mapMax.x - cameraHalfWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMin.y + cameraHalfHeight, mapMax.y - cameraHalfHeight);
        }
        else
        {
            Debug.LogWarning("No valid borders detected. Camera will freely follow the player.");
        }

        // 카메라 위치 적용
        transform.position = targetPosition;
    }
}
