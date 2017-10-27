using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMono : MonoBehaviour
{
    public GameObject obj;
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        Button btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(onBtnClick);
    }

    private void onBtnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene");
    }

}
