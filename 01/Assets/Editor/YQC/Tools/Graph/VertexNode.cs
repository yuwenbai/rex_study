/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/11/2016
 *	文件名：  VertexNode.cs
 *	文件功能描述：
 *  创建标识：yqc.5/11/2016
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/
using System.Collections;
using System.Collections.Generic;
using System;

public class EdgeNode : IEquatable<EdgeNode> ,IEqualityComparer<EdgeNode>

{
    public VertexNode node;
    public EdgeNode(VertexNode node)
    {
        this.node = node;
    }

    public bool Equals(EdgeNode x, EdgeNode y)
    {
        if (object.ReferenceEquals(x, y)) return true;
        return x != null && y != null && x.node.data.Equals(y.node.data);
    }

    public int GetHashCode(EdgeNode obj)
    {
        return node.GetHashCode();
    }

    public bool Equals(EdgeNode other)
    {
        return this.node == other.node
            || this.node.data == other.node.data
            || ((string)this.node.data).Equals((string)other.node.data);
    }
}
public class VertexNode
{
    //数据
    public object data = null;
    public bool isTrim = false;
    //入边
    public List<EdgeNode> inEdgeList = new List<EdgeNode>();
    //出边
    public List<EdgeNode> outEdgeList = new List<EdgeNode>();
}

public class GraphAdjList <T> where T:VertexNode {
    public List<T> adjList;
    //边数
    public int numEdges;
}