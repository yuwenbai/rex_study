Shader "Custom/CubemapShader" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}

	_MainTint("Diffuse Tint", Color) = (1,1,1,1)
	_Cubemap("CubeMap", CUBE) = ""{}
	_ReflAmount("Reflection Amount", Range(0.01, 1)) = 0.5
	_MaskTex("Reflection Mask (RGB)",2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		//sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldRefl;
		};

		sampler2D _MainTex;
		sampler2D _MaskTex;
		samplerCUBE _Cubemap;
		float4 _MainTint;
		float _ReflAmount;

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainTint;
			
			float4 maskcol = tex2D(_MaskTex, IN.uv_MainTex);
			float3 cEmission = texCUBE(_Cubemap, IN.worldRefl).rgb * _ReflAmount;
			o.Emission = maskcol.r*cEmission;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
