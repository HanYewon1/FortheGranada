using UnityEngine;

public class scanner : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Vector3 collisionPoint;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            ResizeBoxCollider();
        }
    }

    void ResizeBoxCollider()
    {
        // �ػ󵵿� ���� ���� ���
        float widthRatio = Screen.width / 1920f; // ���� �ػ�: 1920
        float heightRatio = Screen.height / 1080f; // ���� �ػ�: 1080
        float offset;
        if (Screen.width <= 1024 || Screen.height <= 768)
        {
            offset = 1.4f;
        }
        else
        {
            offset = 1f;
        }
        // BoxCollider ũ�� ���� (���� ũ�⸦ �ػ� ������ ����)
        boxCollider.size = new Vector2(
            boxCollider.size.x * widthRatio * offset,
            boxCollider.size.y * heightRatio * offset
        //boxCollider.size.z ���̴� �������� ����
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("border"))
        {
            GameManager.Instance.is_border = true;
            collisionPoint = new Vector3(other.ClosestPoint(transform.position).x, other.ClosestPoint(transform.position).y, -10);
            Debug.Log("���� ����");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("border"))
        {
            GameManager.Instance.is_border = false;
            Debug.Log(other.name + "���� ���!");
        }
    }
}
