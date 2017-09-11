// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SimpleUnlitShader"
{
	Properties
	{
		// Color property for material inspector, default to white
		_MainTex("Texture", 2D) = "white" {}
		_PulpColor("Pulp color", Color) = (1,1,1,1)
		_TotalThickness("Total thickness", float) = 10
		_SkinColor("Skin color", Color) = (1,1,1,1)
		_SkinThickness("Skin thickness", float) = 1
		_RindColor("Rind color", Color) = (1,1,1,1)
		_RindThickness("Rind thickness", float) = 1
	}
	
	SubShader
	{
		Tags{ 
			"RenderType" = "Opaque" 
			"LightMode" = "ForwardBase"
		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0

			//struct appdata
   //         {
   //             float4 vertex : POSITION;
   //             float2 uv : TEXCOORD0;
   //         };

			struct v2f
            {
                float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vpos : TEXTOORD1;
				fixed4 diff : COLOR0; // diffuse lighting color
            };

			// color from the material
			fixed4 _PulpColor;
			fixed4 _RindColor;
			fixed4 _SkinColor;
			
			float _TotalThickness;
			float _SkinThickness;
			float _RindThickness;

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			// vertex shader
			// this time instead of using "appdata" struct, just spell inputs manually,
			// and instead of returning v2f struct, also just return a single output
			// float4 clip position
			v2f vert(appdata_base v) 
			{
				v2f o;
				
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				o.diff = nl * _LightColor0;
				o.diff.rgb += ShadeSH9(half4(worldNormal, 1));
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vpos = v.vertex;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.texcoord;
				return o;
			}


			// pixel shader, no inputs needed
			fixed4 frag(v2f i) : SV_Target
			{
				//float3 localPos = i.pos - unity_ObjectToWorld[1];// mul(unity_WorldToObject, i.pos).xyz;
				//float dist2 = length(i.vpos);
				//float4 objectOrigin = mul(unity_WorldToObject, float4(0.0,0.0,0.0,1.0));
				float4 texColor = tex2D(_MainTex, i.uv);
				 
				float dist = clamp(0, 1,distance(float3(0,0,0), i.vpos));
				//float t = (_TotalThickness - _SkinThickness + dist) * 0.1;
				
				
				//return fixed4(dist2, 0, 0, 0);
				//return tex2D(_MainTex, i.uv);
				//return lerp(_PulpColor, _SkinColor, t);

				return tex2D(_MainTex, float2(dist * 0.99, 0.5)) * i.diff;
			}
			ENDCG
		}
	}
}
