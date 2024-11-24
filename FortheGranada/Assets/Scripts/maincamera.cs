using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public BoxCollider2D scanner;
    public Transform target; // ?ï¿½ï¿½?ï¿½ï¿½ï¿?? ìºë¦­?ï¿½ï¿½?ï¿½ï¿½ Transform
    public Vector3 offset;   // ì¹´ë©”?ï¿½ï¿½??? ìºë¦­?ï¿½ï¿½ ê°„ì˜ ê±°ë¦¬ (?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½)
    public float smoothSpeed = 0.125f; // ???ì§ì„?ï¿½ï¿½ ï¿???ï¿½ï¿½?ï¿½ï¿½??? ?ï¿½ï¿½?ï¿½ï¿½

    // ë§µì˜ ê²½ê³„ ?ï¿½ï¿½?ï¿½ï¿½
    public Vector3 minBounds; // ê²½ê³„?ï¿½ï¿½ ìµœì†Œï¿?? (x, z ?ï¿½ï¿½?ï¿½ï¿½ x, y)
    public Vector3 maxBounds; // ê²½ê³„?ï¿½ï¿½ ìµœï¿½??ï¿?? (x, z ?ï¿½ï¿½?ï¿½ï¿½ x, y)

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
            Debug.LogWarning("Target?ï¿½ï¿½ ?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½!");
            return;
        }

        OnTriggerEnter2D(scanner);
    }

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.CompareTag("border"))
        {
            Debug.Log("º¸´õ ±ÙÁ¢");
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
        // ëª©í‘œ ?ï¿½ï¿½ï¿?? ê³„ì‚°
        Vector3 desiredPosition = target.position + offset;

        // ì¹´ë©”?ï¿½ï¿½ ?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½ (ï¿?? ê²½ê³„ ?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½)
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedZ = desiredPosition.z; // Zê°’ï¿½?? ?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½ ê²½ìš° ê·¸ï¿½??ï¿?? ?ï¿½ï¿½ï¿??
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y); // Y ï¿?? ?ï¿½ï¿½?ï¿½ï¿½
        Vector3 boundedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // ï¿???ï¿½ï¿½?ï¿½ï¿½ï¿?? ?ï¿½ï¿½ï¿?? ë³´ì •
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundedPosition, smoothSpeed);

        // ì¹´ë©”?ï¿½ï¿½ ?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½
        transform.position = smoothedPosition;

        // ì¹´ë©”?ï¿½ï¿½ï¿?? ?ï¿½ï¿½?ï¿½ï¿½ ????ï¿½ï¿½ ìºë¦­?ï¿½ï¿½ï¿?? ë°”ë¼ë³´ë„ï¿?? ?ï¿½ï¿½?ï¿½ï¿½ (?ï¿½ï¿½?ï¿½ï¿½)
        transform.LookAt(target);

        // Debug
        //Debug.Log($"ìºë¦­?ï¿½ï¿½ ?ï¿½ï¿½ï¿??: {target.position}");
        //Debug.Log($"ì¹´ë©”?ï¿½ï¿½ ?ï¿½ï¿½ï¿??: {Camera.main.transform.position}");
    }
}
