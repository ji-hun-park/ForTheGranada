using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FIRE!");
            StartCoroutine(GameManager.Instance.pc.ChangeColor());
        }
    }
}
