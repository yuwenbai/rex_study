/**
 * @Author GarFey
 * 本地麻将数据存储类
 *
 */
using System.IO;
using System;
using Msg;
using System.Collections.Generic;

namespace projectQ
{
	public static class FightMahjongData
	{
		public static void LOG (LogType t, string p, params object[] v)
		{
			QLoger.LOG (typeof(AnimatorHand_Main), t, p, v);
		}

		/// <summary>
		/// 麻将信息储存Dic 
		/// </summary>
		private static Dictionary<string, MahjInfo> mahJDic = new Dictionary<string, MahjInfo> ();
		/// <summary>
		/// 数据文件保存路径
		/// </summary>
		private static string DataFilePath = PathHelper.PersistentPath + "/MahJiangData";
		/// <summary>
		/// 用户实际麻将数据存储路径
		/// </summary>
		private static string PlayerDataFilePath;
		private static string Datapath;
		private static string IdexDataPath;

		public enum FileState
		{
			Read,
			Write
		}

		public static FileState fileState = FileState.Read;

		/// <summary>
		/// 反序列化存
		/// </summary>
		/// <param name="prob">Prob.</param>
		/// <param name="key">Key.</param>
		public static void MjSerislize (ProtoBuf.IExtensible prob, string key)
		{
			Serialize (prob, key);
		}

		/// <summary>
		/// 序列化取
		/// </summary>
		/// <returns>The de serialize.</returns>
		/// <param name="key">Key.</param>
		//public static object MjDeSerialize<T>(string key)
		public static object MjDeSerialize (string key)
		{
			return DeSerialize (key);
		}

        /// <summary>
        /// 读取本地文件解析到Dic
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		private static bool ReadLocalData (string path)
		{
			string[] localData;
			StreamReader sr = null;
			try {
				sr = File.OpenText (path);

				string dataStr;
				dataStr = sr.ReadToEnd ();
				LOG (LogType.ELog, "LocalDataInfo :: " + dataStr);
				localData = dataStr.Split (';');
				sr.Close ();
				sr.Dispose ();
				if (localData.Length > 0) {
					int tmpStar;
					int tmpCount;
					string tmpKey;
					for (int i = 0; i < localData.Length; i++) {
						string[] tmpStr = localData [i].Split ('_');
						if (tmpStr.Length >= 3) {
							MahjInfo mInfo = new MahjInfo ();
							tmpKey = tmpStr [0];
							tmpStar = int.Parse (tmpStr [1]);
							tmpCount = int.Parse (tmpStr [2]);
							mInfo.startDex = tmpStar;
							mInfo.dataCount = tmpCount;
							if (mahJDic.ContainsKey (tmpKey)) {
								mahJDic.Remove (tmpKey);
							}
							mahJDic.Add (tmpKey, mInfo);
						}
					}
					return true;
				}
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
				return false;
			}
			return false;
		}

		///<summary> 
		/// 序列化 写
		/// </summary> 
		/// <param name="data">要序列化的对象</param> 
		/// <returns>返回存放序列化后的数据缓冲区</returns> 
		private static void Serialize (ProtoBuf.IExtensible data, string key)
		{
			try {
				fileState = FileState.Write;
				CheckFile ();
				byte[] buff;
				FileStream fs;

				if (File.Exists (Datapath)) {
					fs = new FileStream (Datapath, FileMode.Append);
				} else {
					fs = new FileStream (Datapath, FileMode.Create);
				}

				StreamWriter sw;
				FileInfo Idfs = new FileInfo (IdexDataPath);
				buff = TcpDataHandler.SerializeProtoData (data);
				if (!Idfs.Exists) {
					sw = Idfs.CreateText ();
				} else {
					sw = Idfs.AppendText ();
				}

				fs.Write (buff, 0, buff.Length);
				int totalLen = (int)fs.Length;
				int bufLen = buff.Length;
				fs.Close ();
				MahjInfo mInfo = new MahjInfo ();
				mInfo.startDex = totalLen - bufLen;
				mInfo.dataCount = buff.Length;
				if (mahJDic.ContainsKey (key)) {
					mahJDic.Remove (key);
				}
				mahJDic.Add (key, mInfo);

				sw.Write (key + "_" + mInfo.startDex + "_" + mInfo.dataCount + ";");
				sw.Close ();
				sw.Dispose ();
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
			}
		}

		/// <summary>
		/// 反序列化读
		/// </summary>
		/// <param name="key">Key.</param>
		private static object DeSerialize (string key)
		{
			try {
				fileState = FileState.Read;
				CheckFile ();
				LOG (LogType.ELog, "key::" + key);
				if (mahJDic.Count > 0) {
					if (mahJDic.ContainsKey (key)) {
						MahjInfo mInfo = mahJDic [key];
						LOG (LogType.ELog, "ContainsKey::" + key + "start = " + mInfo.startDex + " || count = " + mInfo.dataCount);
						byte[] byt = File2Bytes (Datapath, mInfo.startDex, mInfo.dataCount);
						object obj = CommonTools.DeSerializeProto<MjBalanceNewNotify> (byt);
						if (obj == null) {
							Clear (true);
							return null;
						}
						return obj;
					}
				} else {
					FileInfo Idfs = new FileInfo (IdexDataPath);
					if (Idfs.Exists) {
						if (ReadLocalData (IdexDataPath)) {
							if (mahJDic.ContainsKey (key)) {
								MahjInfo mInfo = mahJDic [key];
								LOG (LogType.ELog, "ContainsKey::" + key + "start = " + mInfo.startDex + " || count = " + mInfo.dataCount);
								byte[] byt = File2Bytes (Datapath, mInfo.startDex, mInfo.dataCount);
								object obj = CommonTools.DeSerializeProto<MjBalanceNewNotify> (byt);
								if (obj == null) {
									Clear (true);
									return null;
								}
								return obj;
							}
						}
					}
				}
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
			}

			return null;
		}

		/// <summary>  
		/// 将文件转换为byte数组  
		/// </summary>  
		/// <param name="path">文件地址</param>  
		/// <returns>转换后的byte数组</returns>  
		private static byte[] File2Bytes (string path, int startDex, int dateCount)
		{
			if (!File.Exists (path)) {
				return new byte[0];
			}
			try {
				LOG (LogType.ELog, " DATAINFO::" + path + "  startDex::" + startDex + " || dateCount::" + dateCount);
				FileInfo fi = new FileInfo (path);
				byte[] buff = new byte[dateCount];
				FileStream fs = fi.OpenRead ();
				fs.Seek (startDex, SeekOrigin.Current);
				int fsint = fs.Read (buff, 0, dateCount);
				fs.Close ();
				return buff;
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
			}
			return new byte[0];
		}

		/// <summary>
		/// 檢查本地文件是否存在，不存在则创建
		/// </summary>
		public static void CheckFile ()
		{
			try {
				PlayerDataFilePath = DataFilePath + "/" + MemoryData.UserID;
				Datapath = PlayerDataFilePath + "/mahJdata.txt";
				IdexDataPath = PlayerDataFilePath + "/IdexMahJdata.txt";
				//LOG(LogType.ELog, "FightMjDataFilePath ::" + PlayerDataFilePath);
				if (fileState == FileState.Write) {
					if (!Directory.Exists (PlayerDataFilePath)) {
						Directory.CreateDirectory (PlayerDataFilePath);
					}
				} 
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
			}
		}

		/// <summary>
		/// 清理本地战斗缓存数据
		/// </summary>
		/// <param name="bol"> false只清理当前用户的麻将信息 <c>true</c> 清理本地所有用户麻将信息 </param>
		public static void Clear (bool bol = false)
		{
			try {
				DirectoryInfo dir;
				if (bol) {
					dir = new DirectoryInfo (DataFilePath);
				} else {
					dir = new DirectoryInfo (PlayerDataFilePath);
				}

				//返回目录中所有文件和子目录
				FileSystemInfo[] fileinfo = dir.GetFileSystemInfos ();
				foreach (FileSystemInfo i in fileinfo) {
					//判断是否文件夹
					if (i is DirectoryInfo) {
						DirectoryInfo subdir = new DirectoryInfo (i.FullName);
						//删除子目录和文件
						subdir.Delete (true);
					} else {
						//删除指定文件
						File.Delete (i.FullName);
					}
				}
				mahJDic.Clear ();
			} catch (Exception e) {
				LOG (LogType.EError, e.ToString ());
				throw;
			}
		}
	}
}