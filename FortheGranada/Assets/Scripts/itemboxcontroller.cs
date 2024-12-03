using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class itemboxcontroller : MonoBehaviour
{
    public bool isOpen;
    public Sprite[] ItemBoxSprites;
    public inneritem ii;
    public Rigidbody2D otherrb;
    SpriteRenderer spriteRenderer;
    public float pushForce = 5f; // 밀쳐내는 힘의 크기

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemBoxSprites[0]; // ���� ���� ���·� ����
        ii = GetComponentInChildren<inneritem>(true);
        pushForce = 20f;
    }
    private void Update()
    {
        if (spriteRenderer != null)
        {
            IsPossible();
            IsItemBoxOpen();
        }
    }

    void IsItemBoxOpen()
    {
        // ������ ���� ������ ���
        if (isOpen) // ���� ������
        {
            spriteRenderer.sprite = ItemBoxSprites[1]; // ���� ���� sprite�� ����
        }
    }

    void IsPossible()
    {
        if (!isOpen && GameManager.Instance.is_catch && !GameManager.Instance.is_delay) // ������ ���ڰ� Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.white; // ���ڻ� �Ͼ������ ����
        }
        else if (!isOpen && (!GameManager.Instance.is_catch || GameManager.Instance.is_delay))// ������ ���ڰ� ��Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.gray;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        otherrb = collision.rigidbody;

        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.is_delay)
        {
            // 밀쳐내는 방향 계산
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

            // 힘 적용
            otherrb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            //otherrb.linearVelocity = new Vector3(pushDirection.x + pushForce, pushDirection.y + pushForce, 0);

            Debug.Log($"PUSH by {pushDirection}");
        }
    }
}
