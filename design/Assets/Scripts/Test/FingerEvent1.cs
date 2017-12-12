﻿//using UnityEngine;
//using System.Collections;

//public class FingerEvent1 : MonoBehaviour
//{

//    void OnEnable()
//    {
//        //启动时调用，这里开始注册手势操作的事件。

//        //按下事件： OnFingerDown就是按下事件监听的方法，这个名子可以由你来自定义。方法只能在本类中监听。下面所有的事件都一样！！！
//        FingerGestures.OnFingerDown += OnFingerDown;
//        //抬起事件
//        FingerGestures.OnFingerUp += OnFingerUp;
//        //开始拖动事件
//        FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
//        //拖动中事件...
//        FingerGestures.OnFingerDragMove += OnFingerDragMove;
//        //拖动结束事件
//        FingerGestures.OnFingerDragEnd += OnFingerDragEnd;
//        //上、下、左、右、四个方向的手势滑动
//        FingerGestures.OnFingerSwipe += OnFingerSwipe;
//        //连击事件 连续点击事件
//        FingerGestures.OnFingerTap += OnFingerTap;
//        //手指触摸屏幕中事件调用一下三个方法
//        FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;
//        FingerGestures.OnFingerStationary += OnFingerStationary;
//        FingerGestures.OnFingerStationaryEnd += OnFingerStationaryEnd;
//        //长按事件
//        FingerGestures.OnFingerLongPress += OnFingerLongPress;

//    }

//    void OnDisable()
//    {
//        //关闭时调用，这里销毁手势操作的事件
//        //和上面一样
//        FingerGestures.OnFingerDown -= OnFingerDown;
//        FingerGestures.OnFingerUp -= OnFingerUp;
//        FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
//        FingerGestures.OnFingerDragMove -= OnFingerDragMove;
//        FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;
//        FingerGestures.OnFingerSwipe -= OnFingerSwipe;
//        FingerGestures.OnFingerTap -= OnFingerTap;
//        FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;
//        FingerGestures.OnFingerStationary -= OnFingerStationary;
//        FingerGestures.OnFingerStationaryEnd -= OnFingerStationaryEnd;
//        FingerGestures.OnFingerLongPress -= OnFingerLongPress;
//    }

//    //按下时调用
//    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
//    {
//        //int fingerIndex 是手指的ID 第一按下的手指就是 0 第二个按下的手指就是1。。。一次类推。
//        //Vector2 fingerPos 手指按下屏幕中的2D坐标

//        //将2D坐标转换成3D坐标
//        transform.position = GetWorldPos(fingerPos);
//        Debug.Log(" OnFingerDown =" + fingerPos);
//    }

//    //抬起时调用
//    void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
//    {

//        Debug.Log(" OnFingerUp =" + fingerPos);
//    }

//    //开始滑动
//    void OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
//    {
//        Debug.Log("OnFingerDragBegin fingerIndex =" + fingerIndex + " fingerPos =" + fingerPos + "startPos =" + startPos);
//    }
//    //滑动结束
//    void OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
//    {

//        Debug.Log("OnFingerDragEnd fingerIndex =" + fingerIndex + " fingerPos =" + fingerPos);
//    }
//    //滑动中
//    void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
//    {
//        transform.position = GetWorldPos(fingerPos);
//        Debug.Log(" OnFingerDragMove =" + fingerPos);

//    }
//    //上下左右四方方向滑动手势操作
//    void OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
//    {
//        //结果是 Up Down Left Right 四个方向
//        Debug.Log("OnFingerSwipe " + direction + " with finger " + fingerIndex);

//    }

//    //连续按下事件， tapCount就是当前连续按下几次
//    void OnFingerTap(int fingerIndex, Vector2 fingerPos, int tapCount)
//    {

//        Debug.Log("OnFingerTap " + tapCount + " times with finger " + fingerIndex);

//    }

//    //按下事件开始后调用，包括 开始 结束 持续中状态只到下次事件开始！
//    // OnFingerStationary 事件和  OnFingerDragMove 有一个区别。
//    //OnFingerStationary 是手指触摸在屏幕中的事件，而OnFingerDragMove是先触摸一下然后滑动的事件。
//    //如果你需要时时捕获手指触摸屏幕中的事件时 用OnFingerStationary 即可
//    void OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
//    {

//        Debug.Log("OnFingerStationaryBegin " + fingerPos + " times with finger " + fingerIndex);
//    }

//    void OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
//    {

//        Debug.Log("OnFingerStationary " + fingerPos + " times with finger " + fingerIndex);

//    }

//    void OnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
//    {

//        Debug.Log("OnFingerStationaryEnd " + fingerPos + " times with finger " + fingerIndex);
//    }

//    //长按事件
//    void OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
//    {

//        Debug.Log("OnFingerLongPress " + fingerPos);
//    }

//    //把Unity屏幕坐标换算成3D坐标
//    Vector3 GetWorldPos(Vector2 screenPos)
//    {
//        Camera mainCamera = Camera.main;
//        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
//    }
//}