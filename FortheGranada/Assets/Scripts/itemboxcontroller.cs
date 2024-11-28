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
        spriteRenderer.sprite = ItemBoxSprites[0]; // 상자 닫힌 상태로 시작
        ii = GetComponentInChildren<inneritem>(true);
    }
    private void Update()
    {
        IsPossible();
        IsItemBoxOpen();
    }

    void IsItemBoxOpen()
    {
        // 아이템 상자 열렸을 경우
        if (isOpen) // 상자 열리면
        {
            spriteRenderer.sprite = ItemBoxSprites[1]; // 상자 열린 sprite로 변경
        }
    }

    void IsPossible()
    {
        if (!isOpen && GameManager.Instance.is_catch && !GameManager.Instance.is_delay) // 아이템 상자가 활성화된 경우
        {
            spriteRenderer.color = Color.white; // 상자색 하얀색으로 변경
        }
        else if (!isOpen && (!GameManager.Instance.is_catch || GameManager.Instance.is_delay))// 아이템 상자가 비활성화된 경우
        {
            spriteRenderer.color = Color.gray;
        }
    }
}
