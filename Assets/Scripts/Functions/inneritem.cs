using UnityEngine;
using System.Collections;

public class inneritem : MonoBehaviour
{
    [SerializeField] private int ItemNumber;
    public int itemNumber
    {
        get => ItemNumber;
        set
        {
            ItemNumber = value;
        }
    }

    public Item item;
    public SpriteRenderer SR;
    public bool is_set;
    Color originalColor;
    Color darkerColor;
    float darkenAmount;
    float newPPU;
    public bool isGet = false;

    private void Awake()
    {
        itemNumber = 10;
        SR = GetComponent<SpriteRenderer>();
        if (SR != null) originalColor = SR.color; 
        darkenAmount = 0f; 
        Alpha0();
    }

    private void Update()
    {
        if (GameManager.Instance.is_preview)
        {
            Alpha255();
        }
        if (is_set && itemNumber != 10)
        {
            item = ItemManager.Instance.itemList[itemNumber];
            if (SR != null) SR.sprite = item.GetItemSprite;
            transform.localScale = new Vector3(0.1f, 0.1f, 1f); 
        }
        if (isGet) StartCoroutine(HIDEITEM());
    }

    public void Alpha0()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            0 
        );
        if (SR != null) SR.color = darkerColor;
    }

    public void Alpha255()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            255
            );
        if (SR != null) SR.color = darkerColor;
    }

    public IEnumerator HIDEITEM()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
