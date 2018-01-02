using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;

public class Test : MonoBehaviour {

	bool isDone;
	Slider slider;
	Text text;
	float progress = 0f;

    public string[] MsgsN =  { "您在{0}麻将[CC9900]{1}[-]排名第[CC3300]{2}[-]名，恭喜您！请继续保持！"
                                  ,"您在{0}麻将[CC9900]{1}[-]排名第[CC3300]{2}[-]名，大奖就在眼前，冲锋！"
                                  ,"您在{0}麻将[CC9900]{1}[-]排名第[CC3300]{2}[-]名，发起最后冲刺，向大奖挺进！"
                                  ,"您在{0}麻将[CC9900]{1}[-]排名第[CC3300]{2}[-]名, 距离大奖还有1毫米！"
                                  ,"您在{0}麻将[CC9900]{1}[-]排名第[CC3300]{2}[-]名, 请继续努力，向大奖挺进！"
                                  ,""
                                  ,""
                                  ,""};

    void Awake()
    {
        //slider = GameObject.Find("Slider").GetComponent<Slider>();
        //text = GameObject.Find("Text").GetComponent<Text>();
        int n = MsgsN.Length;
        string aaa = MsgsN[0];
        int b = n;
        string bbb = aaa;
    }

    HttpDownLoad http;
    //string url = @"http://nj02all01.baidupcs.com/file/430d880872a1df2c585fdc5d2e1792f7?bkt=p3-00002196e1874bd1f274c739d3812e1223d6&fid=790124421-250528-1052161399548620&time=1453345864&sign=FDTAXGERLBH-DCb740ccc5511e5e8fedcff06b081203-EfEvaTITEN88hc7NwREKd3I5MXs%3D&to=nj2hb&fm=Nan,B,G,ny&sta_dx=14&sta_cs=0&sta_ft=test&sta_ct=0&fm2=Nanjing02,B,G,ny&newver=1&newfm=1&secfm=1&flow_ver=3&pkey=00002196e1874bd1f274c739d3812e1223d6&sl=76480590&expires=8h&rt=sh&r=476311478&mlogid=474664903050570371&vuk=790124421&vbdid=3229687100&fin=test&slt=pm&uta=0&rtype=1&iv=0&isw=0&dp-logid=474664903050570371&dp-callid=0.1.1";
    string url = "http://192.168.221.48/ResPub/MahjongAssetsTest/rt416505.rt416505";
    string savePath;
	
	void Start () {
		savePath = Application.streamingAssetsPath;
		http = new HttpDownLoad();
		http.DownLoad(url, savePath, LoadLevel);
	}

	void OnDisable()
	{
		print ("OnDisable");
		http.Close();
	}

	void LoadLevel()
	{
		isDone = true;
	}

	void Update()
	{

		//slider.value = http.progress;
		//text.text = "资源加载中" + (slider.value * 100).ToString("0.00") + "%"; 
		//if(isDone)
		//{
		//	isDone = false;
		//	string url = @"file://" + Application.streamingAssetsPath + "/test";
		//	StartCoroutine(LoadScene(url));
		//}
	}

	IEnumerator LoadScene(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		AssetBundle ab = www.assetBundle;
		SceneManager.LoadScene("Demo2_towers");

	}

}
