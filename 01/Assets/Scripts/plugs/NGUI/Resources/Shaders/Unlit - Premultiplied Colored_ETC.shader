Shader "Unlit/Premultiplied Colored_ETC"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_AlphaTex ("Alpha (A)", 2D) = "black" {}
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			AlphaTest Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
		    //Blend SrcAlpha OneMinusSrcAlpha
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color : COLOR;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 result = tex2D(_MainTex, i.texcoord);
				half4 alptex = tex2D(_AlphaTex,i.texcoord);
				result = result * i.color;				
				result.a = alptex * result.a;
				return result;
			}
			ENDCG
		}
	}

}
