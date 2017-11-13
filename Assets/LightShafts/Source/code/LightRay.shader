﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LightShafts/LightShaftAdditive" {
Properties {

	_ShaftFading ("_Shaft Fading", Range(0.0,1.0)) = 1.0
	_Intensity ("Shaft Intensity", Range(0.01,255.0)) = 1.0
	_ShaftColor ("Tint Color", Color) = (0.5,0.5,0.5,255)
	_ColorWarmt ("extra Color", Color) = (0.5,0.5,0.5,255)
	_SampleColor("Do not edit this",Color)= (255,255,255,255)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0

	
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }


	SubShader {

		Pass {

			Blend SrcColor One
			ColorMask RGB
			Cull Off
			Lighting Off
			ZWrite Off


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			
			float _ShaftFading;
			float _Intensity;
			fixed4 _ShaftColor;
			fixed4 _ColorWarmt;
			fixed4 _SampleColor;
			sampler2D _MainTex;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				
				//this is the color we receive
				fixed4 col = 2.0f * i.color * _ShaftColor * _ColorWarmt * _SampleColor *  tex2D(_MainTex, i.texcoord)*(_Intensity*_ShaftFading);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return col;
			}
			ENDCG 
		}


		//add extra pass to take all fog types into account
/*		Pass{

		AlphaTest Off

		}*/


	}	
}
}
