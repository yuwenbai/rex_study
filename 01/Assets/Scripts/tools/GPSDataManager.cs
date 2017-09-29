///**
// * @Author YQC
// *
// *
// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace projectQ
//{
//    public class GPSData
//    {

//        /// <summary>
//        /// 地址
//        /// </summary>
//        public string Address;
//        /// <summary>
//        /// 海拔高度
//        /// </summary>
//        public float Altitude;

//        /// <summary>
//        /// 水平精度
//        /// </summary>
//        public float HorizontalAccuracy;

//        /// <summary>
//        /// 垂直精度
//        /// </summary>
//        public float VerticalAccuracy;

//        /// <summary>
//        /// 纬度
//        /// </summary>
//        public float _latitude;
//        public float Latitude
//        {
//            set { _latitude = value; }
//            get
//            {
//                if (_latitude <= 0 || _longitude <= 0)
//                {
//                    return 0;
//                }
//                return _latitude;
//            }
//        }

//        /// <summary>
//        /// 经度
//        /// </summary>
//        public float _longitude;
//        public float Longitude
//        {
//            set { _longitude = value; }
//            get
//            {
//                if (_latitude <= 0 || _longitude <= 0)
//                {
//                    return 0;
//                }
//                return _longitude;
//            }
//        }

//        /// <summary>
//        /// 最近一次定位的时间戳，从 1970年开始
//        /// </summary>
//        public double Timestamp;
//        public string locType;
//        public string status;


//        public GPSData()
//        {
//            this.Altitude = 0;
//            this.HorizontalAccuracy = 0;
//            this.VerticalAccuracy = 0;
//            this.Latitude = 0;
//            this.Longitude = 0;
//            this.Timestamp = 0;


//            DebugPro.Log(DebugPro.EnumLog.MemoryData, "GPS数据 空", "维度", Latitude, "精度", Longitude);
//        }

//        public GPSData(LocationInfo gpsInfo)
//        {
//            this.Altitude = gpsInfo.altitude;
//            this.HorizontalAccuracy = gpsInfo.horizontalAccuracy;
//            this.VerticalAccuracy = gpsInfo.verticalAccuracy;
//            this.Latitude = gpsInfo.latitude;
//            this.Longitude = gpsInfo.longitude;
//            this.Timestamp = gpsInfo.timestamp;
//            DebugPro.Log(DebugPro.EnumLog.MemoryData, "GPS数据", "维度", Latitude, "精度", Longitude);
//        }

//        public bool IsEmpty()
//        {
//            return this.Latitude == 0 || this.Longitude == 0;
//        }

//        public bool Equals(GPSData other)
//        {
//            if (this.Latitude == other.Latitude && this.Longitude == other.Longitude)
//                return true;

//            //判断距离
//            double dist = CommonTools.Distance(this.Longitude, this.Latitude, other.Longitude, other.Latitude);
//            //DebugPro.Log(DebugPro.EnumLog.System, "GPS相互距离",dist);
//            return dist <= 100d;
//        }
//        public string GetLocTypeStr()
//        {
//            switch(this.locType)
//            {
//                case "0":
//                    return "定位失败";
//                case "1":
//                    return "GPS定位结果";
//                case "2":
//                    return "前次定位结果";
//                case "4":
//                    return "缓存定位结果";
//                case "5":
//                    return "Wifi定位结果";
//                case "6":
//                    return "基站定位结果";
//                case "7":
//                    return "离线定位结果";
//                default:
//                    return "没有找到响应码" + this.locType;
//            }
//        }
//        public override string ToString()
//        {
//            return ToStringLine();
//        }
//        public string ToStringLine(bool isLine = false)
//        {
//            var sb = new System.Text.StringBuilder();
//            sb.Append(" locType").Append(this.GetLocTypeStr());
//            if (isLine) sb.AppendLine();
//            sb.Append(" 地址").Append(this.Address);
//            if(isLine) sb.AppendLine();
//            sb.Append(" 纬度").Append(this.Latitude);
//            if (isLine) sb.AppendLine();
//            sb.Append(" 经度").Append(this.Longitude);
//            if (isLine) sb.AppendLine();
//            sb.Append(" 时间戳").Append(this.Timestamp);
//            if (isLine) sb.AppendLine();
//            sb.Append(" status").Append(this.status);
//            return sb.ToString();
//        }
//    }
//    public class GPSDataManager : SingletonTamplate<GPSDataManager>
//    {
//        /// <summary>
//        /// 最多保存几条GPS数据
//        /// </summary>
//        public int MaxSaveGPSDataCount = 120;

//        //GPS数据List
//        private List<GPSData> GpsDataList = new List<GPSData>();
//        //public List<GPSData> GpsDataList
//        //{
//        //    get { return _gpsDataList; }
//        //}

//        #region API
//        public void Init()
//        {
//            GameDelegateCache.Instance.InvokeMethodEvent += UpdataNoticeGPSData;
//        }

//        /// <summary>
//        /// 取得最新定位信息
//        /// </summary>
//        /// <returns></returns>
//        public GPSData GetNewGpsData()
//        {

//            string info = "GPS 个数" + GpsDataList.Count;
//            System.Text.StringBuilder sb = new System.Text.StringBuilder("GetNewGpsData Count=");
//            sb.Append(GpsDataList.Count).AppendLine();
//            for (int i = 0; i < GpsDataList.Count; i++)
//            {
//                sb.Append("IDX=").Append(i)
//                    .Append(" _latitude=").Append(GpsDataList[i]._latitude)
//                    .Append(" _longitude=").Append(GpsDataList[i]._longitude)
//                    .AppendLine();

//            }


//            GPSData data = new GPSData();

//            if (GpsDataList.Count > 5)
//            {
//                sb.Append(" >get5");
//                for (int i = 1; i < 5; i++)
//                {

//                    //存入GPS信息的数据小于0的时候不计算
//                    if (GpsDataList[GpsDataList.Count - i].Latitude <= 0 ||
//                        GpsDataList[GpsDataList.Count - i].Longitude <= 0)
//                    {
//                        continue;
//                    }

//                    if (data.Latitude <= 0 || data.Longitude <= 0)
//                    {
//                        data.Latitude = GpsDataList[GpsDataList.Count - i].Latitude;
//                        data.Longitude = GpsDataList[GpsDataList.Count - i].Longitude;
//                    }

//                    float dtemp = GetGPSDistance(
//                            GpsDataList[GpsDataList.Count - i].Latitude, GpsDataList[GpsDataList.Count - i].Longitude,
//                            GpsDataList[GpsDataList.Count - i - 1].Latitude, GpsDataList[GpsDataList.Count - i - 1].Longitude);

//                    double time = Mathf.Abs((float)(GpsDataList[GpsDataList.Count - i].Timestamp -
//                        GpsDataList[GpsDataList.Count - i - 1].Timestamp));
//                    sb.Append(i + ">DISTENS" + (dtemp / time) + "\t");

//                    if (dtemp / time < MAX_POINT_DISTANCE)
//                    {
//                        data.Longitude = (GpsDataList[GpsDataList.Count - i].Longitude + data.Longitude) / 2;
//                        data.Latitude = (GpsDataList[GpsDataList.Count - i].Latitude + data.Latitude) / 2;
//                        data.Timestamp++;
//                    }
//                }
//            }
//            else if (GpsDataList.Count > 0)
//            {
//                sb.Append(" > ");
//                for (int i = 0; i < GpsDataList.Count; i++)
//                {
//                    data = GpsDataList[i];
//                    sb.Append(" >GET" + i);

//                    if (data.Latitude > 0 && data.Longitude > 0)
//                    {
//                        break;
//                    }
//                }
//            }


//            UserActionManager.AddLocalTypeLog("Log1", "[C0FF3E]GET GPS 纬度:" + data.Latitude + ":" + " 经度:" + data.Longitude + "时间戳" + data.Timestamp + "\n (" + sb.ToString() + ")[-]");

//            return data;
//        }

///// <summary>
///// 返回所有GPS数据
///// </summary>
///// <returns></returns>
//public List<GPSData> GetAllGpsData()
//{
//    return GpsDataList;
//}

//        /// <summary>
//        /// 清空所有GPS数据
//        /// </summary>
//        public void ClearAllGpsData()
//        {
//            this.GpsDataList.Clear();
//        }

//        /// <summary>
//        /// 是否有新数据
//        /// </summary>
//        /// <returns></returns>
//        public bool IsHaveNewData()
//        {
//            if (this.GpsDataList.Count >= 2)
//            {
//                if (GpsDataList[GpsDataList.Count - 1] == GpsDataList[GpsDataList.Count - 2])
//                    return false;

//                return !GpsDataList[GpsDataList.Count - 1].Equals(GpsDataList[GpsDataList.Count - 2]);
//            }
//            return false;
//        }

//        #endregion

//        #region GPS数据管理
//        /// <summary>
//        /// 添加GPS数据
//        /// </summary>
//        /// <param name="gpsInfo"></param>
//        public bool AddGPSData(GPSData data)
//        {
//            if (data.Latitude == 0 || data.Longitude == 0) return false;

//            DisposalGPSData();
//            GpsDataList.Add(data);
//            if(GpsDataList.Count == 1)
//            {
//                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGPS_DataUpdate_Rsp, true);
//            }
//            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]添加 GPS 纬度:" + data.Latitude + ":" + " 经度:" + data.Longitude + "时间戳" + data.Timestamp + "[-]");
//            if (GpsDataList.Count >= 2)
//            {
//                var gps1 = GpsDataList[GpsDataList.Count - 1];
//                var gps2 = GpsDataList[GpsDataList.Count - 2];
//                if (gps1 != null && gps2 != null)
//                {
//                    return !gps1.Equals(gps2);
//                }
//            }
//            return true;
//        }

//        /// <summary>
//        /// 整理数据
//        /// </summary>
//        private void DisposalGPSData()
//        {
//            if (MaxSaveGPSDataCount > 0 && this.GpsDataList.Count > MaxSaveGPSDataCount)
//            {
//                this.GpsDataList.RemoveRange(0, this.GpsDataList.Count - MaxSaveGPSDataCount);
//                UserActionManager.AddLocalTypeLog("Log1", "[FF0000]GPS 删除超出个数" + (this.GpsDataList.Count - MaxSaveGPSDataCount) + "剩余个数" + GpsDataList.Count + "[-]");
//            }
//        }
//        #endregion

//        #region GPS数据通知消息
//        /// <summary>
//        /// 上次通知GPS数据时间
//        /// </summary>
//        float NoticeLastTime = 0f;
//        /// <summary>
//        /// 通知GPS数据时间间隔
//        /// </summary>
//        float NoticeTimeInterval = 30f;

//        /// <summary>
//        /// 更新GPS数据
//        /// </summary>
//        private void UpdataNoticeGPSData()
//        {
//            if (Time.realtimeSinceStartup - NoticeLastTime > NoticeTimeInterval)
//            {
//                NoticeLastTime = Time.realtimeSinceStartup;
//#if UNITY_ANDROID
//                AndroidManager.Instance.UnityCallExchange("GetGPSServerData", "");
//#elif UNITY_IOS
//                IOSManager.Instance.startUpdateLocation();
//#endif
//                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGPS_DataUpdate_Rsp, IsHaveNewData());
//            }
//        }
//#endregion

//#region 聂风加的
//        private const float EARTH_RADIUS = 6378137; //地球半径 米
//        private static float MAX_POINT_DISTANCE = 50; //每一次采样的最大理论出错距离 米
//        private static float Rad(float d)
//        {
//            return (float)d * Mathf.PI / 180f;
//        }

//        private static float GetGPSDistance(float lat1, float lng1, float lat2, float lng2)
//        {
//            float radLat1 = Rad(lat1);
//            float radLng1 = Rad(lng1);
//            float radLat2 = Rad(lat2);
//            float radLng2 = Rad(lng2);
//            float a = radLat1 - radLat2;
//            float b = radLng1 - radLng2;
//            float result = 2 * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin(a / 2), 2) + Mathf.Cos(radLat1) * Mathf.Cos(radLat2) * Mathf.Pow(Mathf.Sin(b / 2f), 2f))) * EARTH_RADIUS;
//            return result;
//        }
//#endregion

//    }
//}