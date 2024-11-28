using UnityEngine;
using UnityEngine.Events;

public class bosscontroller : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.boss_max_health = 100f;
        GameManager.Instance.boss_health = GameManager.Instance.boss_max_health; // 시작 시 체력을 최대값으로 설정
        UpdateHealthBar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 데미지 입힘
        {
            TakeDamage(10f);
        }
    }

    // 데미지를 받았을 때 호출
    public void TakeDamage(float damage)
    {
        GameManager.Instance.boss_health -= damage;
        GameManager.Instance.boss_health = Mathf.Clamp(GameManager.Instance.boss_health, 0, GameManager.Instance.boss_max_health); // 체력을 0 이상으로 제한

        UpdateHealthBar(); // 체력 바 업데이트

        if (GameManager.Instance.boss_health <= 0)
        {
            Die(); // 보스 사망 처리
        }
    }

    // 체력 바 UI 업데이트
    private void UpdateHealthBar()
    {
        if (GameManager.Instance.healthSlider != null)
        {
            GameManager.Instance.healthSlider.value = GameManager.Instance.boss_health / GameManager.Instance.boss_max_health; // 0~1로 값 전달
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        // 추가적인 보스 사망 로직 작성
    }
}
