using UnityEngine;
using System.Collections;

public class bossblock : MonoBehaviour
{
    public GameObject ITEM;
    public SpriteRenderer SR;
    public bool ishp = false;
    public bool maxhp = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ITEM = transform.Find("ITEM").gameObject;
        SR = ITEM.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            GameManager.Instance.boscon.bossrb.linearVelocity = pushDirection * 0.5f;
            //if (GameManager.Instance.boscon.isDashing) StartCoroutine(BossDamage());
            GameManager.Instance.boscon.bossrb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.maxHealth > GameManager.Instance.health && !maxhp)
            {
                GameManager.Instance.health++;
                audiomanager.Instance.getitem.Play();
            }
            else if (maxhp && GameManager.Instance.health_item < 2)
            {
                audiomanager.Instance.getitem.Play();
                GameManager.Instance.health++;
                GameManager.Instance.maxHealth++;
                GameManager.Instance.health_item++;
                UIManager.Instance.healthLoseList[GameManager.Instance.maxHealth].gameObject.SetActive(true);
                maxhp = false;
                SR.color = Color.white;
            }
            ishp = false;
            ITEM.SetActive(false);
        }
    }

    public IEnumerator BossDamage()
    {
        Debug.Log("ITEMSELECT!");
        //GameManager.Instance.boss_health -= 5;
        //if (GameManager.Instance.boscon.isDashing) GameManager.Instance.boscon.TakeDamage(5f);
        int hp = Random.Range(1, 101);
        if (!ishp && hp > 80)
        {
            ishp = true;
            ITEM.SetActive(true);
            Debug.Log("HP!");
        }
        else if (!ishp && hp >= 76 && hp <= 80)
        {
            ishp = true;
            maxhp = true;
            ITEM.SetActive(true);
            SR.color = Color.red;
            Debug.Log("MAXHP!");
        }
        yield return new WaitForSeconds(1f);
    }
}
