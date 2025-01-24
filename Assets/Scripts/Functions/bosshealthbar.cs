using UnityEngine;
using UnityEngine.UI;

public class bosshealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // ü�� �����̴�

    public void Start()
    {
        GameManager.Instance.speed = GameManager.OriginSpeed;
        healthSlider = GetComponent<Slider>();
    }

    // ü�� ���� ������Ʈ
    public void Update()
    {
        // ���� �Ŵ������� 0~1 ������ ü�� �������� ������ �����̴� ������Ʈ
        if (healthSlider != null)
        {
            healthSlider.value = GameManager.Instance.GetNormalizedHealth(); // 0~1 ������ ������ �����̴� ������Ʈ
        }
    }
}
