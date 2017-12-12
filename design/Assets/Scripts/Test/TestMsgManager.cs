using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//TestMsgManager
/// <summary>
/// 消息Enum 与 proto对象 对应
/// </summary>
public  class TcpDataHandler
{

    static private Dictionary<int, Type> ms_protobufTypes
    = new Dictionary<int, Type>();
    public static void Init()
    {
        AddType(1000, typeof(int));
        AddType(1001, typeof(int));
    }
    public static void AddType(int no, Type protoGo)
    {
        ms_protobufTypes.Add(no, protoGo);
    }
}
public class LoginSceneHandle : TcpDataHandler
{
    public new static void Init()
    {
        AddType(1003, typeof(int));
        AddType(1004, typeof(int));
    }
}
