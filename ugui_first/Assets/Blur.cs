using UnityEngine;
using System.Collections;

public class Blur : MonoBehaviour {

	/// <summary>
	/// 迭代次数
	/// </summary>
	public int iterations = 3;

	public float blurSpread = 0.6f;

	public Texture mask;

	/// <summary>
	/// 模糊材质球
	/// </summary>
	static Material m_BlurMaterial = null;

	/// <summary>
	/// 输出材质球
	/// </summary>
	static Material m_finalMaterial = null;

	protected Material blurmaterial {
		get {
			if (m_BlurMaterial == null) {
				m_BlurMaterial = new Material(Shader.Find("wk/blur"));
				m_BlurMaterial.hideFlags = HideFlags.DontSave;
			}
			return m_BlurMaterial;
		}
	}

	protected Material finalmaterial{
		get {
			if (m_finalMaterial == null) {
				m_finalMaterial = new Material(Shader.Find("wk/finalblur"));
				m_finalMaterial.SetTexture("_MaskTex", mask);
				m_finalMaterial.hideFlags = HideFlags.DontSave;
			}
			return m_finalMaterial;
		}
	}

	void Start()
	{
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		if (!blurmaterial || !blurmaterial.shader.isSupported) {
			enabled = false;
			return;
		}
		if (!finalmaterial || !finalmaterial.shader.isSupported) {
			enabled = false;
			return;
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		int rtW = source.width/4;
		int rtH = source.height/4;
		RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
		
		DownSample4x (source, buffer);
		
		for(int i = 0; i < iterations; i++)
		{
			RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
			FourTapCone (buffer, buffer2, i);
			RenderTexture.ReleaseTemporary(buffer);
			buffer = buffer2;
		}
		finalmaterial.SetTexture ("_SrcTex", source);
		Graphics.Blit(buffer, destination, finalmaterial);
		RenderTexture.ReleaseTemporary(buffer);
	}

	/// <summary>
	/// Fours the tap cone.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="dest">Destination.</param>
	/// <param name="iteration">Iteration.</param>
	public void FourTapCone (RenderTexture source, RenderTexture dest, int iteration)
	{
		float off = 0.5f + iteration * blurSpread;
		Graphics.BlitMultiTap (source, dest, blurmaterial,
		                       new Vector2(-off, -off),
		                       new Vector2(-off,  off),
		                       new Vector2( off,  off),
		                       new Vector2( off, -off)
		                       );
	}

	/// <summary>
	/// 4分采样
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="dest">Destination.</param>
	private void DownSample4x (RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		Graphics.BlitMultiTap (source, dest, blurmaterial,
		                       new Vector2(-off, -off),
		                       new Vector2(-off,  off),
		                       new Vector2( off,  off),
		                       new Vector2( off, -off)
		                       );
	}
}
