/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class MjMusicController
{

    private EventDispatcheHelper _EventHelper = new EventDispatcheHelper();
    public void AddEvents()
    {
        _EventHelper.AddEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), PlaySoundFX);                          //麻将播放声音事件 

        _EventHelper.AddEvent(MJEnum.MusicEnum.ME_ActionFx.ToString(), MusicPlayActionBase);

        _EventHelper.AddEvent(MJEnum.MusicEnum.ME_PutCard.ToString(), MusicPlayPutCard);
        _EventHelper.AddAllEvent();
    }
    private void PlaySoundFX(object[] param)
    {
        GEnum.SoundEnum type = (GEnum.SoundEnum)param[0];
        MusicCtrl.Instance.Music_SoundPlay(type);
    }
    private void MusicPlayActionBase(object[] param)
    {
        long userID = (long)param[0];
        EnumMjOpAction opaction = (EnumMjOpAction)param[1];
        int playNum = -1;

        switch (opaction)
        {
            case EnumMjOpAction.MjOp_Peng:
                {
                    playNum = (int)EnumMjMusicType.ActionPeng;
                }
                break;
            case EnumMjOpAction.MjOp_Gang:
                {
                    playNum = (int)EnumMjMusicType.ActionGang;
                }
                break;
            case EnumMjOpAction.MjOp_Ting:
                {
                    playNum = (int)EnumMjMusicType.ActionTing;
                }
                break;
            case EnumMjOpAction.MjOp_HuPai:
                {
                    playNum = (int)EnumMjMusicType.ActionHu;
                }
                break;
            case EnumMjOpAction.MjOp_Chi:
                {
                    playNum = (int)EnumMjMusicType.ActionChi;
                }
                break;
            case EnumMjOpAction.MjOp_Zimo:
                {
                    playNum = (int)EnumMjMusicType.ActionZimo;
                }
                break;
            case EnumMjOpAction.MjOp_PiCi:
                {
                    playNum = (int)EnumMjMusicType.ActionPiCi;
                }
                break;
            case EnumMjOpAction.MjOp_CiHu:
                {
                    playNum = (int)EnumMjMusicType.ActionCiHu;
                }
                break;
            case EnumMjOpAction.MjOp_Minglou:
                {
                    playNum = (int)EnumMjMusicType.ActionMinglouShou;
                }
                break;
            case EnumMjOpAction.MjOp_Guafeng:
                {
                    playNum = (int)EnumMjMusicType.ActionGuafeng;
                }
                break;
            case EnumMjOpAction.MjOp_Xiayu:
                {
                    playNum = (int)EnumMjMusicType.ActionXiaYu;
                }
                break;
            case EnumMjOpAction.MjOp_Minglao:
                {
                    playNum = (int)EnumMjMusicType.ActionMinglao;
                }
                break;
            case EnumMjOpAction.MjOp_Minglv:
                {
                    playNum = (int)EnumMjMusicType.ActionMinglv;
                }
                break;
            case EnumMjOpAction.MjOp_MinglouMu:
                {
                    playNum = (int)EnumMjMusicType.ActionMinglouMu;
                }
                break;
            case EnumMjOpAction.MjOp_AnlouMu:
                {
                    playNum = (int)EnumMjMusicType.ActionAnLouMu;
                }
                break;
            case EnumMjOpAction.MjOp_AnlouShou:
                {
                    playNum = (int)EnumMjMusicType.ActionAnLouShou;
                }
                break;

        }

        if (playNum != -1)
        {
            MusicCtrl.Instance.Music_VoicePlay(playNum, userID);
        }
    }

    public void MusicPlayPutCard(object[] param)
    {
        int seatID = (int)param[0];
        int cardID = (int)param[1];
        int baseNum = -1;
        int subNum = -1;
        if (cardID <= 7)
        {
            //feng
            baseNum = (int)EnumMjMusicType.FengDong - 1;
            subNum = cardID;
        }
        else if (cardID <= 16)
        {
            //wan
            baseNum = (int)EnumMjMusicType.WanStart - 1;
            subNum = cardID - 7;
        }
        else if (cardID <= 25)
        {
            //tiao
            baseNum = (int)EnumMjMusicType.TiaoStart - 1;
            subNum = cardID - 16;
        }
        else if (cardID <= 34)
        {
            //tong
            baseNum = (int)EnumMjMusicType.TongStart - 1;
            subNum = cardID - 25;
        }

        int playNum = baseNum + subNum;
        if (playNum >= 1000)
        {
            //canplay
            long userID = MjDataManager.Instance.GetCurrentUserIDBySeatID(seatID);
            MusicCtrl.Instance.Music_VoicePlay(playNum, userID);
        }
    }

    public void RemoveAllEvents()
    {
        _EventHelper.RemoveAllEvent();
    }
}
