using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class diffselectUI : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.mm.FBS();
            gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        audiomanager.Instance.menusfx.Play();
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }

    public void OnClickEasyButton()
    {
        ButtonActions(1);
    }

    public void OnClickNormalButton()
    {
        ButtonActions(2);
    }

    public void OnClickChallengeButton()
    {
        ButtonActions(3);
    }

    private void ButtonActions(int selectDiff)
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        
        switch (selectDiff)
        {
            case 1:
                GameManager.Instance.health = 5;
                GameManager.Instance.diff = 1;
                break;
            case 2:
                GameManager.Instance.health = 3;
                GameManager.Instance.diff = 2;
                break;
            case 3:
                GameManager.Instance.health = 1;
                GameManager.Instance.diff = 3;
                break;
            default:
                Debug.LogError("Out of Diff!");
                break;
        }
        
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Play();
        audiomanager.Instance.ingamebgm.loop = true;
        GameManager.Instance.is_running = true;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.OriginSpeed;
        GameManager.Instance.key = 0;
        CleanItems();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage_1");
    }

    public void CleanItems()
    {
        GameManager.Instance.armor_item = 0;
        GameManager.Instance.ressurection_item = 0;
        GameManager.Instance.preview_item = 0;
        GameManager.Instance.stealth_item = 0;
        GameManager.Instance.speed_item = 0;
        GameManager.Instance.haste_item = 0;
        GameManager.Instance.key_item = 0;
        GameManager.Instance.is_ressurection = false;
        GameManager.Instance.is_stealth = false;
        GameManager.Instance.is_preview = false;
        GameManager.Instance.is_attacked_speed = false;
    }
}
