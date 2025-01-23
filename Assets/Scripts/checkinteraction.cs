using UnityEngine;
using UnityEngine.Events;

public class checkinteraction : MonoBehaviour
{
    public bool inRange; //��ȣ�ۿ� ������ �Ǵ���
    public UnityEvent interaction;
    KeyCode interactKey = KeyCode.F; //FŰ ������ ��ȣ�ۿ�

    void Awake()
    {

    }

    private void Update()
    {
        ItemBoxInteraction();
    }

    void ItemBoxInteraction()
    {
        if (inRange) //��ȣ�ۿ� ���� ������
        {
            if (Input.GetKeyDown(interactKey)) //��ȣ�ۿ� Ű ������
            {
                Debug.Log("Interactable");
            }
        }
    }

    //�ݶ��̴� �浹���� ��ȣ�ۿ� ���� �Ǵ�

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
