using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using projectQ;


public class FindAllAtlasSprites : MonoBehaviour
{
    public bool RecordsSpritesNames = true;
    public bool RecrodsBigTexture = true;
    public bool RecordUnRefre = true;
    public bool RecordRefre = true;
    public float Widht_Max = 300;
    public float Height_Max = 300;
    private List<UIAtlas> _AllAtlasList = new List<UIAtlas>();
    private string _AllAtlasFileName = "AllAtlasSprites";
    private StreamWriter _FileLog;

    private GameObject[] _TopGameObjects;
    //每个图集在项目中用到的所有小图信息
    private Dictionary<UIAtlas, List<string>> _RefrencedSprites = new Dictionary<UIAtlas, List<string>>();
    // Use this for initialization
    void Start()
    {
        Debug.Log("统计中...");
        _TopGameObjects = Resources.LoadAll<GameObject>("");//PrefabPaths
        FindAllAtlas();
        InitFileWriter();
        WriteAtlasNames();
        WriteRecordLock();
        RecordAllAtlasInfo();
        FindAtlasSprites();
        FindAllBigTextures();
        RecordAtlasUnRefre();
        RecordAtlasRefre();
        CloseFile();
        Debug.Log("所有图集的信息统计完毕:");
    }
    private void WriteAtlasNames()
    {

        _FileLog.WriteLine("项目中所有图集名称如下：总数量为：" + _AllAtlasList.Count);
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            _FileLog.WriteLine("图集名称：" + _AllAtlasList[i].name);
        }
        
        _FileLog.WriteLine("----------");
    }
    private void WriteRecordLock()
    {
        _FileLog.WriteLine("是否统计每个图集包含所有图片名称:     " + (RecordsSpritesNames? "是" : "否"));
        _FileLog.WriteLine("是否统计每个图集包含大图:             " + (RecrodsBigTexture ? "是" : "否"));
        _FileLog.WriteLine("是否统计每个图集没有静态引用的图片:   " + (RecordUnRefre ? "是" : "否"));
        _FileLog.WriteLine("是否统计每个图集正在项目中引用的图片: " + (RecordRefre ? "是" : "否"));
    }
    private void CloseFile()
    {
        _FileLog.Flush();
        _FileLog.Close();
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
    private void InitFileWriter()
    {
        string logPath = Application.persistentDataPath + "/Atlas_Statistics_Result";
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }
        string _PathAndFileName = logPath +"/"+ _AllAtlasFileName + ".txt";
        if (File.Exists(_PathAndFileName))
        {
            File.Delete(_PathAndFileName);
        }
        Debug.Log("所有图集的信息开始统计（等待中...）文件目录：" + _PathAndFileName);
        FileStream logFile = new FileStream(_PathAndFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _FileLog = new StreamWriter(logFile);
        _FileLog.WriteLine("所有图集信息如下：");
        _FileLog.WriteLine("----------------");
        _FileLog.WriteLine("----------------");
        _FileLog.Flush();
    }
    private void FindAtlasSprites()
    {
        if (!RecordsSpritesNames)
        {
            return;
        }
        _FileLog.WriteLine("所有图集中包含的图片信息如下：");
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            _FileLog.WriteLine("----------------");
            for (int j = 0; j < _AllAtlasList[i].spriteList.Count;j++)
            {
              UISpriteData item=  _AllAtlasList[i].spriteList[j];
              _FileLog.WriteLine("图集名称：" + _AllAtlasList[i].name + "               包含图片：" + item.name + "      图片尺寸：" + item.width+"*"+item.height);
            }
            _FileLog.WriteLine("-----write over-----");
            _FileLog.WriteLine("----------------");
        }
        _FileLog.Flush();
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
                if (!_RefrencedSprites.ContainsKey(_AllAtlasList[j]))
                {
                    _RefrencedSprites.Add(_AllAtlasList[j], new List<string>());
                }
                RecordAtlasInfo(_TopGameObjects[i], _TopGameObjects[i], _AllAtlasList[j]);
            }
        }
    }
    private void RecordAtlasInfo(GameObject topObj, GameObject obj, UIAtlas atlas)
    {
        UISprite sprite = obj.GetComponent<UISprite>();
        if (sprite != null && sprite.atlas != null && sprite.atlas.name == name)
        {
            AddSpriteName(atlas, sprite.spriteName);
        }
        UIDefinedButton button = obj.GetComponent<UIDefinedButton>();
        if (button != null && button.tweenTarget != null)
        {
            UISprite spriteBtn = button.tweenTarget.GetComponent<UISprite>();
            if (spriteBtn != null && spriteBtn.atlas != null && spriteBtn.atlas.name == name)
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
    private void AddSpriteName(UIAtlas atlas, string spriteName)
    {
        if (!string.IsNullOrEmpty(spriteName))
        {
            if (!_RefrencedSprites[atlas].Contains(spriteName))
            {
                _RefrencedSprites[atlas].Add(spriteName);
            }
        }

    }
    private void FindAllBigTextures()
    {
        if (!RecrodsBigTexture)
        {
            return;
        }
        _FileLog.WriteLine("所有图集大图信息如下：规定尺寸:" + Widht_Max + "*" + Height_Max);
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            _FileLog.WriteLine("----------------");
            for (int j = 0; j < _AllAtlasList[i].spriteList.Count; j++)
            {
                UISpriteData item = _AllAtlasList[i].spriteList[j];
                if (item.width >= Widht_Max || item.height >= Height_Max)
                    _FileLog.WriteLine("图集名称：" + _AllAtlasList[i].name + "               图片名称：" + item.name + "    尺寸：" + item.width + "*" + item.height);
            }
            _FileLog.WriteLine("-----write over-----");
            _FileLog.WriteLine("----------------");
        }
        _FileLog.Flush();
    }

    private void RecordAtlasUnRefre()
    {
        if (!RecordUnRefre)
        {
            return;
        }
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            UIAtlas atlas = _AllAtlasList[i];
            _FileLog.WriteLine(atlas.name + "图集中所有图片数量：" + atlas.GetListOfSprites().size);
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine(atlas.name + "图集中无引用的图片：");
            int count = 0;
            IEnumerator<string> sprite = atlas.GetListOfSprites().GetEnumerator();
            while (sprite.MoveNext())
            {
                if (!_RefrencedSprites[atlas].Contains(sprite.Current))
                {
                    count++;
                    _FileLog.WriteLine("无UI引用：" + sprite.Current);
                }
            }
            _FileLog.WriteLine(atlas.name + "图集中无引用的图片数量为：" + count);
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
            _FileLog.Flush();
        }
        
    }
    
    private void RecordAtlasRefre()
    {
        if (!RecordRefre)
        {
            return;
        }
        for (int i = 0; i < _AllAtlasList.Count; i++)
        {
            UIAtlas atlas = _AllAtlasList[i];
            _FileLog.WriteLine(atlas.name + "图集中所有图片数量：" + atlas.GetListOfSprites().size);
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine(atlas.name + "图集中有引用的图片如下：");
            int count = 0;
            IEnumerator<string> sprite = atlas.GetListOfSprites().GetEnumerator();
            while (sprite.MoveNext())
            {
                if (!_RefrencedSprites[atlas].Contains(sprite.Current))
                {
                    count++;
                    _FileLog.WriteLine("图片名称：" + sprite.Current);
                }
            }
            _FileLog.WriteLine(atlas.name + "图集中有引用的图片数量为：" + count);
            _FileLog.WriteLine("----------------");
            _FileLog.WriteLine("----------------");
            _FileLog.Flush();
        }
    }
}
