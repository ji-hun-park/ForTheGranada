using UnityEngine;
using UnityEngine.SceneManagement;

public class stageclear : MonoBehaviour
{
    public bool is_inRange;
    public string stagename;
    public belongings blg;
    public bool is_onlyone;

    public SpriteRenderer[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blg = GetComponentInChildren<belongings>();
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        blg.Alpha0();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        is_onlyone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_onlyone) OnlyOne();
        BoxInteraction();
    }

    void OnlyOne()
    {
        blg.Alpha0();
        is_onlyone = true;
    }

    void BoxInteraction()
    {
        if (is_inRange)
        {
            if (GameManager.Instance.key == GameManager.Instance.req_key && Input.GetKeyDown(GameManager.Instance.interactKey))
            {
                audiomanager.Instance.menusfx.Play();
                blg.Alpha0();
                if (GameManager.Instance.stage == GameManager.MaxStage)
                {
                    GameManager.Instance.key = 0;
                    GameManager.Instance.key_item = 0;
                    GameManager.Instance.is_ingame = false;
                    GameManager.Instance.is_boss = true;
                    GameManager.Instance.stage++;
                    GameManager.Instance.speed = GameManager.Instance.speed_for_boss_stage;
                    SceneManager.LoadScene("Stage_Boss");
                    Debug.Log("Enter BossStage!");
                }
                else
                {
                    if (!GameManager.Instance.is_ingame) GameManager.Instance.is_ingame = true;
                    GameManager.Instance.key = 0;
                    GameManager.Instance.key_item = 0;
                    GameManager.Instance.stage++;
                    stagename = "Stage_" + GameManager.Instance.stage;
                    SceneManager.LoadScene(stagename);
                    Debug.Log("Stage Clear!");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inRange = true;
            if (GameManager.Instance.key == GameManager.Instance.req_key)
            {
                sprites[0].gameObject.SetActive(false);
                sprites[1].gameObject.SetActive(true);
                blg.Alpha255();
            }
            Debug.Log("in range");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inRange = false;
            sprites[0].gameObject.SetActive(true);
            sprites[1].gameObject.SetActive(false);
            blg.Alpha0();
            Debug.Log("not in range");
        }
    }
}
