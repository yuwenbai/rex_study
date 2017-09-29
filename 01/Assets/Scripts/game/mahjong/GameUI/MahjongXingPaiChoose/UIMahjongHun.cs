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

    public class UIMahjongHun : UIViewBase
    {
        public override void Init()
        {

        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            IniData();
        }

        public override void OnPushData(object[] data)
        {
            specialType = (EnumMjSpecialType)data[0];
            changeCode = (List<int>)data[1];
            gameType = (int)data[2];
            closeTime = (float)data[3];
        }

        public UIGrid grid_Parent = null;
        public GameObject obj_Fx = null;
        public UISprite sprite_type = null;
        public UISprite sprite_TypeAnim = null;
        public UISprite sprite_pai = null;


        private EnumMjSpecialType specialType = EnumMjSpecialType.Null;
        private List<int> changeCode = null;
        private int gameType = -1;
        private float closeTime = -1f;


        private void IniData()
        {
            if (changeCode != null)
            {
                string spriteName = null;
                switch (specialType)
                {
                    case EnumMjSpecialType.Gui:
                        {
                            spriteName = "state_icon_guipai";
                        }
                        break;
                    case EnumMjSpecialType.Hun:
                        {
                            spriteName = "state_icon_hunpai";
                        }
                        break;
                    case EnumMjSpecialType.Jin:
                        {
                            spriteName = "state_icon_jinpai";
                        }
                        break;
                    case EnumMjSpecialType.Ci:
                        {
                            spriteName = "state_icon_cizhang";
                        }
                        break;
                    case EnumMjSpecialType.Hun_Caishen:
                        {
                            spriteName = "state_icon_caishen";
                        }
                        break;
                    case EnumMjSpecialType.Hun_Laizi:
                        {
                            spriteName = "state_icon_laizi";
                        }
                        break;
                    case EnumMjSpecialType.Hun_JinPai:
                        {
                            spriteName = "state_icon_jinpai";
                        }
                        break;
                }

                sprite_type.spriteName = spriteName;
                sprite_type.MakePixelPerfect();
                sprite_TypeAnim.spriteName = spriteName;
                sprite_TypeAnim.MakePixelPerfect();

                obj_Fx.SetActive(true);
                UITools.CreateChild(grid_Parent.transform, sprite_pai.gameObject, changeCode.Count, CreateCallBack, true);
                grid_Parent.Reposition();

                this.gameObject.SetActive(true);
                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_hunqian);
            }

            StartCoroutine(SetClose());
        }


        private void CreateCallBack(GameObject obj, int index)
        {
            UISprite spriteIcon = obj.GetComponent<UISprite>();
            if (spriteIcon)
            {
                CardHelper.SetRecordUI(spriteIcon, changeCode[index]);
            }
            obj.SetActive(true);
        }


        private IEnumerator SetClose()
        {
            yield return new WaitForSeconds(closeTime);
            Close();
        }

    }

}
