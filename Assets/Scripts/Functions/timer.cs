using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TMP_Text timerText;
    public const float TotalTime = 900f;
    private float timeLeft;

    public void Awake()
    {
        timerText = transform.GetComponentInChildren<TMP_Text>();
        timeLeft = TotalTime;
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
        UIManager.Instance.UIList[8].gameObject.SetActive(true);
        //Time.timeScale = 0;
        GameManager.Instance.speed = 0;
        StartCoroutine(GameManager.Instance.WaitThreeSecond());
    }
}
