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
    public double Quantity { get; set; }
}
public class ClassA : BaseClass
{
    public ClassA()
    {
        
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

    Heroine mHeroine = null;
    void Awake()
    {
        Application.logMessageReceived += HandleException;

        stateManager = new HeroineStateManager();

        //ClassA aa = new ClassA();
        //aa.Quantity = 20;
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
            //stateManager.HandleInput(null);
            //List<Interpreter> mList = new List<Interpreter>();
            //mList.Add(new TerminalInterpreter());
            //mList.Add(new NonterminalInterpreter());
            //foreach(var inter in mList)
            //{
            //    inter.Interpret();
            //}

            //ConcreteMediator m = new ConcreteMediator();
            //ConcreteColleague concreteColleague = new ConcreteColleague(m);
            //DisConcreteColleague disconcreteColleague = new DisConcreteColleague(m);
            //concreteColleague.ColleagueSend("test 111");
            //disconcreteColleague.ColleagueSend("test 222");
            //Originator o = new Originator();
            //o.State = "On";
            //Caretaker c = new Caretaker();
            //c.Memento = o.CreateMemento();
            //o.State = "Off";
            //o.SetMemento(c.Memento);

            //ConcreteSubject s = new ConcreteSubject();
            //s.Attach(new ConcreteObserver(s, "A"));
            //s.Attach(new ConcreteObserver(s, "B"));
            //s.Attach(new ConcreteObserver(s, "C"));

            //s.SubjectState = "ABC";
            //s.Notify();
            //s.SubjectState = "DEF";
            //s.Notify();

            //AbstractClass abstractClassA = new ConcreteClassA();
            //abstractClassA.TemplateMethod();
            //AbstractClass abstractClassB = new ConcreteClassB();
            //abstractClassB.TemplateMethod();

            //ObjectStructure o = new ObjectStructure();
            //o.Attach(new ElementA());
            //o.Attach(new ElementB());

            //o.Accept(new ConcreteVistor1());
            //o.Accept(new ConcreteVistor2());

            //Build build1 = new ConcreteBuilder1();
            //Build build2 = new ConcreteBuilder2();
            //Product p = new Product();

            //ProductDirector productDirector = new ProductDirector();
            //productDirector.Constuct(build1);
            //Product p1 = build1.GetResult();
            //p1.Show();
            //productDirector.Constuct(build2);
            //Product p2 = build2.GetResult();
            //p2.Show();

            //ConcretePrototype1 p1 = new ConcretePrototype1("I");
            //ConcretePrototype1 c1 = (ConcretePrototype1)p1.Clone();
            //Debug.Log("Cloned: " + c1.Id);

            //ConcretePrototype2 p2 = new ConcretePrototype2("II");
            //ConcretePrototype2 c2 = (ConcretePrototype2)p2.Clone();
            //Debug.Log("Cloned: " + c2.Id);

            //CloneFactory cloneFactory = new CloneFactory();
            //Sheep sally = new Sheep();
            //Sheep clonedSheep = (Sheep)cloneFactory.GetClone(sally);
            //Debug.Log("Sally: " + sally.ToStringEX());
            //Debug.Log("Clone of Sally: " + clonedSheep.ToStringEX());
            //Debug.Log("Sally Hash: " + sally.GetHashCode() + " - Cloned Sheep Hash: " + clonedSheep.GetHashCode());

            //RPGGame rPGGame = new RPGGame();
            //rPGGame.Start();
            //rPGGame.Update();

            //Breed troll = new Breed(null, 25, "The troll hits you!");
            //Breed trollArcher = new Breed(troll, 0, "The troll archer fires an arrow!");
            //Breed trollWizard = new Breed(troll, 0, "The troll wizard casts a spell on you!");

            ////通过种类创建monster对象
            //Monster trollMonster = troll.NewMonster();
            //trollMonster.ShowAttack();

            //Monster trollArcherMonster = trollArcher.NewMonster();
            //trollArcherMonster.ShowAttack();

            //Monster trollWizardMonster = trollWizard.NewMonster();
            //trollWizardMonster.ShowAttack();

            //int externalState = 22;
            //FlyweightFactory flyweightFactory = new FlyweightFactory();
            //Flyweight concreteFlyweight =  flyweightFactory.GetFlyweight("X");
            //concreteFlyweight.Operation(--externalState);

            //Flyweight fy = flyweightFactory.GetFlyweight("Y");
            //fy.Operation(--externalState);

            //Flyweight fz = flyweightFactory.GetFlyweight("Z");
            //fz.Operation(--externalState);

            //UnsharedConcreteFlyweight fu = new UnsharedConcreteFlyweight();

            //fu.Operation(--externalState);

            //string document = "AAZZBBZB";
            //char[] chars = document.ToCharArray();
            //CharacterFactory factory = new CharacterFactory();

            //// extrinsic state
            //int pointSize = 10;

            //// For each character use a flyweight object
            //foreach (char c in chars)
            //{
            //    pointSize++;
            //    CharacterBase character = factory.GetCharacter(c);
            //    character.Display(pointSize);
            //}

            //IBM imb = new IBM();
            //imb.Attach(new ObserverOrange());
            //imb.Attach(new ObserverApple());
            //imb.name = "test ";
            //imb.Notify();

            //Debug.Log("rextest 分割线");
            //imb.name = "anothor test";
            //imb.Notify();

            //AnimatorController animatorController = new AnimatorController();
            //foreach (var layer in animatorController.layers)
            //{
            //    AnimatorStateMachine sm = layer.stateMachine;
            //    foreach (var state in sm.states)
            //    {
            //       // state.state.name;
            //    }
            //}
            //Animator aor = new Animator();
            //for (int i = 0; i < amtor.layerCount; ++i)
            //{
            //    amtor.get
            //}
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

        if (mHeroine == null)
        {
            mHeroine = new Heroine();
        }
        mHeroine.HandleInput();
    }
}
