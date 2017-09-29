using UnityEngine;
using System.Collections;
using UnityEditor;

public class MahjongCardEditor : Editor
{
    [MenuItem("Tools/MahjongEditor/Code/AddCodeCard13", false, 200)]
    private static void AddCodeCard13()
    {
        CreateCodeCardByNum(26);
    }

    [MenuItem("Tools/MahjongEditor/Code/AddCodeCard14", false, 200)]
    private static void AddCodeCard14()
    {
        CreateCodeCardByNum(28);
    }

    [MenuItem("Tools/MahjongEditor/Code/AddCodeCard15", false, 200)]
    private static void AddCodeCard15()
    {
        CreateCodeCardByNum(30);
    }

    [MenuItem("Tools/MahjongEditor/Code/AddCodeCard17", false, 200)]
    private static void AddCodeCard17()
    {
        CreateCodeCardByNum(34);
    }


    [MenuItem("Tools/MahjongEditor/Code/AddCodeCard18", false, 200)]
    private static void AddCodeCard18()
    {
        CreateCodeCardByNum(36);
    }


    [MenuItem("Tools/MahjongEditor/Down/AddDownCardNormal", false, 200)]
    private static void AddDownCardNormal()
    {
        CreateDownCardByNum(14);
    }

    [MenuItem("Tools/MahjongEditor/Down/AddDownCardWithPeng1", false, 200)]
    private static void AddDownCardWithPeng1()
    {
        CreateDownCardByNum(11);
    }


    [MenuItem("Tools/MahjongEditor/Down/AddDownCardWithPeng2", false, 200)]
    private static void AddDownCardWithPeng2()
    {
        CreateDownCardByNum(8);
    }


    [MenuItem("Tools/MahjongEditor/Down/AddDownCardWithPeng3", false, 200)]
    private static void AddDownCardWithPeng3()
    {
        CreateDownCardByNum(5);
    }

    [MenuItem("Tools/MahjongEditor/Down/AddDownCardWithPeng4", false, 200)]
    private static void AddDownCardWithPeng4()
    {
        CreateDownCardByNum(2);
    }


    [MenuItem("Tools/MahjongEditor/Peng/CreatePeng1", false, 200)]
    private static void CreatePeng1()
    {
        CreatePengCardByNum(1);
    }

    [MenuItem("Tools/MahjongEditor/Peng/CreatePeng2", false, 200)]
    private static void CreatePeng2()
    {
        CreatePengCardByNum(2);
    }

    [MenuItem("Tools/MahjongEditor/Peng/CreatePeng3", false, 200)]
    private static void CreatePeng3()
    {
        CreatePengCardByNum(3);
    }

    [MenuItem("Tools/MahjongEditor/Peng/CreatePeng4", false, 200)]
    private static void CreatePeng4()
    {
        CreatePengCardByNum(4);
    }


    [MenuItem("Tools/MahjongEditor/Gang/CreateGangMing4", false, 200)]
    private static void CreateGangMing4()
    {
        CreateAnGangByNum(4);
    }

    [MenuItem("Tools/MahjongEditor/Gang/CreateGangAn", false, 200)]
    private static void CreateGangAn()
    {

    }

    [MenuItem("Tools/MahjongEditor/HE/CreateHePai", false, 200)]
    private static void CreateHePai()
    {
        CreateHePaiByNum(9);
    }


    [MenuItem("Tools/MahjongEditor/Hu/CreateHuPai", false, 200)]
    private static void CreateHuPai()
    {
        CreateHuPaiByNum();
    }

    [MenuItem("Tools/MahjongEditor/Hua/CreateHuaPai", false, 200)]
    private static void CreateHuaPai()
    {
        CreateHuaPaiByNum(4);
    }

    [MenuItem("Tools/MahjongEditor/Put/CreatePutPai", false, 200)]
    private static void CreatePutPai()
    {
        CreatePutPaiByNum(2);
    }


    #region Private Func
    private static void CreateHuaPaiByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        for (int i = 0; i < num; i++)
        {
            Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            go.SetParent(parentTrans, false);
            go.localPosition = (i * GameConst.mahJongWidth) * Vector3.right;
            go.localRotation = Quaternion.Euler(Vector3.zero);
            go.gameObject.SetActive(true);
        }
    }


    private static void CreateHuPaiByNum()
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        float posY = 0f;
        for (int i = 0; i < 2; i++)
        {
            Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            go.SetParent(parentTrans, false);
            go.localPosition = (i * GameConst.mahJongWidth) * Vector3.right + Vector3.down * posY;
            go.localRotation = Quaternion.Euler(Vector3.zero);
            go.gameObject.SetActive(true);

        }

        posY = GameConst.mahJongLenght;
        for (int i = 0; i < 2; i++)
        {
            Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            go.SetParent(parentTrans, false);
            go.localPosition = (i * GameConst.mahJongWidth) * Vector3.right + Vector3.down * posY;
            go.localRotation = Quaternion.Euler(Vector3.zero);
            go.gameObject.SetActive(true);

        }

    }


    private static void CreateHePaiByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < num; i++)
            {
                Transform card = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
                card.SetParent(parentTrans, false);

                int nowCount = i + (num * j);
                float localPosX = (nowCount % num) * (GameConst.mahJongWidth + 0.01f);
                float localPoxY = Mathf.Min((nowCount / num), 2) * (GameConst.mahJongLenght + 0.05f);
                Vector3 aimPos = Vector3.right * localPosX + Vector3.up * localPoxY;
                card.localRotation = Quaternion.Euler(Vector3.zero);
                card.localPosition = aimPos;
                card.gameObject.SetActive(true);
            }

        }

    }

    private static void CreatePutPaiByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        int nowRowBase = 9;

        int nowCount = 0;
        for (int i = 0; i < num; i++)
        {
            Transform cardP = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            cardP.SetParent(parentTrans, false);

            MahJongCard card = cardP.GetComponent<MahJongCard>();

            bool isMan = nowCount >= 3 * nowRowBase;


            float localPoxY = -1f;
            int valueY = -1;
            //set y
            if (isMan)
            {
                valueY = (nowCount - 3 * nowRowBase) / (11 - nowRowBase);

                if (valueY > 2)
                {
                    valueY -= 3;
                }
            }
            else
            {
                valueY = nowCount / nowRowBase;
                if (valueY > 2)
                {
                    valueY -= 3;
                }
            }


            localPoxY = valueY * (GameConst.mahJongLenght + 0.05f);

            //set x
            float localPosX = -1f;

            if (isMan)
            {
                localPosX = ((nowCount - 3 * nowRowBase) % (11 - nowRowBase) + nowRowBase) * (GameConst.mahJongWidth + 0.01f);
            }
            else
            {
                localPosX = (nowCount % nowRowBase) * (GameConst.mahJongWidth + 0.01f);
            }

            Vector3 aimPos = Vector3.right * localPosX + Vector3.up * localPoxY;
            Vector3 startPos = Vector3.right * (aimPos.x + 1 * GameConst.mahJongWidth) + Vector3.up * (aimPos.y + 1 * GameConst.mahJongLenght);
            Vector3 wordStart = Vector3.zero;
            Vector3 wordEnd = Vector3.zero;


            card.selfTransform.localRotation = Quaternion.Euler(Vector3.zero);
            card.selfTransform.localPosition = startPos;
            wordStart = card.selfTransform.position;

            card.selfTransform.localPosition = aimPos;
            wordEnd = card.selfTransform.position;



            nowCount += 1;
        }


    }




    private static void CreateCodeCardByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;
        for (int i = 1; i <= num; i++)
        {
            MahJongCard card = GetAMahjongCard();
            Transform cardTrans = card.transform;
            AddACardToCode(cardTrans, i, parentTrans);
        }
    }

    private static void AddACardToCode(Transform card, int index, Transform parent)
    {
        card.SetParent(parent, false);
        card.localRotation = Quaternion.Euler(Vector3.zero);
        card.localPosition = Vector3.forward * (((index - 1) % 2) * GameConst.mahJongHeight) +
            Vector3.right * (((index - 1) / 2) * GameConst.mahJongWidth);
    }


    private static void CreateDownCardByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;
        for (int i = 0; i < num - 1; i++)
        {
            Transform card = GetAMahjongCard().transform;
            AddACardToDown(card, i, parentTrans);
        }

        Transform tempCard = GetAMahjongCard().transform;
        AddACardToTemp(tempCard, num - 1, parentTrans);
    }

    private static void AddACardToDown(Transform card, int index, Transform parent)
    {
        card.SetParent(parent, false);
        card.localPosition = Vector3.right * (index * GameConst.mahJongWidth);
        card.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private static void AddACardToTemp(Transform card, int index, Transform parent)
    {
        card.SetParent(parent, false);
        card.localPosition = Vector3.right * (index * GameConst.mahJongWidth + GameConst.mahJongHeight);
        card.localRotation = Quaternion.Euler(Vector3.zero);
        card.localScale = Vector3.one;
    }


    private static void CreatePengCardByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        for (int i = 0; i < num; i++)
        {
            Transform peng = GetAMahjongPeng();
            AddCardsToPeng(peng, i, parentTrans);
        }
    }


    private static void AddCardsToPeng(Transform peng, int index, Transform parent)
    {
        peng.SetParent(parent, false);
        float offSetBase = (3 * GameConst.mahJongWidth + GameConst.mahJongPengWidth);
        peng.localPosition = (index * offSetBase) * Vector3.right;
        peng.localRotation = Quaternion.Euler(Vector3.zero);
    }


    private static void CreateAnGangByNum(int num)
    {
        GameObject obj = new GameObject("Editor_Parent");
        Transform parentTrans = obj.transform;

        for (int i = 0; i < num; i++)
        {
            Transform peng = GetAMahjongAnGang();
            AddCardsToGang(peng, i, parentTrans);
        }
    }


    private static void AddCardsToGang(Transform peng, int index, Transform parent)
    {
        peng.SetParent(parent, false);
        float offSetBase = (3 * GameConst.mahJongWidth + GameConst.mahJongLenght + GameConst.mahJongPengWidth);
        peng.localPosition = (index * offSetBase) * Vector3.right;
        peng.localRotation = Quaternion.Euler(Vector3.zero);
    }



    private static MahJongCard GetAMahjongCard()
    {
        Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
        go.gameObject.SetActive(true);
        return go.GetComponent<MahJongCard>();
    }


    private static Transform GetAMahjongPeng()
    {
        GameObject objPeng = new GameObject("Editor_Parent_Peng");
        Transform objPengTrans = objPeng.transform;
        for (int i = 0; i < 3; i++)
        {
            Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            go.SetParent(objPengTrans, false);
            go.localPosition = (i * GameConst.mahJongWidth) * Vector3.right;
            go.localRotation = Quaternion.Euler(Vector3.zero);
            go.gameObject.SetActive(true);
        }
        return objPengTrans;
    }


    private static Transform GetAMahjongAnGang()
    {
        GameObject objPeng = new GameObject("Editor_Parent_Peng");
        Transform objPengTrans = objPeng.transform;
        float stateLength = 0f;
        for (int i = 0; i < 4; i++)
        {
            Transform go = GameTools.InstantiatePrefab(GameConst.path_MahjongPrefab);
            go.SetParent(objPengTrans, false);

            bool isRotate = i == 0;
            Vector3 rotate = isRotate ? (90f * Vector3.back) : Vector3.zero;
            go.localRotation = Quaternion.Euler(rotate);
            if (isRotate)
            {
                float diff = (GameConst.mahJongLenght / 2 - GameConst.mahJongWidth / 2);
                go.localPosition = (stateLength + diff) * Vector3.right + Vector3.up * (diff + 0.01f);
            }
            else
            {
                go.localPosition = stateLength * Vector3.right;
            }

            stateLength += isRotate ? GameConst.mahJongLenght : GameConst.mahJongWidth;
            go.gameObject.SetActive(true);
        }

        return objPengTrans;
    }


    #endregion



}
