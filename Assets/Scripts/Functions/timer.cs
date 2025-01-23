using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public TMP_Text timerText; // UI Text ������Ʈ�� ����
    public float totalTime = 10f; // �� Ÿ�̸� �ð� (900��)
    private float timeLeft; // ���� �ð�
    //private bool is_Running = true; // Ÿ�̸� ���� (���� ������ ����)

    public void Awake()
    {
        timerText = this.transform.GetComponentInChildren<TMP_Text>();
        totalTime = 900f;
        timeLeft = totalTime; // �ʱ�ȭ
    }

    void Start()
    {
        //timeLeft = totalTime; // �ʱ�ȭ
        //UpdateTimerText();
    }

    public void OnEnable()
    {
        //timeLeft = totalTime; // �ʱ�ȭ
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //timeLeft = totalTime; // �ʱ�ȭ
    }

    void Update()
    {
        if (GameManager.Instance.is_running && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();

            if (timeLeft <= 0)
            {
                TimeOut();
            }
        }
    }

    // Ÿ�̸� ���߱�
    public void PauseTimer()
    {
        GameManager.Instance.is_running = false;
    }

    // Ÿ�̸� �ٽ� �����ϱ�
    public void ResumeTimer()
    {
        GameManager.Instance.is_running = true;
    }

    // ���� �ð��� �ؽ�Ʈ�� ������Ʈ
    private void UpdateTimerText()
    {
        int m, s, t;
        string timestring = null;

        t = (int)Mathf.Ceil(timeLeft);
        m = t / 60;
        s = t % 60;

        if (s >= 10)
        {
            timestring = m.ToString() + " : " + s.ToString();
        }
        else
        {
            timestring = m.ToString() + " : 0" + s.ToString();
        }

        timerText.text = timestring;
    }

    private void TimeOut()
    {
        audiomanager.Instance.ingamebgm.Stop();
        audiomanager.Instance.gameover.Play();
        timeLeft = 0;
        GameManager.Instance.is_running = false;
        GameManager.Instance.is_ingame = false;
        Debug.Log("Time Out!");
        GameManager.Instance.ui_list[7].gameObject.SetActive(true);
        //Time.timeScale = 0;
        GameManager.Instance.speed = 0;
        StartCoroutine(GameManager.Instance.WaitThreeSecond());
    }
}
