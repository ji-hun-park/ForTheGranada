using UnityEngine;

public class belongings : MonoBehaviour
{
    public SpriteRenderer SR;
    public Sprite[] sprites;
    Color originalColor;
    Color darkerColor;
    float darkenAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        originalColor = SR.color; // ���� ��
        darkenAmount = 0f; // ��Ӱ� �� ���� (0: ���� ��, 1: ������ ������, 0.5: �ݸ� ��ο���)
        sprites = new Sprite[4];
        sprites[1] = Resources.Load<Sprite>("Ring");
        sprites[2] = Resources.Load<Sprite>("Necklace");
        sprites[3] = Resources.Load<Sprite>("Earring");
    }

    // Update is called once per frame
    void Update()
    {
        if (SR != null) SR.sprite = sprites[GameManager.Instance.stage];
        transform.localScale = new Vector3(0.1f, 0.1f, 1f); // ũ�� ����
    }

    public void Alpha0()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            0 // ���İ� ����(originalColor.a)
        );
        if (SR != null) SR.color = darkerColor;
    }

    public void Alpha255()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            255 // ���İ� ����(originalColor.a)
            );
        if (SR != null) SR.color = darkerColor;
    }
}
