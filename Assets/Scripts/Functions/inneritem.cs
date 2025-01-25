using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
    
    Color originalColor;
    Color darkerColor;
    float darkenAmount;
    float newPPU;
    public UnityEvent OnSetFlagChangedToTrue; // UnityEvent 선언
    public event Action OnCoroutineRequested; // 코루틴 전달용 이벤트
    
    private bool isGet;
    public bool is_get
    {
        get { return isGet; }
        set
        {
            isGet = value; 
            // 이벤트 호출
            if (isGet) OnCoroutineRequested?.Invoke();
        }
    }
    
    private bool isSet;

    public bool is_set
    {
        get { return isSet; }
        set
        {
            isSet = value;
            // 이벤트 호출
            if (isSet) OnSetFlagChangedToTrue?.Invoke();
        }
    }

    private void Awake()
    {
        itemNumber = 10;
        SR = GetComponent<SpriteRenderer>();
        if (SR != null) originalColor = SR.color; 
        darkenAmount = 0f; 
        Alpha0();
    }

    private void OnEnable()
    {
        // 이벤트 구독
        OnCoroutineRequested += StartHideCoroutine;
        OnSetFlagChangedToTrue.AddListener(SetItemBox);
        // GameManager의 이벤트 구독
        GameManager.Instance.OnPreviewEventTriggered += Alpha255;
    }
    
    private void OnDisable()
    {
        // 이벤트 해제
        OnCoroutineRequested -= StartHideCoroutine;
        OnSetFlagChangedToTrue.RemoveListener(SetItemBox);
        // GameManager의 이벤트 구독
        GameManager.Instance.OnPreviewEventTriggered -= Alpha255;
    }

    private void SetItemBox()
    {
        if (itemNumber != 10)
        {
            item = ItemManager.Instance.itemList[itemNumber];
            if (SR != null) SR.sprite = item.GetItemSprite;
            transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        }
    }

    private void Alpha0()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            0 
        );
        if (SR != null) SR.color = darkerColor;
    }

    private void Alpha255()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            255
            );
        if (SR != null) SR.color = darkerColor;
    }

    private void StartHideCoroutine()
    {
        StartCoroutine(HideItem());
    }

    private IEnumerator HideItem()
    {
        Alpha255();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
