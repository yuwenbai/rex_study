/**
* 作者：周腾
* 作用：
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class EmojChatItem : MonoBehaviour
    {

        public UISprite sprite;
        //public UITexture texture;
        private string chatMsg;
        public delegate void EmojClick(string chatMsg);
        public EmojClick dele_emojClick;
        public void InitItem(string id, string content)
        {

            //Texture tex = Resources.Load<Texture>("Texture/ChatEmoj/"+ itemId);
            //texture.mainTexture = tex;

            chatMsg = id;
            sprite.spriteName = content + "1";
            sprite.MakePixelPerfect();

            GEnum.SoundEnum playType = GEnum.SoundEnum.desk_biaoqing_fanqie;
            if (chatMsg == "icon_biaoqing_dabian1")
            {
                //GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.SOUND_16;
                playType = GEnum.SoundEnum.desk_biaoqing_fanqie;
            }
            else if (chatMsg == "icon_biaoqing_hecha1")
            {
                //GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.SOUND_15;
                playType = GEnum.SoundEnum.desk_biaoqing_hecha;
            }
            else if (chatMsg == "icon_biaoqing_qiaozhuo1")
            {
                //GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.SOUND_14;
                playType = GEnum.SoundEnum.desk_biaoqing_qiaozhuo;
            }
            else if (chatMsg == "icon_biaoqing_zan1")
            {
                //GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.SOUND_13;
                playType = GEnum.SoundEnum.desk_biaoqing_dianzan;
            }
            //EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), playType);
        }

        #region LifiCycle 
        public EmojChatItem()
        {
#if __DEBUG_LIFE_CYCLE
#endif
        }

        // Use this for per initialization
        void Awake()
        {
            UIEventListener.Get(gameObject).onClick = OnEmojClick;
        }

        private void OnEmojClick(GameObject go)
        {
            if (dele_emojClick != null)
            {
                dele_emojClick(chatMsg);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void LateUpdate()
        {

        }

        void FixedUpdate()
        {
        }

        void OnDestroy()
        {
#if __DEBUG_LIFE_CYCLE
#endif
        }

        #endregion //LifiCycle 

    }
}