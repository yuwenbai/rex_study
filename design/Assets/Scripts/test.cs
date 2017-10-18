using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    //test variable resources
    //material
    Material mat;
    //prefab gameobject
    GameObject player;
    //audoclip
    AudioClip adi;
    //sprite
    Sprite spt;
    //texture
    Texture tex;
	// Use this for initialization
	void Start () {


        player =  ResourceLoader.Instance.Load<GameObject>("Mesh/Cube");
        Instantiate(player);

        //Resources.LoadAll()
        //mat = Resources.Load<Material>("Material/Unlit_OutLine1");
        //GetComponent<MeshRenderer>().material = mat;
        //GetComponent<MeshRenderer>().sharedMaterial = mat;


        //player = Resources.Load<GameObject>("Mesh/Cube");



        //adi = Resources.Load<AudioClip>("Audio/adi");

        //AudioSource adiSource = gameObject.AddComponent<AudioSource>();
        //adiSource.clip = adi;
        //adiSource.playOnAwake = true;
        //adiSource.loop = true;
        //adiSource.Play();

        //spt = Resources.Load<Sprite>("Sprite/icon-1024");

        //tex = Resources.Load<Texture>("Texture/icon-1024");


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
