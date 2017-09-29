using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using projectQ;

public class FindSpriteRefrences : MonoBehaviour
{
    public List<string> SpriteNames = new List<string>();
    private GameObject[] _TopGameObjects;
    private Dictionary<string, List<string>> _SpritePrefesInfoDic = new Dictionary<string, List<string>>();
    private string _FindSpriteRefreInfo = "FindSpriteRefreInfo";
    private StreamWriter _FileLog;
    // Use this for initialization
    void Start()
    {

        Debug.Log("统计中...");
        InitFileWriter();
        CollectSpriteInfo();
        WriteRefreInfo();
        CloseFile();
        Debug.Log("图片搜索引用信息统计完毕:");

    }
    private void CollectSpriteInfo()
    {
        _TopGameObjects = Resources.LoadAll<GameObject>("");//PrefabPaths
        for (int j = 0; j < SpriteNames.Count; j++)
        {
            for (int i = 0; i < _TopGameObjects.Length; i++)
            {
                FindObjectAtlasRefre(SpriteNames[j], _TopGameObjects[i], _TopGameObjects[i]);
            }
        }
    }
    private void WriteRefreInfo()
    {
        IEnumerator<KeyValuePair<string, List<string>>> ienum = _SpritePrefesInfoDic.GetEnumerator();
        while (ienum.MoveNext())
        {
            _FileLog.WriteLine(ienum.Current.Key + "引用信息如下：");
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
            for (int i = 0; i < ienum.Current.Value.Count; i++)
            {
                _FileLog.WriteLine("图片：" + ienum.Current.Key + "引用信息：" + ienum.Current.Value[i]);
            }
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
        }
    }
    private void CloseFile()
    {
        _FileLog.Flush();
        _FileLog.Close();
    }
    private void InitFileWriter()
    {
        string logPath = Application.persistentDataPath + "/Atlas_Statistics_Result";
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }
        string _PathAndFileName = logPath + "/" + _FindSpriteRefreInfo + ".txt";
        Debug.Log("图片搜索引用信息开始统计（等待中...）文件目录：" + _PathAndFileName);
        if (File.Exists(_PathAndFileName))
        {
            File.Delete(_PathAndFileName);
        }
        FileStream logFile = new FileStream(_PathAndFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _FileLog = new StreamWriter(logFile);
        _FileLog.WriteLine("引用信息如下：");
        _FileLog.WriteLine("----------------");
        _FileLog.WriteLine("----------------");
        _FileLog.Flush();
    }
    private void FindObjectAtlasRefre(string spriteName, GameObject topObj, GameObject obj)
    {
        UISprite sprite = obj.GetComponent<UISprite>();
        string msg = null;
        if (!_SpritePrefesInfoDic.ContainsKey(spriteName))
        {
            _SpritePrefesInfoDic.Add(spriteName, new List<string>());
        }
        if (sprite != null && sprite.atlas != null && sprite.spriteName == spriteName)
        {
            msg = "Prefab名称 :" + topObj.name + "               引用此图片的结点名称:" + obj.name + "               Sprite名称：" + sprite.spriteName + "  尺寸：" + sprite.width + "*" + sprite.height + "所在图集：" + sprite.atlas.name;
            _SpritePrefesInfoDic[spriteName].Add(msg);
        }
        UITexture texture = obj.GetComponent<UITexture>();
        if (texture != null && texture.name == spriteName)
        {
            msg = "Prefab名称 :" + topObj.name + "               引用此图片的结点名称:" + obj.name + "               Sprite名称：" + texture.name + "  尺寸：" + texture.width + "*" + texture.height + "类型为UITexture";
            _SpritePrefesInfoDic[spriteName].Add(msg);
        }
        UIDefinedButton button = obj.GetComponent<UIDefinedButton>();
        if (button != null && button.tweenTarget != null)
        {
            UISprite spriteBtn = button.tweenTarget.GetComponent<UISprite>();
            if (spriteBtn != null && spriteBtn.atlas != null && spriteBtn.atlas.name == spriteName)
            {
                msg = "Prefab名称 :" + topObj.name + "               引用此图片的结点名称:" + obj.name + "               Sprite名称：" + sprite.spriteName + "  尺寸：" + sprite.width + "*" + sprite.height;
                _SpritePrefesInfoDic[spriteName].Add(msg);
            }
        }
# if  !UNITY_EDITOR
#else
        //  ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        if (obj.GetComponent<Renderer>())
        {
            Material material = obj.GetComponent<Renderer>().sharedMaterial;
            if (material != null)
            {
                Shader shader = material.shader;
                for (int i = 0; i < UnityEditor.ShaderUtil.GetPropertyCount(shader); ++i)
                {
                    if (UnityEditor.ShaderUtil.GetPropertyType(shader, i) == UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        string propertyName = UnityEditor.ShaderUtil.GetPropertyName(shader, i);
                        Texture tex = material.GetTexture(propertyName);
                        AddRefre(topObj, obj, tex);
                    }
                }
            }
        }
#endif
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                FindObjectAtlasRefre(spriteName, topObj, obj.transform.GetChild(i).gameObject);
            }
        }

    }
    private void AddRefre(GameObject topObj, GameObject obj, Texture tex)
    {
        if (tex == null || !_SpritePrefesInfoDic.ContainsKey(tex.name))
        {
            return;
        }
        string msg = "Prefab名称 :" + topObj.name + "               引用此图片的结点名称:" + obj.name + " 贴图尺寸：" + tex.width + "*" + tex.height;
        _SpritePrefesInfoDic[tex.name].Add(msg);
    }

}

