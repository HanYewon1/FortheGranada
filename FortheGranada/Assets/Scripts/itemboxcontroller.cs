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
    public Vector3 pushForce; // 밀쳐내는 힘의 크기
    public Vector3 pushDirection;
    public Vector2 V2;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemBoxSprites[0]; // ���� ���� ���·� ����
        ii = GetComponentInChildren<inneritem>(true);
        pushForce = new Vector3(-50, -50, 0);
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
            //pushDirection = (collision.transform.position - transform.position).normalized;           
            //V2 = (Vector2)Vector3.Scale(pushDirection, pushForce);
            // 힘 적용
            //otherrb.AddForce(V2, ForceMode2D.Impulse);
            //otherrb.linearVelocity = new Vector3(pushDirection.x + pushForce, pushDirection.y + pushForce, 0);

            audiomanager.Instance.bossdash.Play();

            // 밀치는 방향 계산
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

            // 밀어내는 힘의 크기를 곱하여 새로운 위치 계산
            Vector2 newPosition = otherrb.position + (Vector2)pushDirection * 1f;

            // Rigidbody2D의 위치 이동
            otherrb.MovePosition(newPosition);

            // 디버그 출력
            Debug.Log($"Player moved to: {newPosition}");

            //Debug.Log($"PUSH! Direction: {pushDirection}, Force: {V2}, Magnitude: {V2.magnitude}");
            //Debug.Log($"Player Velocity: {otherrb.linearVelocity}");
        }
    }
}
