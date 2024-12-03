using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class bosscontroller : MonoBehaviour
{
    private Collider2D bossCollider;
    private Rigidbody2D bossrb;
    private Animator animator;
    private bool isDead = false;
    private bool isDashing = false;
    private bool isJumping = false;
    private bool isPhase2 = false;
    private bool isFire = false;
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    public float jumpHeight = 1.5f; // Z축으로 올라가는 듯한 높이
    public float jumpDuration = 0.5f; // 점프 시간
    public float damageAmount = 5f; // 데미지량
    public float maxShadowScale = 1.5f; // 점프 시 그림자 크기 변화
    public float maxShadowOffset = 0.5f; // 그림자 위치 변화 (Y축)
    private Vector2 dashDirection; // 대쉬 방향
    private Vector3 originalScale; // 원래 크기 저장
    private Vector3 targetScale;   // 점프 시 크기
    public GameObject firePrefab;
    public GameObject shadowPrefab;
    public GameObject shadow;
    public Transform shadowTransform; // 그림자 오브젝트
    public Transform[] summonPoints;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        // 체력 세팅
        GameManager.Instance.boss_max_health = 100f;
        GameManager.Instance.boss_health = GameManager.Instance.boss_max_health; // 보스 최대 체력으로 현재 체력 초기화
        UpdateHealthBar();
        // 애니메이터 세팅
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Boss!");
        }
        // 콜라이더, 리지드바디 가져오기
        bossCollider = GetComponent<Collider2D>();
        bossrb = GetComponent<Rigidbody2D>();
        // IDLE 상태로 전환
        SetIdle(true);
        // 변수들 초기화, 도전 난이도면 다르게
        dashSpeed = 10f;
        dashDuration = 4f;
        jumpHeight = 1.5f;
        jumpDuration = 3f;
        originalScale = transform.localScale;
        targetScale = originalScale * 1.2f; // 점프 시 커지는 효과
        summonPoints = new Transform[31];
        for (int i = 0; i < 31; i++)
        {
            string positionname = "FIREPOSITION_" + i;
            summonPoints[i] = GameObject.Find(positionname).transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 바 누르면 체력 100% 감소
        {
            TakeDamage(100f);
        }
        // 3초마다 랜덤 행동
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
            SummonFire();
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

    // 보스 피격 시 체력 감소
    public void TakeDamage(float damage)
    {
        GameManager.Instance.boss_health -= damage;
        GameManager.Instance.boss_health = Mathf.Clamp(GameManager.Instance.boss_health, 0, GameManager.Instance.boss_max_health); // 체력이 0보다 작아지면 0으로 보정
        PlayHitAnimation();
        Debug.Log($"Boss took {damage} damage! Remaining health: {GameManager.Instance.boss_max_health}");
        SetIdle(true); // 데미지 입으면 행동 중단
        UpdateHealthBar(); // 체력바 UI 업데이트

        if (GameManager.Instance.boss_health <= 0)
        {
            BossDie(); // 보스 사망 트리거 실행
        }
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
        animator.SetTrigger("JUMP");
    }

    public void PlayLandingAnimation()
    {
        if (isDead) return;
        animator.SetTrigger("LANDING");
    }

    public void PlayFireAnimation()
    {
        if (isDead) return;
        animator.SetTrigger("FIRE");
    }

    private IEnumerator RandomCoroutine()
    {
        yield return new WaitForSeconds(8f);
        // 8초 대기 후 아이들 상태면 랜덤 행동 실행
        if (IsIdle() && !isDashing && !isJumping)
        {
            int rr = Random.Range(1, 5);

            switch (rr)
            {
                case 1:
                case 2:
                case 3:
                    Debug.Log("Executing Dash()");
                    Dash();
                    break;
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
        if (isDashing)
        {
            Debug.Log("Dash is already in progress, skipping...");
            return;
        }

        SetIdle(false);
        isDashing = true;
        PlayDashAnimation();

        Vector2 direction = (GameManager.Instance.player.position - transform.position).normalized;
        int dr = 0;
        dr = Random.Range(0, 2);

        // 위, 왼쪽일 경우 2방향 중 랜덤으로 하나 선택
        if (direction.y >= 0 && direction.x < 0)
        {
            switch (dr)
            {
                case 0:
                    animator.SetInteger("DIRECTION", 2);
                    break;
                case 1:
                    animator.SetInteger("DIRECTION", 3);
                    break;
                default:
                    Debug.LogError("Out Of Random Range");
                    break;
            }
        }
        // 아래, 왼쪽일 경우 2방향 중 랜덤으로 하나 선택
        else if (direction.y < 0 && direction.x < 0)
        {
            switch (dr)
            {
                case 0:
                    animator.SetInteger("DIRECTION", 1);
                    break;
                case 1:
                    animator.SetInteger("DIRECTION", 3);
                    break;
                default:
                    Debug.LogError("Out Of Random Range");
                    break;
            }
        }
        // 위, 오른쪽일 경우 2방향 중 랜덤으로 하나 선택
        else if (direction.y >= 0 && direction.x >= 0)
        {
            switch (dr)
            {
                case 0:
                    animator.SetInteger("DIRECTION", 2);
                    break;
                case 1:
                    animator.SetInteger("DIRECTION", 4);
                    break;
                default:
                    Debug.LogError("Out Of Random Range");
                    break;
            }
        }
        // 아래, 오른쪽일 경우 2방향 중 랜덤으로 하나 선택
        else
        {
            switch (dr)
            {
                case 0:
                    animator.SetInteger("DIRECTION", 1);
                    break;
                case 1:
                    animator.SetInteger("DIRECTION", 4);
                    break;
                default:
                    Debug.LogError("Out Of Random Range");
                    break;
            }
        }

        dashDirection = direction;
        Debug.Log($"Dash Direction: {dashDirection}");
        // 대쉬 실행
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float timer = 0f;

        while (timer < dashDuration)
        {
            if (dashDirection != Vector2.zero)
            {
                bossrb.MovePosition(bossrb.position + dashDirection * dashSpeed * Time.deltaTime);
            }

            timer += Time.deltaTime;
            yield return null;
            if (IsIdle() == true) break;
        }
        animator.SetBool("ISDASH", false);
        isDashing = false;
        SetIdle(true);
    }

    public void Jump()
    {
        if (isJumping) return;
        SetIdle(false);
        isJumping = true;
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        float timer = 0f;
        SummonShadow();
        // 올라가는 효과
        PlayJumpAnimation();
        while (timer < jumpDuration / 2)
        {
            timer += Time.deltaTime;
            float progress = timer / (jumpDuration / 2);

            // 크기 조정 (Z축 상승 효과)
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            // 그림자 효과 업데이트
            UpdateShadow(progress);
            yield return null;
        }

        // 내려오는 효과
        PlayLandingAnimation();
        timer = 0f;
        while (timer < jumpDuration / 2)
        {
            timer += Time.deltaTime;
            float progress = timer / (jumpDuration / 2);

            // 크기 복원
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            // 그림자 효과 업데이트
            UpdateShadow(1f - progress);
            DestroyShadow();
            yield return null;
        }

        isJumping = false;
        SetIdle(true);
    }

    public void UpdateShadow(float heightPercentage)
    {
        // 그림자 크기 축소
        float shadowScale = Mathf.Lerp(maxShadowScale, 1f, heightPercentage);
        shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1f);

        // 그림자 위치 변경
        float shadowOffset = Mathf.Lerp(maxShadowOffset, 0f, heightPercentage);
        shadowTransform.localPosition = new Vector3(shadowTransform.localPosition.x, -shadowOffset, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "Block" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Block"))
        {
            TakeDamage(damageAmount);
        }
    }

    public void SummonFire()
    {
        SetIdle(false);
        PlayFireAnimation();
        // for문으로 여러 개 생성, 이지, 노말은 반만 소환
        if (GameManager.Instance.diff != 3)
        {
            for (int j = 0; j < 31; j += 2)
            {
                Instantiate(firePrefab, summonPoints[j].position, Quaternion.identity);
            }
        }
        else
        {
            for (int j = 0; j < 31; j++)
            {
                Instantiate(firePrefab, summonPoints[j].position, Quaternion.identity);
            }
        }
        SetIdle(true);
    }

    public void SummonShadow()
    {
        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
    }

    public void DestroyShadow()
    {
        Destroy(shadow, 0.5f);
    }

    private void BossDie()
    {
        SetIdle(false);
        Debug.Log("Boss has been defeated!");
        bossCollider.enabled = false; // 충돌 비활성화
        if (isDead) return; // 이미 사망 상태인 경우 중복 실행 방지
        isDead = true; // 사망 상태로 설정
        animator.SetTrigger("DIE");
        //StartCoroutine(GameManager.Instance.EndingCoroutine());
        Destroy(gameObject, 1.1f);
    }
}
