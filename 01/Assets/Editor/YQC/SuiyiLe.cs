

using projectQ;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SuiyiLe {
    public class VertexNodeAAA : VertexNode
    {

    }
    //[MenuItem("Custom/Tools/测试", false, 1)]
    public static void aaa()
    {
        Texture tt = Resources.Load<Texture>("Texture/Tex_Common/Texture_head_01");
        Object select = Resources.Load<Object>("Texture/Tex_Common/userinfo_user_01");
        Debug.Log(tt);
        var list = EditorHelpers.CollectAll2<UITexture>("Assets/Resources", ".prefab", true);
        for (int i=0; i<list.Count; ++i)
        {
            var tex = list[i].Value.mainTexture;
            if (tex != null && tex == select)
            {
                Debug.Log(list[i].Key + "____" + list[i].Value.mainTexture);
                list[i].Value.mainTexture = tt;
                //list[i].Value.WaitTime = 0.05f;
                EditorUtility.SetDirty(list[i].Value);
                //Debug.Log(list[i].Key + "____" + list[i].Value.WaitTime);
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("完成");



        //var fgs = new FileGraphSearch<VertexNodeAAA>();
        //fgs.Search("Assets/Resources/UIPrefab");
        //for(int i=0;i<fgs.adj.adjList.Count; ++i)
        //{
        //    GameObject go = fgs.adj.adjList[i].data as GameObject;
        //    if (go == null) continue;
        //    var buttons = go.GetComponentsInChildren<UIDefinedButton>();
        //    for (int j = 0; j < buttons.Length; ++j)
        //    {
        //        Debug.Log(fgs.adj.adjList[i].data +"___"+ buttons[j].WaitTime);
        //        //if (buttons[j].WaitTime == 0)
        //        //    buttons[j].WaitTime = 0.25f;
        //    }
        //}
    }    	
}
