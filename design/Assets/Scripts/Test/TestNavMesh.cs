using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour {

    private NavMeshAgent agent;
    // Use this for initialization
    void Start () {
        //获取组件  
        agent = GetComponent<NavMeshAgent>();
        //if (TargetObject != null)
        //{
        //    GetComponent<NavMeshAgent>().destination = TargetObject.position;
        //}
    }
	
	// Update is called once per frame
	void Update () {
        //鼠标左键点击  
        if (Input.GetMouseButtonDown(0))
        {
            //摄像机到点击位置的的射线  
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //判断点击的是否地形  
                if (!hit.collider.name.Equals("Plane"))
                {
                    return;
                }
                //点击位置坐标  
                Vector3 point = hit.point;
                //转向  
                transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
                //设置寻路的目标点  
                agent.SetDestination(point);
            }
        }
    }
}
