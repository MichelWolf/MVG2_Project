Shader "Custom/Alpha" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		// rim light -> äußere Kanten leuchten
		[HDR] _RimColor("Rim Color", Color) = (0,0,0,1)
		_RimPower("Rim Power", float) = 1.0

		_OverlayTex("Overlay Texure", 2D) = "black" {}
		_XScrollSpeed("X Scroll Speed", float) = 0.0
		_YScrollSpeed("Y Scroll Speed", float) = 0.0

	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200

		Pass
		{
			// keine inneren Faces mehr anzeigen
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		sampler2D _MainTex, _OverlayTex;

		struct Input {
			float2 uv_MainTex, uv_OverlayTex;
			float3 viewDir;
		};

		half _Glossiness, _RimPower, _Metallic, _XScrollSpeed, _YScrollSpeed;
		fixed4 _Color, _RimColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;


			float2 ovlUV = IN.uv_OverlayTex;
			ovlUV += float2(_XScrollSpeed * _SinTime.z, _YScrollSpeed * _Time.y);
			fixed4 ovl = tex2D(_OverlayTex, ovlUV);

			half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
			rim = pow(rim, _RimPower);

			o.Albedo = c.rgb;
			o.Emission = (_RimColor + ovl) * rim;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
