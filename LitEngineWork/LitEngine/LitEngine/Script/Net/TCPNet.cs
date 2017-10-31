using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Net;
namespace LitEngine
{
    namespace NetTool
    {

        public class TCPNet : NetBase
        {
            #region socket属性

            static private TCPNet sInstance = null;
            
            #endregion

            #region 构造析构

            static public TCPNet Instance
            {
                get
                {
                    if (sInstance == null)
                    {
                        GameObject tobj = new GameObject();
                        UnityEngine.Object.DontDestroyOnLoad(tobj);
                        sInstance = tobj.AddComponent<TCPNet>();
                        tobj.name = sInstance.mNetTag + "-Object";
                    }
                    return sInstance;
                }
            }

            public TCPNet():base()
            {
                mNetTag = "TCP";
            }

            override protected void OnDestroy()
            {
                sInstance = null;
                base.OnDestroy();
            }

            #endregion

            #region 建立Socket

            override public void ConnectToServer()
            {
                if (IsCOrD())
                {
                    DLog.LOG(DLogType.Error,mNetTag + "-ConnectToServer->连接中或关闭中，不可进行ConnectToServer操作！");
                    return;
                }
                if (mSocket != null && mSocket.Connected)
                {
                    AddMainThreadMsgReCall(GetMsgReCallData(MSG_RECALL.MSG_RECALL_SOCKET_CONNECT_ERRO, mNetTag + "重复建立连接"));
                    return;
                }
                mConnecting = true;
                Thread tcreatthread = new Thread(ThreatConnect);
                tcreatthread.IsBackground = true;
                tcreatthread.Start();
            }

            virtual protected bool TCPConnect()
            {
                bool ret = false;

                IPTYPE tiptype = GetSelfIPType();
                DLog.LOG(DLogType.Log, "本地网络协议支持:" + tiptype);

                List<IPAddress> tipds = GetServerIpAddress(mHostName);
                if (tipds.Count == 0) DLog.LOG(DLogType.Error, "IPAddress列表为空!");

                foreach (IPAddress tip in tipds)
                {
                    try
                    {
                        DLog.LOG(DLogType.Log, "开始连接:" + " HostName:" + mHostName + "IpAddress: " + tip.ToString() + " AddressFamily:" + tip.AddressFamily.ToString());
                        mSocket = new Socket(tip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        ChoseSocketTimeOutAndBuffer();
                        mSocket.Connect(tip, mPort);
                        DLog.LOG(DLogType.Log, "连接成功!");
                        ret = true;
                        break;
                    }
                    catch (Exception e)
                    {
                        DLog.LOG(DLogType.Error, "网络连接异常,连接信息如下:" + " HostName:" + mHostName + " IP:" + tip.ToString() + " AddressFamily:" + tip.AddressFamily.ToString() + " 错误信息:" + e.ToString());
                    }
                }

                return ret;
            }

            protected void ThreatConnect()
            {

                bool tok = TCPConnect();
                string tmsg= "";
                if (tok)
                {
                    try
                    {
                        mStartThread = true;
                        #region 发送;
                        CreatSend();
                        #endregion

                        #region 接收;
                        CreatRec();
                        #endregion
                        mConnected = true;
                    }
                    catch (Exception e)
                    {
                        tmsg = e.ToString();
                        CloseSRThread();
                        tok = false;
                    }

                }

                mConnecting = false;
                if (!tok)
                    AddMainThreadMsgReCall(GetMsgReCallData(MSG_RECALL.MSG_RECALL_SOCKET_CONNECT_ERRO, mNetTag + tmsg));
                else
                    AddMainThreadMsgReCall(GetMsgReCallData(MSG_RECALL.MSG_RECALL_SOCKET_CONNECT_FINISHED, mNetTag + "建立连接完成"));
            }

            protected void CreatSend()
            {
                mSendThread = new Thread(SendMessageThread);
                mSendThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                mSendThread.IsBackground = true;
                mSendThread.Start();
            }

            protected void CreatRec()
            {
                mRecThread = new Thread(ReceiveMessage);
                mRecThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                mRecThread.IsBackground = true;
                mRecThread.Start();
            }

            #endregion

            #region 发送

            override public void AddSend(SendData _data)
            {
                if(_data == null)
                {
                    DLog.LOG(DLogType.Error, "试图添加一个空对象到发送队列!AddSend");
                    return;
                }
                if (!mStartThread) return;
                mSendDataList.Enqueue(_data);

            }

            #region 线程发送模式
            protected void SendMessageThread()
            {

                try
                {
                    while (mStartThread)
                    {
                        Thread.Sleep(2);
                        if (mSendDataList.Count == 0)
                            continue;
                        SendThread(mSendDataList.Dequeue());
                    }

                }
                catch (Exception e)
                {
                    DLog.LOG(DLogType.Error, mNetTag + ":SendMessageThread->" + e.ToString());
                    CloseSRThread();
                    AddMainThreadMsgReCall(GetMsgReCallData(MSG_RECALL.MSG_RECALL_SOCKET_SENDERRO, mNetTag + "-" + e.ToString()));
                }

            }
            virtual protected void SendThread(SendData _data)
            {
                if (_data == null) return;
                byte[] tbuffer = null;
                int tlen = 0;
                try {
                    tbuffer = _data.GetData();
                    tlen = _data.Len + SocketDataBase.mFirstLen;//len必须在取得data后使用才是正确的
                }
                catch (Exception e)
                {
                    DLog.LOG(DLogType.Error, mNetTag + ":SendThread->" + e.ToString());
                }
                if (tbuffer == null) return ;
                int sendlen = mSocket.Send(tbuffer, tlen, SocketFlags.None);
                DebugMsg(_data.Cmd, tbuffer, 0, tlen, "Send-SendThread");
            }
            #endregion
            #endregion

            #region　接收

            protected void ReceiveMessage()
            {
                try
                {
                    while (mStartThread)
                    {
                        if (mSocket.Available != 0)
                        {
                            int receiveNumber = 0;
                            receiveNumber = mSocket.Receive(mRecbuffer, 0, mReadMaxLen, SocketFlags.None);
                            if (receiveNumber > 0)
                                Processingdata(receiveNumber, mRecbuffer);
                        }
                        Thread.Sleep(1);
                    }

                }
                catch (Exception e)
                {
                    DLog.LOG(DLogType.Error, mNetTag + ":ReceiveMessage->" + e.ToString());
                    CloseSRThread();
                    AddMainThreadMsgReCall(GetMsgReCallData(MSG_RECALL.MSG_RECALL_SOCKET_RECERRO, mNetTag + "-" + e.ToString()));
                }
            }

            override protected void Processingdata(int _len, byte[] _buffer)
            {
                try {
                    DebugMsg(-1, _buffer, 0, _len, "接收-bytes");
                    mBufferData.Push(_buffer, _len);
                    while (mBufferData.IsFullData())
                    {
                        ReceiveData tssdata = mReceiveDataList[mUseIndex];
                        mUseIndex++;
                        mBufferData.SetReceiveData(tssdata);
                        mBufferData.Pop();
                        DebugMsg(tssdata.Cmd, tssdata.Data, 0, tssdata.Len, "接收-ReceiveData");
                    }
                }
                catch (Exception e)
                {
                    DLog.LOG(DLogType.Error, mNetTag + ":Processingdata->" + e.ToString());
                }
                
            }

            #endregion

            protected void Update()
            {
                UpdateReCalledMsg();
                UpdateRecMsg();
            }
        }
    }
}