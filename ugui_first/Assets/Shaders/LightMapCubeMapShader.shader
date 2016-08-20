Shader "Custom/LightMapCubeMapShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//_MaskTex("Reflection Mask (RGB)",2D) = "white" {}
		_NormalMap("normalmap  (RGB)",2D) = "white" {}
		_Cubemap("CubeMap", CUBE) = ""{}
		_ReflAmount("Reflection Amount", Range(0.01, 1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		samplerCUBE _Cubemap;
		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _MaskTex;
		float4 _MainTint;
		float _ReflAmount;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 worldRefl;
			INTERNAL_DATA
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			float3 normals = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap)).rgb;
			o.Normal = normals;
			//float4 maskcol = tex2D(_MaskTex, IN.uv_MainTex);
			float3 cEmission = texCUBE(_Cubemap, WorldReflectionVector(IN, o.Normal)).rgb * _ReflAmount;
			//o.Emission = maskcol.r*cEmission;
			o.Emission = cEmission;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
