using UnityEngine;

public enum HiddenStatus
{
    Entrance,
    Exit,
    Box
}

public class hidden : MonoBehaviour
{
    public HiddenStatus mystatus;
    public Transform[] hiddendoor = new Transform[2];
    public Item hiddenItem;
    public bool is_used;

    public void SelectHiddenItem()
    {
        switch (Random.Range(1, 3))
        {
            case 1:
                ItemManager.Instance.getItem(ItemManager.Instance.itemList[4]);
                break;
            case 2:
                ItemManager.Instance.getItem(ItemManager.Instance.itemList[6]);
                break;
            default:
                Debug.LogError("Out of ItemNum");
                break;
        }
        is_used = true;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (mystatus == HiddenStatus.Entrance)
            {
                Debug.Log("입구");
            }

            if (mystatus == HiddenStatus.Exit)
            {
                Debug.Log("출구");
            }

            if (mystatus == HiddenStatus.Box)
            {
                Debug.Log("상자");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(GameManager.Instance.interactKey))
            {
                if (mystatus == HiddenStatus.Entrance)
                {
                    Debug.Log("입장");
                    GameManager.Instance.player.position = hiddendoor[1].position;
                }

                if (mystatus == HiddenStatus.Exit)
                {
                    Debug.Log("퇴장");
                    GameManager.Instance.player.position = hiddendoor[0].position;
                }

                if (mystatus == HiddenStatus.Box)
                {
                    Debug.Log("템 겟");
                    if (!is_used) SelectHiddenItem();
                }
            }
        }
    }
}
