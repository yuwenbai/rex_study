using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Character
{
    private Dictionary<ItemEnum.ItemEnumPart,CharacterPart> mListCharacterPart = new Dictionary<ItemEnum.ItemEnumPart,CharacterPart>();
    private GameObject SkeletonInstance = null;
    private Animation animationController = null;
    public void Create(string skeletonName, List<string> generateList)
    {
        InitBaseSkeleton(skeletonName);
        InitItemPart(generateList);
    }
    void InitBaseSkeleton(string skeletonName)
    {
        SkeletonInstance = GetInstance(skeletonName);
        SkeletonInstance.transform.position = new Vector3(0, -1, -5);
        SkeletonInstance.transform.eulerAngles = new Vector3(0, 180, 0);
    }
    void InitItemPart(List<string> generateList)
    {
        mListCharacterPart.Clear();
        foreach (var sName in generateList)
        {
            //Debug.Log("rextest Character Create List Out :" + sName);
        }

        for (int i = (int)ItemEnum.ItemEnumPart.ItemEnumPart_Base; i < (int)ItemEnum.ItemEnumPart.ItemEnumPart_Limit; ++i)
        {
            string sName = generateList[i];
            if (false == string.IsNullOrEmpty(sName))
            {
                ChangePart((ItemEnum.ItemEnumPart)i, sName);
            }
        }
    }
    GameObject GetInstance(string resName)
    {
        var obj = Resources.Load("Model/CharacterPrefab/" + resName);
        return GameObject.Instantiate(obj) as GameObject;
    }

    public void ChangePart(ItemEnum.ItemEnumPart part, string name)
    {
        CharacterPart cp;
        if (mListCharacterPart.TryGetValue(part, out cp))
        {
            cp.ChangeObj(name);
        }
        else
        {
            cp = new CharacterPart(part, name);
            mListCharacterPart[part] = cp;
        }
        UpdateSkeleton();
        // Only for display
        animationController = SkeletonInstance.GetComponent<Animation>();
        PlayStand();
    }
    public void PlayStand()
    {
        animationController.wrapMode = WrapMode.Loop;
        animationController.Play("breath");
    }
    void UpdateSkeleton()
    {
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(SkeletonInstance.GetComponentsInChildren<Transform>(true));

        List<Transform> bones = new List<Transform>();//the list of bones
        List<Material> materials = new List<Material>();//the list of materials
        List<CombineInstance> combineInstances = new List<CombineInstance>();//the list of meshes

        //collect bones 
        foreach (var cpart in mListCharacterPart)
        {
            SkinnedMeshRenderer smr = cpart.Value.Obj.GetComponentInChildren<SkinnedMeshRenderer>();
            // Collect materials
            materials.AddRange(smr.materials); 
              // Collect meshes
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add(ci);
            }
            for (int bone = 0; bone < smr.bones.Length ; ++bone)
            {
                for (int tBase = 0; tBase < transforms.Count; tBase++)
                {
                    if (smr.bones[bone].name.Equals(transforms[tBase].name))
                    {
                        bones.Add(transforms[tBase]);
                        break;
                    }
                }
            }
        }

        // Create a new SkinnedMeshRenderer
        SkinnedMeshRenderer oldSKinned = SkeletonInstance.GetComponent<SkinnedMeshRenderer>();
        if (oldSKinned != null)
        {

            GameObject.DestroyImmediate(oldSKinned);
        }
        SkinnedMeshRenderer r = SkeletonInstance.AddComponent<SkinnedMeshRenderer>();
        //r.materials = materials.ToArray();
        Material newMaterial = new Material(Shader.Find("Mobile/Diffuse"));
        List<Vector2[]> oldUV = new List<Vector2[]>();
        List<Texture2D> Textures = new List<Texture2D>();
        string COMBINE_DIFFUSE_TEXTURE = "_MainTex";
        for (int i = 0; i < materials.Count; i++)
        {
            Textures.Add(materials[i].GetTexture(COMBINE_DIFFUSE_TEXTURE) as Texture2D);
        }
        Texture2D newDiffuseTex = new Texture2D(512, 512,TextureFormat.RGBA32,true);
        Rect[] uvs = newDiffuseTex.PackTextures(Textures.ToArray(), 0);
        newMaterial.mainTexture = newDiffuseTex;

        // reset uv
        Vector2[] uva, uvb;
        for (int j = 0; j < combineInstances.Count; ++j)
        {
            uva = (Vector2[])combineInstances[j].mesh.uv;
            uvb = new Vector2[uva.Length];
            for (int k = 0; k < uva.Length; k++)
            {
                uvb[k] = new Vector2((uva[k].x * uvs[j].width) + uvs[j].x, (uva[k].y * uvs[j].height) + uvs[j].y);
            }
            oldUV.Add(combineInstances[j].mesh.uv);
            combineInstances[j].mesh.uv = uvb;
        }

        r.material = newMaterial;
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, false);

        //r.sharedMesh = new Mesh();
        //r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
        //r.materials = materials.ToArray();


        r.bones = bones.ToArray();// Use new bones
    }
}
