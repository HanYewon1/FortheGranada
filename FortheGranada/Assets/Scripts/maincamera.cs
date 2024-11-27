using UnityEngine;

public class maincamera : MonoBehaviour
{
    public Transform target; // ?ï¿½ï¿½?ï¿½ï¿½ï¿?? ìºë¦­?ï¿½ï¿½?ï¿½ï¿½ Transform
    public Vector3 offset;   // ì¹´ë©”?ï¿½ï¿½??? ìºë¦­?ï¿½ï¿½ ê°„ì˜ ê±°ë¦¬ (?ï¿½ï¿½?ï¿½ï¿½?ï¿½ï¿½)
    public float smoothSpeed = 0.125f; // ???ì§ì„?ï¿½ï¿½ ï¿???ï¿½ï¿½?ï¿½ï¿½??? ?ï¿½ï¿½?ï¿½ï¿½
    public scanner scan;

    // ë§µì˜ ê²½ê³„ ?ï¿½ï¿½?ï¿½ï¿½
    public Vector3 minBounds; // ê²½ê³„?ï¿½ï¿½ ìµœì†Œï¿?? (x, z ?ï¿½ï¿½?ï¿½ï¿½ x, y)
    public Vector3 maxBounds; // ê²½ê³„?ï¿½ï¿½ ìµœï¿½??ï¿?? (x, z ?ï¿½ï¿½?ï¿½ï¿½ x, y)

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
            Debug.LogWarning("TargetÀ» Ã£Áö ¸øÇß½À´Ï´Ù!");
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

    void UpdateBoundCam(Vector3 collisionPoint)
    {
        Debug.Log(collisionPoint);
        // Å¸°ÙÀÇ ¿øÇÏ´Â À§Ä¡ °è»ê
        Vector3 desiredPosition = target.position + offset;

        // Ãæµ¹ ÁöÁ¡À» ±âÁØÀ¸·Î °æ°è°ª °»½Å (¿¹: minBounds¿Í maxBounds)
        if (collisionPoint != Vector3.zero)
        {
            // Ãæµ¹ ÁöÁ¡À» ±âÁØÀ¸·Î °æ°è°ª ¾÷µ¥ÀÌÆ®
            minBounds.x = Mathf.Min(minBounds.x, desiredPosition.x);
            minBounds.y = Mathf.Min(minBounds.y, desiredPosition.y);
            maxBounds.x = Mathf.Max(maxBounds.x, desiredPosition.x);
            maxBounds.y = Mathf.Max(maxBounds.y, desiredPosition.y);
        }

        // À§Ä¡ Á¦ÇÑ Àû¿ë
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        float clampedZ = desiredPosition.z; // Z ÃàÀº Á¦ÇÑÇÏÁö ¾ÊÀ½
        Vector3 boundedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // ºÎµå·´°Ô À§Ä¡ ÀÌµ¿
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundedPosition, smoothSpeed);

        // Ä«¸Ş¶ó À§Ä¡ Àû¿ë
        transform.position = smoothedPosition;

        // Å¸°ÙÀ» ¹Ù¶óº½
        transform.LookAt(target);
    }
}
