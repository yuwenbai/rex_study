// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/test"
{
	//Properties
	//{
	//	_MainTex ("Texture", 2D) = "white" {}
	//	_ColorA("Refraction color1", Color) = (.34, .85, .92, 1) // 颜色
	//    _ColorB("Refraction color2", Color) = (.34, .85, .92, 1) // 颜色
	//	_ValueA("Color Param A", Range(0, 1)) = 1
	//}
	//SubShader
	//{
	//	Tags { "RenderType"="Opaque" }
	//	LOD 100

	//	Pass
	//	{
	//		CGPROGRAM
	//		#pragma vertex vert
	//		#pragma fragment frag
	//		// make fog work
	//		#pragma multi_compile_fog

	//		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON 
	//		
	//		#include "UnityCG.cginc"

	//		struct appdata
	//		{
	//			float4 vertex : POSITION;
	//			float2 uv : TEXCOORD0;
	//		};

	//		struct v2f
	//		{
	//			float2 uv : TEXCOORD0;
	//			UNITY_FOG_COORDS(1)
	//			float4 vertex : SV_POSITION;
	//		};

	//		sampler2D _MainTex;
	//		float4 _MainTex_ST;
	//		fixed4 _ColorA;
	//		fixed4 _ColorB;
	//		float  _ValueA;
	//		v2f vert (appdata v)
	//		{
	//			v2f o;
	//			o.vertex = UnityObjectToClipPos(v.vertex);
	//			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	//			UNITY_TRANSFER_FOG(o,o.vertex);
	//			return o;
	//		}
	//		
	//		fixed4 frag (v2f i) : SV_Target
	//		{
	//			// sample the texture
	//			fixed4 col = tex2D(_MainTex, i.uv);
	//			col.r = _ColorA.r + _ValueA;
	//			//col.r = _ColorA.r + _ColorB.r;
	//			//col.r = _ColorA.r * _ColorB.r;
	//			// apply fog
	//			UNITY_APPLY_FOG(i.fogCoord, col);
	//			return col;
	//		}
	//		ENDCG
	//	}
	//}
	Properties{
	   _MainTex("maintex ",2D)= "write" {}
	//_ValueA("Color Param A", Range(0, 1)) = 1
	}
		SubShader{
		Tags {"RenderType"="Opaque"}
		LOD 100
		Pass {
			CGPROGRAM 
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#include "UnityCG.cginc"
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord  : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};
			struct v2f {
				float4 vertex:POSITION;
				float2 texcoord:TEXCOORD0;
				#ifndef LIGHTMAP_OFF  
				half2 uvLM : TEXCOORD1;
				#endif   
				UNITY_FOG_COORDS(1)

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
#ifndef LIGHTMAP_OFF  
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;

			}
			fixed4 frag(v2f i):SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.texcoord);
				UNITY_APPLY_FOG(i.fogCoord, col);
				UNITY_OPAQUE_ALPHA(col.a);
#ifndef LIGHTMAP_OFF  
				fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
				col.rgb *= lm;
#endif  
				return col;
			}
				ENDCG
		}
	}
}
