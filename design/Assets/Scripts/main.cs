﻿using Game;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
public class MjPaiKou
{
    public bool canPeng;
    public bool CanPeng
    {
        get { return canPeng; }
        set { canPeng = value; }
    }
    public bool canGang;
    public bool canTing;
    public bool canHu;
    public bool canZiMo;
    public bool canChi;
    public bool canMingLou;
    public bool canTingGang;
    public bool canPiao;
    public bool canCiHu;            //次胡
    public void aaaa()
    {
        int a = 30;
        int b  = 20;
    }
}
public class utils
{
    //public static string Output<T>( T t)
    //{
    //    var type = t.GetType();
    //    var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    //    StringBuilder sb = new StringBuilder();
    //    foreach (var finfo in Fields)
    //    {
    //        var test = finfo.GetValue(t);
    //        sb.Append(finfo.Name.ToString());
    //        sb.Append(": ");
    //        sb.Append(test.ToString());
    //        sb.AppendLine();
    //    }
    //    return sb.ToString();
    //}
};

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {


        Queue<int> aaa = new Queue<int>();
        for (int i_a = 0; i_a < 10; ++i_a)
        {
            aaa.Enqueue(i_a);
        }

        Queue<int> bbb = new Queue<int>();

        int[] aaaaa = new int[aaa.Count];
        aaa.CopyTo(aaaaa, 0);
        bbb = new Queue<int>(aaaaa);

        LoginSceneHandle.Init();
        aaa.Clear();
        //foreach (var _a in bbb)
        //{
        //    Debug.Log("aaaaa " + _a);
        //}

        //MjPaiKou _MjPaiKou = new MjPaiKou();
        //_MjPaiKou.canPeng = true;
        //_MjPaiKou.canMingLou = true;

        //Debug.Log(utils.Output(_MjPaiKou));
        //Debug.Log("test RefactionObject " + CommonTools.ReflactionObject(_MjPaiKou));

        //FileLog fileLog = new FileLog(Application.dataPath + "/testlog", true);
        //fileLog.Log("111111111test 111");
        //fileLog.Flush();
        //TestThreadLoom(10000);

        //Debug.Log("ffffff" + CommonTools.)
        //string str = MjPaiKou.ToString();
        //var type = MjPaiKou.GetType();
        //object obj = Activator.CreateInstance(type, true);
        //var fff = type.GetMembers();
        //var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //var aaa = Fields[0];
        //Debug.Log("rextest aaa " + aaa.Name);

        //var props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //foreach (var finfo in Fields)
        //{
        //    var test = finfo.GetValue(obj);
        //    Debug.Log("rextest aaa " + finfo.Name + " value is "+ test.ToString());
        //}
        //var fi = type.GetField(MjPaiKou.canChi.ToString());
        //var memmbers = typeof(pMjPaiKou).GetMembers();
        //foreach (var p in memmbers)
        //{
        //    Debug.Log(p.Name);
        //}
        //object obj = Activator.CreateInstance(type);
        //PropertyInfo[] props = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        //foreach (PropertyInfo p in props)
        //{
        //    Debug.Log(p);
        //}
        //Debug.Log("rextest main android test");

        phone apple = new applephone();
        Decoder decoder = new Sticker(apple);
        decoder.print();
        Decoder accessories = new Accessories(apple);
        accessories.print();

        facade _facade = new facade();
        _facade.registerfacade("shuxue");

        //命令
        Receiver receiver = new Receiver();
        Commond commond = new ConcreteCommand(receiver);
        Invoker i = new Invoker(commond);
        i.ExecuteCommond();


        TenXun tenXun = new TenXunGame("TenXun Game");
        tenXun.addObserver(new Subscriber("Learning Hard"));
        tenXun.addObserver(new Subscriber("Tom"));
        tenXun.update();

        //策略
        StrategyPattern _StrategyPattern = new StrategyPattern();
        _StrategyPattern.init();

        //简单工厂
        Food food = SimpleFactory.CreateFood(1);
        food.print();

        food = SimpleFactory.CreateFood(2);
        food.print();

        //工厂
        Factory abstractFactory = new TomatoScrambledEggsFactory();
        AbsFood absfood = abstractFactory.CreateFoodFactory();
        absfood.Print();
        abstractFactory = new ShreddedPorkWithPotatoesFactory();
        absfood = abstractFactory.CreateFoodFactory();
        absfood.Print();

        //抽象工厂
        AbstractFactory absFactory = new NanChangFactory();
        absFactory.CreateYaBo().Print();
        absFactory.CreateYaJia().Print();

        absFactory = new WuHanFactory();
        absFactory.CreateYaBo().Print();
        absFactory.CreateYaJia().Print();

        //构造模式
        Director director = new Director();
        Builder builderA = new ConcreteBuilderA_Builder();
        director.Construct(builderA);

        Builder builderB = new ConcreteBuilderB_Builder();
        director.Construct(builderB);


        Computer computera = builderA.GetComputer();
        computera.show();

        Computer computerb = builderB.GetComputer();
        computerb.show();

        //桥接模式
        RemoteControl Rc = new RemoteControl();
        ChangeHong changeHong = new ChangeHong();
        Rc.TVtor = changeHong;
        Rc.TVtor.On();

        Sony sony = new Sony();
        Rc.TVtor = sony;
        Rc.TVtor.On();



    }

    // Update is called once per frame
    void Update () {
		

	}
    void TestThreadLoom(int nTimes)
    {
        //Run the action on a new thread  
        Loom.RunAsync(() =>
        {
            for (var i = 0; i < nTimes; ++i)
            {
                Debug.Log("rextest TestThreadLoom " + i);
            }
            //Run some code on the main thread  
            //to update the mesh  
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("rextest QueueOnMainThread " + nTimes);
            });
        });
    }
}
