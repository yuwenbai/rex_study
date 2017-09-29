using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using projectQ;

public class MainScene : SceneBase
{
    public MainScene()
    {
        StateName = "Main";
    }
    public override void StateBegin()
    {
        //closeYuyin
        EventDispatcher.FireEvent(GEnum.NamedEvent.EYuyinState, false);
        base.StateBegin();
        MemoryData.GameStateData.IsMahjongGameIn = false;
        MemoryData.GameStateData.IsLoadMahjongScene = false;
}

    public override void StateEnd()
    {
        base.StateEnd();
        _R.ui.ClearAll();
    }
}