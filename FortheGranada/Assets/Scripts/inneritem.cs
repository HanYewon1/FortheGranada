using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class inneritem : MonoBehaviour
{
    [SerializeField] private int Itemnumber;
    public int itemnumber
    {
        get => Itemnumber;
        set
        {
            Itemnumber = value;
        }
    }

    public Item item;
    public SpriteRenderer SR;
    public bool is_set;
    Color originalColor;
    Color darkerColor;
    float darkenAmount;
    float newPPU;

    private void Awake()
    {
        itemnumber = 10;
        SR = GetComponent<SpriteRenderer>();
        if (SR != null) originalColor = SR.color; // 원래 색
        darkenAmount = 0f; // 어둡게 할 정도 (0: 원래 색, 1: 완전히 검은색, 0.5: 반만 어두워짐)
        Alpha0();
    }

    private void Update()
    {
        if (GameManager.Instance.is_preview)
        {
            Alpha255();
        }
        else
        {
            //Alpha0();
        }
        if (is_set && itemnumber != 10)
        {
            //newPPU = 200f;
            //sprite.texture.filterMode = FilterMode.Point;
            item = GameManager.Instance.im.itemlist[itemnumber];
            if (SR != null) SR.sprite = item.GetItemSprite;
            transform.localScale = new Vector3(0.1f, 0.1f, 1f); // 크기 조정
        }
    }

    public void Alpha0()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            0 // 알파값 변경(originalColor.a)
        );
        if (SR != null) SR.color = darkerColor;
    }

    public void Alpha255()
    {
        darkerColor = new Color(
            originalColor.r * (1 - darkenAmount),
            originalColor.g * (1 - darkenAmount),
            originalColor.b * (1 - darkenAmount),
            255 // 알파값 변경(originalColor.a)
            );
        if (SR != null) SR.color = darkerColor;
    }

    /*Color originalColor = spriteRenderer.color; // 상자 원래 색
    float darkenFactor = 0.1f; // 어두워지는 정도 (0: 원래 색, 1: 완전히 검은색, 0.5: 반만 어두워짐)
    Color darkerColor = Color.Lerp(originalColor, Color.black, darkenFactor); // 러프로 반만 어두워지게 함
    spriteRenderer.color = darkerColor; // 색 적용*/
}
