Shader "Hidden/DiffusBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	_BackTex("Alpha Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BackTex;
			fixed4 frag (v2f i) : SV_Target
			{
			//	SetTexture[_MainTex] //the default Texture  
			//{
			//	Combine texture
			//}
			//	SetTexture[_BackTex] //use the combine lerp to mix two texture by the Main color's Alpha  
			//{
			//	constantColor[_Color]
			//	Combine previous lerp(constant) texture
			//}
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
