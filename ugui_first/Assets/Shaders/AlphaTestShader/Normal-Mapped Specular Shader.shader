﻿Shader "Custom/Normal-Mapped Specular Shader" {
	Properties {
		_Diffuse("Base (RGB) Specular Amount (A)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump"{}
		_SpecIntensity("Specular Width", Range(0.01, 1)) = 0.5
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Diffuse;

		struct Input {
			half2 uv_Diffuse;
		};
		sampler2D _NormalMap;
		fixed _SpecIntensity;
		fixed _Glossiness;
		fixed _Metallic;
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_Diffuse, IN.uv_Diffuse);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_Diffuse));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
