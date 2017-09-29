/**
 * @Author GarFey
 * 
 */
using System;
using UnityEngine;
namespace BuildUpdateEditor
{
    /// <summary>
    /// 竖列组
    /// </summary>
    public class VerticalBlock : IDisposable
    {
        public VerticalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public VerticalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }
    /// <summary>
    /// 滑动组
    /// </summary>
    public class ScrollviewBlock : IDisposable
    {
        public ScrollviewBlock(ref Vector2 scrollPos, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, options);
        }

        public void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }
    /// <summary>
    /// 横列组
    /// </summary>
    public class HorizontalBlock : IDisposable
    {
        public HorizontalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public HorizontalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }
    /// <summary>
    /// 空格
    /// </summary>
    public class wordWrapped:IDisposable
    {
        public wordWrapped(params GUILayoutOption[] options)
        {
            GUILayout.Label("", options);
        }
        public void Dispose()
        {

        }
    }
}



