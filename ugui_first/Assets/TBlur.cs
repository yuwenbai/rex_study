using UnityEngine;
using System.Collections;

public class TBlur : MonoBehaviour {

	public Texture mask;

	private Material matBlur = null;
	private Material matComb = null;

	// Use this for initialization
	void Start () {
		matBlur = new Material(Shader.Find("Custom/TBlur"));
		matComb = new Material(Shader.Find("Custom/TComb"));
		matComb.SetTexture("_MaskTex", mask);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		int hw = source.width / 4;
		int hh = source.height / 4;

		RenderTexture buffer = RenderTexture.GetTemporary(hw, hh, 0);
		RTT(source, buffer, 1.0f);

		for (int i = 0; i < 3; i++) {
			RenderTexture buffer2 = RenderTexture.GetTemporary(hw, hh, 0);
			RTT(buffer, buffer2, 1.0f + i * 0.5f);
			RenderTexture.ReleaseTemporary(buffer);
			buffer = buffer2;
		}

		matComb.SetTexture("_SrcTex", source);
		Graphics.Blit(buffer, destination, matComb);

		RenderTexture.ReleaseTemporary(buffer);
	}

	private void RTT(RenderTexture source, RenderTexture dest, float offset) {
		Graphics.BlitMultiTap(source, dest, matBlur, 
		                      	new Vector2(-offset, -offset),
		                      	new Vector2(-offset,  offset),
		                        new Vector2( offset, -offset),
		                      	new Vector2( offset,  offset)
		                      );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
