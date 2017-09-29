/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;
namespace projectQ
{
    public class ParlorItemData
    {
        public MjRoom hall;
        public int index;
        public Action<ParlorItemData, GameObject> OnClick;
    }

    public class ParlorItem : ScrollViewItemBase<ParlorItemData>
    {
        public UILabel TextPriendName;
        public UILabel TextPriendID;
        public UILabel TextPriendNum;
        public UISprite SpriteState;


        public override void Refresh()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.UIData.hall.RoomName);
            TextPriendName.text = sb.ToString();
            sb.Length = 0;
            sb.Append("ID:").Append(this.UIData.hall.RoomID);
            TextPriendID.text = sb.ToString();
            sb.Length = 0;
            sb.Append("人员:").Append(this.UIData.hall.CurMemberNum);
            TextPriendNum.text = sb.ToString();

            if(this.UIData.hall.RankType == 0)
            {
                SpriteState.gameObject.SetActive(false);
            }
            else
            {
                SpriteState.gameObject.SetActive(false);
                switch(this.UIData.hall.RankType)
                {
                    case 1:
                        SpriteState.spriteName = "public_button_01";
                        break;
                    case 2:
                        SpriteState.spriteName = "public_button_02";
                        break;
                    case 3:
                        SpriteState.spriteName = "public_button_03";
                        break;
                }
            }

            UISprite sp = this.GetComponent<UISprite>();
            if (sp != null)
            {
                if (this.UIData.index < 6)
                {
                    sp.spriteName = "room_button_bj01";
                }
                else
                {
                    sp.spriteName = "room_button_bj02";
                }
            }
        }

        public void OnButtonClick(GameObject go)
        {
            if (this.UIData.OnClick != null)
            {
                this.UIData.OnClick(this.UIData, gameObject);
            }
        }

        private void Awake()
        {
            UIEventListener.Get(gameObject).onClick = OnButtonClick;
        }
    }
}