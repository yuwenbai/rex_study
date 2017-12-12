using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Node : BaseNode
{
    protected Node ParentNode;
    public List<Node> mChildList
    {
        get;
        set;
    }
    public Node()
    {
        mChildList = new List<Node>();
    }
    override public void Dispose()
    {
        if (mIsDestoryed) return;

        if (mChildList == null)
            for (int i = mChildList.Count - 1; i >= 0; i--)
                mChildList[i].Dispose();

        base.Dispose();
    }
    public void AddChild(Node node)
    {
        if (NullHelper.IsObjectIsNull(node))
            return;
        node.ParentNode = this;
        mChildList.Add(node);
    }
    public void RemoveChild(Node node, bool bClear)
    {
        if (NullHelper.IsObjectIsNull(node))
            return;
        node.ParentNode = null;
        mChildList.Remove(node);
        if (bClear)
            node.Dispose();

    }
    public void RemoveFromParent(bool bClear)
    {
        if (ParentNode == null) return;
        ParentNode.RemoveChild(this, bClear);
    }
    override public void Update()
    {
        base.Update();

        UpdateChildren();
    }
    virtual protected void UpdateChildren()
    {
        if (mChildList == null) return;
        for (int i = mChildList.Count - 1; i >= 0; i--)
            mChildList[i].Update();
    }
}
