using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TestApp2
{
    string teset2appstr = "testapp2测试字符串";
    public TestApp2()
    {

    }

    public void testCall()
    {
        DLog.LOG(DLogType.Log, teset2appstr + "--------------");
    }
}

