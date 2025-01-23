using UnityEngine;

public class info : MonoBehaviour
{
    public bool is_inrange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        InfoBoxInteraction();
    }

    void InfoBoxInteraction()
    {
        if (is_inrange)
        {
            if (Input.GetKeyDown(GameManager.Instance.interactKey))
            {
                audiomanager.Instance.menusfx.Play();
                GameManager.Instance.ui_list[6].gameObject.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Interactable");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inrange = true;
            Debug.Log("in range");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inrange = false;
            Debug.Log("not in range");
        }
    }
}
