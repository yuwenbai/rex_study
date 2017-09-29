

using projectQ;
/**
* @Author lyb
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTet : MonoBehaviour
{
    private bool isBol;

    /// <summary>
    /// 当前是男的还是女的
    /// </summary>
    public SexEnum AnimSex;
    public Animator_Hand AnimElement;

    void OnGUI()
    {
        Draw_BtnMain();

        if (isBol)
        {
            Draw_BtnAnim();
        }
    }

    /// <summary>
    /// 画出一级按钮
    /// </summary>
    void Draw_BtnMain()
    {
        if (GUI.Button(new Rect(10, 80, 150, 80), "男人胳膊"))
        {
            if (!isBol)
            {
                AnimSex = SexEnum.Man;

                EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Init.ToString(), SexEnum.Man);

                isBol = true;
            }
        }

        if (GUI.Button(new Rect(10, 200, 150, 80), "女人胳膊"))
        {
            if (!isBol)
            {
                AnimSex = SexEnum.Woman;

                EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Init.ToString(), SexEnum.Woman);

                isBol = true;
            }
        }
    }

    #region lyb----------------测试----------------------------------

    /// <summary>
    /// 画出控制动画按钮
    /// </summary>
    void Draw_BtnAnim()
    {
        int wide = 1000;
        int hight = 60;

        if (GUI.Button(new Rect(wide, hight, 100, 50), "吃碰杠" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_chipenggang, "chipenggang");
        }

        if (GUI.Button(new Rect(wide, hight * 2, 100, 50), "手出牌" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_chupai, "chupai");
        }

        if (GUI.Button(new Rect(wide, hight * 3, 100, 50), "手扣牌" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_koupai, "koupai");
        }

        if (GUI.Button(new Rect(wide, hight * 4, 100, 50), "手推牌" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_tuipai, "tuipai");
        }

        if (GUI.Button(new Rect(wide, hight * 5, 100, 50), "手插排" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_chapai, "chapai");
        }

        if (GUI.Button(new Rect(wide, hight * 7, 100, 50), "抓牌" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_zhuapai, "zhuapai");
        }

        if (GUI.Button(new Rect(wide, hight * 8, 100, 50), "整理牌型" + ((int)AnimSex).ToString()))
        {
            AnimatorPlay(AnimatorEnum.Anim_zhengli, "zhengli");
        }


        if (GUI.Button(new Rect(900, hight * 8, 100, 50), "上家出牌"))
        {
            Vector3 v31 = new Vector3(-2.368f, 10.473f, 0.0f);
            Vector3 v32 = new Vector3(-1.73f, 10.473f, 1.68f);

            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(),
                AnimatorEnum.Anim_chupai, v31, v32, 2, -1);
        }

        if (GUI.Button(new Rect(800, hight * 8, 100, 50), "本家出牌"))
        {
            Vector3 v31 = new Vector3(-2.368f, 10.473f, 0.0f);
            Vector3 v32 = new Vector3(-1.73f, 10.473f, 1.68f);

            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(),
                AnimatorEnum.Anim_chupai, v31, v32, 0, -1);
        }
    }

    void AnimatorPlay(AnimatorEnum animEnum, string animStr, bool isBol = false)
    {
        if (isBol)
        {
            AnimElement.Model_Hand.GetComponent<Animator>().CrossFade(animStr, 0);
        }
        else
        {
            Vector3 v31 = new Vector3(-2.368f, 10.473f, 0.0f);
            //Vector3 v32 = new Vector3(-2.368f, 10.473f, 0.0f);
            Vector3 v32 = new Vector3(-1.73f, 10.473f, 1.68f);

            if (animEnum == AnimatorEnum.Anim_chipenggang)
            {
                v31 = new Vector3(-2.368f, 10.473f, 0.0f);
                v32 = new Vector3(-2.368f, 10.473f, 0.0f);
            }

            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(),
                animEnum, v31, v32, 0, -1);
        }
    }

    #endregion ------------------------------------------------------
}