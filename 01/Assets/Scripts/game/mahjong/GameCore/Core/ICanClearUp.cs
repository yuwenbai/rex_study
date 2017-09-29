/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICanReset
{
      void Reset();
}
public interface ICanClearUp  {
     void ClearUp();
}
public interface ICanReconect
{
    void OnReconnect(object obj);
}
