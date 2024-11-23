//using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class minigamemanager : MonoBehaviour
{

    public int[] RanNumGen()
    {
        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < 8)
        {
            uniqueNumbers.Add(Random.Range(1, 67));
        }
        return uniqueNumbers.ToArray();
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

        for (int i = 41; i < 67; i++)
        {
            string imgname = "MG_1_0" + i;
            texture = Resources.Load<Texture2D>(imgname);
            img_list[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        return img_list;
    }
}
