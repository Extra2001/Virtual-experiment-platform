// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/Gradient" 
{
    Properties 
	{
	    [HideInInspector] colorMap  ("Color Map(RGBA)", 2D) = "white" { }
		
		[HideInInspector] _SrcBlend ("SrcBlend", Float)     = 5
		[HideInInspector] _DstBlend ("DestBlend", Float)    = 10
		[HideInInspector] _ZTest    ("ZTest", Float)        = 4
		[HideInInspector] _ZWrite   ("ZWrite", Float)       = 0
		[HideInInspector] _Cull     ("CullMode", Float)     = 2
		[HideInInspector] _Factor   ("OffsetFactor", Float) = 0
		[HideInInspector] _Units    ("OffsetUnits", Float)  = 0
		
		[HideInInspector] scaleX    ("scaleX", Vector)      = (1,1,1,1)
		[HideInInspector] scaleY    ("scaleY", Vector)      = (1,1,1,1)
		[HideInInspector] offsetX   ("offsetX", Vector)     = (0,0,0,0)
		[HideInInspector] offsetY   ("offsetY", Vector)     = (0,0,0,0)
	}

	SubShader 
	{
		Tags     { "PreviewType"="Plane" }
		Fog      { Mode Off }
		LOD      100
		Lighting Off Cull[_Cull] Blend[_SrcBlend][_DstBlend] ZWrite[_ZWrite] ZTest[_ZTest] Offset[_Factor] , [_Units]
		
		Pass
		{		
			CGPROGRAM
			#pragma vertex         Vert
			#pragma fragment       Frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma multi_compile __ SVG_TOOLS_TRANSPARENT
            #pragma multi_compile __ SVG_TOOLS_TWO_SIDED
			#pragma multi_compile __ SVG_TOOLS_SLICED
			#pragma multi_compile __ SVG_TOOLS_RADIAL
            #pragma multi_compile __ SVG_TOOLS_FOCAL
			#pragma multi_compile __ SVG_TOOLS_PAD
			#pragma multi_compile __ SVG_TOOLS_REFLECT
			
			#include "UnityShaderVariables.cginc"
			
			sampler2D colorMap;
			
			float4    scaleX;
			float4    scaleY;
			float4    offsetX;
			float4    offsetY;
			
			struct in_t
			{
			    float4 pos : POSITION;	
				
			#ifdef SVG_TOOLS_SLICED
			    float4 tan : TANGENT;
				float2 uv1 : TEXCOORD0;
				
				#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			        float2 uv2 : TEXCOORD1;
			    #endif
				
				#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			        float2 uv2 : TEXCOORD1;
				    float2 uv3 : TEXCOORD2;
			    #endif
			#else 
			    float2 uv    : TEXCOORD0;
				float2 uv1   : TEXCOORD1;
				
				#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			        float2 uv2 : TEXCOORD2;
			    #endif
				
				#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			        float2 uv2 : TEXCOORD2;
				    float2 uv3 : TEXCOORD3;
			    #endif
			#endif			   
			};
			
			struct out_t
			{
			    float4 pos         : SV_POSITION;
				float3 uvSpread    : TEXCOORD0;
			    float2 distOpacity : TEXCOORD1;
				
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    float2 radialFocal : TEXCOORD2;
			#endif 
			
			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float4 radialFocal : TEXCOORD2;
			#endif			
			};
			
			out_t Vert(in_t i)
			{
			    out_t  o = (out_t)0;
				int    n = 1;
				float2 z = float2(i.pos.z, -i.pos.z); 
				
			#ifdef SVG_TOOLS_TWO_SIDED
				float3 e = normalize(mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1.0f)).xyz - float3(i.pos.x, i.pos.y, 0.0f));
				n = dot(float3(0.0f, 0.0f, 1.0f), e) < 0.0f;
			#endif
			
			    float4 w = float4(i.pos.x, i.pos.y, z[n], 1.0f);
				
			#ifdef SVG_TOOLS_SLICED
				w.xy = w.xy * float2(scaleX[i.tan.z], scaleY[i.tan.w]) + float2(offsetX[i.tan.z], offsetY[i.tan.w]);
			#endif
			
			    o.pos         = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
				o.uvSpread.xy = i.uv1;
				
			#ifdef SVG_TOOLS_SLICED
				o.distOpacity.x = i.tan.x;
				o.distOpacity.y = floor(i.tan.y) / 255.0f;				
				o.uvSpread.z    = round(frac(i.tan.y) * 10.0f);
			#else
				o.distOpacity.x = i.uv.x;
				o.distOpacity.y = floor(i.uv.y) / 255.0f;				
				o.uvSpread.z    = round(frac(i.uv.y) * 10.0f);
			#endif
			
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)	
			    o.radialFocal = i.uv2;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)	
			    o.radialFocal.xy = i.uv2;
				o.radialFocal.zw = i.uv3;
			#endif
			
			    return o;
			}
			
			half4 Frag(out_t i) : SV_Target
			{
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    i.uvSpread.x = sqrt(i.radialFocal.x * i.radialFocal.x + i.radialFocal.y * i.radialFocal.y);
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float fx = i.radialFocal.z;
				float fy = i.radialFocal.w;
			    float dx = i.radialFocal.x - fx;
				float dy = i.radialFocal.y - fy;
				float d2 = dx * fy - dy * fx;
				float d3 = dx * dx + dy * dy - d2 * d2;
				i.uvSpread.x = (dx * fx + dy * fy + sqrt(abs(d3))) * (1.0f/(1.0f - fx * fx - fy * fy));
			#endif

			    float2 uv[3] = { i.uvSpread.xy, i.uvSpread.xy, i.uvSpread.xy };
				
			#ifdef SVG_TOOLS_PAD
				uv[0].x = clamp(i.uvSpread.x, 0.0f, 0.99999f);
			#endif
			
			#ifdef SVG_TOOLS_REFLECT
			    float f = frac(i.uvSpread.x * 0.5f) * 2.0f; 
				uv[2].x = 0.99999f - abs(f - 1.0f);
			#endif
			
			#if(SVG_TOOLS_PAD || SVG_TOOLS_REFLECT)
			    half4 color = tex2D(colorMap, uv[i.uvSpread.z]); 
			#else
			    half4 color = tex2D(colorMap, i.uvSpread.xy);
			#endif
			
			#ifdef SVG_TOOLS_TRANSPARENT
				float w = 0.707f * fwidth(i.distOpacity.x);
				float a = smoothstep(0.5f - w, 0.5f + w, i.distOpacity.x);
				color.a = color.a * a * i.distOpacity.y;
			#endif
			    			
				return color;
			}

			ENDCG
        }
	} 
}
