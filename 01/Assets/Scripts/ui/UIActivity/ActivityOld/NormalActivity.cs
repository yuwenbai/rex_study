

using projectQ;
/**
* 作者：周腾
* 作用：
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalActivity : MonoBehaviour {

    Texture2D image;
    public UIScrollView textScrollView;
    public UILabel textLab;

    public UITexture textureTexture;

    public UITexture textTexture;
    public UIScrollView textTextureScrollView;
    public UILabel textTextureLab;


    public GameObject textObj;
    public GameObject textureObj;
    public GameObject textTextureObj;
    private Texture2D texturet;
    private Texture2D texturetext;
    /// <summary>
    /// 这里应该是清除信息的方法
    /// </summary>
    public void ClearInfo() { }
    /// <summary>
    /// 文字
    /// </summary>
    public void ShowText(string desc)
    {
        textObj.SetActive(true);
        textureObj.SetActive(false);
        textTextureObj.SetActive(false);
        textLab.text = desc;
    }
    /// <summary>
    /// 图片
    /// </summary>
    public void ShowTexture(string resUrl)
    {
        textObj.SetActive(false);
        textureObj.SetActive(true);
        textTextureObj.SetActive(false);

        DownHeadTexture.Instance.Activity_TextureGet(resUrl, SetTexturet);       
    }

    void SetTexturet(Texture2D t, string headName)
    {
        textureTexture.mainTexture = t;
    }
    /// <summary>
    /// 图片加文字
    /// </summary>
    public void ShowTextTexture(string resUrl,string desc)
    {
        textObj.SetActive(false);
        textureObj.SetActive(false);
        textTextureObj.SetActive(true);
        textTextureLab.text = desc;
        if (texturet == null)
        {
            DownHeadTexture.Instance.Activity_TextureGet(resUrl, SetTexturetext);
        }
        else
        {
            textTexture.mainTexture = texturetext;
        }
    }
    void SetTexturetext(Texture2D t, string headName)
    {
        if (t == null)
        {
            Texture2D local = Resources.Load<Texture2D>("Texture/UILottery/Activity01");
            textTexture.mainTexture = local;
            texturetext = local;
        }
        else
        {
            texturetext = t;
            textTexture.mainTexture = t;
        }
    }
}
