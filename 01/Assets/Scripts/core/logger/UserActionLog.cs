using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;

namespace projectQ {


    public class UserAction {
        public UserAction(string key, string value) {
            this.key = key;
            this.value = value;
            this.timestep = System.DateTime.Now.Millisecond;
        }

        string GUID {
            get {
                return UserActionManager.GUID;
            }

        }

        long UID {
            get {
                return UserActionManager.UID;
            }
        }

        long timestep;

        public string key;
        public string value;


        override
        public string ToString() {
            return string.Format("{0}_{1}_{2}", this.timestep, this.key, this.value);
        }
        public Msg.ClientStatReport logData {
            get {
                var data = new Msg.ClientStatReport();
                data.UserID = this.UID;
                data.ClientKey = Application.platform.ToString() + _R.OSName;

                data.ReportKey = this.key;
                data.ReportContent = this.value;
                return data;
            }
        }
    }

    public class UserActionManager : SingletonTamplate<UserActionManager> {


        public static string SAVE_LOG_FILE = "";
        public static string SAVE_LOG_FILE_PATH = "";

        public const int MAX_CACHE_ACTION_COUNT = 1024;
        System.Threading.Thread _send_thread = null;
        System.Threading.Semaphore _semaphore = null;



        public static void StartSend() {
      
#if __OPEN_USER_ACTION
            UserActionManager.StopSend();

            UserActionManager.Instance.StartThread();
#endif

        }


        void StartThread() {

            end_sending = false;
            _send_thread = new System.Threading.Thread(new System.Threading.ThreadStart(SendLog));
            _semaphore = new System.Threading.Semaphore(0, MAX_CACHE_ACTION_COUNT);
            _send_thread.Priority = System.Threading.ThreadPriority.Lowest;
            _send_thread.Start();
        }

        public static void StopSend() {
#if __OPEN_USER_ACTION
            UserActionManager.Instance.end_sending = true;
            if (UserActionManager.Instance._semaphore != null) {
                UserActionManager.Instance._semaphore.Release(2);
            }
#endif
        }

        public static void Init() {
#if __OPEN_USER_ACTION
            UserActionManager.DSend = UserActionManager.Instance.Send;
#endif
        }

        bool end_sending = false;
        void SendLog() {
#if __OPEN_USER_ACTION
            do {
                if (end_sending) {//发送前判断是否能发
                    break;
                }

                if (DSend == null) {//发送前判断是否有发送代理
                                    //TODO-JEFF 这里不要用Sleep；

                    //QLoger.LOG("-----------------------");
                    //Thread.Sleep(1000);
                    break;
                }

                _semaphore.WaitOne();

                if (end_sending) {
                    break;
                }

                UserAction log = null;
                lock (_queue) {
                    if (_queue.Count > 0) {
                        log = _queue.Dequeue();
                    }
                }

                if (DSend != null && log != null) {
                    try {
                        DSend(log);
                    } catch (System.Exception ex) {
                        //QLoger.ERROR("发送日志错误" + ex.ToString());
                    }
                }

            } while (!end_sending);
            this._semaphore.Close();
            this._semaphore = null;
            this._send_thread = null;
#endif
        }

        Queue<UserAction> _queue = new Queue<UserAction>();

        public static System.Action<UserAction> DSend = null;
        /// <summary>
        /// Add the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void Add(string key, string value = null) {
#if __OPEN_USER_ACTION
            UserActionManager.Instance.add(key, value);
#endif
        }


        public static void AddLocalLog(string type, string value) {

           
#if __OPEN_USER_ACTION
            Add("USER_LOG_LOCAL", string.Format("{0}-{1}", type, value));
            //Debug.Log(type + value);
#endif
        }

        public static void AddLocalTypeLog(string key , string value) {
#if __OPEN_USER_ACTION
            Add("USER_LOG_LOCAL", string.Format("{0}-{1}\n",
                key,value.Replace("\n", "--换行--")) );

            Debug.Log(key + value);
#endif
        }


        /// <summary>
        /// Adds the coding string.
        /// </summary>
        /// <param name="code">Code. etc: othernames[CODE_KEY,CODE_VALUE]</param>
        public static void AddCodingString(string code) {
#if __OPEN_USER_ACTION
            int s = code.LastIndexOf("[");
            int e = code.LastIndexOf("]");
            if (s > 0 && e > 0 && e > s + 1) {
                var key = code.Substring(s + 1, e - s - 1);

                int p = key.IndexOf(",");
                string value = null;
                if (p > 0 && key.Length > p) {
                    value = key.Substring(p + 1);
                    key = key.Substring(0, p);
                }
                //QLoger.ERROR (code + "k:" + code + " v:" + value );

                if (!string.IsNullOrEmpty(key)) {
                    UserActionManager.Add(key, value);
                }
            }
#endif
        }



        public static void KillSend() {
#if __OPEN_USER_ACTION
            if (Instance._send_thread != null) {
                Instance._send_thread.Abort();
                Instance._semaphore.Close();
                Instance._semaphore = null;
                Instance._send_thread = null;
                Instance.end_sending = true;
            }
#endif
        }


        public static string GUID = null;
        public static long UID {
            get {
                return MemoryData.UserID;
            }
        }

        public UserActionManager() {
            string guid = PlayerPrefs.GetString("EACH_GUID_LOCAL");

            if (guid == null) {
                guid = System.Guid.NewGuid().ToString();
                PlayerPrefs.SetString("EACH_GUID_LOCAL", guid);
            }

            SAVE_LOG_FILE_PATH = Application.persistentDataPath + "/logs/";
            SAVE_LOG_FILE = string.Format("log_{1}_{0}.log",
                System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss"),
                UserActionManager.GUID);
            
            //Debug.LogError(SAVE_LOG_FILE_PATH);
            
            //QLoger.ERROR (SAVE_LOG_FILE_PATH);	
        }

        void add(string key, string value) {

            lock (this._queue) {
                if (this._semaphore == null)
                    return;
                this._queue.Enqueue(new UserAction(key, value));
                if (this._queue.Count < MAX_CACHE_ACTION_COUNT) {
                    this._semaphore.Release();
                    //Logger.logic ("增加有效的日志" + this._queue.Count);
                } else {
                    //Logger.logic ("增加NO效的日志"+ this._queue.Count);
                    this._queue.Dequeue();
                }
            }
        }


        public void Send(UserAction action) {

            if ("USER_LOG_LOCAL".Equals(action.key)) {
                this.WriteToFile(action.value);
            } else {
                ModelNetWorker.Instance.ClientStatReport(action);
            }
        }

#if __OPEN_USER_ACTION
        public Queue<string> _write_buffer = new Queue<string>();
#endif

        public void WriteToFile(string text) {
#if __OPEN_USER_ACTION
            try {
                _write_buffer.Enqueue(text);

                if (_write_buffer.Count > 10) {
                    string _path = SAVE_LOG_FILE_PATH;
                    CommonTools.MakeDir(_path);

                    StreamWriter fs = null;

                    if (File.Exists(_path + SAVE_LOG_FILE)) {
                        fs = File.AppendText(_path + SAVE_LOG_FILE);
                    } else {
                        fs = File.CreateText(_path + SAVE_LOG_FILE);
                    }

                   
                    try {
                        do {
                            fs.WriteLine(_write_buffer.Dequeue());
                        } while (_write_buffer.Count > 0);
                    } finally {
                        fs.Close();
                    }
                    Thread.Sleep(25);


                }

            } catch (Exception ex) {
               
            }
#endif
        }
    }

    public partial class ModelNetWorker {

        public void ClientStatReport(UserAction action) {
            this.send(action.logData);
        }

    }

}
