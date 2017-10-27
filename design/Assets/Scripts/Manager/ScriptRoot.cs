using UnityEngine;
using System.Collections;

public class ScriptRoot : MonoBehaviour
{

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
        ScriptsDataSet<main>();
        ScriptsDataSet<TestBundle>();
        ScriptsDataSet<TestLoadNewAB>();
        ScriptsDataSet<Car>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
