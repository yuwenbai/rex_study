
using System.Collections;
using System.Collections.Generic;

using System;

using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace projectQ {
    [System.Obsolete("这个需要在下一个版本使用异步Socket")]
    public class NetWork {

        static public void IsSuportIpv6() {

            if (Socket.OSSupportsIPv6) {

            }
        }

        public static ManualResetEvent connectDone = new ManualResetEvent(false);

        public static void DoBeginConnect1(string host, int port) {

            host = "fe80::d8d:13c6:8ea:1c32%22";
            port = 8000;


            // Connect asynchronously to the specifed host.
            TcpClient t = new TcpClient(AddressFamily.InterNetworkV6);
            //            IPAddress remoteHost = new IPAddress(host);
            IPAddress[] remoteHost = Dns.GetHostAddresses(host);

            connectDone.Reset();

            t.BeginConnect(remoteHost[0], port,
                new AsyncCallback(ConnectCallback), t);

            // Wait here until the callback processes the connection.
            connectDone.WaitOne();
        }

        static void ConnectCallback(IAsyncResult ar) {

            TcpClient client = ar.AsyncState as TcpClient;


            QLoger.LOG("已经链接到服务器");
            connectDone.Set();
        }

    }
}