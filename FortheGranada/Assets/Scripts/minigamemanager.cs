//using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class minigamemanager : MonoBehaviour
{
    //Random random = new Random();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[] RanNumGen()
    {
        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < 3)
        {
            uniqueNumbers.Add(Random.Range(1, 41));
        }
        return uniqueNumbers.ToArray();
    }

    public string[] AnswerSet()
    {
        string[] ansset = new string[102];

        ansset[1] = "����";
        ansset[2] = "����";
        ansset[3] = "����";
        ansset[4] = "����";
        ansset[5] = "�Ĺ�";
        ansset[6] = "����";
        ansset[7] = "����";
        ansset[8] = "ǳ��";
        ansset[9] = "��";
        ansset[10] = "�׸�";
        ansset[11] = "����";
        ansset[12] = "��";
        ansset[13] = "��ü";
        ansset[14] = "��ü";
        ansset[15] = "��ü";
        ansset[16] = "ũ��";
        ansset[17] = "�۴�";
        ansset[18] = "�д�";
        ansset[19] = "����";
        ansset[20] = "�ϴ�";
        ansset[21] = "�ٴ�";
        ansset[22] = "��";
        ansset[23] = "����";
        ansset[24] = "����";
        ansset[25] = "��";
        ansset[26] = "����";
        ansset[27] = "��ȭ";
        ansset[28] = "�ٱ�";
        ansset[29] = "��";
        ansset[30] = "���";
        ansset[31] = "���׶��";
        ansset[32] = "������";
        ansset[33] = "�Ķ���";
        ansset[34] = "�����";
        ansset[35] = "�ʷϻ�";
        ansset[36] = "�����";
        ansset[37] = "��Ȳ��";
        ansset[38] = "����";
        ansset[39] = "���";
        ansset[40] = "������";

        return ansset;
    }

    public Sprite[] ImageSet()
    {
        Texture2D texture;
        Sprite[] img_list = new Sprite[102];

        texture = Resources.Load<Texture2D>("MG_1_001");
        img_list[1] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_002");
        img_list[2] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_003");
        img_list[3] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_004");
        img_list[4] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_005");
        img_list[5] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_006");
        img_list[6] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_007");
        img_list[7] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_008");
        img_list[8] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_009");
        img_list[9] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_010");
        img_list[10] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_011");
        img_list[11] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_012");
        img_list[12] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_013");
        img_list[13] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_014");
        img_list[14] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_015");
        img_list[15] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_016");
        img_list[16] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_017");
        img_list[17] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_018");
        img_list[18] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_019");
        img_list[19] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_020");
        img_list[20] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_021");
        img_list[21] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_022");
        img_list[22] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_023");
        img_list[23] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_024");
        img_list[24] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_025");
        img_list[25] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_026");
        img_list[26] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_027");
        img_list[27] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_028");
        img_list[28] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_029");
        img_list[29] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_030");
        img_list[30] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_031");
        img_list[31] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_032");
        img_list[32] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_033");
        img_list[33] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_034");
        img_list[34] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_035");
        img_list[35] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_036");
        img_list[36] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_037");
        img_list[37] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_038");
        img_list[38] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_039");
        img_list[39] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        texture = Resources.Load<Texture2D>("MG_1_040");
        img_list[40] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return img_list;
    }
}
