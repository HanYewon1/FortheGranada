using UnityEngine;

public class scanner : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    [SerializeField] private Vector2 mapMin = Vector2.zero; // 현재 경계 최소 좌표
    [SerializeField] private Vector2 mapMax = Vector2.zero; // 현재 경계 최대 좌표
    [SerializeField] private bool hasBorders = false;      // 경계값 유효 여부
    //[SerializeField] private Transform player;             // 플레이어 Transform

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            ResizeBoxCollider();
        }
    }

    private void ResizeBoxCollider()
    {
        // 해상도에 따른 비율 계산
        float widthRatio = Screen.width / 1920f; // 기준 해상도: 1920
        float heightRatio = Screen.height / 1080f; // 기준 해상도: 1080
        float offset = (Screen.width <= 1024 || Screen.height <= 768) ? 1.4f : 1f;

        // BoxCollider 크기 조정
        boxCollider.size = new Vector2(
            boxCollider.size.x * widthRatio * offset,
            boxCollider.size.y * heightRatio * offset
        );
    }

    private void Update()
    {
        if (GameManager.Instance.player == null) return;

        // 모든 Border 태그를 가진 객체 검색
        GameObject[] borders = GameObject.FindGameObjectsWithTag("border");
        if (borders.Length == 0) return;

        // 가장 가까운 Border 찾기
        float shortestDistance = float.MaxValue;
        GameObject nearestBorder = null;

        foreach (GameObject border in borders)
        {
            float distance = Vector2.Distance(GameManager.Instance.player.position, border.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestBorder = border;
            }
        }

        // 가까운 Border의 Bounds를 사용하여 경계 업데이트
        if (nearestBorder != null)
        {
            Collider2D borderCollider = nearestBorder.GetComponent<Collider2D>();
            if (borderCollider != null)
            {
                Bounds bounds = borderCollider.bounds;
                mapMin = bounds.min;
                mapMax = bounds.max;
                hasBorders = true;
            }
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
