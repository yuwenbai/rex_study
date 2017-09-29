/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using projectQ;
using projectQ.chectKey;
public class UIPrepareGameTest : MonoBehaviour {
    public GameObject ButtonSend;
    public GameObject ButtonSetting;
    public UIInput Inputa;

    private void OnButtonSendClick(GameObject go)
    {
        var val = GetList(Inputa.value);
        try
        {
            ModelNetWorker.Instance.MjTestReq(MjDataManager.Instance.MjData.curUserData.selfDeskID,
                val[0], val[1], val[2], val[3], val[4], val[5], val[6], val[7]);
        }catch
        {
            QLoger.ERROR("阿萨德");
        }
    }

    private void OnButtonSettingClick(GameObject go)
    {
        _R.ui.OpenUI("UICheatKeyList");
    }
    private List<List<int>> GetList(string str)
    {
        List<List<int>> rs = new List<List<int>>();
        try
        {
            var l1 = str.Split(';');
            for (int i = 0; i < l1.Length; i++)
            {
                var l2 = l1[i].Split(',');
                var l = new List<int>();
                for (int j = 0; j < l2.Length; j++)
                {
                    if(l2[j].Trim().Length > 0)
                    {
                        l.Add(int.Parse(l2[j]));
                    }
                }
                rs.Add(l);
            }
        }
        catch
        {
            QLoger.ERROR("秘籍错误");
        }
        finally
        {
        }

        return rs ;
    }

    private void SendCallback()
    {
        Inputa.Set(CheatKeyManager.Instance.GetCheatKeyData().Content);
    }
    private void Awake()
    {
        CheatKeyManager.Instance.ReadCheatData();
        SendCallback();
        CheatKeyManager.Instance.SendCallBack = SendCallback;
        UIEventListener.Get(ButtonSend).onClick = OnButtonSendClick;
        UIEventListener.Get(ButtonSetting).onClick = OnButtonSettingClick;
    }

}
