Shader "Custom/AlphaTest" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_TransVal("Transparency Value", Range(0,1)) = 0.5
	}
		SubShader{
			//Tags { "RenderType"="Opaque" }
				Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
			LOD 200

			CGPROGRAM


			//// Physically based Standard lighting model, and enable shadows on all light types
			//#pragma surface surf Standard fullforwardshadows

			//// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			//sampler2D _MainTex;

			//struct Input {
			//	float2 uv_MainTex;
			//};

			//half _Glossiness;
			//half _Metallic;
			//fixed4 _Color;

			//void surf (Input IN, inout SurfaceOutputStandard o) {
			//	// Albedo comes from a texture tinted by color
			//	fixed4 c = tex2D (_MainTex, IN.uv_MainTex)  *_Color;
			//	//clip(1);
			//	//c.r = 1;
			//	//c.g = 1;
			//	//c.b = 1;
			//	o.Albedo = c.rgb;
			//	// Metallic and smoothness come from slider variables
			//	o.Metallic = _Metallic;
			//	o.Smoothness = _Glossiness;
			//	o.Occlusion = 1;
			//	//o.Emission = fixed3(1, 0,0);
			//	//o.Alpha = 0.5;
			//	//o.r = 1;
			//	//o.g = 1;
			//	//o.b = 1;
			//}

	#pragma surface surf Lambert alpha

			// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0

			sampler2D _MainTex;
		float _TransVal;
			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutput o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) /* *_Color*/;
				//clip(1);
				//c.r = 1;
				//c.g = 1;
				//c.b = 1;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				//o.Occlusion = 1;
				//o.Emission = fixed3(1, 0,0);
				o.Alpha = 1;
				//o.r = 1;
				//o.g = 1;
				//o.b = 1;
			}

			ENDCG
		}
			FallBack "Diffuse"
}
