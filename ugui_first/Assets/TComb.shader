Shader "Custom/TComb" {
	Properties {
		_SrcTex ("SrcTex", 2D) = "white" {}
		_MainTex ("MainTex", 2D) = "white" {}
		_MaskTex ("MaskTex", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_img v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				float4 uv;
				uv.xy = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				uv.zw = 0;
				o.uv = uv;
				return o;
			}
			
			sampler2D _SrcTex;
			sampler2D _MainTex;
			sampler2D _MaskTex;
			
			fixed4 frag(v2f i) : COLOR {
				fixed4 c1 = tex2D(_SrcTex, i.uv.xy);
				fixed4 c2 = tex2D(_MainTex, i.uv.xy);
				fixed4 c3 = tex2D(_MaskTex, i.uv.xy);	
				return lerp(c2, c1, c3);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
