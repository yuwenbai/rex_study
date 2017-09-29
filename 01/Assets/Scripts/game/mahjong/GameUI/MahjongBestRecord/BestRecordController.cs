/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public class BestRecordController : MonoBehaviour
    {

        public UITable table_Best = null;
        public UIGrid grid_Single = null;
        public UIGrid grid_Three = null;

        public GameObject obj_Single = null;
        public GameObject obj_Peng = null;
        public GameObject obj_Chi = null;
        public GameObject obj_AnGang = null;
        public GameObject obj_ZhiGang = null;

        public void IniBestRecord(BestMjRecord bestRecord)
        {
            this.Clear();

            List<MjPai> mjList = bestRecord.mjList;

            List<int> singList = new List<int>();
            List<int> hunList = new List<int>();
            List<int> chiListTemp = new List<int>();
            List<List<int>> chiList = new List<List<int>>();
            List<int> pengList = new List<int>();
            List<int> anGangList = new List<int>();
            List<int> zhiGangList = new List<int>();
            List<int> maoListTemp = new List<int>();

            EnumMjSpecialType curSingleType = EnumMjSpecialType.Null;
            List<int> specailList = new List<int>();
            if (bestRecord.specialList != null && bestRecord.specialList.Count > 0)
            {
                for (int i = 0; i < bestRecord.specialList.Count; i++)
                {
                    specailList.Add(bestRecord.specialList[i].mjCode);
                }
            }


            for (int i = 0; i < mjList.Count; i++)
            {
                switch (mjList[i].codeType)
                {
                    case EnumMjCodeType.Code_Hands:
                        {
                            if (mjList[i].specialType != EnumMjSpecialType.Null)
                            {
                                if (curSingleType == EnumMjSpecialType.Null)
                                {
                                    curSingleType = mjList[i].specialType;
                                }
                                hunList.Add(mjList[i].mjCode);
                            }
                            else
                            {
                                singList.Add(mjList[i].mjCode);
                            }
                        }
                        break;
                    case EnumMjCodeType.Code_Gang_An:
                        {
                            anGangList.Add(mjList[i].mjCode);
                        }
                        break;
                    case EnumMjCodeType.Code_Gang_Zhi:
                    case EnumMjCodeType.Code_Gang_Bu:
                        {
                            zhiGangList.Add(mjList[i].mjCode);
                        }
                        break;
                    case EnumMjCodeType.Code_Peng:
                        {
                            pengList.Add(mjList[i].mjCode);
                        }
                        break;
                    case EnumMjCodeType.Code_Chi:
                        {
                            chiListTemp.Add(mjList[i].mjCode);
                        }
                        break;
                    case EnumMjCodeType.Code_Mao:
                        {
                            maoListTemp.Add(mjList[i].mjCode);
                        }
                        break;
                }
            }

            chiListTemp.Sort();
            int chiCount = chiListTemp.Count / 3;
            for (int i = 0; i < chiCount; i++)
            {
                List<int> chiItem = chiListTemp.GetRange((i * 3), 3);
                chiList.Add(chiItem);
            }

            int nameIndex = 0;

            hunList.Sort();
            for (int i = 0; i < hunList.Count; i++)
            {

                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Single, grid_Single.transform);
                item.IniCommonCard(hunList[i], curSingleType, bestRecord.mjType);
                nameIndex += 1;
                item.gameObject.name = nameIndex.ToString();
                item.gameObject.SetActive(true);
            }


            //single
            singList.Sort();
            for (int i = 0; i < singList.Count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Single, grid_Single.transform);
                bool isSpecial = specailList.Contains(singList[i]);
                item.IniCommonCard(singList[i], isSpecial ? curSingleType : EnumMjSpecialType.Null, bestRecord.mjType);
                nameIndex += 1;
                item.gameObject.name = nameIndex.ToString();
                item.gameObject.SetActive(true);
            }

            //chi
            for (int i = 0; i < chiList.Count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Chi, grid_Three.transform);
                item.IniListCard(chiList[i]);
                item.gameObject.SetActive(true);
            }

            if (maoListTemp != null && maoListTemp.Count > 0)
            {
                //mao
                UIBestRecordItem itemMao = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Chi, grid_Three.transform);
                itemMao.IniListCard(maoListTemp);
                itemMao.gameObject.SetActive(true);
            }


            //peng
            for (int i = 0; i < pengList.Count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Peng, grid_Three.transform);
                item.IniCommonCard(pengList[i]);
                item.gameObject.SetActive(true);
            }

            //an
            for (int i = 0; i < anGangList.Count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_AnGang, grid_Three.transform);
                item.IniCommonCard(anGangList[i]);
                item.gameObject.SetActive(true);
            }

            //zhi
            for (int i = 0; i < zhiGangList.Count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_ZhiGang, grid_Three.transform);
                item.IniCommonCard(zhiGangList[i]);
                item.gameObject.SetActive(true);
            }


            if (this.isActiveAndEnabled)
            {
                StartCoroutine(UpdateGrid());
            }
        }


        IEnumerator UpdateGrid()
        {
            grid_Single.gameObject.SetActive(false);
            grid_Three.gameObject.SetActive(false);
            yield return null;
            grid_Single.gameObject.SetActive(true);
            grid_Three.gameObject.SetActive(true);
            yield return null;
            grid_Single.repositionNow = true;
            grid_Three.repositionNow = true;
            table_Best.repositionNow = true;
            yield return null;
            grid_Single.Reposition();
            grid_Three.Reposition();
            table_Best.Reposition();
        }


        public void ClearRecord()
        {
            grid_Single.transform.DestroyChildren();
            grid_Three.transform.DestroyChildren();
            table_Best.transform.DestroyChildren();
            Destroy(this.gameObject);
        }


        public void Clear()
        {
            grid_Single.transform.DestroyChildren();
            grid_Three.transform.DestroyChildren();
        }

    }


}
