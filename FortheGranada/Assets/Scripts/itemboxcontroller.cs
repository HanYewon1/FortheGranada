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
        spriteRenderer.sprite = ItemBoxSprites[0]; //상자 닫힌 상태로 시작 
    }
    private void Update()
    {
        IsItemBoxOpen();
    }

    void IsItemBoxOpen() { //아이템 상자 열렸을 경우
        if(isOpen){ //상자 열리면
            spriteRenderer.sprite = ItemBoxSprites[1]; //상자 열린 sprite로 변경
        }
    }
}
