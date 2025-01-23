#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;

public class npcattack : MonoBehaviour
{
    public GameObject weaponPrefab;
    public float weaponSpeed = 8f;
    public float attackRange = 3f;
    public float Cooltime = 1f;

    private npcsight npc_sight;
    private Transform target;
    private float lastAttackTime;
    private float epsilon = 0.2f; // 허용 오차 거리
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.diff == 1)
        {
            weaponSpeed = 6f;
            Cooltime = 2f;
        }
        npc_sight = GetComponent<npcsight>();
        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon prefab could not be loaded from Resources folder. Check the path and filename.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (npc_sight.DetectPlayer && targetInRange())
        {
            Attack();
        }
    }



    void Attack() //°ø°Ý
    {
        if (Time.time - lastAttackTime < Cooltime || weaponPrefab == null) return;

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        audiomanager.Instance.npcattack.Play();
        Vector2 playerDirection = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

        weapon.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(playerDirection * weaponSpeed, ForceMode2D.Impulse);
        }
        lastAttackTime = Time.time;
    }

    bool targetInRange()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange + epsilon;
    }
}
