using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.XCodeEditor;
using UnityEditor.Callbacks;
using System.IO;
namespace AutoBuild
{
	public class IOSTools
	{

		/// <summary>
		/// Xcode Build ipa 包（带进度条）
		/// </summary>
		/// <param name="scriptPath">脚本路径.</param>
		/// <param name="xcodePath">Xcode项目路径</param>
		public static void XcodebuildIPA(string scriptPath, string xcodePath, string ipaPath) {

			if (!ExistScriptFile (scriptPath)) {
				UnityEngine.Debug.Log ("scriptPath不存在:"+scriptPath);
				return;
			}
				
			if (!ExistXcodePath (xcodePath)) {
				UnityEngine.Debug.Log ("scriptPath不存在:"+scriptPath);
				return;
			}

			System.Diagnostics.Process process = new System.Diagnostics.Process ();

			try {
				System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
				// shell 路径
				psi.FileName = "/bin/sh";
				//psi.UseShellExecute = false;
				psi.UseShellExecute = false;
				psi.RedirectStandardOutput = true;
				//psi.RedirectStanderInput = true;
				psi.CreateNoWindow = false;
				psi.ErrorDialog = true;

				if (xcodePath.Length > 0 && ipaPath.Length >0) {
					psi.Arguments = scriptPath + " " + xcodePath +" " +ipaPath;
				} else {
					psi.Arguments = scriptPath;
				}

				process.StartInfo = psi;
				process.Start();
//				System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
				string strOutput = process.StandardOutput.ReadToEnd(); 
				UnityEngine.Debug.Log ("script LOG:" + strOutput);
//				process.WaitForExit(); 
				process.Close();
			}catch (Exception e){
				UnityEngine.Debug.LogException(e);
				process.Kill();
				process = null;
			}
				
		}

		/// <summary>
		/// Xcode Build ipa 包（带进度条）
		/// </summary>
		/// <param name="scriptPath">脚本路径.</param>
		/// <param name="xcodePath">Xcode项目路径</param>
		public static void XcodebuildIPAWithProcessBar(string scriptPath, string xcodePath,string ipaPath) {

			if (!ExistScriptFile (scriptPath))
				return;
			if (!ExistXcodePath (xcodePath))
				return;
			//		string scriptPath = "/Users/user/Documents/development_v1.2.0.1272/369mahjong_ios/AutoPackageScript/AutoPackageScript.sh";
			//		string xcodePath = "/Users/user/Documents/development_v1.2.0.1272/369mahjong_ios";
			string args = scriptPath + " " + xcodePath + " " + ipaPath;

			System.Diagnostics.Process process = new System.Diagnostics.Process ();
			process.StartInfo.FileName = "/bin/sh";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Arguments = args;
			process.Start ();

			string strRst = "";

			while(strRst != "finished")
			{	
				if(process.StandardOutput.Peek() == -1)
				{
					continue;
				}

				strRst = process.StandardOutput.ReadLine();
				if(EditorUtility.DisplayCancelableProgressBar("xcodebuild", strRst, 1.0f) == true)
				{
					process.Close();
					process = null;
					EditorUtility.ClearProgressBar();

					return ;	
				}
			}

			process.StandardOutput.Close();
			EditorUtility.ClearProgressBar();

			string error = null;
			if(process.StandardError.Peek() != -1)
			{
				error = process.StandardError.ReadToEnd();
			}

			if(error != "" && error != null && error.Contains("Unity4XC.xcplugin") == false)
			{	
				if(EditorUtility.DisplayDialog("错误", error, "好的") == true)
				{
					process.StandardError.Close();
					process.Close();
					process = null;

					return;
				}
			}
			else
			{
				process.StandardError.Close();
			}

			process.Close();
			process = null;
		}





		/// <summary>
		/// 复制OC文件到Xcode项目中
		/// </summary>
		/// <param name="filePath">Xcode项目路径</param>
		public static void CopyOCFileToXcode(string filePath)
		{
			// 删除Unity生成的UnityAppController.m和UnityAppController.h文件
			string appDelegate_h = filePath + "/Classes/UnityAppController.h";
			string appDelegate_m = filePath + "/Classes/UnityAppController.mm";
			if (System.IO.File.Exists(appDelegate_h))
			{
				Debug.Log ("UnityAppController.h文件存在：" + appDelegate_h);
				System.IO.File.Delete(appDelegate_h);
			}
			if (System.IO.File.Exists(appDelegate_m))
			{
				Debug.Log ("UnityAppController.m文件存在：" + appDelegate_m);
				System.IO.File.Delete(appDelegate_m);
			}
			//	PBXGroup group = new PBXGroup ();
			// 替换Unity原生的UnityAppController
			string sourceFile_h = Application.dataPath + @"/Editor/XUPort/Mods/AppDelegate/AppDelegate.h";
			if (System.IO.File.Exists(sourceFile_h))
			{
				Debug.Log ("AppDelegate.h文件存在：" + sourceFile_h);
				//			System.IO.File.Move(sourceFile_h, appDelegate_h);
				System.IO.File.Copy(sourceFile_h, appDelegate_h, true);
			}

			string sourceFile_m = Application.dataPath + @"/Editor/XUPort/Mods/AppDelegate/AppDelegate.m";
			Debug.Log ("AppDelegate.m文件：" + sourceFile_m);
			if (System.IO.File.Exists(sourceFile_m))
			{
				Debug.Log ("AppDelegate.m文件存在：" + sourceFile_h);
				//			System.IO.File.Move(sourceFile_m, appDelegate_m);
				System.IO.File.Copy(sourceFile_m, appDelegate_m, true);
			}

		}

		/// <summary>
		/// 复制自动打包shell脚本到Xcode项目路径下
		/// </summary>
		/// <param name="filePath">File path.</param>
		public static void CopyScriptToXcode(string filePath)
		{
			string toDirect = filePath + "/AutoPackageScript";
			string fromDirect = Application.dataPath + @"/Editor/XUPort/Mods/Setting/iOSAutoBuildScript/AutoPackageScript";
			//		if (System.IO.Directory.Exists (toDirect)) {
			//			Debug.Log ("AutoPackageScript文件夹存在：" + toDirect);
			//			System.IO.Directory.Delete(toDirect);
			//			Debug.Log ("先删除原有文件夹");
			//		}

			if (!System.IO.Directory.Exists(toDirect))
			{
				System.IO.Directory.CreateDirectory(toDirect);
			}

			if (System.IO.Directory.Exists (fromDirect)) {
				string[] files = System.IO.Directory.GetFiles (fromDirect);
				foreach (string file in files) {
					// Use static Path methods to extract only the file name from the path.
					string fileName = System.IO.Path.GetFileName (file);
					string destFile = System.IO.Path.Combine (toDirect, fileName);
					System.IO.File.Copy (file, destFile, true);
				}
				Debug.Log ("脚本源复制成功");
			} else {
				Debug.Log ("脚本源文件不存在");
			}
		}

		/// <summary>
		/// 检查脚本是否存在
		/// </summary>
		/// <returns><c>true</c>, if script file was existed, <c>false</c> otherwise.</returns>
		/// <param name="scriptFile">Script file.</param>
		private static bool ExistScriptFile(string scriptFile) {
			bool exist = false;
			if (System.IO.File.Exists (scriptFile)) {
				exist = true;
			} else {
				Debug.Log ("script don`t exist:"+scriptFile);
			}

			return exist;

		}

		/// <summary>
		/// 检查Xcode项目是否存在
		/// </summary>
		/// <returns><c>true</c>, if xcode path was existed, <c>false</c> otherwise.</returns>
		/// <param name="xcodePath">Xcode path.</param>
		private static bool ExistXcodePath (string xcodePath) {
			bool exist = false;
			if (System.IO.Directory.Exists (xcodePath)) {
				exist = true;
			} else {
				Debug.Log ("Xcode file don`t exist:"+xcodePath);
			}

			return exist;
		}


		/// <summary>
		/// 拷贝Xcode配置文件
		/// </summary>
		/// <param name="isIJM">If set to <c>true</c> is IJ.</param>
		public static void copySettingProjectMod (bool isIJM){
			string toPath = Application.dataPath + "/Editor/XUPort/Mods/Setting.projmods";
			string fromPath;
			if (isIJM) {
				fromPath = Application.dataPath + @"/Editor/XUPort/Setting/Setting_ijiami.setting";
			} else {
				fromPath = Application.dataPath + @"/Editor/XUPort/Setting/Setting.setting";
			}

			System.IO.File.Copy (fromPath, toPath, true);
		}


		/// <summary>
		/// 删除Xcode配置文件
		/// </summary>
		/// <param name="isIJM">If set to <c>true</c> is IJ.</param>
		public static void removeSettingProjectMod (){
			string settingPath = Application.dataPath + "/Editor/XUPort/Mods/Setting.projmods";
			if (File.Exists (settingPath)) {
				System.IO.File.Delete (settingPath);
			}
		}


		/// <summary>
		/// 魔窗脚本路径
		/// </summary>
		/// <returns>The JS path.</returns>
		public static string FingerprintJSPath (){
			
			DirectoryInfo asetsPath = new DirectoryInfo(Application.dataPath);
			string filePath = asetsPath.Parent.FullName +"/MagicWindow.bundle";

			Debug.Log ("filePath:"+filePath);
			return filePath;
		}


//		/// <summary>
//		/// 复制魔窗脚本
//		/// </summary>
//		public static void copyMagicWindow(){
//			string fromPath = Application.dataPath + "/Editor/XUPort/Mods/Xcode/SDK/mwSDK/MagicWindowSDK/MagicWindow.bundle/Contents/Resources/fingerprint";
//			if (System.IO.File.Exists(fromPath) ) {
//				System.IO.File.Copy (fromPath, FingerprintJSPath(), true);
//			} else {
//				Debug.Log ("fingerprint file don`t exist:"+fromPath);
//			}
//		}
//
//		/// <summary>
//		/// 删除魔窗脚本
//		/// </summary>
//		public static void removeMagicWindow(){
//			
//			string filePath = FingerprintJSPath();
//			if (System.IO.File.Exists(filePath) ) {
//				System.IO.File.Delete(filePath);
//			} else {
//				Debug.Log ("fingerprint file don`t exist:"+filePath);
//			}
//		}



		/// <summary>
		/// 复制文件夹（及文件夹下所有子文件夹和文件）
		/// </summary>
		/// <param name="sourcePath">待复制的文件夹路径</param>
		/// <param name="destinationPath">目标路径</param>
		public static void CopyDirectory(String sourcePath, String destinationPath)
		{
			System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(sourcePath);
			System.IO.Directory.CreateDirectory(destinationPath);
			foreach (System.IO.FileSystemInfo fsi in info.GetFileSystemInfos())
			{
				String destName = System.IO.Path.Combine(destinationPath, fsi.Name);

				if (fsi is System.IO.FileInfo)          //如果是文件，复制文件
					System.IO.File.Copy(fsi.FullName, destName);
				else                                    //如果是文件夹，新建文件夹，递归
				{
					System.IO.Directory.CreateDirectory(destName);
					CopyDirectory(fsi.FullName, destName);
				}
			}
		}
			
	}
}

