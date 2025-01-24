using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class playercontroller : MonoBehaviour
{
    public Sprite deadSprite;
    public int room_x;
    public int room_y;
    public int next_room_x;
    public int next_room_y;
    public float pushed_force;
    public string minimap_name;
    public Coroutine ASCoroutine;
    public Coroutine STCoroutine;

    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Vector3 add_door_position;

    float player_x;//鮈�? ???鴔�?
    float player_y;//??��踝蕭??��踝蕭 ???鴔�?

    bool isDead;
    public bool is_door;
    bool is_horizon_move; //4방향 결정
    bool is_finish;
    bool is_damaged;

    private Vector2 externalVelocity = Vector2.zero; // 충돌로 인한 추가 속도 저장
    private float decayRate = 5f; // 충돌 효과 감소 속도


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    void Update()
    {
        PlayerMove();

        if (Input.GetKeyDown(GameManager.Instance.interactKey))

        {
            Debug.Log(1);
            useDoor();
        }

        if (GameManager.Instance.is_stealth && GameManager.Instance.is_detected)
        {
            if (STCoroutine == null && ASCoroutine == null) STCoroutine = StartCoroutine(GameManager.Instance.STCoroutine());
        }
    }

    private void FixedUpdate()
    {

        Vector2 move_vec = is_horizon_move ? new Vector2(player_x, 0) : new Vector2(0, player_y);
        rigidbody2d.linearVelocity = (move_vec * GameManager.Instance.speed) + externalVelocity;

        // 외부 속도를 서서히 줄임
        externalVelocity = Vector2.Lerp(externalVelocity, Vector2.zero, Time.fixedDeltaTime * decayRate);
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //鮈�? ??��踝蕭??��踝蕭
        player_y = Input.GetAxisRaw("Vertical"); //??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //??��踝蕭??��踝蕭鮈�? ??��踝蕭??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭 魽國?��
        if (player_x_down)
        {
            is_horizon_move = true; //鮈�? ??��踝蕭??��踝蕭
        }
        else if (player_y_down)
        {
            is_horizon_move = false; //??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭
        }
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //나중에 입력받는 방향을 우선시함
        if (is_horizon_move)
        {
            player_y = 0;
        }
        else
        {
            player_x = 0;
        }

        //??��踝蕭??��踝蕭諰�???��踝蕭
        if (animator.GetInteger("player_move_x") != player_x) //鮈�?
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //??��踝蕭??��踝蕭
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //?��踝蕭?��豎嫡橘蕭?��踝蕭 ?��踝蕭
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(ChangeColor());
        }

        if (collision.gameObject.CompareTag("Chest"))
        {
            Transform target = collision.gameObject.GetComponent<Transform>();
        
            if (target != null)
            {
                GameManager.Instance.currentbox = target.gameObject.GetComponent<itemboxcontroller>();
                Debug.Log("Near Box");
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            itemboxcontroller itembox = collision.gameObject.GetComponent<itemboxcontroller>();
            if (itembox.isUse == false)
            {
                Vector2 pushDirection = collision.contacts[0].point - (Vector2)transform.position;
                pushDirection = -pushDirection.normalized;

                externalVelocity += pushDirection * pushed_force;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Weapon"))
            StartCoroutine(ChangeColor());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Door")
        {
            is_door = true;
            next_room_x = room_x;
            next_room_y = room_y;
            is_door = true;
            Debug.Log(collision.name + " : " + is_door);
            if (collision.name == "door_up")
            {
                next_room_y = room_y - 1;
                add_door_position = new Vector3(0, 7.4f, 0);
            }
            else if (collision.name == "door_down")
            {
                next_room_y = room_y + 1;
                add_door_position = new Vector3(0, -7.4f, 0);
            }
            else if (collision.name == "door_right")
            {
                next_room_x = room_x + 1;
                add_door_position = new Vector3(7.4f, 0, 0);
            }
            else if (collision.name == "door_left")
            {
                next_room_x = room_x - 1;
                add_door_position = new Vector3(-7.4f, 0, 0);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            Debug.Log("Exit");
            is_door = false;
        }
    }
    public IEnumerator ChangeColor()
    {
        if (is_damaged == false)
        {
            audiomanager.Instance.playerdamaged.Play();
            if (GameManager.Instance.armor_item >= 1)
            {
                GameManager.Instance.armor--;
                GameManager.Instance.armor_item--;
                GameManager.Instance.health_list[8].gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.health--;
                if (GameManager.Instance.health <= 0) GameManager.Instance.health = 0;
            }
            if (GameManager.Instance.is_attacked_speed) { if (ASCoroutine == null && STCoroutine == null) ASCoroutine = StartCoroutine(GameManager.Instance.ASCoroutine()); }
            is_damaged = true;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(1f); //1?��褊蛛?��?��踝蕭 ?��踝蕭?��踝蕭
            spriteRenderer.color = originalColor; //?��踝蕭?��踝蕭 ?��踝蕭?��踝蕭?��踝蕭 ?��踝蕭?��複選?��
            is_damaged = false;
        }
    }

    //?�踝??��豎對??��踝蕭 ?�踝??���?

    public void Dead()

    {
        if (isDead) return; // ?�諒對蕭 ?�踝??��踝蕭 ?�踝??��?�塚??���??�踝??��踝蕭?�踝??��踝蕭 ?�踝??��踝蕭

        isDead = true; // ?�踝??��踝蕭 ?�踝??��踝蕭 ?�踝??��踝蕭
        spriteRenderer.color = Color.gray; // ?�踝??��踝蕭 ?�踝??��踝蕭
        spriteRenderer.sprite = deadSprite; // ?�踝??��踝蕭?�踝??��踝蕭???�踝??��踝蕭
        animator.enabled = false; // ?�誰棲賂??��諒潘???�踝??��?�踝??��
        Debug.Log("Game Over");
    }

    void useDoor()
    {
        Debug.Log("use_door: " + is_door);
        Debug.Log("current door: " + room_y + " " + room_x);
        Debug.Log("next door: " + next_room_y + " " + next_room_x);
        if (is_door)
        {
            string minimap_current_room = minimap_name + room_y + room_x;
            string minimap_next_room = minimap_name + next_room_y + next_room_x;
            Debug.Log(minimap_current_room);
            Debug.Log(minimap_next_room);
            GameObject minimap_image = GameObject.Find(minimap_current_room);
            Image image = minimap_image.GetComponent<Image>();
            if (is_finish)
            {
                image.color = Color.blue;
                is_finish = false;
            }
            else
            {
                image.color = Color.white;
            }
            minimap_image = GameObject.Find(minimap_next_room);
            image = minimap_image.GetComponent<Image>();
            if (image.color == Color.blue)
            {
                is_finish = true;
            }

            image.color = Color.red;
            room_x = next_room_x;
            room_y = next_room_y;

            audiomanager.Instance.menusfx.Play();
            this.transform.position = this.transform.position + add_door_position;
            GameManager.Instance.sc.UpdateBorder();
            is_damaged = true;
            Invoke("changeIsDamaged", 0.5f);
        }

    }

    void changeIsDamaged()
    {
        is_damaged = false;
        Debug.Log("무적 시간이 풀렸습니다.");
    }

}
