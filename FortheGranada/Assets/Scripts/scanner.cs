using UnityEngine;

public class scanner : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    [SerializeField] private Vector2 mapMin = Vector2.zero; // �� �ּ� ��ǥ
    [SerializeField] private Vector2 mapMax = Vector2.zero; // �� �ִ� ��ǥ
    [SerializeField] private bool hasBorders = false;      // ��谪 ��ȿ ����

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
            // Border�� Collider2D���� ��谪 ���
            Collider2D borderCollider = other.GetComponent<Collider2D>();
            if (borderCollider != null)
            {
                Bounds bounds = borderCollider.bounds;
                mapMin = bounds.min; // �ٿ�� �ڽ� �ּҰ�
                mapMax = bounds.max; // �ٿ�� �ڽ� �ִ밪
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
            Debug.Log(other.name + "���� ���!");
        }
    }

    // �ּ�/�ִ� ��ǥ ��ȯ �޼���
    public Vector2 GetMapMin() => mapMin;
    public Vector2 GetMapMax() => mapMax;
    public bool HasValidBorders() => hasBorders;

    // �����: ��踦 �ð�ȭ
    private void OnDrawGizmos()
    {
        if (hasBorders)
        {
            Gizmos.color = Color.red;

            // ��� �簢�� �׸���
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMin.y, 0), new Vector3(mapMax.x, mapMin.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMin.y, 0), new Vector3(mapMax.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMax.x, mapMax.y, 0), new Vector3(mapMin.x, mapMax.y, 0));
            Gizmos.DrawLine(new Vector3(mapMin.x, mapMax.y, 0), new Vector3(mapMin.x, mapMin.y, 0));
        }
    }
}
