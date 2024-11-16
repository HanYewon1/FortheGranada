using Unity.VisualScripting;
using UnityEngine;

public class itemboxcontroller : MonoBehaviour
{
    public bool isOpen;
    public Sprite[] ItemBoxSprites;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemBoxSprites[0]; //���� ���� ���·� ���� 
    }
    private void Update()
    {
        IsItemBoxOpen();
    }

    void IsItemBoxOpen() { //������ ���� ������ ���
        if(isOpen){ //���� ������
            spriteRenderer.sprite = ItemBoxSprites[1]; //���� ���� sprite�� ����
        }
    }
}
