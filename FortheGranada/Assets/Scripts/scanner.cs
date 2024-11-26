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
        // 해상도에 따른 비율 계산
        float widthRatio = Screen.width / 1920f; // 기준 해상도: 1920
        float heightRatio = Screen.height / 1080f; // 기준 해상도: 1080
        float offset;
        if (Screen.width <= 1024 || Screen.height <= 768)
        {
            offset = 1.4f;
        }
        else
        {
            offset = 1f;
        }
        // BoxCollider 크기 조정 (기존 크기를 해상도 비율로 조정)
        boxCollider.size = new Vector2(
            boxCollider.size.x * widthRatio * offset,
            boxCollider.size.y * heightRatio * offset
        //boxCollider.size.z 깊이는 조정하지 않음
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("border"))
        {
            GameManager.Instance.is_border = true;
            collisionPoint = new Vector3(other.ClosestPoint(transform.position).x, other.ClosestPoint(transform.position).y, -10);
            Debug.Log("보더 접근");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("border"))
        {
            GameManager.Instance.is_border = false;
            Debug.Log(other.name + "에서 벗어남!");
        }
    }
}
