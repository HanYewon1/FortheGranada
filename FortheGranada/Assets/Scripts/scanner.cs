using UnityEngine;

public class scanner : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    [SerializeField] private Vector2 mapMin = Vector2.zero; // 맵 최소 좌표
    [SerializeField] private Vector2 mapMax = Vector2.zero; // 맵 최대 좌표
    [SerializeField] private bool hasBorders = false;      // 경계값 유효 여부

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
            // Border의 Collider2D에서 경계값 계산
            Collider2D borderCollider = other.GetComponent<Collider2D>();
            if (borderCollider != null)
            {
                Bounds bounds = borderCollider.bounds;
                mapMin = bounds.min; // 바운딩 박스 최소값
                mapMax = bounds.max; // 바운딩 박스 최대값
                hasBorders = true;

                //Debug.Log($"Border detected! MapMin: {mapMin}, MapMax: {mapMax}");
            }
            else
            {
                Debug.LogError("Border object does not have a Collider2D component!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("border"))
        {
            Debug.Log(other.name + "에서 벗어남!");
        }
    }

    // 최소/최대 좌표 반환 메서드
    public Vector2 GetMapMin() => mapMin;
    public Vector2 GetMapMax() => mapMax;
    public bool HasValidBorders() => hasBorders;

    // 디버그: 경계를 시각화
    private void OnDrawGizmos()
    {
        if (hasBorders)
        {
            Gizmos.color = Color.red;

            // 경계 사각형 그리기
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMin.y, 0), new Vector3(mapMax.x, mapMin.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMin.y, 0), new Vector3(mapMax.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMax.y, 0), new Vector3(mapMin.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMax.y, 0), new Vector3(mapMin.x, mapMin.y, 0));
        }
    }
}
