using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BaseNode
{
    protected bool mIsDestoryed = false;
    public BaseNode()
    {

    }
    ~BaseNode()
    {
        DisposeC(false);
    }
    virtual public void Dispose()
    {
        if (mIsDestoryed) return;
        DisposeC(true);
    }
    virtual public void DisposeC(bool bClear)
    {
        if (mIsDestoryed) return;
        //TODO:Clean opt
        mIsDestoryed = true;
    }
    //TODO: Update
    virtual public void Update()
    {

    }

}
