using System.Collections;
using System.Collections.Generic;
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
            GameManager.Instance.ui_list[0].GetComponent<mainmenuUI>().FBS();
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
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Play();
        audiomanager.Instance.ingamebgm.loop = true;
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 1;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 5;
        GameManager.Instance.key = 0;
        CleanItems();
        Time.timeScale = 1;
        //SceneManager.LoadScene("PlayScene");
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickNormalButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Play();
        audiomanager.Instance.ingamebgm.loop = true;
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 2;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 3;
        GameManager.Instance.key = 0;
        CleanItems();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickChallengeButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Play();
        audiomanager.Instance.ingamebgm.loop = true;
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 3;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 1;
        GameManager.Instance.key = 0;
        CleanItems();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickMapTest()
    {

        SceneManager.LoadScene("SampleScene");
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
