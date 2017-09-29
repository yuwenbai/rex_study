using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using projectQ;

/// <summary>
/// 此功能建议空场景测试
/// </summary>
public class FindUIAtlasRefrences : MonoBehaviour
{
    public bool IsRecordAllSprite = true;
    public bool IsNonReferenceRecorded = true;
    public bool IsReferenceRecorded = true;
    public bool IsBigTextureRecord = true;
    public bool IsPrefabWarningRecord = true;
    public int Width_Max = 300;
    public int Height_Max = 300;

    private GameObject[] _TopGameObjects;
    private List<UIAtlas> _AllAtlasList = new List<UIAtlas>();
    private Dictionary<UIAtlas, StreamWriter> _AtlasLogWriter = new Dictionary<UIAtlas, StreamWriter>();
    private Dictionary<UIAtlas, StreamWriter> _AtlasErrorWriter = new Dictionary<UIAtlas, StreamWriter>();
    //每个图集在项目中用到的所有小图信息
    private Dictionary<UIAtlas, List<string>> _RefrencedSprites = new Dictionary<UIAtlas, List<string>>();
    // Use this for initialization
    void Start()
    {
        Debug.Log("统计中...");
        FindAllAtlas();
        CreateAtlasLogFiles();
        WriteRecordLock();
        _TopGameObjects = Resources.LoadAll<GameObject>("");//PrefabPaths
        RecordAllAtlasInfo();
        WriteAtlasSprites();
        WriteUnRefrences();
        WriteRefrences();
        WriteBigTexture();
        CloseAllFiles();
        Debug.Log("每个图集信息统计完毕:");
    }
    private void WriteRecordLock()
    {
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            ienum.Current.Value.WriteLine("是否统计每个图集包含所有图片名称:     " + (IsRecordAllSprite ? "是" : "否"));
            ienum.Current.Value.WriteLine("是否统计每个图集包含大图:             " + (IsBigTextureRecord ? "是" : "否"));
            ienum.Current.Value.WriteLine("是否统计每个图集没有静态引用的图片:   " + (IsNonReferenceRecorded ? "是" : "否"));
            ienum.Current.Value.WriteLine("是否统计每个图集正在项目中引用的图片: " + (IsReferenceRecorded ? "是" : "否"));
            ienum.Current.Value.WriteLine("是否统计Prefab制作的警告日志: " + (IsPrefabWarningRecord ? "是" : "否"));
        }
    }
    private void WriteAtlasSprites()
    {
        if (!IsRecordAllSprite)
        {
            return;
        }
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            ienum.Current.Value.WriteLine("图集名称：" + ienum.Current.Key.name);
            ienum.Current.Value.WriteLine("图集中所有的图片数量为：" + ienum.Current.Key.spriteList.Count);
            for (int i = 0; i < ienum.Current.Key.spriteList.Count; i++)
            {
                UISpriteData item = ienum.Current.Key.spriteList[i];
                ienum.Current.Value.WriteLine("图片名称：" + item.name + "          尺寸：" + item.width + "*" + item.height);
            }
            ienum.Current.Value.WriteLine("图集中所有的图片名称打印结束");
        }


    }
    private void CloseAllFiles()
    {
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            ienum.Current.Value.Close();
        }
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienumError = _AtlasErrorWriter.GetEnumerator();
        while (ienumError.MoveNext())
        {
            ienumError.Current.Value.Close();
        }
    }
    private void FindAllAtlas()
    {
        UIAtlas[] allAtlas = Resources.LoadAll<UIAtlas>("");
        if (allAtlas == null || allAtlas.Length <= 0)
        {
            return;
        }
        for (int i = 0; i < allAtlas.Length; i++)
        {
            _AllAtlasList.Add(allAtlas[i]);
        }
    }
    private void CreateAtlasLogFiles()
    {
        //创建文件夹
        string logPath = Application.persistentDataPath + "/Atlas_Statistics_Result";
        Debug.Log("每个图集信息开始统计（等待中...）文件加目录：" + logPath);
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }
        //根据每个atlas单独统计文件
        string logFileName;
        string errorFileName;
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            logFileName = logPath + "/" + _AllAtlasList[i].name + ".txt";
            errorFileName = logPath + "/" + _AllAtlasList[i].name + "Error.txt";
            if (File.Exists(logFileName))
            {
                File.Delete(logFileName);
            }
            if (File.Exists(errorFileName))
            {
                File.Delete(errorFileName);
            }
            FileStream logFile = new FileStream(logFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FileStream errorFile = new FileStream(errorFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _AtlasLogWriter.Add(_AllAtlasList[i], new StreamWriter(logFile));
            _AtlasErrorWriter.Add(_AllAtlasList[i], new StreamWriter(errorFile));
        }


    }
    private void RecordAllAtlasInfo()
    {
        if (_TopGameObjects == null || _AllAtlasList == null)
        {
            return;
        }

        for (int i = 0; i < _TopGameObjects.Length; i++)
        {
            for (int j = 0; j < _AllAtlasList.Count; j++)
            {
                RecordAtlasInfo(_TopGameObjects[i], _TopGameObjects[i], _AllAtlasList[j]);
            }
        }
    }

    private void WriteBigTexture()
    {
        if (!IsBigTextureRecord)
        {
            return;
        }
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            StreamWriter fileWriter = ienum.Current.Value;
            UIAtlas atlas = ienum.Current.Key;
            fileWriter.WriteLine(ienum.Current.Key + "超过尺寸的图片如下：规定尺寸为" + Width_Max + "*" + Height_Max);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine(atlas.name + "图集中超过设定尺寸的图片如下：");
            int bigTexCount = 0;
            for (int i = 0; i < atlas.spriteList.Count; i++)
            {
                if (atlas.spriteList[i].width >= Width_Max || atlas.spriteList[i].height >= Height_Max)
                {
                    bigTexCount++;
                    fileWriter.WriteLine("图片名称：" + atlas.spriteList[i].name + "                 尺寸:" + atlas.spriteList[i].width + "*" + atlas.spriteList[i].height);
                }
            }
            fileWriter.WriteLine(atlas.name + "超过尺寸的图片数量为：" + bigTexCount);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.Flush();
        }
    }
    private void AddSpriteName(UIAtlas atlas, string spriteName)
    {
        if (!_RefrencedSprites.ContainsKey(atlas))
        {
            _RefrencedSprites.Add(atlas,new List<string>());
        }
        if (!string.IsNullOrEmpty(spriteName))
        {
            if (!_RefrencedSprites[atlas].Contains(spriteName))
            {
                _RefrencedSprites[atlas].Add(spriteName);
            }
        }

    }
    private void RecordAtlasInfo(GameObject topObj, GameObject obj, UIAtlas atlas)
    {
        UISprite sprite = obj.GetComponent<UISprite>();
        string msg = null;
        if (sprite != null && sprite.atlas == null)
        {
            msg = "Prefab 名称 :" + topObj.name + "               引用图集为空子结点名称:" + obj.name;
            if (IsPrefabWarningRecord && _AtlasErrorWriter.ContainsKey(atlas) && _AtlasErrorWriter[atlas] != null)
            {
                _AtlasErrorWriter[atlas].WriteLine(msg);
            }
        }
        if (sprite != null && sprite.atlas != null && sprite.atlas == atlas)
        {
            msg = "Prefab 名称 :" + topObj.name + "               引用此图集的子结点名称:" + obj.name + "               引用的小图名称：" + sprite.spriteName;
            if (_AtlasLogWriter.ContainsKey(atlas) && _AtlasLogWriter[atlas] != null)
            {
                _AtlasLogWriter[atlas].WriteLine(msg);
            }
            AddSpriteName(atlas, sprite.spriteName);
        }

        UIDefinedButton button = obj.GetComponent<UIDefinedButton>();
        if (button != null && button.tweenTarget != null)
        {
            UISprite spriteBtn = button.tweenTarget.GetComponent<UISprite>();
            if (spriteBtn != null && spriteBtn.atlas != null && spriteBtn.atlas == atlas)
            {
                AddSpriteName(atlas, button.normalSprite);
                AddSpriteName(atlas, button.pressedSprite);
                AddSpriteName(atlas, button.hoverSprite);
                AddSpriteName(atlas, button.disabledSprite);
            }
        }

        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                RecordAtlasInfo(topObj, obj.transform.GetChild(i).gameObject, atlas);
            }
        }
    }
    private void WriteUnRefrences()
    {
        if (!IsNonReferenceRecorded)
        {
            return;
        }
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            StreamWriter fileWriter = ienum.Current.Value;
            UIAtlas atlas = ienum.Current.Key;
            fileWriter.WriteLine(ienum.Current.Key + "图集中所有图片数量：" + atlas.GetListOfSprites().size);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine(atlas.name + "图集中无引用的图片：");
            int count = 0;
            IEnumerator<string> sprite = atlas.GetListOfSprites().GetEnumerator();
            while (sprite.MoveNext())
            {
                if (_RefrencedSprites.ContainsKey(atlas) && !_RefrencedSprites[atlas].Contains(sprite.Current))
                {
                    count++;
                    fileWriter.WriteLine("无UI引用：" + sprite.Current);
                }
            }
            fileWriter.WriteLine(atlas.name + "图集中无引用的图片数量为：" + count);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.Flush();
        }


    }

    private void WriteRefrences()
    {
        if (!IsReferenceRecorded)
        {
            return;
        }
        IEnumerator<KeyValuePair<UIAtlas, StreamWriter>> ienum = _AtlasLogWriter.GetEnumerator();
        while (ienum.MoveNext())
        {
            StreamWriter fileWriter = ienum.Current.Value;
            UIAtlas atlas = ienum.Current.Key;
            fileWriter.WriteLine(ienum.Current.Key + "图集中所有图片数量：" + atlas.GetListOfSprites().size);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine(atlas.name + "图集中有引用的图片：");
            int count = 0;
            IEnumerator<string> sprite = atlas.GetListOfSprites().GetEnumerator();
            while (sprite.MoveNext())
            {
                if (_RefrencedSprites.ContainsKey(atlas) && _RefrencedSprites[atlas].Contains(sprite.Current))
                {
                    count++;
                    fileWriter.WriteLine("有引用：" + sprite.Current);
                }
            }
            fileWriter.WriteLine(atlas.name + "图集中有引用的图片数量为：" + count);
            fileWriter.WriteLine("----------------");
            fileWriter.WriteLine("----------------");
            fileWriter.Flush();
        }


    }
}
