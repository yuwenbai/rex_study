using UnityEngine;
using System.Collections;
using Game;

public class ScriptRoot : MonoBehaviour
{
    int d;
    private static ScriptRoot instance = null;

    public static ScriptRoot Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
    }
    public T ScriptsDataSet<T>() where T : MonoBehaviour
    {
        T data = gameObject.GetComponent<T>();
        if (data == null)
        {
            data = gameObject.AddComponent<T>();
        }

        return data;
    }
    private void Init()
    {
        
        DontDestroyOnLoad(gameObject);
        ScriptsDataSet<TestException>();
        ScriptsDataSet<TestBundle>();
        ScriptsDataSet<TestLoadNewAB>();
        ScriptsDataSet<Car>();
    }
    // Use this for initialization
    void Start()
    {
        Application.logMessageReceivedThreaded += logCallback;
    }
    public static void logCallback(string log, string stackTrace, UnityEngine.LogType _type)
    {
        //FileLog fileLog = new FileLog(Application.dataPath +"/" + System.DateTime.Now.ToString("yyyyMMdd") + ".log", true);
        //fileLog.Log(log);
        //fileLog.Log(stackTrace);
        //fileLog.Flush();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        
        if (GUILayout.Button("main script"))
        {
            ScriptsDataSet<main>();
        }
    }
}
