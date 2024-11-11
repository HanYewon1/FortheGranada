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
            if(item.GetItemID == 1)//ü�� ������
            {
                if(health_item < item.GetNumNesting)
                {
                    maxHealth++;
                    health++;
                }
                else if(maxHealth > health)//�ִ� ���� �ʰ��� ȸ�� �����۰� ���� ȿ��
                {
                    health++;
                }
            }
            else if (item.GetItemID == 2)//ȸ�� ������
            {
                if (maxHealth > health)
                {
                    health++;
                }
            }
        }
        else if(item.GetItemType == ItemType.Passive)
        {
            if (item.GetItemID == 4 && speed_item < item.GetNumNesting)//�ӵ� ������
            {
                speed += 0.1f;
            }
            else if (item.GetItemID == 6)//�ǰ� ������
            {
                is_attacked_speed = true;
            }
            else if (item.GetItemID == 7 && stealthTime < item.GetNumNesting)//���� ������
            {
                stealthTime++;
            }
            else if (item.GetItemID == 8)//���� ������
            {
                is_preview = true;
            }
        }
        else if(item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3)//���� ������
            {
                armor++;
            }
        }
        else if(item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && ressurectiom_item < item.GetNumNesting)//��Ȱ ������
            {
                ressurection++;
            }
        }
        else if(item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9)//���� ����
            {
                key++;
            }
        }
    }
}
