using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
public class SocketClient : MonoBehaviour {
    private Socket tcpClient;
    private string serverIp = "127.0.0.1";
    private int serverPort = 5000;
   public void Start()
    {
        //创建一个socket
        tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse(serverIp);
        EndPoint endPoint = new IPEndPoint(ipAddress, serverPort);
        //建立一个连接请求
        tcpClient.Connect(endPoint);
        Debug.Log("请求服务器成功");
        //接收发送消息
        byte[] data = new byte[1024];
        int length = tcpClient.Receive(data);
        string strMessage = Encoding.UTF8.GetString(data);
        Debug.Log("客户端接收到一个消息: " + strMessage);
        //发送消息到服务端
        string strMessage2 = "client send message to server!";
        tcpClient.Send(Encoding.UTF8.GetBytes(strMessage2));
        Debug.Log("客户端发送消息成功: " + strMessage2);
    }
}
