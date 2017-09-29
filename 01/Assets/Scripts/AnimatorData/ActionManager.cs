using UnityEngine;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour
{

    private bool mPlay = false;
    private static ActionManager _instance;
    public static ActionManager Instance
    {
        get { return _instance; }
    }
    Queue<BaseAction> ActionList = new Queue<BaseAction>();
    public void UpdateAction(float delta)
    {
        if (ActionList.Count <= 0)
            return;

        if (mPlay == false)
            return;

        BaseAction ba = ActionList.Peek();
        if (ba.IsStop())
        {
            ActionList.Dequeue();
            if (ActionList.Count <= 0)
            {
                ActionList.Clear();
                return;
            }
            ba = ActionList.Peek();
        }
        else
        {
            ba.Step(delta);
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        UpdateAction(Time.deltaTime);
    }
    public void AddAction(BaseAction action)
    {
        ActionList.Enqueue(action);
    }
    public void Play()
    {
        mPlay = true;
    }
    public void StopAll()
    {
        mPlay = false;
        ActionList.Clear();
    }
}