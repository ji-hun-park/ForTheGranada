using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
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
        if (SR != null) originalColor = SR.color; // ���� ��
        darkenAmount = 0f; // ��Ӱ� �� ���� (0: ���� ��, 1: ������ ������, 0.5: �ݸ� ��ο���)
        Alpha0();
    }

    private void Update()
    {
        if (GameManager.Instance.is_preview)
        {
            Alpha255();
        }
        else
        {
            //Alpha0();
        }
        if (is_set && itemNumber != 10)
        {
            //newPPU = 200f;
            //sprite.texture.filterMode = FilterMode.Point;
            item = ItemManager.Instance.itemList[itemNumber];
            if (SR != null) SR.sprite = item.GetItemSprite;
            transform.localScale = new Vector3(0.1f, 0.1f, 1f); // ũ�� ����
        }
        if (isGet) StartCoroutine(HIDEITEM());
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

    public IEnumerator HIDEITEM()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    /*Color originalColor = spriteRenderer.color; // ���� ���� ��
    float darkenFactor = 0.1f; // ��ο����� ���� (0: ���� ��, 1: ������ ������, 0.5: �ݸ� ��ο���)
    Color darkerColor = Color.Lerp(originalColor, Color.black, darkenFactor); // ������ �ݸ� ��ο����� ��
    spriteRenderer.color = darkerColor; // �� ����*/
}
