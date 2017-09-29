/**
 * @Author Hailong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UISijiaMaiMaFirstInfo : MonoBehaviour
    {
        public GameObject obj_Pai = null;
        public Transform grid_Pai = null;
        public UISprite bg = null;
        protected List<int> mjCodeList = null;

        public void InitData(int uiSeatID, List<int> mjCode)
        {
            mjCodeList = mjCode;
            if (mjCode != null && mjCode.Count > 0)
            {
                int width = obj_Pai.GetComponent<UISprite>().width;
                grid_Pai.transform.DestroyChildren();
                UITools.CreateChild(grid_Pai.transform, obj_Pai, mjCode.Count, CreateDetailCall);
                for (int i = 0; i < grid_Pai.transform.childCount; i++)
                {
                    if (i == 0 || i == 1 || i == 2)
                    {
                        bg.width = (i + 1) * width + (int)Mathf.Pow((i + 1), 2);
                    }
                    else
                    {
                        bg.width = (i + 1) * width + (int)Mathf.Pow((i + 1), 2) - 3;
                    }
                }
                grid_Pai.transform.GetComponent<UIGrid>().Reposition();
            }
        }

        private void CreateDetailCall(GameObject obj, int index)
        {
            UISiJiaMaiMaFirstPai paiItem = obj.GetComponent<UISiJiaMaiMaFirstPai>();
            if (paiItem != null)
            {
                paiItem.IniData(mjCodeList[index]);
            }
        }
    }
}
