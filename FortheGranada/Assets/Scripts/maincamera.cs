using UnityEngine;

public class maincamera : MonoBehaviour
{
    public Transform target; // ?��?���?? 캐릭?��?�� Transform
    public Vector3 offset;   // 카메?��??? 캐릭?�� 간의 거리 (?��?��?��)
    public float smoothSpeed = 0.125f; // ???직임?�� �???��?��??? ?��?��
    public scanner scan;

    // 맵의 경계 ?��?��
    public Vector3 minBounds; // 경계?�� 최소�?? (x, z ?��?�� x, y)
    public Vector3 maxBounds; // 경계?�� 최�??�?? (x, z ?��?�� x, y)

    void Awake()
    {
        offset = new Vector3(0, 0, -10);
        maxBounds = new Vector3(10, 10, -10);
        minBounds = new Vector3(-10, -10, -10);
        if (GameManager.Instance.is_ingame) scan = GameObject.Find("Scanner").GetComponent<scanner>();
    }

    void Start()
    {
        target = GameManager.Instance.player;
    }

    void Update()
    {
        //
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target�� ã�� ���߽��ϴ�!");
            return;
        }

        /*if (!GameManager.Instance.is_border)
        {
            UpdateCam();
        }
        else
        {
            UpdateBoundCam(scan.collisionPoint);
        }*/
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetComponent<Collider>().CompareTag("border"))
        {
            Debug.Log("border");
        }
        else
        {
            UpdateCam();
        }
    }*/

    void UpdateCam()
    {
        // 목표 ?���?? 계산
        Vector3 desiredPosition = target.position + offset;

        // 카메?�� ?���?? ?��?�� (�?? 경계 ?��?��?���?? ?��?��)
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedZ = desiredPosition.z; // Z값�?? ?��?��?���?? ?��?�� 경우 그�??�?? ?���??
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y); // Y �?? ?��?��
        Vector3 boundedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // �???��?���?? ?���?? 보정
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundedPosition, smoothSpeed);

        // 카메?�� ?���?? ?��?��?��?��
        transform.position = smoothedPosition;

        // 카메?���?? ?��?�� ????�� 캐릭?���?? 바라보도�?? ?��?�� (?��?��)
        transform.LookAt(target);

        // Debug
        //Debug.Log($"캐릭?�� ?���??: {target.position}");
        //Debug.Log($"카메?�� ?���??: {Camera.main.transform.position}");
    }

    void UpdateBoundCam(Vector3 collisionPoint)
    {
        Debug.Log(collisionPoint);
        // Ÿ���� ���ϴ� ��ġ ���
        Vector3 desiredPosition = target.position + offset;

        // �浹 ������ �������� ��谪 ���� (��: minBounds�� maxBounds)
        if (collisionPoint != Vector3.zero)
        {
            // �浹 ������ �������� ��谪 ������Ʈ
            minBounds.x = Mathf.Min(minBounds.x, desiredPosition.x);
            minBounds.y = Mathf.Min(minBounds.y, desiredPosition.y);
            maxBounds.x = Mathf.Max(maxBounds.x, desiredPosition.x);
            maxBounds.y = Mathf.Max(maxBounds.y, desiredPosition.y);
        }

        // ��ġ ���� ����
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        float clampedZ = desiredPosition.z; // Z ���� �������� ����
        Vector3 boundedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // �ε巴�� ��ġ �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundedPosition, smoothSpeed);

        // ī�޶� ��ġ ����
        transform.position = smoothedPosition;

        // Ÿ���� �ٶ�
        transform.LookAt(target);
    }
}
