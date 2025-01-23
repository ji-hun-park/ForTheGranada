using UnityEngine;
using UnityEngine.Events;

public class checkinteraction : MonoBehaviour
{
    public bool inRange; //상호작용 범위가 되는지
    public UnityEvent interaction;
    KeyCode interactKey = KeyCode.F; //F키 눌러서 상호작용

    void Awake()
    {

    }

    private void Update()
    {
        ItemBoxInteraction();
    }

    void ItemBoxInteraction()
    {
        if (inRange) //상호작용 범위 내에서
        {
            if (Input.GetKeyDown(interactKey)) //상호작용 키 누르면
            {
                Debug.Log("Interactable");
            }
        }
    }

    //콜라이더 충돌으로 상호작용 범위 판단

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            GameManager.Instance.is_closebox = true;
            Debug.Log("in range");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            GameManager.Instance.is_closebox = false;
            Debug.Log("not in range");
        }
    }

}
