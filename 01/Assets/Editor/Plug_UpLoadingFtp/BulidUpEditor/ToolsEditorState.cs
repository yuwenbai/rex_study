/**
* @Author GarFey
* Editor 状态类
*/
namespace BuildUpdateEditor
{
    /// <summary>
    /// toolsEditor 状态类
    /// </summary>
    public enum TEditorState
    {
        /// <summary>
        /// 无状态
        /// </summary>
        NULL,
        /// <summary>
        /// 生成文件
        /// </summary>
        MakeDir,
        /// <summary>
        /// 檢查更新
        /// </summary>
        CHECKUPDATE,
        /// <summary>
        /// 一键出包
        /// </summary>
        ONEKEYBUILD
    }

    public class ToolsEditorState
    {
        /// <summary>
        /// toolsEditor 当前状态 默认是NULL
        /// </summary>
        public static TEditorState toolsEidtorCurState = TEditorState.NULL;
    }

    public enum TBuildState
    {
        /// <summary>
        /// 空状态
        /// </summary>
        NULL,
        /// <summary>
        /// 打包Android
        /// </summary>
        BuildAndroid,
        /// <summary>
        /// 打包Ios
        /// </summary>
        BuildIos,
        /// <summary>
        /// 打包Windows
        /// </summary>
        BuildWindows,
        /// <summary>
        /// 上传包
        /// </summary>
        UpBuildBag
    }

    public class BuildUpPlatformState
    { 
        /// <summary>
        /// 打包上传 当前状态 默认 Null
        /// </summary>
        public static TBuildState buildPlatformCurState = TBuildState.NULL;
    }
}
