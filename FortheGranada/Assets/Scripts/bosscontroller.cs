using UnityEngine;
using UnityEngine.Events;

public class bosscontroller : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.boss_max_health = 100f;
        GameManager.Instance.boss_health = GameManager.Instance.boss_max_health; // ���� �� ü���� �ִ밪���� ����
        UpdateHealthBar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ������ ����
        {
            TakeDamage(10f);
        }
    }

    // �������� �޾��� �� ȣ��
    public void TakeDamage(float damage)
    {
        GameManager.Instance.boss_health -= damage;
        GameManager.Instance.boss_health = Mathf.Clamp(GameManager.Instance.boss_health, 0, GameManager.Instance.boss_max_health); // ü���� 0 �̻����� ����

        UpdateHealthBar(); // ü�� �� ������Ʈ

        if (GameManager.Instance.boss_health <= 0)
        {
            Die(); // ���� ��� ó��
        }
    }

    // ü�� �� UI ������Ʈ
    private void UpdateHealthBar()
    {
        if (GameManager.Instance.healthSlider != null)
        {
            GameManager.Instance.healthSlider.value = GameManager.Instance.boss_health / GameManager.Instance.boss_max_health; // 0~1�� �� ����
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        // �߰����� ���� ��� ���� �ۼ�
    }
}
