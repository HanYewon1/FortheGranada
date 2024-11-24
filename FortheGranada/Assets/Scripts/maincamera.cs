using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public BoxCollider2D scanner;
    public Transform target; // ?��?���?? 캐릭?��?�� Transform
    public Vector3 offset;   // 카메?��??? 캐릭?�� 간의 거리 (?��?��?��)
    public float smoothSpeed = 0.125f; // ???직임?�� �???��?��??? ?��?��

    // 맵의 경계 ?��?��
    public Vector3 minBounds; // 경계?�� 최소�?? (x, z ?��?�� x, y)
    public Vector3 maxBounds; // 경계?�� 최�??�?? (x, z ?��?�� x, y)

    void Awake()
    {
        offset = new Vector3(0, 0, -10);
        maxBounds = new Vector3(10000, 10000, 10000);
        minBounds = new Vector3(-10000, -10000, -10000);
    }

    void Start()
    {
        target = GameManager.Instance.player;
        scanner = GameManager.Instance.player.GetComponentInChildren<BoxCollider2D>();
    }

    void Update()
    {
        //
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target?�� ?��?��?���?? ?��?��?��?��?��!");
            return;
        }

        OnTriggerEnter2D(scanner);
    }

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.CompareTag("border"))
        {
            Debug.Log("���� ����");
        }

        else
        {
            UpdateCam();
        }
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
}
