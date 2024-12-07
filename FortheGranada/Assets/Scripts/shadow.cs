using UnityEngine;

public class shadow : MonoBehaviour
{
    public bool isHot = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isHot)
        {
            //GameManager.Instance.health--;
            GameManager.Instance.pc.ChangeColor();
            Debug.Log("Hot!");
            isHot = false;
        }
    }
}
