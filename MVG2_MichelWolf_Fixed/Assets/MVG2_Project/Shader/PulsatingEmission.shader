Shader "Custom/PulsatingEmission" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0


		_EmissionMap("Emission Map", 2D) = "black" {}
		_SinDivider("Sin Divider", Range(0.00001, 20)) = 2.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex, _EmissionMap;

		struct Input {
			float2 uv_MainTex, uv_EmissionMap;
		};

		half _Glossiness;
		half _Metallic, _SinDivider;
		fixed4 _Color;



		// value: Input | from1 & to1 : alte Range | from2 & to2 : neue Range
		// z.B. remap(input, 3, 10, 0, 1) - remaps input (3, 10) zu (0, 1)
		float remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 em = tex2D(_EmissionMap, IN.uv_EmissionMap);
			//float t = _SinTime.w / _SinDivider; //?
			float t = sin(_Time.y / _SinDivider);
			//float t = sin(_Time.y) / _SinDivider;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			t = remap(t, -1, 1, 0, 1);
			o.Emission = lerp(0, em, t);

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
