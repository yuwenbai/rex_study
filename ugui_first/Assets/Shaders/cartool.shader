Shader "Custom/Bump" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Tooniness("Tooniness", Range(0.1,20)) = 4
	_BumpMap("Albedo (RGB)", 2D) = "bump" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert finalcolor:final  

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _BumpMap;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	float _Tooniness;

	void surf(Input IN, inout SurfaceOutput  o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex)/* * _Color*/;
		o.Albedo = c.rgb;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		// Metallic and smoothness come from slider variables
		//o.Metallic = _Metallic;
		//o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	void final(Input IN, SurfaceOutput o, inout fixed4 color) {
		color = floor(color * _Tooniness) / _Tooniness;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
