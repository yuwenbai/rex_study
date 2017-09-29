/**
 * @Author GarFey
 *  图片压缩工具
 */

using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;

namespace projectQ
{
	public static class TextureUtility
	{
		public static void LOG(LogType t, string p, params object[] v)
		{
			QLoger.LOG(typeof(AnimatorHand_Main), t, p, v);
		}
		public class ThreadData
 		{
	         public int start;
	         public int end;
	         public ThreadData (int s, int e) {
	             start = s;
	             end = e;
	         }
 	 	}

		private static Color[] texColors;
	    private static Color[] newColors;
	    private static int w;
	    private static float ratioX;
	    private static float ratioY;
	    private static int w2;
	    private static int finishCount;
	    private static Mutex mutex;

		/// <summary>
		/// 压缩Point
		/// </summary>
		/// <param name="tex">Tex.</param>
		/// <param name="newWidth">New width.需要压缩的宽</param>
		/// <param name="newHeight">New height.需要压缩的高</param>
		/// <param name="path">Path.压缩完成后保存的路径</param>
		public static void ScalePoint (Texture2D tex, int newWidth, int newHeight,string path="")
	    {
	        ThreadedScale (tex, newWidth, newHeight, false,path);
		}
		/// <summary>
		/// 压缩Biliner
		/// </summary>
		/// <param name="tex">Tex.</param>
		/// <param name="newWidth">New width.需要压缩的宽</param>
		/// <param name="newHeight">New height.需要压缩的高</param>
		/// <param name="path">Path.压缩完成后保存的路径</param>
		public static void ScaleBilinear (Texture2D tex, int newWidth, int newHeight,string path="")
		{
			ThreadedScale (tex, newWidth, newHeight, true,path);
	    }

		private static void ThreadedScale (Texture2D tex, int newWidth, int newHeight, bool useBilinear,string path = "")
	    {
	         texColors = tex.GetPixels();
	         newColors = new Color[newWidth * newHeight];
	         if (useBilinear)
	         {
	             ratioX = 1.0f / ((float)newWidth / (tex.width-1));
	             ratioY = 1.0f / ((float)newHeight / (tex.height-1));
	         }
	         else 
			 {
	             ratioX = ((float)tex.width) / newWidth;
	             ratioY = ((float)tex.height) / newHeight;
	         }
	         w = tex.width;
	         w2 = newWidth;
	         var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
	         var slice = newHeight/cores;
	         
	         finishCount = 0;
	         if (mutex == null) {
	             mutex = new Mutex(false);
	         }
	         if (cores > 1)
	         {
	             int i = 0;
	             ThreadData threadData;
	             for (i = 0; i < cores-1; i++) {
		                 threadData = new ThreadData(slice * i, slice * (i + 1));
		                 ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
		                 Thread thread = new Thread(ts);
		                 thread.Start(threadData);
		             }
	             threadData = new ThreadData(slice*i, newHeight);
	             if (useBilinear)
		             {
		                 BilinearScale(threadData);
		             }
	             else
		             {
		                PointScale(threadData);
		             }
	             while (finishCount < cores)
		             {
                 		Thread.Sleep(1);
		             }
	         }
	         else
	         {
	             ThreadData threadData = new ThreadData(0, newHeight);
	             if (useBilinear)
		             {
		                 BilinearScale(threadData);
		             }
	             else
		             {
		                 PointScale(threadData);
		             }
	         }
	        
	         tex.Resize(newWidth, newHeight);
	         tex.SetPixels(newColors);
	         tex.Apply();
			if (path != "") {
				byte[] buffer = tex.EncodeToPNG ();
				LOG(LogType.ELog,"path :: "+path);
				File.WriteAllBytes (path, buffer);
			}

		}
		public static void BilinearScale (System.Object obj)
	    {
	         ThreadData threadData = (ThreadData) obj;
	         for (var y = threadData.start; y < threadData.end; y++)
	         {
	             int yFloor = (int)Mathf.Floor(y * ratioY);
	             var y1 = yFloor * w;
	             var y2 = (yFloor+1) * w;
	             var yw = y * w2;
	             
	             for (var x = 0; x < w2; x++) {
	                 int xFloor = (int)Mathf.Floor(x * ratioX);
	                 var xLerp = x * ratioX-xFloor;
	                 newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor+1], xLerp),
	                                                        ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor+1], xLerp),
		                                                        y*ratioY-yFloor);
	             }
	         }
         
	         mutex.WaitOne();
	         finishCount++;
	         mutex.ReleaseMutex();
	    }
	     
	    public static void PointScale (System.Object obj)
	     {
	         ThreadData threadData = (ThreadData) obj;
	         for (var y = threadData.start; y < threadData.end; y++)
	         {
             	 var thisY = (int)(ratioY * y) * w;
	             var yw = y * w2;
	             for (var x = 0; x < w2; x++) {
		                 newColors[yw + x] = texColors[(int)(thisY + ratioX*x)];
		             }
	         }
         
	         mutex.WaitOne();
	         finishCount++;
	         mutex.ReleaseMutex();
	     }
		private static Color ColorLerpUnclamped (Color c1, Color c2, float value)
     	{
	         return new Color (c1.r + (c2.r - c1.r)*value, 
	                           c1.g + (c2.g - c1.g)*value, 
	                           c1.b + (c2.b - c1.b)*value, 
	                           c1.a + (c2.a - c1.a)*value);
 		}
	}
}

