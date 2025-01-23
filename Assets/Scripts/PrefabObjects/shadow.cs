using UnityEngine;
//using System.Collections;

public class shadow : MonoBehaviour
{
    public bool isHot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isHot)
        {
            //GameManager.Instance.health--;
            GameManager.Instance.boscon.jumpdamage();
            Debug.Log("Hot!");
            isHot = false;
        }
    }
}
