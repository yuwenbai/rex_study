// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/OutLine1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RimColor("rim color",Color) = (1,1,1,1)
		_RimPower("Rim power",Range(0,10)) = 2
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 NdotV :COLOR;
			};

			float4 _RimColor;
			float  _RimPower;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				float3 V = WorldSpaceViewDir(v.vertex);
				V = mul(unity_WorldToObject, float4(V,1));
				o.NdotV.x = saturate(dot(v.normal, normalize(V)));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.rgb += pow((1 - i.NdotV.x), _RimPower)* _RimColor.rgb;
				return col;
			}
			ENDCG
		}
	}
}
