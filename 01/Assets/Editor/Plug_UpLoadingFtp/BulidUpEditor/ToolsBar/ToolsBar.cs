/**
* @Author GarFey
* 工具条类
*/
using PlugProject;
using System;
using UnityEditor;
using UnityEngine;
namespace BuildUpdateEditor
{
    public class ToolsBar
    {
        private static ToolsBar Inst;
        public static ToolsBar GetInstance
        {
            get { return Inst = Inst == null ? new ToolsBar() : Inst; }
        }

        private string selectLabel = "";
        private string toolBtn = "合并△";
        private bool isToolBtn = true;
        /// <summary>
        /// 当前点击也签
        /// </summary>
        int curSelecetdId;
        int selected = -1;
        string[] selcNames = { "生成上传文件", "检查MD5上传","生成版本 | 版本上传" };
        /// <summary>
        /// 当前选择上传服务器类型
        /// </summary>
        int curSelectedUpSize = -1;
        int selectedUps = 0;
        string[] upType = { "正式Path", "测试Path"};
        int[] sizes = { 0, -1 };

        /// <summary>
        /// 当前选择上传资源类型
        /// </summary>
        int curSelectedAssetsSize = -1;
        int selectedAsets = 0;
        string[] upAssetsType = { "XML", "图片"/*, "AssetBundle"*/ };
        int[] AssetSizes = { 0, 1, 2 };
        /// <summary>
        /// 获取当前选择 build 类型
        /// </summary>
        /// <returns></returns>
        public string GetSelect()
        {
            return selectLabel;
        }

        public void OnGUI()
        {
            using (new VerticalBlock())
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label(selectLabel);
                    isToolBtn = GUILayout.Toggle(isToolBtn, isToolBtn ? "合并△" : "展开▽", "button", GUILayout.ExpandWidth(false));
                }
                if(isToolBtn)
                {
                    using (new HorizontalBlock(GUILayout.Height(20)))
                    {
                        selectedUps = EditorGUILayout.IntPopup("选择上传类型: ", selectedUps, upType, sizes);
                        selectedAsets = EditorGUILayout.IntPopup("选择上传资源类型: ", selectedAsets, upAssetsType, AssetSizes);
                        selectLabel = upType[Array.IndexOf(sizes, selectedUps)] +" | "+upAssetsType[selectedAsets];
                        switch(upType[Array.IndexOf(sizes, selectedUps)])
                        {
                            case "正式Path":
                                seleUpRef();
                                break;
                            case "测试Path":
                                seleUpRef();
                                break;
                        }
                        switch (upAssetsType[selectedAsets])
                        {
                            case "XML":
                                CheckUpdateControl.mUpDateType = UpdateType.Xml;
                                seleAsetsRef();
                                break;
                            case "图片":
                                CheckUpdateControl.mUpDateType = UpdateType.Texture;
                                seleAsetsRef();
                                break;
                            case "AssetBundle":
                                CheckUpdateControl.mUpDateType = UpdateType.AssetBundle;
                                break;
                        }
                    }
                }
                using (new HorizontalBlock())
                {
                    selected = GUILayout.Toolbar(selected, selcNames, "button", GUILayout.Height(45));
                    if(curSelecetdId != selected)
                    {
                        curSelecetdId = selected;
                        switch(curSelecetdId)
                        {
                            case 0:///生成上传文件
                                MakeDirView.GetInstance.Init();
                                ToolsEditorState.toolsEidtorCurState = TEditorState.MakeDir;
                                break;
                            case 1:///检查MD5上传
                                UpLoadingInit.Init(curSelectedUpSize.ToString());
                                break;
                            case 2:///一键打包
                                OneKeyBuildView.GetInstance.Init();
                                OneKeyBuildView.isUnity = true;
                                ToolsEditorState.toolsEidtorCurState = TEditorState.ONEKEYBUILD;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void seleUpRef()
        {
            if (curSelectedUpSize != selectedUps)
            {
                curSelectedUpSize = selectedUps;
                if (curSelecetdId == 1)
                {
                    UpLoadingInit.Init(curSelectedUpSize.ToString());
                }
            }
        }

        private void seleAsetsRef()
        {
            if (curSelectedAssetsSize != selectedAsets)
            {
                curSelectedAssetsSize = selectedAsets;
                if (curSelecetdId == 1)
                {
                    CheckForUpdateView.GetInstance.Init();
                }
            }
        }

        public void OnDestory()
        {
            Inst = null;
        }
    }
}