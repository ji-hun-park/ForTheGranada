using UnityEngine;

public class weaponcontroller : MonoBehaviour
{
    public float destroyTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("border") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
