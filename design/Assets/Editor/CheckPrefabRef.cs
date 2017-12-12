using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Daemo {
    public class EditorMenu : BaseEditor
    {

        //[MenuItem("Tools/检查选中预制体引用是否丢失")]
        //public static void CheckPrefabRef()
        //{
        //    EDCheckPrefabRef.StartCheckPrefabRef();
        //}

        [MenuItem("Tools/检查目录中中预制体引用是否丢失")]
        public static void CheckDirPrefabRef()
        {
            EDCheckPrefabRef.StartCheckDirPrefabRef();
        }

    }
    public class EDCheckPrefabRef : BaseEditor
    {
        static string m_strCurPath;

        public static void StartCheckPrefabRef() {
            if (Selection.objects != null && Selection.objects.Length > 0 && Selection.objects[0] != null)
            {
                GameObject obj = Selection.objects[0] as GameObject;
                CheckPrefabRef(obj.transform);
            }
        }

        public static void StartCheckDirPrefabRef() {
            //HandlelDirections(Application.dataPath + "/Resources", HandleDirectionsAct);
            List<GameObject> list = GetAllUIPrefabs();
            checkStr = "";
            for (int i = 0; i < list.Count; i++) {
                CheckPrefabRef(list[i].transform);
            }
            Debug.LogError(checkStr);
            checkStr = "";
        }

        private static void HandleDirectionsAct(string path) {
            if (path.EndsWith(".meta")) {
                return;
            }
            if (path.EndsWith(".prefab")) {
                //path = path.Split('.')[0];
                path = "Assets"+ SplitPath(path, "Assets");
                m_strCurPath = path;
                Debug.Log("path:" + path);
                GameObject o = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                //o = GameObject.Instantiate(o);
                CheckPrefabRef(o.transform);
            }
        }
        private static List<GameObject> allPrefabs = new List<GameObject>();
        public static List<GameObject> GetAllUIPrefabs() {
            allPrefabs.Clear();
            HandlelDirections(Application.dataPath + "/Resources/UI", PrefabCallBack);
            return allPrefabs;
        }
        private static void PrefabCallBack(string path)
        {
            if (path.EndsWith(".meta"))
            {
                return;
            }
            if (path.EndsWith(".prefab"))
            {
                //path = path.Split('.')[0];
                path = "Assets" + SplitPath(path, "Assets");
                m_strCurPath = path;
                GameObject searchObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (searchObj != null)
                {
                    allPrefabs.Add(searchObj);
                }

            }
        }
        #region 不规范图片组
        private static List<Texture2D> noGoodImgs = new List<Texture2D>();
        public static List<Texture2D> goodImgs = new List<Texture2D>();
        public static List<Texture2D> packImgs = new List<Texture2D>();
        private static Dictionary<int, bool> goodsSizes = new Dictionary<int, bool>();
        public static List<Texture2D> GetNoGoodImgs() {
            for (int i = 1; i < 12; i++) {
                goodsSizes[(1 << i)] = true;
            }
            noGoodImgs.Clear();
            goodImgs.Clear();
            packImgs.Clear();
            HandlelDirections(Application.dataPath + "/Assets", ImgCallBack);
            HandlelDirections(Application.dataPath + "/Resources", ImgCallBack);
            return noGoodImgs;
        }
        private static void ImgCallBack(string path)
        {
            if (path.EndsWith(".meta"))
            {
                return;
            }
            if (path.EndsWith(".png") || path.EndsWith(".jpg"))
            {
                //path = path.Split('.')[0];
                path = "Assets" + SplitPath(path, "Assets");
                m_strCurPath = path;
                bool isSprite = false;
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite != null) {
                    if (sprite.packed)
                    {
                        isSprite = true;
                        packImgs.Add(sprite.texture);
                        //Texture2D img = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                        //if (img != null)
                        //{
                        //    packImgs.Add(img);
                        //}
                    }
                }
                if (!isSprite)
                {
                    Texture2D img = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    if (img != null)
                    {
                        
                        if (!goodsSizes.ContainsKey(img.width) || !goodsSizes.ContainsKey(img.height))
                        {
                            noGoodImgs.Add(img);
                        }
                        else
                        {
                            goodImgs.Add(img);
                        }
                    }
                }
            }
        }
#endregion
        #region 替换字体
        private static string nowFontName = string.Empty;
        private static bool replaceFontStatus = false;
        private static Font replaceFont;
        private static int replaceFontIndex = 0;
        private static int fontStyleIndex;
        public static List<Text> useTexts = new List<Text>();
        private static string checkFontName = string.Empty;
        public static void ReplaceUIFont(string nowName, Font toFont,int selectIndex) {
            nowFontName = nowName;
            replaceFontIndex = 1;
            replaceFont = toFont;
            fontStyleIndex = selectIndex;
            EditorUtility.DisplayProgressBar("Replace Fonting", "start", replaceFontIndex);
            HandlelDirections(Application.dataPath + "/Resources/UI", FontCallBack);
            EditorUtility.ClearProgressBar();
            EditorApplication.SaveScene();
        }

        public static void CheckUIFontUse(string fontName) {
            useTexts.Clear();
            checkFontName = fontName;
            HandlelDirections(Application.dataPath + "/Resources/UI", FontCallBack1);
        }
        private static void FontCallBack(string path) {
            if (path.EndsWith(".meta"))
            {
                return;
            }
            if (path.EndsWith(".prefab"))
            {
                //path = path.Split('.')[0];
                path = "Assets" + SplitPath(path, "Assets");
                m_strCurPath = path;
                GameObject searchObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (searchObj != null)
                {
                    replaceFontStatus = false;
                    CheckPrefabFont(searchObj.transform);
                    if (replaceFontStatus)
                    {
                        replaceFontIndex++;
                        EditorUtility.DisplayProgressBar("replace fonting", path, replaceFontIndex);
                        Debug.Log("Replace Frefab:" + path);
                        
                    }
                }

            }
        }
        private static void CheckPrefabFont(Transform t)
        {
            CycleChild(t, CheckFontReplace); 
        }
        private static void CheckPrefabFont1(Transform t)
        {
            CycleChild(t, CheckFontUse);
        }
        private static void CheckFontUse(Transform t)
        {
            Text[] texts = t.gameObject.GetComponents<Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                Text text = texts[i];
                if (text.font != null && text.font.name == checkFontName)
                {
                    useTexts.Add(text);
                }
            }
        }
        static string checkStr;
        private static void CheckPrefabRef(Transform t)
        {
            CycleChild(t, CycleChildAct);
        }
        private static void CheckFontReplace(Transform t)
        {
            Text[] texts = t.gameObject.GetComponents<Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                Text text = texts[i];
                if (text.font != null && text.font.name == nowFontName)
                {
                    text.font = replaceFont;
                    if (fontStyleIndex > 1) {
                        if (fontStyleIndex == 2)
                        {
                            text.fontStyle = FontStyle.Normal;
                        }else if (fontStyleIndex == 3)
                        {
                            text.fontStyle = FontStyle.Bold;
                        }else if (fontStyleIndex == 4)
                        {
                            text.fontStyle = FontStyle.Italic;
                        }else if (fontStyleIndex == 5)
                        {
                            text.fontStyle = FontStyle.BoldAndItalic;
                        }
                    }
                    EditorUtility.SetDirty(text);
                    replaceFontStatus = true;
                }
            }
        }
        private static void FontCallBack1(string path)
        {
            if (path.EndsWith(".meta"))
            {
                return;
            }
            if (path.EndsWith(".prefab"))
            {
                //path = path.Split('.')[0];
                path = "Assets" + SplitPath(path, "Assets");
                m_strCurPath = path;
                GameObject searchObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (searchObj != null)
                {
                    CheckPrefabFont1(searchObj.transform);
                }

            }
        }

        #endregion
        private static void CycleChildAct(Transform t) {
            Component[] components = t.gameObject.GetComponents(typeof(MonoBehaviour));
            string path = AssetDatabase.GetAssetPath(t.gameObject.GetInstanceID());
            foreach (Component m in components)
            {
                if (m == null)
                {
                    Debug.LogError("path:" + path + "      " + t.gameObject.name + "  有空引用脚本");
                }
                else
                {
                    Type type = m.GetType();
                    FieldInfo[] infos = type.GetFields();
                    for (int i = 0; i < infos.Length; i++)
                    {
                        if (!infos[i].FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
                        {
                            continue;
                        }
                        //if (infos[i].FieldType == typeof(UGUIToggle)|| infos[i].FieldType == typeof(UGUIButton)
                        //    || infos[i].FieldType == typeof(UGUIToggleGroup)
                        //    || infos[i].FieldType == typeof(UnityEngine.UI.Graphic)) {
                        //    continue;
                        //}
                        if (infos[i].Name == "GlassBackGround"
                            ||infos[i].Name == "redDot"
                            || infos[i].Name == "glowEffect"
                            || infos[i].Name == "GlassBackGround") {
                            continue;
                        }
                        var hideInInspector = infos[i].GetCustomAttributes(typeof(HideInInspector), false).FirstOrDefault();
                        var nonSerialized = infos[i].GetCustomAttributes(typeof(NonSerializedAttribute), false).FirstOrDefault();
                        //var ignoreCheck = infos[i].GetCustomAttributes(typeof(IgnoreCheck), false).FirstOrDefault();
                        if (hideInInspector != null|| nonSerialized != null/*|| ignoreCheck!=null*/)
                        {
                            continue;
                        }
                        if (infos[i].IsPrivate&&infos[i].IsNotSerialized)
                        {
                            continue;
                        }
                        if (infos[i].IsStatic) {
                            continue;
                        }
                        object o = infos[i].GetValue(m);
                        if (o == null)
                        {
                            Debug.LogError(path + ",   ObjName:" + m.name + "    字段名:" + infos[i].Name + "    FieldType:" + infos[i].FieldType);
                            //Debug.LogError(path + ",   ObjName:" + m.name + "    字段名:" + infos[i].Name + "    类型名:" + type.Name
                            //    + "    MemberType:" + infos[i].MemberType
                            //    + "    IsPublic:" + infos[i].IsPublic
                            //    + "    IsStatic:" + infos[i].IsStatic
                            //    + "    IsNotSerialized:" + infos[i].IsNotSerialized
                            //    + "    Attributes:" + infos[i].Attributes
                            //    + "    FieldHandle:" + infos[i].FieldHandle
                            //    + "    FieldType:" + infos[i].FieldType);
                            //checkStr = checkStr + path + "," + infos[i].Name + " , " + o + "\n";
                        }
                        else
                        {
                            
                            string s = o.ToString();
                            if (s == "null")
                            {
                                Debug.LogError(path + ",   ObjName:" + m.name + "    字段名:" + infos[i].Name + "    FieldType:" + infos[i].FieldType);
                                //checkStr = checkStr + path + "," + infos[i].Name + " , " + o + "\n";
                            }
                            else
                            {
                                //Debug.Log(infos[i].Name + " , " + o);
                            }
                        }
                        
                    }
                }
                
            }
        }
        



    }

}


