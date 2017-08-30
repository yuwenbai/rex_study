using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    //用于存储绘制三角形的顶点坐标  
    private Vector3[] vertices;
    //用于记录绘制三角形所需要的顶点ID顺序  
    private int[] triangles;
    // Use this for initialization
    private List<Vector3> list;

    //定义Mesh  
    private Mesh mesh;

    //记录顶点数  
    private int count = 0;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        list = new List<Vector3>();
        GetComponent<MeshRenderer>().material.color = Color.green;

        //选择Mesh中的Shader  
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Shaders/OutLine1");

        mesh.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //点击鼠标左键  
        if (Input.GetMouseButton(0))
        {
            //顶点数+1  
            count++;
            //将获得的鼠标坐标转换为世界坐标，然后添加到list链表中。  
            list.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.8f)));

        }

        //如果顶点数>=3，那么就开始渲染Mesh  
        if (count >= 3)
        {
            //根据顶点数来计算绘制出三角形的所以顶点数  
            triangles = new int[3 * (count - 2)];
            //根据顶点数来创建记录顶点坐标  
            vertices = new Vector3[count];
            //将链表中的顶点坐标赋值给vertices  
            for (int i = 0; i < count; i++)
            {
                vertices[i] = list[i];

            }

            //三角形个数  
            int triangles_count = count - 2;
            //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）  
            for (int i = 0; i < triangles_count; i++)
            {
                //这个算法好好琢磨一下吧~  
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = i + 2;
                triangles[3 * i + 2] = i + 1;
            }
            //设置顶点坐标  
            mesh.vertices = vertices;
            //设置顶点索引  
            mesh.triangles = triangles;
        }

    }
}
