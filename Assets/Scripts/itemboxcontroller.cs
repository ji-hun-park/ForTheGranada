using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class itemboxcontroller : MonoBehaviour
{
    public bool isOpen;
    public bool isUse;
    public Sprite[] ItemBoxSprites;
    public inneritem ii;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemBoxSprites[0]; // ���� ���� ���·� ����
        ii = GetComponentInChildren<inneritem>(true);
    }
    private void Update()
    {
        if (spriteRenderer != null)
        {
            IsPossible();
            IsItemBoxOpen();
        }
    }

    void IsItemBoxOpen()
    {
        // ������ ���� ������ ���
        if (isOpen) // ���� ������
        {
            spriteRenderer.sprite = ItemBoxSprites[1]; // ���� ���� sprite�� ����
        }
    }

    void IsPossible()
    {
        if (!isOpen && GameManager.Instance.is_catch && !GameManager.Instance.is_delay) // ������ ���ڰ� Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.white; // ���ڻ� �Ͼ������ ����
            isUse = true;
        }
        else if (!isOpen && (!GameManager.Instance.is_catch || GameManager.Instance.is_delay))// ������ ���ڰ� ��Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.gray;
            isUse = false;
        }
    }

}
