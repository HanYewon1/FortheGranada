using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 캐릭터의 Transform
    public Vector3 offset;   // 카메라와 캐릭터 간의 거리 (오프셋)
    public float smoothSpeed = 0.125f; // 움직임의 부드러움 정도

    // 맵의 경계 설정
    public Vector3 minBounds; // 경계의 최소값 (x, z 또는 x, y)
    public Vector3 maxBounds; // 경계의 최대값 (x, z 또는 x, y)

    void Awake()
    {
        offset = new Vector3(0,0,-10);
        maxBounds = new Vector3(1000,1000,1000);
        minBounds = new Vector3(-1000,-1000,-1000);
    }

    void Start()
    {
        target = GameManager.Instance.player;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target이 설정되지 않았습니다!");
            return;
        }

        // 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;

        // 카메라 위치 제한 (맵 경계 내에서만 이동)
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedZ = desiredPosition.z; // Z값은 제한하지 않는 경우 그대로 유지
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y); // Y 값 제한
        Vector3 boundedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // 부드럽게 위치 보정
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundedPosition, smoothSpeed);

        // 카메라 위치 업데이트
        transform.position = smoothedPosition;

        // 카메라가 항상 대상 캐릭터를 바라보도록 설정 (옵션)
        transform.LookAt(target);

        // Debug
        //Debug.Log($"캐릭터 위치: {target.position}");
        //Debug.Log($"카메라 위치: {Camera.main.transform.position}");
    }
}
