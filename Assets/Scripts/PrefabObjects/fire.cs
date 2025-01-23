using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour
{
    private Coroutine currentCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FIRE!");
            StartCoroutine(GameManager.Instance.pc.ChangeColor());
            // 데미지 입히는 코루틴 중복 방지
            /*if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(HIT());
            }*/
        }
    }

    public IEnumerator HIT()
    {
        GameManager.Instance.health--;
        yield return new WaitForSeconds(1f);
        currentCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }
}
