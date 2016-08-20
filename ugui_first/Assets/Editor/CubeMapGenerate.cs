using UnityEngine;
using System.Collections;
using UnityEditor;
public class CubeMapGenerate : ScriptableWizard
{

    public Transform renderPosition;
    public Cubemap cubemap;
    [MenuItem("CookBook/Render Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard("Render CubeMap", typeof(CubeMapGenerate), "Render!");
    }
    // Use this for initialization
    void OnWizardCreate()
    {
        GameObject go = new GameObject("CubeCam", typeof(Camera));

        go.transform.position = renderPosition.position;
        go.transform.rotation = Quaternion.identity;

        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        DestroyImmediate(go);
    }

    void OnWizardUpdate()
    {
        helpString = "Select transform to render" +
            " from and cubemap to render into";
        if (renderPosition != null && cubemap != null)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }
}
