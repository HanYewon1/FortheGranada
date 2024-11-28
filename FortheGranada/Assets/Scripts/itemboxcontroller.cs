using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class itemboxcontroller : MonoBehaviour
{
    public bool isOpen;
    public Sprite[] ItemBoxSprites;
    public inneritem ii;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Object[] sprites = Resources.LoadAll("FDR_Dungeon");
        spriteRenderer = GetComponent<SpriteRenderer>();
        ItemBoxSprites = new Sprite[2];
        ItemBoxSprites[0] = sprites[535] as Sprite;
        ItemBoxSprites[1] = sprites[536] as Sprite;
        spriteRenderer.sprite = ItemBoxSprites[0]; // ���� ���� ���·� ����
        ii = GetComponentInChildren<inneritem>(true);
    }
    private void Update()
    {
        IsPossible();
        IsItemBoxOpen();
    }

    void IsItemBoxOpen()
    {
        // ������ ���� ������ ���
        if (isOpen) // ���� ������
        {
            spriteRenderer.sprite = ItemBoxSprites[1]; // ���� ���� sprite�� ����
        }
    }

    void IsPossible()
    {
        if (!isOpen && GameManager.Instance.is_catch && !GameManager.Instance.is_delay) // ������ ���ڰ� Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.white; // ���ڻ� �Ͼ������ ����
        }
        else if (!isOpen && (!GameManager.Instance.is_catch || GameManager.Instance.is_delay))// ������ ���ڰ� ��Ȱ��ȭ�� ���
        {
            spriteRenderer.color = Color.gray;
        }
    }
}
