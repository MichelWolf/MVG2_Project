Shader "Custom/Triplanar" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_Tiling ("Tiling", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex, _BumpMap;
		//float4 _MainTex_ST;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos, wNormal;
		};

		half _Glossiness, _Tiling;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.wNormal = UnityObjectToWorldNormal(v.normal);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed3 blend = normalize(abs(IN.wNormal));
			blend = saturate(pow(blend, 2));
			
			float2 tiling = float2(_Tiling, _Tiling);

			float2 uvX = IN.worldPos.zy * tiling;
			float2 uvY = IN.worldPos.xz * tiling;
			float2 uvZ = IN.worldPos.xy * tiling;

			fixed4 texX = tex2D (_MainTex, uvX) * blend.x;
			fixed4 texY = tex2D (_MainTex, uvY) * blend.y;
			fixed4 texZ = tex2D (_MainTex, uvZ) * blend.z;

			fixed4 norX = tex2D (_BumpMap, uvX) * blend.x;
			fixed4 norY = tex2D (_BumpMap, uvY) * blend.y;
			fixed4 norZ = tex2D (_BumpMap, uvZ) * blend.z;

			fixed4 c = saturate(texX + texY + texZ);
			fixed3 n = UnpackNormal(norX + norY + norZ);


			o.Albedo = c;
			o.Normal = n;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
