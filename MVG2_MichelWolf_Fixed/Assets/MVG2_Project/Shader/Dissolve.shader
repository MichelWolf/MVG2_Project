Shader "Custom/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[HDR]_Emission("Emission", Color) = (1,1,1,1)

		_Ramp("Ramp", 2D) = "white" {}	//Ramp Textures IMMER auf Clamp stellen!
		_Cutoff("Cutoff", Range(0, 1)) = 0.0
		_DissolveNoise("Dissolve Noise", 2D) = "white" {}
		_DissolveDirection("Dissolve Direction", 2D) = "white" {}
		_RampPower("Ramp Power", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _Ramp, _DissolveNoise, _DissolveDirection;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DissolveNoise;
			float2 uv_DissolveDirection;
		};

		half _Glossiness;
		half _Metallic;
		half _Cutoff;
		half _RampPower;
		fixed4 _Color;
		fixed4 _Emission;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed noiseTex = tex2D(_DissolveNoise, IN.uv_DissolveNoise).r;
			fixed noiseDir = tex2D(_DissolveDirection, IN.uv_DissolveDirection).r;

			fixed4 e = tex2D(_MainTex, IN.uv_MainTex) * _Emission;

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = e.rgb;
			o.Alpha = min(0.99, noiseTex * noiseDir) - _Cutoff;
			//o.Emission = min(0.99, 1 - (e.rgb * (noiseTex * noiseDir) + 0.01)) - _Cutoff + 0.01;
			e = e * step(o.Alpha, 0.01);
			if (_Cutoff == 0)
			{
				e = e * 0;
			}

			o.Emission = e.rgb;
			//o.Emission = e.rgb * step(o.Alpha, 0.01);
			clip(o.Alpha);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
