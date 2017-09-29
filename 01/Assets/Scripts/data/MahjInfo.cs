using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ {
	public class MahjInfo 
	{
		/// <summary>
		/// 开始位置
		/// </summary>
		private int StartDex;
		/// <summary>
		/// 数据长度
		/// </summary>
		private int DataCount;
		public int startDex
		{
			get{ return StartDex;}
			set{StartDex = value;}
		}
		public int dataCount
		{
			get{ return DataCount;}
			set{ DataCount = value;}
		}
	}
}
