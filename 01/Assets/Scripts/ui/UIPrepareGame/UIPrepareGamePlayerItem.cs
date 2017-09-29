/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIPrepareGamePlayerItem 
    {
        public MahJongPlayerInfo playScript;
        public GameObject Root;
        //public UIWidget SpritePrepare;
        //public UITexture TexHead;
        //public UISprite DeskOwner;

        public long UserId;
        /// <summary>
        /// 刷新UI
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPrepared">是否准备完毕</param>
        public void RefreshUI(long id, bool isPrepared)
        {
            this.UserId = (long)id;
            Prepare(isPrepared);
        }

        public void Prepare(bool isPrepared)
        {
            this.playScript.SetPrepareActive(isPrepared);
        }


        public void Init(MahJongPlayerInfo script)
        {
            playScript = script;
            Root = script.gameObject;
            //TexHead = script.texture_PlayerIcon;
            //SpritePrepare = Root.transform.FindChild("SpritePrepare").GetComponent<UILabel>();
        }
    }
}