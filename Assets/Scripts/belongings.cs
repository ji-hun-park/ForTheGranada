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
        originalColor = SR.color; // 원래 색
        darkenAmount = 0f; // 어둡게 할 정도 (0: 원래 색, 1: 완전히 검은색, 0.5: 반만 어두워짐)
        sprites = new Sprite[4];
        sprites[1] = Resources.Load<Sprite>("Ring");
        sprites[2] = Resources.Load<Sprite>("Necklace");
        sprites[3] = Resources.Load<Sprite>("Earring");
    }

    // Update is called once per frame
    void Update()
    {
        if (SR != null) SR.sprite = sprites[GameManager.Instance.stage];
        transform.localScale = new Vector3(0.1f, 0.1f, 1f); // 크기 조정
    }

    public void Alpha0()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            0 // 알파값 변경(originalColor.a)
        );
        if (SR != null) SR.color = darkerColor;
    }

    public void Alpha255()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            255 // 알파값 변경(originalColor.a)
            );
        if (SR != null) SR.color = darkerColor;
    }
}
