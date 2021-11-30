// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/Rtt" 
{
    Properties 
	{
	    colorMap ("Color Map (RGBA)", 2D) = "white" { }
	}

	SubShader 
	{
		Tags     { "RenderType"="Transparent" "Queue"="Transparent" }
		Fog      { Mode Off }
		LOD      100
		Lighting Off Cull Off ZWrite Off ZTest Always Blend One OneMinusSrcAlpha
		
		Pass
		{		
			CGPROGRAM
			#pragma vertex         Vert
			#pragma fragment       Frag
			#pragma fragmentoption ARB_precision_hint_nicest

			#include "UnityShaderVariables.cginc"
			
			sampler2D colorMap;
			
			struct in_t
			{
			    float4 pos : POSITION;
				float2 uv1 : TEXCOORD0;	
			    float2 uv2 : TEXCOORD1;
			    float2 uv3 : TEXCOORD2;
				float2 uv4 : TEXCOORD3;
			};
			
			struct out_t
			{
			    float4 pos         : SV_POSITION;
				float3 uvSpread    : TEXCOORD0;
				float2 distOpacity : TEXCOORD1;
			    float4 radialFocal : TEXCOORD2;
			};
			
			out_t Vert(in_t i)
			{
			    out_t o = (out_t)0;
				
				o.pos            = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, mul(unity_ObjectToWorld, float4(i.pos.x, i.pos.y, 0.0f, 1.0f))));
				o.uvSpread.xy    = i.uv2;
				o.uvSpread.z     = frac(i.uv4.y) * 10.0f;
				o.distOpacity.x  = i.uv4.x;
				o.distOpacity.y  = floor(i.uv4.y) / 255.0f;
			    o.radialFocal.xy = i.uv1;
				o.radialFocal.zw = i.uv3;

			    return o;
			}
			
			float4 Frag(out_t i) : SV_Target
			{
			    float fx = i.radialFocal.z;
				float fy = i.radialFocal.w;
			    float dx = i.radialFocal.x - fx;
				float dy = i.radialFocal.y - fy;
				float d2 = dx * fy - dy * fx;
				float d3 = dx * dx + dy * dy - d2 * d2;
				i.uvSpread.x = (dx * fx + dy * fy + sqrt(abs(d3))) * (1.0f/(1.0f - fx * fx - fy * fy));

			    float2 uv[3] = { i.uvSpread.xy, i.uvSpread.xy, i.uvSpread.xy };
				
				uv[0].x = clamp(i.uvSpread.x, 0.0f, 0.99999f);
			
			    float f = frac(i.uvSpread.x * 0.5f) * 2.0f; 
				uv[2].x = 0.99999f - abs(f - 1.0f);
			
			    float4 color = tex2D(colorMap, uv[(int)i.uvSpread.z]);
			
			    float w = 0.707f * fwidth(i.distOpacity.x);
				float a = smoothstep(0.5f - w, 0.5f + w, i.distOpacity.x);
				//a = i.distOpacity.x;
				color.a = color.a * a * i.distOpacity.y;
				color.rgb *= color.a;
							
				return color;
			}

			ENDCG
        }
	} 
}