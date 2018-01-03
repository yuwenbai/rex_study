using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class BaseClass : MonoBehaviour
{
    public int a;
}
public class ClassA : BaseClass
{
    public ClassA()
    {
        Debug.Log("rextest Class A");
    }
}
public class ClassB : BaseClass
{
    public ClassB()
    {
        Debug.Log("rextest Class B");
    }
}
public class TestException : MonoBehaviour {

    public Text label;
    public GameObject AnimObj;
    public Character mTestCharacter = null;
    HeroineStateManager stateManager = null;
    void Awake()
    {
        Application.logMessageReceived += HandleException;

        stateManager = new HeroineStateManager();
    }

    void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error)
        {
            //handle here
            Debug.Log("rextest log exception " + stackTrace);
        }
    }
    public enum aaa
    {
        aaaa,
        bbbb
    }
    // Use this for initialization
    void Start () {

        //BaseClass A = new ClassA();
        //A.a = 20;
        //gameObject.
        //gameObject.AddComponent<typeof(A)> ();

        //if(gameObject.GetComponent<Base>)
        //var  aaa = gameObject.GetComponent<BaseClass>().a;

        ClassB bbb = gameObject.GetComponent<BaseClass>() as ClassB;

        //GameObject gFbx = (GameObject)Instantiate(Resources.Load("Mesh/FBX"));
        //bool b = gFbx.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.isReadable;
        //bool a = b;

    }

    // Update is called once per frame
    void OnGUI()
    {

        if (GUI.Button(new Rect(0 * 100, 30, 100, 50), "111" + (true ? "(√)" : "")))
        {
            if (mTestCharacter == null)
            {
                CharacterManager characterManager = new CharacterManager();
                mTestCharacter = characterManager.GetCharacter();
                mTestCharacter.Create("ch_pc_hou",new List<string>() { "","","","","",""});
            }
        }
        if (GUI.Button(new Rect(1 * 100, 30, 100, 50), "foot" + (true ? "(√)" : "")))
        {
            mTestCharacter.ChangePart(ItemEnum.ItemEnumPart.ItemEnumPart_Foot, "ch_pc_hou_004_jiao");
        }

        if (GUI.Button(new Rect(2 * 100, 30, 100, 50), "head" + (true ? "(√)" : "")))
        {
            mTestCharacter.ChangePart(ItemEnum.ItemEnumPart.ItemEnumPart_Head, "ch_pc_hou_004_tou");
        }

        if (GUI.Button(new Rect(0 * 100, 80, 100, 50), "body" + (true ? "(√)" : "")))
        {
            //mTestCharacter.ChangePart(ItemEnum.ItemEnumPart.ItemEnumPart_Body, "ch_pc_hou_006_shen");
            stateManager.HandleInput(null);
        }
        if (GUI.Button(new Rect(1 * 100, 80, 100, 50), "dunkingstate" + (true ? "(√)" : "")))
        {
            stateManager.SetState(new DuckingState());
        }
        if (GUI.Button(new Rect(2 * 100, 80, 100, 50), "attackingstate" + (true ? "(√)" : "")))
        {
            stateManager.SetState(new AttackingState());
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.baidu.com");    //创建一个请求示例
            //HttpWebResponse response  = (HttpWebResponse)request.GetResponse();　　//获取响应，即发送请求
            //Stream responseStream = response.GetResponseStream();
            //StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            //string html = streamReader.ReadToEnd();
            //Console.WriteLine(html);
        }
        //if (GUI.Button(new Rect(50, 300, 128, 64), "点我就可以"))
        //{
        //    //GameObject objPrefab = (GameObject)Resources.Load("UI/TestPrefab");
        //    //Instantiate(objPrefab);
        //    //var obj = ResourceLoader.Instance.Load<GameObject>("UI/TestPrefab");
        //    //Instantiate(obj);
        //    //var script = obj.GetComponent<TestPrefab>();
        //    //if (script == null )
        //    //{
        //    //    //Debug.LogError("re11111");
        //    //    obj.AddComponent<TestPrefab>();
        //    //}
        //    //var player = ResourceLoader.Instance.Load<GameObject>("Mesh/Cube");
        //    //for (int i = 0; i < 10; ++i)
        //    //{
        //    //    int _a = UnityEngine.Random.Range(0, 4);
        //    //    int _b = UnityEngine.Random.Range(0, 4);
        //    //    var obj = Instantiate(player);
        //    //    obj.GetComponent<Transform>().localScale = new Vector3(_a, _b, 1);
        //    //    obj.GetComponent<MeshRenderer>().material.color = new Color(1, _a, 1);
        //    //    Debug.Log("rextest add cobe " + i);
        //    //    nCubeNum += 1;
        //    //}
        //    //label.text = "" + nCubeNum;
        //}
    }
	void Update () {
    }
}
