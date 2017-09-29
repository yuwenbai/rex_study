/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
/// <summary>
/// 主要是对传入的参数进行判空
/// </summary>
public class NullHelper
{
    public static bool IsObjectIsNull(object obj)
    {
        if (obj == null)
        {
            DebugPro.DebugWarning("obj is null");
            return true;
        }
        return false;
    }
    public static bool IsListNullOrEmpty(List<object> list)
    {
        if (list == null || list.Count <= 0)
        {
            DebugPro.LogError("list is null");
            return true;
        }
        return false;
    }
    public static bool IsListNullOrEmpty(List<int> list)
    {
        if (list == null || list.Count <= 0)
        {
            DebugPro.LogError("list is null");
            return true;
        }
        return false;
    }
    public static bool IsListNullOrEmpty(object[] array)
    {
        if (array == null || array.Length <= 0)
        {
            DebugPro.LogError("array is null");
            return true;
        }
        return false;
    }
    public static bool IsInvalidIndex(int index, List<MahJongCard> list)
    {
        if (NullHelper.IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
            return true;
        }
        return false;
    }
    public static bool IsInvalidIndex(int index, List<Transform> list)
    {
        if (NullHelper.IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
            return true;
        }
        return false;
    }
    public static bool IsInvalidIndex(int index, List<int> list)
    {
        if (NullHelper.IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
            return true;
        }
        return false;
    }
    public static bool IsInvalidIndex(int index, int[] list)
    {
        if (NullHelper.IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Length)
        {
            return true;
        }

        return false;
    }

    public static bool IsInvalidIndex(int index, object[] list)
    {
        if (NullHelper.IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Length)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 判定元素数组长度是否与传递进来的数量陪陪
    /// </summary>
    /// <returns></returns>
    public static bool IsInvalidMap(int count, object[] vas)
    {
        bool mapValue = false;
        if (vas != null && vas.Length == count)
        {
            mapValue = true;
        }

        return mapValue;
    }

}
public class GameObjectHelper
{
    public static bool SetParentAndEnableObj(GameObject srcObj, Transform parent, bool active)
    {
        if (NullHelper.IsObjectIsNull(srcObj))
        {
            return false;
        }
        srcObj.transform.parent = parent;
        NormalizationTransform(srcObj.transform);
        SetEnable(srcObj, active);
        return true;
    }
    public static void SetEnable(GameObject obj, bool enable)
    {
        if (obj)
        {
            if (obj.activeSelf != enable)
            {
                obj.SetActive(enable);
            }
        }
    }
    public static void NormalizationTransform(Transform transform)
    {
        if (transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
public class ListHelper
{
    public static List<int> CopyValueToNewList(List<int> src)
    {
        List<int> result = new List<int>();
        if (NullHelper.IsObjectIsNull(src))
        {
            return result;
        }
        for (int i = 0; i < src.Count; i++)
        {
            result.Add(src[i]);
        }
        return result;
    }
    public static List<int> InverseValueToNewList(List<int> src)
    {
        List<int> result = new List<int>();
        if (NullHelper.IsObjectIsNull(src))
        {
            return result;
        }
        for (int i = src.Count - 1; i >= 0; i--)
        {
            result.Add(src[i]);
        }
        return result;
    }
    public static bool IsTwoListSameValue(List<int> left, List<int> right)
    {
        if (NullHelper.IsObjectIsNull(left) || NullHelper.IsObjectIsNull(right))
        {
            return false;
        }
        if (left.Count != right.Count)
        {
            return false;
        }
        for (int i = 0; i < left.Count; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }
        return true;
    }
}
