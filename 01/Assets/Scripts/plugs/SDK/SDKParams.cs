/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class SDKParams  {

    protected string _url;
    public string URL
    {
        get { return _url; }
    }
    public string InsertUrlParams(params object[] urlParams)
    {
        if (urlParams == null || urlParams.Length <= 0)
        {
            return _url;
        }
        try
        {
            _url = string.Format(_url, urlParams);
        }
        catch
        {
            QLoger.ERROR("string.Format error: _url:" + _url);
            QLoger.ERROR("string.Format error: urlParams:" + urlParams.ToString());
        }

        return _url;
    }
}
