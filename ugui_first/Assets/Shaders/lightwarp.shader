Shader "Custom/lightwarp" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Ramp

		//	half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten) {
		//	half NdotL = dot(s.Normal, lightDir);
		//	half diff = NdotL * 0.5 + 0.5;
		//	half4 c;
		//	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
		//	c.a = s.Alpha;
		//	return c;
		//}
		sampler2D _Ramp;
		half4 LightingRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize(lightDir + viewDir);

			half diff = max(0, dot(s.Normal, lightDir));

			float nh = max(0, dot(s.Normal, h));
			float spec = pow(nh, 48.0);

			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
			c.a = s.Alpha;
			return c;
		}

		//half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten) {
		//	half NdotL = dot(s.Normal, lightDir);
		//	half diff = NdotL * 0.5 + 0.5;
		//	half3 ramp = tex2D(_Ramp, float2(diff, diff)).rgb;
		//	half4 c;
		//	c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
		//	c.a = s.Alpha;
		//	return c;
		//}
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)/* * _Color*/;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
