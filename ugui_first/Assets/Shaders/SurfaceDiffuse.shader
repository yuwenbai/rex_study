Shader "Custom/SurfaceDiffuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissiveColor("Emissive Color", Color) = (1,1,1,1)
		_AmbientColor("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue("This is a Slider", Range(0, 10)) = 2.5
		// Add two properties  
		_ScrollXSpeed("X Scroll Speed", Range(0, 10)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0, 10)) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Lambert
		#pragma surface surf BasicDiffuse
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		half4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = max(0, dot(s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
			//col.rgb = float3(1, 1, 0);
			col.a = s.Alpha;
			return col;
		}
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

	
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
		void surf(Input IN, inout SurfaceOutput o) {
			fixed2 scrolledUV = IN.uv_MainTex;

			fixed xScrollValue = _ScrollXSpeed * _Time.y;
			fixed yScrollValue = _ScrollYSpeed * _Time.y;

			scrolledUV += fixed2(xScrollValue, yScrollValue);

			half4 c = tex2D(_MainTex, scrolledUV);
			o.Albedo = c.rgb;
			//o.Albedo = fixed3(xScrollValue, yScrollValue,1);
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
