using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitEngine;
using System.Diagnostics;
using System.Reflection;
using System.Text;
public class testmono : MonoBehaviour {

    List<object> mList = new List<object>();
	// Use this for initialization
	void Awake () {
        
        LogToFile.InitLogCallback();
        Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
        DLog.LOGColor(DLogType.Error, "testtttttt", LogColor.YELLO);

        //初始化main APP
        string tpath = "G:/Work/Code/Project/UnityTest/testilruntime/dllproject/testproj/testproj/bin/Release/testproj";
        AppCore.Main.SManager.LoadProject(tpath);//主app加载dll文件

        //初始化app2
        string tpathapp2 = "G:/Work/Code/Project/UnityTest/testilruntime/testapp2/testapp2/bin/Release/testapp2";
        AppCore.CreatGameCore("testapp").SManager.LoadProject(tpathapp2);//其他app创建

        DLog.LOGColor(DLogType.Log,"------------------------App2-------------------------------",LogColor.YELLO);
        //app2测试
        object tobj = AppCore.App["testapp"].SManager.CodeTool.GetCSLEObjectParmas("TestApp2");
        DLog.LOG(DLogType.Log, tobj);
        AppCore.App["testapp"].SManager.CodeTool.CallMethodByName("testCall", tobj);



        //main app测试
        DLog.LOGColor(DLogType.Log, "--------------------------Main App---------------------------", LogColor.YELLO);
        TestGetILObject();
        TestAddInterface();


        string teststr = "testssss/ddfffs/ddname.DB";
        int tstartindex = teststr.LastIndexOf("/");
        int endindex = teststr.LastIndexOf(".DB");
        string tassetsname = teststr.Substring(tstartindex+1, endindex - tstartindex-1);
        DLog.LOG(DLogType.Log, tassetsname);

        // AppCore.App["MainGameCore"].LManager.LoadResourcesBundleByRelativePathNameAsync("test", "MJ/testcube", "MJ/testcube", TestCallBack, progress, null);

        // Object tres =  GameCore.LManager.LoadResourcesBundleByRelativePathName("MJ/testcube", "MJ/testcube");
        // GameObject.Instantiate(tres);
        // GameObject tgameobject =  GameObject.Find("testmono1");
        //  GameObject.Destroy(tgameobject);
        //AppCore.DestroyGameCore("MainGameCore");
    }

    void progress(string _key,float _pro)
    {
        DLog.LOG(DLogType.Log, _key +"|"+ _pro);
    }

    void TestCallBack(string _key,object res , object _tar)
    {
        DLog.LOG(DLogType.Log, _key +"|"+res);
    }

    void TestAddInterface()
    {
        //ScriptInterfaceOnEnable 是 接口的其中之一,Onenable代表包含OnEnable和OnDisable,可以再脚本中实现响应.ScriptInterface空间为借口的命名空间.请根据后缀选取合适的接口
        //所有接口都会检测Update,FixedUpdate,LateUpdate,OnGUI函数,上述函数不支持参数.默认有0.1秒的间隔时间.如果需要无间隔,需要调用相关设置 SetUpdateInterval 等
        //接口类拥有 Call系列函数,用于调用脚本内任意函数
        //不推荐函数重载,会有查找问题.反射模式不支持重载.LS模式支持参数数量不同的重载,
        //UseScriptType.UseScriptType_LS模式是用脚本解析 Sys模式是用系统反射.android平台下,Sys模式有效率优势,但不支持il2cpp编译.

        LitEngine.ScriptInterface.ScriptInterfaceOnEnable tinterface = gameObject.AddComponent<LitEngine.ScriptInterface.ScriptInterfaceOnEnable>();
        tinterface.InitScript("TestB", "MainGameCore");
    }

    void TestGetILObject()
    {
        object tobj = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEObjectParmas("TestA", 2);
        DLog.LOG(DLogType.Log, tobj);
        AppCore.App["MainGameCore"].SManager.CodeTool.CallMethodByName("log", tobj);
        AppCore.App["MainGameCore"].SManager.CodeTool.SetTargetMember(tobj, 0, 4);
        AppCore.App["MainGameCore"].SManager.CodeTool.CallMethodByName("log", tobj);

        object tt = AppCore.App["MainGameCore"].SManager.CodeTool.GetTargetMemberByIndex(1, tobj);
        DLog.LOG(DLogType.Log, tt);

        AppCore.App["MainGameCore"].SManager.CodeTool.SetTargetMember(tobj, 1, "更改字符串11111");
        tt = AppCore.App["MainGameCore"].SManager.CodeTool.GetTargetMemberByIndex(1, tobj);
        DLog.LOG(DLogType.Log, tt);

        var ttype = AppCore.App["MainGameCore"].SManager.CodeTool.GetLType("TestA");

        System.Action tact = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEDelegate<System.Action>("TestCall", ttype, tobj);
        tact();


        System.Action<float> tact1 = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEDelegate<System.Action<float>, float>("TestCall1", ttype, tobj);
        tact1(99.33f);

        System.Action<float, float> tact2 = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEDelegate<System.Action<float, float>, float, float>("TestCal2",  ttype, tobj);
        tact2(99.33f, 88.1f);

        System.Action<float, float, float> tact3 = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEDelegate<System.Action<float, float, float>, float, float, float>("TestCall3",  ttype, tobj);
        tact3(99.33f, 88.1f, 77.1f);

        System.Action<float, float, float, float> tact4 = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEDelegate<System.Action<float, float, float, float>, float, float, float, float>("TestCall4",  ttype, tobj);
        tact4(99.33f, 88.1f, 77.1f, 66.1f);

        object tproto = AppCore.App["MainGameCore"].SManager.CodeTool.GetCSLEObjectParmas("TestProto");
        DLog.LOG(DLogType.Log, tproto);
        AppCore.App["MainGameCore"].SManager.CodeTool.SetTargetMember(tproto, 4, "更改字符串测试proto");
        //test proto
        LitEngine.ProtoCSLS.ProtoBufferWriterBuilderCSLS twbuilder = new LitEngine.ProtoCSLS.ProtoBufferWriterBuilderCSLS(AppCore.App["MainGameCore"].SManager.CodeTool,tproto);
        byte[] tbuff =  twbuilder.GetBuffer();

        object tCSLEObject = LitEngine.ProtoCSLS.ProtoBufferReaderBuilderCSLS.GetCSLEObject(AppCore.App["MainGameCore"].SManager.CodeTool,tbuff, tbuff.Length, "TestProto");
        DLog.LOG(DLogType.Log, AppCore.App["MainGameCore"].SManager.CodeTool.GetTargetMemberByIndex(4, tCSLEObject));

    
    }

    // Update is called once per frame
    void Update () {
       
     

    }
}

public class testa
{
    public void testcall()
    {
        testb tb = new testb();
        tb.testcall();
    }
}

public class testb
{
    public void testcall()
    {
        testc tc = new testc();
        tc.testcall();
    }
}
public class testc
{
    public void testcall()
    {
        object ttttssss = null;
        ttttssss.GetType();
    }
}