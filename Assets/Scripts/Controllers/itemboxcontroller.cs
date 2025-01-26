using UnityEngine;
using UnityEngine.Events;

public class itemboxcontroller : MonoBehaviour
{
    public UnityEvent OnOpen;
    private bool isOpen;

    public bool is_open
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            if (isOpen) OnOpen?.Invoke();
        }
    }
    public bool isUse;
    public Sprite[] ItemBoxSprites;
    public inneritem ii;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemBoxSprites[0]; 
        ii = GetComponentInChildren<inneritem>(true);
    }

    void OnEnable()
    {
        OnOpen.AddListener(IsItemBoxOpen);
    }

    void OnDisable()
    {
        OnOpen.RemoveListener(IsItemBoxOpen);
    }
    
    private void Update()
    {
        if (!is_open) IsPossible();
    }

    void IsItemBoxOpen()
    {
        if (spriteRenderer != null) 
        {
            spriteRenderer.sprite = ItemBoxSprites[1]; 
        }
    }

    void IsPossible()
    {
        if (spriteRenderer != null)
        {
            if (GameManager.Instance.is_catch && !GameManager.Instance.is_delay)
            {
                spriteRenderer.color = Color.white;
                isUse = true;
            }
            else if (!GameManager.Instance.is_catch || GameManager.Instance.is_delay)
            {
                spriteRenderer.color = Color.gray;
                isUse = false;
            }
        }
    }
}
