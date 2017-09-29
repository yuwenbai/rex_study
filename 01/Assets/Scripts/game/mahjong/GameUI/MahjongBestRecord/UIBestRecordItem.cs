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

    public class UIBestRecordItem : MonoBehaviour
    {
        public enum EnumItemType
        {
            Single,
            Double,
            Chi,
            Peng,
            AnGang,
            ZhiOrMingGang,
        }

        public EnumItemType curItemType = EnumItemType.Single;
        private EnumMjSpecialType curSpType = EnumMjSpecialType.Null;
        private int _gameType = -1;

        public UISprite[] spriteArray = null;
        public UISprite sprite_Special = null;


        public void IniCommonCard(int mjCode, EnumMjSpecialType specialType = EnumMjSpecialType.Null, int gameType = -1)
        {
            List<int> iniIndexList = new List<int>();
            curSpType = specialType;
            _gameType = gameType;

            switch (curItemType)
            {
                case EnumItemType.Single:
                    {
                        iniIndexList.Add(0);
                    }
                    break;
                case EnumItemType.Double:
                    {
                        iniIndexList.AddRange(new int[] { 0, 1 });
                    }
                    break;
                case EnumItemType.Chi:
                case EnumItemType.Peng:
                    {
                        iniIndexList.AddRange(new int[] { 0, 1, 2 });
                    }
                    break;
                case EnumItemType.AnGang:
                    {
                        iniIndexList.AddRange(new int[] { 3 });
                    }
                    break;
                case EnumItemType.ZhiOrMingGang:
                    {
                        iniIndexList.AddRange(new int[] { 0, 1, 2, 3 });
                    }
                    break;
            }

            //show
            for (int i = 0; i < iniIndexList.Count; i++)
            {
                SetData(iniIndexList[i], mjCode);
            }

        }


        public void IniListCard(List<int> codeList)
        {
            codeList.Sort();

            for (int i = 0; i < codeList.Count; i++)
            {
                SetData(i, codeList[i]);
            }

        }

        private void SetData(int index, int mjCode)
        {
            UISprite sprite = spriteArray[index];
            CardHelper.SetRecordUI(sprite, mjCode);

            if (sprite_Special != null)
            {
                sprite_Special.gameObject.SetActive(false);
                if (curItemType == EnumItemType.Single && curSpType != EnumMjSpecialType.Null)
                {
                    string showName = CardHelper.GetHunSpriteName((int)(curSpType), _gameType);
                    sprite_Special.spriteName = showName;
                    sprite_Special.gameObject.SetActive(true);
                }
            }

        }

    }

}
