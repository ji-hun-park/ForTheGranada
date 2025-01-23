using UnityEngine;
using UnityEngine.Tilemaps;

public class bosscam : MonoBehaviour
{
    public float mapHeight;
    public float mapWidth;
    public float padding = 0f; // 화면 경계 여백
    GameObject mapObject;
    Tilemap map;
    Bounds bounds;

    void Start()
    {
        // 맵 오브젝트와 Tilemap 찾기
        mapObject = GameObject.Find("border");
        if (mapObject == null)
        {
            Debug.LogError("BossRoom의 border 객체를 찾을 수 없습니다.");
            return;
        }

        map = mapObject.GetComponent<Tilemap>();
        if (map == null)
        {
            Debug.LogError("BossRoom의 border에 Tilemap이 없습니다.");
            return;
        }

        // 맵 경계 계산
        map.CompressBounds();
        bounds = map.localBounds;
        Vector3 scale = mapObject.transform.localScale;

        // 실제 크기 계산 (스케일 반영)
        mapWidth = bounds.size.x * scale.x;
        mapHeight = bounds.size.y * scale.y;

        if (mapWidth == 0 || mapHeight == 0)
        {
            Debug.LogError("맵 크기가 0입니다. Tilemap이 올바르게 설정되지 않았을 수 있습니다.");
            return;
        }

        Debug.Log($"Map Width: {mapWidth}, Map Height: {mapHeight}");
        Debug.Log($"Map Center: {bounds.center}");

        // 화면 비율 계산
        float aspectRatio = (float)Screen.width / Screen.height;

        // 카메라 크기 계산 (화면 비율에 따라 조정)
        float cameraHeight = (mapHeight / 2f) + padding;
        float cameraWidth = (mapWidth / 2f) / aspectRatio + padding;

        // 카메라 크기 선택 (가로와 세로 중 더 큰 크기에 맞춤)
        //Camera.main.orthographicSize = Mathf.Max(cameraHeight, cameraWidth);

        // 카메라 크기 선택 (가로와 세로 중 더 큰 크기에 맞춤)
        float calculatedSize = Mathf.Max(cameraHeight, cameraWidth);

        // 카메라 크기를 줄이는 조정 계수 (0.9 = 90% 크기)
        float sizeAdjustmentFactor = 0.9f; // 카메라 크기를 90%로 줄이기
        Camera.main.orthographicSize = calculatedSize * sizeAdjustmentFactor;

        // 카메라 크기를 줄이는 고정값 (-2만큼 줄임)
        // Camera.main.orthographicSize = calculatedSize - 2f;

        Debug.Log($"Calculated Camera Size: {calculatedSize}, Adjusted Camera Size: {Camera.main.orthographicSize}");


        //Debug.Log($"Calculated Camera Size: {Camera.main.orthographicSize}");

        // 카메라 위치 조정 (맵 중심으로 이동)
        Vector3 cameraPosition = new Vector3(bounds.center.x * scale.x, bounds.center.y * scale.y, -10);
        Camera.main.transform.position = cameraPosition;

        Debug.Log($"Camera Position: {Camera.main.transform.position}");
    }
}
