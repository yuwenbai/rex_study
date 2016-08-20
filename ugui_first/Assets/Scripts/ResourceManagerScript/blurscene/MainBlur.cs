using UnityEngine;
using System.Collections;

public class MainBlur : MonoBehaviour {

    // Use this for initialization
    public Material mat;
    void Start () {
       //  mat = new Material(Shader.Find("blur.shader"));
        //mat = new Material("alpha_laserShot_fx");
	}

    
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //if (mat != null)
        //{
        //    Graphics.Blit(src, dest, mat);
        //}
        
    }
    // Update is called once per frame
    void Update () {
	
	}
}
