using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class bosscontroller : MonoBehaviour
{
    private Collider2D bossCollider;
    public Rigidbody2D bossrb;
    private Animator animator;
    private DashPathIndicator DPI;
    private bool isDead = false;
    private bool isMove = false;
    public bool isDashing = false;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool isPhase2 = false;
    private bool isFire = false;
    private bool isInvincible = false;
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    public float jumpHeight = 1.5f; // Z축으로 올라가는 듯한 높이
    public float jumpDuration = 0.5f; // 점프 시간
    public float damageAmount = 5f; // 데미지량
    public float maxShadowScale = 1.5f; // 점프 시 그림자 크기 변화
    public float maxShadowOffset = 0.5f; // 그림자 위치 변화 (Y축)
    private Vector3 moveDirection; // 이동 방향
    private Vector2 dashDirection; // 대쉬 방향
    private Vector2 backDirection; // 밀려나는 방향
    private Vector3 originalScale; // 원래 크기 저장
    private Vector3 targetScale;   // 점프 시 크기
    public GameObject firePrefab;
    public GameObject pointPrefab;
    public GameObject[] points;
    public GameObject shadowPrefab;
    public GameObject shadow;
    public GameObject bomb;
    public GameObject bombPrefab;
    public Transform shadowTransform; // 그림자 오브젝트
    public Transform summonPoint;
    public Transform[] summonPoints;
    private Coroutine currentCoroutine;
    private Coroutine damageCoroutine;
    private Coroutine dashCoroutine;

    private void Awake()
    {
        GameManager.Instance.boss_health = GameManager.Instance.boss_max_health; // 보스 최대 체력으로 현재 체력 초기화
        UpdateHealthBar();
        animator = GetComponent<Animator>(); // 애니메이터 세팅
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Boss!");
        }
        // 콜라이더, 리지드바디 가져오기
        bossCollider = GetComponent<Collider2D>();
        bossrb = GetComponent<Rigidbody2D>();
        bossrb.linearVelocity = Vector3.zero;
        // IDLE 상태 전환
        SetIdle(true);
        // 변수들 초기화, 도전 난이도면 다르게
        if (GameManager.Instance.diff == 3)
        {
            dashSpeed = 24f;
        }
        else
        {
            dashSpeed = 18f;
        }
        moveSpeed = 2f;
        jumpDuration = 1f;
        dashDuration = 1.5f;
        jumpHeight = 1.5f;
        GameManager.Instance.health_item = 0;
        switch (GameManager.Instance.diff)
        {
            case 1:
                GameManager.Instance.maxHealth = 5;
                break;
            case 2:
                GameManager.Instance.maxHealth = 3;
                break;
            case 3:
                GameManager.Instance.maxHealth = 1;
                break;
            default:
                Debug.Log("Out of Diff");
                break;
        }
        originalScale = transform.localScale;
        targetScale = originalScale * 1.2f; // 점프 시 커지는 효과
        summonPoint = GameObject.Find("FIREPOSITIONS").GetComponent<Transform>();
        summonPoints = summonPoint.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        StartCoroutine(InitializeBossBattleWithDelay());
    }

    private IEnumerator InitializeBossBattleWithDelay()
    {
        yield return null; // 한 프레임 대기
        InitializeBossBattle();
    }

    private void InitializeBossBattle()
    {
        DPI = GameObject.Find("LineRenderer").GetComponent<DashPathIndicator>();
        DPI.HideDashPath();
        // IDLE 상태 전환
        SetIdle(true);
        SetMove(true);
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Stop();
        audiomanager.Instance.bossstagebgm.Play();
        audiomanager.Instance.bossstagebgm.loop = true;
    }

    private void Update()
    {
        if (isMove && !isDashing && !isDead && !isJumping && !isLanding)
        {
            moveDirection = (GameManager.Instance.player.transform.position - transform.position).normalized;
            moveDirection.z = 0;
            if (moveDirection != Vector3.zero)
            {
                if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
                {
                    if (moveDirection.x > 0)
                        animator.Play("BOSSMOVERIGHT");
                    else
                        animator.Play("BOSSMOVELEFT");
                }
                else
                {
                    if (moveDirection.y > 0)
                        animator.Play("BOSSMOVEUP");
                    else
                        animator.Play("BOSSMOVEDOWN");
                }
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 바 누르면 체력 50% 감소
        {
            TakeDamage(50f);
        }
        // 랜덤 행동 코루틴 중복 방지
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(RandomCoroutine());
        }
        // 체력이 절반 이하가 되면 2페이즈 돌입
        if (GameManager.Instance.boss_health <= 50f) isPhase2 = true;
        // 한 번만 불길 소환
        if (isPhase2 && !isFire)
        {
            isFire = true;
            StartCoroutine(SummonFire());
        }
    }

    public void SetIdle(bool isIdle)
    {
        if (isDead) return; // 사망 상태에서는 다른 애니메이션 재생 안 함
        animator.SetBool("IDLE", isIdle);
    }

    public bool IsIdle()
    {
        if (animator != null)
        {
            // 현재 애니메이션 상태 정보 가져오기
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // "idle" 상태 이름과 비교 (layer 0 기준)
            return stateInfo.IsName("BOSSIDLE");
        }
        return false;
    }

    public bool Isfire()
    {
        if (animator != null)
        {
            // 현재 애니메이션 상태 정보 가져오기
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // "idle" 상태 이름과 비교 (layer 0 기준)
            return stateInfo.IsName("BOSSFIRE");
        }
        return false;
    }

    public void SetMove(bool ismove)
    {
        if (isDead) return; // 사망 상태에서는 다른 애니메이션 재생 안 함
        isMove = ismove;
        animator.SetBool("ISMOVE", ismove);
    }

    // 보스 피격 시 체력 감소
    public void TakeDamage(float damage)
    {
        audiomanager.Instance.bossdamaged.Play();
        SetMove(false);
        if (!isInvincible)
        {
            GameManager.Instance.boss_health -= damage;
            GameManager.Instance.boss_health = Mathf.Clamp(GameManager.Instance.boss_health, 0, GameManager.Instance.boss_max_health); // 체력이 0보다 작아지면 0으로 보정

            SetIdle(true); // 데미지 입으면 행동 중단
            UpdateHealthBar(); // 체력바 UI 업데이트

            Debug.Log($"Boss took {damage} damage! Remaining health: {GameManager.Instance.boss_health}");

            if (GameManager.Instance.boss_health <= 0)
            {
                BossDie(); // 보스 사망 트리거 실행
                return;
            }

            PlayHitAnimation();
        }
        animator.SetBool("ISDASH", false);
        animator.SetBool("ISJUMP", false);
        animator.SetBool("ISLAND", false);
        isDashing = false;
        isJumping = false;

        SetIdle(true);
    }

    public void PlayHitAnimation()
    {
        if (isDead) return; // 사망 상태에서는 피격 애니메이션 재생 안 함
        animator.SetTrigger("DAMAGED");
    }

    public void PlayDashAnimation()
    {
        if (isDead) return;
        animator.SetBool("ISDASH", true);
    }

    public void PlayJumpAnimation()
    {
        if (isDead) return;
        animator.SetBool("ISJUMP", true);
        //animator.SetBool("ISLAND", false);
    }

    public void PlayLandingAnimation()
    {
        if (isDead) return;
        //animator.SetBool("ISJUMP", true);
        animator.SetBool("ISLAND", true);
    }

    public void PlayFireAnimation()
    {
        if (isDead) return;
        animator.SetBool("ISDASH", false);
        animator.SetBool("ISJUMP", false);
        animator.SetBool("ISLAND", false);
        isDashing = false;
        isJumping = false;
        SetMove(false);
        animator.SetTrigger("FIRE");
    }

    private IEnumerator RandomCoroutine()
    {
        yield return new WaitForSeconds(8f);
        SetIdle(true);
        SetMove(false);
        // 대기 후 아이들 상태면 랜덤 행동 실행
        if (!isDashing && !isJumping && !isLanding && !isDead && Isfire() == false)
        {
            int rr = Random.Range(1, 6);

            switch (rr)
            {
                case 1:
                case 2:
                case 5:
                    Debug.Log("Executing Dash()");
                    Dash();
                    break;
                case 3:
                case 4:
                    Debug.Log("Executing Jump()");
                    Jump();
                    break;
                default:
                    Debug.LogError("Out Of Random Range");
                    break;
            }
        }
        currentCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }

    // 보스 체력바 UI 업데이트
    private void UpdateHealthBar()
    {
        if (GameManager.Instance.healthSlider != null)
        {
            GameManager.Instance.healthSlider.value = GameManager.Instance.boss_health / GameManager.Instance.boss_max_health; // 0~1 정규화된 값으로 할당
        }
    }

    public void Dash()
    {
        if (isDashing || isJumping || isLanding)
        {
            Debug.Log("Dashing(Jumping) is already in progress, skipping...");
            return;
        }

        SetIdle(false);
        isDashing = true;
        PlayDashAnimation();
        DPI.UpdatePath();
        Vector2 direction = (GameManager.Instance.player.position - transform.position).normalized;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0) animator.SetInteger("DIRECTION", 2); // Up
            else animator.SetInteger("DIRECTION", 1); // Down
        }
        else
        {
            if (direction.x > 0) animator.SetInteger("DIRECTION", 4); // Right
            else animator.SetInteger("DIRECTION", 3); // Left
        }

        dashDirection = direction;
        Debug.Log($"Dash Position: {bossrb.position}, Direction: {dashDirection}");

        // 기존 코루틴 중단
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }

        // 새로운 대쉬 코루틴 시작
        dashCoroutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        audiomanager.Instance.bossdash.Play();
        audiomanager.Instance.bossdash.loop = true;
        yield return new WaitForSeconds(0.5f);
        float timer = 0f;
        //Debug.DrawLine(transform.position, transform.position + (Vector3)dashDirection * 2, Color.red, 1f);

        while (timer < dashDuration)
        {
            if (dashDirection != Vector2.zero)
            {
                //bossrb.MovePosition(bossrb.position + dashDirection * dashSpeed * Time.deltaTime);
                bossrb.linearVelocity = dashDirection * dashSpeed;
            }
            else
            {
                audiomanager.Instance.bossdash.Stop();
                Debug.LogWarning("Dash direction is zero! Check player and boss positions.");
                yield break; // 대쉬를 중단합니다.
            }
            timer += Time.deltaTime;
            yield return null;
            if (isDashing == false)
            {
                audiomanager.Instance.bossdash.Stop();
                break;
            }
        }
        DPI.HideDashPath();
        animator.SetBool("ISDASH", false);
        isDashing = false;
        yield return new WaitForSeconds(0.1f); // 애니메이션 전환 대기
        audiomanager.Instance.bossdash.Stop();
        SetIdle(true);
        SetMove(true);
    }

    public void Jump()
    {
        if (isJumping || isDashing || isLanding) return;
        SetIdle(false);
        isJumping = true;
        isInvincible = true;
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        float timer = 0f;
        bossCollider.enabled = false;
        audiomanager.Instance.bossjump.Play();
        SummonShadow(); // 그림자 생성
        PlayJumpAnimation(); // 올라가는 효과
        //yield return new WaitForSeconds(0.25f);
        Vector3 originposition = transform.position;
        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;
            originposition.y += jumpHeight * Time.deltaTime;
            transform.position = originposition;

            //float progress = timer / (jumpDuration / 2);
            // 크기 조정 (Z축 상승 효과)
            //transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            // 그림자 효과 업데이트
            //UpdateShadow(progress);
            yield return null;
        }

        isLanding = true;
        isJumping = false;
        animator.SetBool("ISJUMP", false);
        // 난이도에 따른 대기 시간
        if (GameManager.Instance.diff == 3)
        {
            yield return new WaitForSeconds(2.95f);
        }
        else
        {
            yield return new WaitForSeconds(3.2f);
        }

        transform.position = shadowTransform.position;
        audiomanager.Instance.bosslanding.Play();
        PlayLandingAnimation(); // 내려오는 효과
        UpdateShadow(0);

        /*timer = 0f;
        while (timer < jumpDuration / 2)
        {
            timer += Time.deltaTime;
            //originposition.y += -jumpHeight * Time.deltaTime;
            transform.position = originposition;

            //float progress = timer / (jumpDuration / 2);
            // 크기 복원
            //transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            // 그림자 효과 업데이트
            //UpdateShadow(1f - progress);
            yield return null;
        }*/
        // 점프 완료: 콜라이더 활성화
        yield return new WaitForSeconds(0.5f); // 착지 애니메이션이 끝난 후
        bossCollider.enabled = true;
        SummonBomb();
        DestroyShadow();
        DestroyBomb();

        isLanding = false;

        animator.SetBool("ISLAND", false);
        SetIdle(true);
        isInvincible = false;
        SetMove(true);
    }

    public void UpdateShadow(float heightPercentage)
    {
        // 그림자 크기 축소
        //float shadowScale = Mathf.Lerp(maxShadowScale, 1f, heightPercentage);
        //shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1f);

        // 그림자 위치 변경
        //float shadowOffset = Mathf.Lerp(maxShadowOffset, 0f, heightPercentage);
        //shadowTransform.localPosition = new Vector3(shadowTransform.localPosition.x, -shadowOffset, 0f);
        //StartCoroutine(WaitPointSeconds());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Crashed!");
            backDirection = (transform.position - collision.transform.position).normalized;
            //bossrb.MovePosition(bossrb.position + direction * 5f);
            if (isDashing)
            {
                isDashing = false;
                animator.SetBool("ISDASH", false);
                SetMove(false);
                bossrb.linearVelocity = backDirection * 5f;
                StartCoroutine(WaitPointsSecond());
            }
            StartCoroutine(GameManager.Instance.pc.ChangeColor());
            /*if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(HIT());
            }*/
        }

        if (collision.gameObject.CompareTag("border"))
        {
            if (isDashing)
            {
                audiomanager.Instance.bossdamaged.Play();
                Debug.Log("Border Crashed!");
                SetMove(false);
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                bossrb.linearVelocity = direction * 0f;
                Debug.Log("Collision detected during dash, stopping dash...");
                animator.SetBool("ISDASH", false);
                isDashing = false;
            }
        }

        // 충돌한 오브젝트가 "Block" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Block"))
        {
            if (isDashing)
            {
                Debug.Log("Block Crashed!");
                SetMove(false);
                backDirection = (transform.position - collision.transform.position).normalized;
                if (GameManager.Instance.diff == 3) TakeDamage(damageAmount);
                else TakeDamage(damageAmount * 2);
                StartCoroutine(collision.gameObject.GetComponent<bossblock>().BossDamage());
                Debug.Log("Collision detected during dash, stopping dash...");
                animator.SetBool("ISDASH", false);
                isDashing = false;
                bossrb.linearVelocity = backDirection * 5f;
                StartCoroutine(WaitPointsSecond());
            }
        }
    }

    private IEnumerator SummonFire()
    {
        SetMove(false);
        SetIdle(false);
        animator.SetBool("ISDASH", false);
        animator.SetBool("ISJUMP", false);
        animator.SetBool("ISLAND", false);
        PlayFireAnimation();
        points = new GameObject[61];
        Vector3 sumpo = Vector3.zero;
        if (GameManager.Instance.diff == 1)
        {
            for (int j = 1; j < 31; j++)
            {
                sumpo = new Vector3(summonPoints[j].position.x, summonPoints[j].position.y + -0.3f, 0f);
                points[j] = Instantiate(pointPrefab, sumpo, Quaternion.identity);
            }
        }
        else if (GameManager.Instance.diff == 2)
        {
            for (int j = 1; j < 31; j++)
            {
                sumpo = new Vector3(summonPoints[j].position.x, summonPoints[j].position.y + -0.3f, 0f);
                points[j] = Instantiate(pointPrefab, sumpo, Quaternion.identity);
            }
        }
        else if (GameManager.Instance.diff == 3)
        {
            for (int j = 1; j < 61; j++)
            {
                sumpo = new Vector3(summonPoints[j].position.x, summonPoints[j].position.y + -0.3f, 0f);
                points[j] = Instantiate(pointPrefab, sumpo, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(2f);

        if (GameManager.Instance.diff == 1)
        {
            for (int j = 1; j < 31; j++)
            {
                points[j].SetActive(false);
            }
        }
        else if (GameManager.Instance.diff == 2)
        {
            for (int j = 1; j < 31; j++)
            {
                points[j].SetActive(false);
            }
        }
        else if (GameManager.Instance.diff == 3)
        {
            for (int j = 1; j < 61; j++)
            {
                points[j].SetActive(false);
            }
        }
        // for문으로 여러 개 생성, 이지, 노말, 도전 다 다르게 소환
        if (GameManager.Instance.diff == 1)
        {
            for (int j = 1; j < 31; j++)
            {
                Instantiate(firePrefab, summonPoints[j].position, Quaternion.identity);
            }
        }
        else if (GameManager.Instance.diff == 2)
        {
            for (int j = 1; j < 31; j++)
            {
                Instantiate(firePrefab, summonPoints[j].position, Quaternion.identity);
            }
        }
        else if (GameManager.Instance.diff == 3)
        {
            for (int j = 1; j < 61; j++)
            {
                Instantiate(firePrefab, summonPoints[j].position, Quaternion.identity);
            }
        }
        SetIdle(true);
        audiomanager.Instance.bossfire.Play();
    }

    private IEnumerator WaitPointSeconds()
    {
        yield return new WaitForSeconds(1f);
        shadow.GetComponent<SpriteRenderer>().color = Color.red;
        shadow.GetComponent<shadow>().isHot = true;
    }

    public void SummonShadow()
    {
        //Debug.Log("SummonShadow method called");
        //Debug.Log(shadowPrefab == null ? "shadowPrefab is null" : "shadowPrefab is assigned");

        shadow = Instantiate(shadowPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        shadow.transform.parent = null; // 부모 설정 해제
        shadowTransform = shadow.transform;
        //Debug.Log(shadow == null ? "Shadow instantiation failed" : "Shadow instantiated successfully");
    }
    public void SummonBomb()
    {
        bomb = Instantiate(bombPrefab, shadowTransform.position, Quaternion.identity);
        bomb.transform.parent = null; // 부모 설정 해제
        bomb.GetComponent<shadow>().isHot = true;
    }

    public void DestroyShadow()
    {
        Destroy(shadow, 0.3f);
        //Debug.Log("DestroyShadow method called");
    }
    public void DestroyBomb()
    {
        Destroy(bomb, 0.3f);
        //Debug.Log("DestroyShadow method called");
    }

    private void BossDie()
    {
        audiomanager.Instance.bossstagebgm.Stop();
        audiomanager.Instance.bossdash.Stop();
        audiomanager.Instance.bossdead.Play();
        if (isDead) return; // 이미 사망 상태인 경우 중복 실행 방지
        SetIdle(false);
        Debug.Log("Boss has been defeated!");
        bossCollider.enabled = false; // 충돌 비활성화
        isDead = true; // 사망 상태로 설정
        animator.SetTrigger("DIE");
        //StartCoroutine(GameManager.Instance.EndingCoroutine());
        Destroy(gameObject, 0.8f);
    }

    public IEnumerator HIT()
    {
        GameManager.Instance.health--;
        yield return new WaitForSeconds(1f);
        damageCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }

    public IEnumerator WaitPointsSecond()
    {
        yield return new WaitForSeconds(0.5f);
        bossrb.linearVelocity = backDirection * 0f;
    }

    public void jumpdamage()
    {
        StartCoroutine(GameManager.Instance.pc.ChangeColor());
    }
}
