using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NullHelper
{
    public static bool IsObjectIsNull(Object obj)
    {
        return obj == null ? true : false;
    }
    public static bool IsListNullOrEmpty(List<object> list)
    {
        if (list == null || list.Count <= 0)
        {
            return true;
        }
        return false;
    }
    public static bool IsArrayNullOrEmpty(object[] array)
    {
        if (array == null || array.Length <= 0)
        {
            return true;
        }
        return false;
    }
    public static bool IsInvalidIndex(int index, List<Object> list)
    {
        if (IsObjectIsNull(list))
        {
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
            return true;
        }
        return false;
    }
}
