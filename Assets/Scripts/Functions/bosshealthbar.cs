using UnityEngine;
using UnityEngine.UI;

public class bosshealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 체력 슬라이더

    public void Start()
    {
        GameManager.Instance.speed = GameManager.Instance.speed_for_boss_stage;
        healthSlider = GetComponent<Slider>();
    }

    // 체력 상태 업데이트
    public void Update()
    {
        // 게임 매니저에서 0~1 사이의 체력 비율값을 가져와 슬라이더 업데이트
        if (healthSlider != null)
        {
            healthSlider.value = GameManager.Instance.GetNormalizedHealth(); // 0~1 사이의 값으로 슬라이더 업데이트
        }
    }
}
