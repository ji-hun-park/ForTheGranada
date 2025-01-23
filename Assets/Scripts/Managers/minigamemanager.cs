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
        while (uniqueNumbers.Count < 3)
        {
            uniqueNumbers.Add(Random.Range(1, 101));
        }
        return uniqueNumbers.ToArray();
    }

    public int[] RanNumGenWithNum(int num1, int num2)
    {
        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < num1)
        {
            uniqueNumbers.Add(Random.Range(0, num2));
        }
        return uniqueNumbers.ToArray();
    }

    public void FailRequest()
    {
        int r = Random.Range(1, 25);

        switch (r)
        {
            case 1:
                GameManager.Instance.rannum3[0] = 1;
                GameManager.Instance.rannum3[1] = 2;
                GameManager.Instance.rannum3[2] = 3;
                APIManager.Instance.APIResponse = "동물";
                break;
            case 2:
                GameManager.Instance.rannum3[0] = 4;
                GameManager.Instance.rannum3[1] = 5;
                GameManager.Instance.rannum3[2] = 6;
                APIManager.Instance.APIResponse = "동물";
                break;
            case 3:
                GameManager.Instance.rannum3[0] = 7;
                GameManager.Instance.rannum3[1] = 8;
                GameManager.Instance.rannum3[2] = 9;
                APIManager.Instance.APIResponse = "동물";
                break;
            case 4:
                GameManager.Instance.rannum3[0] = 11;
                GameManager.Instance.rannum3[1] = 12;
                GameManager.Instance.rannum3[2] = 13;
                APIManager.Instance.APIResponse = "음식";
                break;
            case 5:
                GameManager.Instance.rannum3[0] = 14;
                GameManager.Instance.rannum3[1] = 15;
                GameManager.Instance.rannum3[2] = 16;
                APIManager.Instance.APIResponse = "음식";
                break;
            case 6:
                GameManager.Instance.rannum3[0] = 17;
                GameManager.Instance.rannum3[1] = 18;
                GameManager.Instance.rannum3[2] = 19;
                APIManager.Instance.APIResponse = "음식";
                break;
            case 7:
                GameManager.Instance.rannum3[0] = 21;
                GameManager.Instance.rannum3[1] = 22;
                GameManager.Instance.rannum3[2] = 23;
                APIManager.Instance.APIResponse = "괴물";
                break;
            case 8:
                GameManager.Instance.rannum3[0] = 24;
                GameManager.Instance.rannum3[1] = 25;
                GameManager.Instance.rannum3[2] = 26;
                APIManager.Instance.APIResponse = "괴물";
                break;
            case 9:
                GameManager.Instance.rannum3[0] = 27;
                GameManager.Instance.rannum3[1] = 28;
                GameManager.Instance.rannum3[2] = 29;
                APIManager.Instance.APIResponse = "괴물";
                break;
            case 10:
                GameManager.Instance.rannum3[0] = 31;
                GameManager.Instance.rannum3[1] = 32;
                GameManager.Instance.rannum3[2] = 33;
                APIManager.Instance.APIResponse = "종족";
                break;
            case 11:
                GameManager.Instance.rannum3[0] = 34;
                GameManager.Instance.rannum3[1] = 35;
                GameManager.Instance.rannum3[2] = 36;
                APIManager.Instance.APIResponse = "종족";
                break;
            case 12:
                GameManager.Instance.rannum3[0] = 37;
                GameManager.Instance.rannum3[1] = 38;
                GameManager.Instance.rannum3[2] = 39;
                APIManager.Instance.APIResponse = "종족";
                break;
            case 13:
                GameManager.Instance.rannum3[0] = 43;
                GameManager.Instance.rannum3[1] = 41;
                GameManager.Instance.rannum3[2] = 42;
                APIManager.Instance.APIResponse = "직업";
                break;
            case 14:
                GameManager.Instance.rannum3[0] = 44;
                GameManager.Instance.rannum3[1] = 45;
                GameManager.Instance.rannum3[2] = 46;
                APIManager.Instance.APIResponse = "직업";
                break;
            case 15:
                GameManager.Instance.rannum3[0] = 47;
                GameManager.Instance.rannum3[1] = 48;
                GameManager.Instance.rannum3[2] = 49;
                APIManager.Instance.APIResponse = "직업";
                break;
            case 16:
                GameManager.Instance.rannum3[0] = 51;
                GameManager.Instance.rannum3[1] = 52;
                GameManager.Instance.rannum3[2] = 53;
                APIManager.Instance.APIResponse = "의상";
                break;
            case 17:
                GameManager.Instance.rannum3[0] = 54;
                GameManager.Instance.rannum3[1] = 55;
                GameManager.Instance.rannum3[2] = 56;
                APIManager.Instance.APIResponse = "신발";
                break;
            case 18:
                GameManager.Instance.rannum3[0] = 57;
                GameManager.Instance.rannum3[1] = 55;
                GameManager.Instance.rannum3[2] = 56;
                APIManager.Instance.APIResponse = "신발";
                break;
            case 19:
                GameManager.Instance.rannum3[0] = 60;
                GameManager.Instance.rannum3[1] = 59;
                GameManager.Instance.rannum3[2] = 58;
                APIManager.Instance.APIResponse = "판타지";
                break;
            case 20:
                GameManager.Instance.rannum3[0] = 62;
                GameManager.Instance.rannum3[1] = 61;
                GameManager.Instance.rannum3[2] = 63;
                APIManager.Instance.APIResponse = "판타지";
                break;
            case 21:
                GameManager.Instance.rannum3[0] = 64;
                GameManager.Instance.rannum3[1] = 65;
                GameManager.Instance.rannum3[2] = 66;
                APIManager.Instance.APIResponse = "도구";
                break;
            case 22:
                GameManager.Instance.rannum3[0] = 67;
                GameManager.Instance.rannum3[1] = 68;
                GameManager.Instance.rannum3[2] = 69;
                APIManager.Instance.APIResponse = "물약";
                break;
            case 23:
                GameManager.Instance.rannum3[0] = 77;
                GameManager.Instance.rannum3[1] = 84;
                GameManager.Instance.rannum3[2] = 85;
                APIManager.Instance.APIResponse = "꽃";
                break;
            case 24:
                GameManager.Instance.rannum3[0] = 73;
                GameManager.Instance.rannum3[1] = 89;
                GameManager.Instance.rannum3[2] = 90;
                APIManager.Instance.APIResponse = "건물";
                break;
            default:
                Debug.LogError("OutOfRange");
                break;
        }
    }

    public string[] AnswerSet()
    {
        string[] ansset = new string[102];

        ansset[1] = "바위";
        ansset[2] = "가위";
        ansset[3] = "나무";
        ansset[4] = "세모";
        ansset[5] = "식물";
        ansset[6] = "종족";
        ansset[7] = "괴물";
        ansset[8] = "풍경";
        ansset[9] = "원";
        ansset[10] = "네모";
        ansset[11] = "동물";
        ansset[12] = "벽";
        ansset[13] = "액체";
        ansset[14] = "고체";
        ansset[15] = "기체";
        ansset[16] = "크다";
        ansset[17] = "작다";
        ansset[18] = "넓다";
        ansset[19] = "좁다";
        ansset[20] = "하늘";
        ansset[21] = "바다";
        ansset[22] = "숲";
        ansset[23] = "연못";
        ansset[24] = "공원";
        ansset[25] = "성";
        ansset[26] = "동굴";
        ansset[27] = "벽화";
        ansset[28] = "바깥";
        ansset[29] = "안";
        ansset[30] = "사람";
        ansset[31] = "동그라미";
        ansset[32] = "빨간색";
        ansset[33] = "파란색";
        ansset[34] = "노란색";
        ansset[35] = "초록색";
        ansset[36] = "보라색";
        ansset[37] = "주황색";
        ansset[38] = "남색";
        ansset[39] = "흰색";
        ansset[40] = "검은색";
        ansset[41] = "갈색";
        ansset[42] = "둥글다";
        ansset[43] = "네모나다";
        ansset[44] = "동그랗다";
        ansset[45] = "가깝다";
        ansset[46] = "멀다";
        ansset[47] = "뜨겁다";
        ansset[48] = "차갑다";
        ansset[49] = "판타지";
        ansset[50] = "환타지";
        ansset[51] = "픽셀";
        ansset[52] = "픽셀아트";
        ansset[53] = "픽셀 아트";
        ansset[54] = "정물화";
        ansset[55] = "음식";
        ansset[56] = "갑옷";
        ansset[57] = "점묘화";
        ansset[58] = "그림";
        ansset[59] = "거대";
        ansset[60] = "전설";
        ansset[61] = "없음";
        ansset[62] = "쪼그맣다";
        ansset[63] = "큼직하다";
        ansset[64] = "싱그럽다";
        ansset[65] = "징그럽다";
        ansset[66] = "무지개색";
        ansset[67] = "픽셀화";
        ansset[68] = "중세";
        ansset[69] = "고무적";
        ansset[70] = "테두리";
        ansset[71] = "있음";
        ansset[72] = "커다랗다";
        ansset[73] = "소인";
        ansset[74] = "대인";
        ansset[75] = "성벽";
        ansset[76] = "신화";
        ansset[77] = "그리스";
        ansset[78] = "중구난방";
        ansset[79] = "기사";
        ansset[80] = "정어리";
        ansset[81] = "제한";
        ansset[82] = "제안";
        ansset[83] = "정각";
        ansset[84] = "직각";
        ansset[85] = "삼각";
        ansset[86] = "사각";
        ansset[87] = "삼각형";
        ansset[88] = "사각형";
        ansset[89] = "모서리";
        ansset[90] = "귀퉁이";
        ansset[91] = "악귀";
        ansset[92] = "천사";
        ansset[93] = "악마";
        ansset[94] = "재앙";
        ansset[95] = "심각함";
        ansset[96] = "적당함";
        ansset[97] = "부족함";
        ansset[98] = "과함";
        ansset[99] = "넘쳐흐름";
        ansset[100] = "미세";


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

        for (int i = 41; i < 100; i++)
        {
            string imgname = "MG_1_0" + i;
            texture = Resources.Load<Texture2D>(imgname);
            img_list[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        texture = Resources.Load<Texture2D>("MG_1_100");
        img_list[100] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return img_list;
    }
}
