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
        spriteRenderer.sprite = ItemBoxSprites[0]; // 상자 닫힌 상태로 시작
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
            /*Color originalColor = spriteRenderer.color; // 상자 원래 색
            float darkenFactor = 0.1f; // 어두워지는 정도 (0: 원래 색, 1: 완전히 검은색, 0.5: 반만 어두워짐)
            Color darkerColor = Color.Lerp(originalColor, Color.black, darkenFactor); // 러프로 반만 어두워지게 함
            spriteRenderer.color = darkerColor; // 색 적용
            
            Color originalColor = spriteRenderer.color; // 원래 색
            float darkenAmount = 0.1f; // 어둡게 할 정도 (0: 원래 색, 1: 완전히 검은색, 0.5: 반만 어두워짐)
            Color darkerColor = new Color(
                originalColor.r * (1 - darkenAmount),
                originalColor.g * (1 - darkenAmount),
                originalColor.b * (1 - darkenAmount),
                originalColor.a // 알파값 유지
            );
            spriteRenderer.color = darkerColor; // 색 적용*/
            spriteRenderer.color = Color.gray;
        }
    }
}
