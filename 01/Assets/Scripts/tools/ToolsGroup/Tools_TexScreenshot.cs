/****************************************************
*
*  unity屏幕截图，并转换成Base64码
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class Tools_TexScreenshot : MonoBehaviour
{
    public delegate void WeChatTexScreenShotDelegate(bool isSucc);
    public WeChatTexScreenShotDelegate WeChatTexScreenShotCallBack;

    private static Tools_TexScreenshot _Instance;
    public static Tools_TexScreenshot Instance
    {
        get { return _Instance; }
    }

    void Awake()
    {
        _Instance = this;
    }

    void OnDestroy()
    {
        _Instance = null;

        Texture_ShotDelete();
    }

    /// <summary>
    /// 截图图片保存路径
    /// </summary>
    private string texShotPath
    {
        get
        {
            return projectQ.PathHelper.PersistentPath + "/screencapture.jpg";
        }
    }
    /// <summary>
    /// 压缩后的截图图片保存路径
    /// </summary>
    private string texZipShotPath
    {
        get
        {
            return projectQ.PathHelper.PersistentPath + "/texShare.jpg";
        }
    }

    /// <summary>
    /// 屏幕截图保存
    /// </summary>
    /// <param name="startPoint">截图起点坐标</param>
    /// <param name="shotSize">截图大小</param>
    public void Texture_Screenshot(Vector2 startPoint, Vector2 shotSize, WeChatTexScreenShotDelegate shotBack)
    {
        StartCoroutine(GetCapture(startPoint, shotSize, shotBack));
    }

    IEnumerator GetCapture(Vector2 startPoint, Vector2 shotSize, WeChatTexScreenShotDelegate shotBack)
    {
        yield return new WaitForEndOfFrame();

        int width = (int)shotSize.x;

        int height = (int)shotSize.y;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(startPoint.x, startPoint.y, width, height), 0, 0, true);

        //对屏幕缓存进行压缩  
        tex.Compress(false);

        //对图片进行压缩
        Texture2D texZip = Tools_TexGzip.TexGzipBegin(tex, 0.5f);

        //转化为png图
        byte[] imagebytes = texZip.EncodeToJPG();

        //存储png图
        File.WriteAllBytes(texShotPath, imagebytes);

        yield return null;

        if (shotBack != null)
        {
            shotBack(true);
        }
    }

    /// <summary>
    /// 图片压缩
    /// </summary>
    void Texture_Gzip(int height, int width, WeChatTexScreenShotDelegate shotBack)
    {
        if (shotBack != null)
        {
            shotBack(true);
        }
    }

    /// <summary>
    /// 将图片数据转换为Base64字符串
    /// </summary>
    public string Texture_ToBase64()
    {
        if (!System.IO.File.Exists(texShotPath))
        {
            Debug.Log(" 图片路径无效： " + texShotPath);
            return null;
        }

        var base64Img = Convert.ToBase64String(System.IO.File.ReadAllBytes(texShotPath));

        Debug.Log(" #[图片转换成Base64]# 成功 图片信息" + base64Img);

        //  Texture_ShotDelete();

        return base64Img.ToString();
    }

    /// <summary>
    /// 将指定路径图片转换为Base64字符串
    /// </summary>
    public string Texture_FixedToBase64(string texFixedPath)
    {
        var base64Img = Convert.ToBase64String(System.IO.File.ReadAllBytes(texFixedPath));
        
        return base64Img.ToString();
    }

    /// <summary>
    /// 将本地图片转换为Base64字符串
    /// </summary>
    public string Texture_LocalToBase64(Texture2D tex2d)
    {
        var base64Img = Convert.ToBase64String(tex2d.EncodeToJPG());

        return base64Img.ToString();
    }

    /// <summary>
    /// 将保存下来的用于分享的图片删除掉
    /// </summary>
    public void Texture_ShotDelete()
    {
        if (File.Exists(texShotPath))
        {
            Debug.Log(" #[如果截图的图片还在则删掉]# ");

            File.Delete(texShotPath);
        }

        if (File.Exists(texZipShotPath))
        {
            Debug.Log(" #[图片转换成Base64成功后删除]# ");

            File.Delete(texZipShotPath);
        }
    }
}