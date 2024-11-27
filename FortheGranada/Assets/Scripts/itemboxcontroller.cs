using Unity.VisualScripting;
using UnityEngine;

public class itemboxcontroller : MonoBehaviour
{
    public bool isOpen;
    public Sprite[] ItemBoxSprites;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Object[] sprites = Resources.LoadAll("FDR_Dungeon");
        spriteRenderer = GetComponent<SpriteRenderer>();
        ItemBoxSprites = new Sprite[2];
        ItemBoxSprites[0] = sprites[535] as Sprite;
        ItemBoxSprites[1] = sprites[536] as Sprite;
        spriteRenderer.sprite = ItemBoxSprites[0]; // ���� ���� ���·� ����
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
            /*Color originalColor = spriteRenderer.color; // ���� ���� ��
            float darkenFactor = 0.1f; // ��ο����� ���� (0: ���� ��, 1: ������ ������, 0.5: �ݸ� ��ο���)
            Color darkerColor = Color.Lerp(originalColor, Color.black, darkenFactor); // ������ �ݸ� ��ο����� ��
            spriteRenderer.color = darkerColor; // �� ����
            
            Color originalColor = spriteRenderer.color; // ���� ��
            float darkenAmount = 0.1f; // ��Ӱ� �� ���� (0: ���� ��, 1: ������ ������, 0.5: �ݸ� ��ο���)
            Color darkerColor = new Color(
                originalColor.r * (1 - darkenAmount),
                originalColor.g * (1 - darkenAmount),
                originalColor.b * (1 - darkenAmount),
                originalColor.a // ���İ� ����
            );
            spriteRenderer.color = darkerColor; // �� ����*/
            spriteRenderer.color = Color.gray;
        }
    }
}
