using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int health;
    public float speed;
    public int maxHealth;
    public int armor;
    public int stealthTime;
    public int key;
    public int ressurection;
    public int health_item;
    public int armor_item;
    public int stealth_item;
    public int key_item;
    public int speed_item;
    public int ressurectiom_item;
    public bool is_attacked_speed;
    public bool is_preview;
    

    // Start is called before the first frame update
    void Start()
    {
        is_attacked_speed = false;
        is_preview = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getItem(Item item)
    {
        if(item.GetItemType == ItemType.Expendables)
        {
            if(item.GetItemID == 1)//체력 아이템
            {
                if(health_item < item.GetNumNesting)
                {
                    maxHealth++;
                    health++;
                }
                else if(maxHealth > health)//최대 갯수 초과시 회복 아이템과 같은 효과
                {
                    health++;
                }
            }
            else if (item.GetItemID == 2)//회복 아이템
            {
                if (maxHealth > health)
                {
                    health++;
                }
            }
        }
        else if(item.GetItemType == ItemType.Passive)
        {
            if (item.GetItemID == 4 && speed_item < item.GetNumNesting)//속도 아이템
            {
                speed += 0.1f;
            }
            else if (item.GetItemID == 6)//피격 아이템
            {
                is_attacked_speed = true;
            }
            else if (item.GetItemID == 7 && stealthTime < item.GetNumNesting)//감지 아이템
            {
                stealthTime++;
            }
            else if (item.GetItemID == 8)//투시 아이템
            {
                is_preview = true;
            }
        }
        else if(item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3)//갑옷 아이템
            {
                armor++;
            }
        }
        else if(item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && ressurectiom_item < item.GetNumNesting)//부활 아이템
            {
                ressurection++;
            }
        }
        else if(item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9)//열쇠 조각
            {
                key++;
            }
        }
    }
}
