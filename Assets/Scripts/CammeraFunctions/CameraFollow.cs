using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private scanner scanner;
    private float cameraZ = -10f;
    private Vector3 targetPosition;

    private void Start()
    {
        scanner = GameManager.Instance.sc;
        player = GameManager.Instance.player;

        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;
        
        targetPosition = player.position;
        targetPosition.z = cameraZ;
        
        if (scanner != null && scanner.HasValidBorders())
        {
            Vector2 mapMin = scanner.GetMapMin();
            Vector2 mapMax = scanner.GetMapMax();
            
            float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            float cameraHalfHeight = Camera.main.orthographicSize;                    
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMin.x + cameraHalfWidth, mapMax.x - cameraHalfWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMin.y + cameraHalfHeight, mapMax.y - cameraHalfHeight);
        }
        else
        {
            Debug.LogWarning("No valid borders detected. Camera will freely follow the player.");
        }
        
        transform.position = targetPosition;
    }
}
