using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class HeroineStateManager
{
    private HeroineState mState;

    public void SetState(HeroineState state)
    {
        if (mState !=null )
        {
            mState.leave();
        }
        mState = state;
    }
    public void HandleInput(Input input)
    {
        if (mState != null)
        {
            mState.Init();
        }
    }
}
