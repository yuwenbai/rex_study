using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
public class ButtunManager : MonoBehaviour
{

    public Text errorPopMessage;
    public InputField username;
    public InputField passward;
    public void OnButtonClick()
    {
        string strusername = this.username.text;
        string strpassward = this.passward.text;
        if ("admin" == strusername && "admin" == strpassward)
        {
            Debug.LogFormat("Botton was clicked strusername !!! {0,1}", strusername, strpassward);
        }
        else
        {
            errorPopMessage.gameObject.SetActive(true);
            errorPopMessage.text = "用户名称或者密码错误，请重新输入!";
            StartCoroutine(HideErrorPopMessageTimer());
        }
    }
    IEnumerator HideErrorPopMessageTimer()
    {
        yield return new WaitForSeconds(2);
        errorPopMessage.gameObject.SetActive(false);
    }
}
