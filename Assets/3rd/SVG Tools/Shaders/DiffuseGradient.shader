// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/DiffuseGradient" 
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
		Lighting On Cull[_Cull] Offset[_Factor] , [_Units]		
		
		Pass
		{
		    Tags { "LightMode"="ForwardBase" }
			Blend[_SrcBlend][_DstBlend] ZWrite[_ZWrite] ZTest[_ZTest]	
				
			CGPROGRAM
			#pragma multi_compile_fwdbase
			
			#pragma vertex         Vert
			#pragma fragment       Frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma multi_compile __ SVG_TOOLS_TRANSPARENT
			#pragma multi_compile __ SVG_TOOLS_SLICED
			#pragma multi_compile __ SVG_TOOLS_RADIAL
            #pragma multi_compile __ SVG_TOOLS_FOCAL
			#pragma multi_compile __ SVG_TOOLS_PAD
			#pragma multi_compile __ SVG_TOOLS_REFLECT

			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"
			
			sampler2D colorMap;
			
			float4    scaleX;
			float4    scaleY;
			float4    offsetX;
			float4    offsetY;
			
			struct in_t
			{
			    float4 vertex : POSITION;	
				
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
			    float4 pos          : SV_POSITION;
				fixed4 diffuse      : TEXCOORD0;
				float4 uvSpreadDist : TEXCOORD1;
				float3 world        : TEXCOORD2;
				
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    float2 radialFocal : TEXCOORD3;
			#endif 
			
			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float4 radialFocal : TEXCOORD3;
			#endif	
			
			    SHADOW_COORDS(4)		
			};
			
			out_t Vert(in_t v)
			{
			    out_t o = (out_t)0;
				
				float2 z      = float2(1.0f, -1.0f);
				float3 e      = normalize(mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1.0f)).xyz - float3(v.vertex.x, v.vertex.y, 0.0f));
				int    n      = dot(float3(0.0f, 0.0f, 1.0f), e) < 0.0f;
				float3 normal = normalize(mul(float3(0.0f, 0.0f, z[n]), (float3x3)unity_WorldToObject));
				float3 light  = normalize(_WorldSpaceLightPos0.xyz);
				float  NdotL  = max(0.0f, dot(normal, light));
				
				float4 w = float4(v.vertex.x, v.vertex.y, v.vertex.z * z[n], 1.0f);
			
			#ifdef SVG_TOOLS_SLICED
				w.xy = w.xy * float2(scaleX[v.tan.z], scaleY[v.tan.w]) + float2(offsetX[v.tan.z], offsetY[v.tan.w]);
			#endif
				
				o.pos             = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
			    o.diffuse         = NdotL * _LightColor0;
				o.world           = w.xyz;
				o.uvSpreadDist.xy = v.uv1;
				
			#ifdef SVG_TOOLS_SLICED
			    o.diffuse.a       = floor(v.tan.y) / 255.0f;
				o.uvSpreadDist.z  = round(frac(v.tan.y) * 10.0f);
				o.uvSpreadDist.w  = v.tan.x;
			#else
				o.diffuse.a       = floor(v.uv.y) / 255.0f;
				o.uvSpreadDist.z  = round(frac(v.uv.y) * 10.0f);
				o.uvSpreadDist.w  = v.uv.x;
			#endif
			
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)	
			    o.radialFocal = v.uv2;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)	
			    o.radialFocal.xy = v.uv2;
				o.radialFocal.zw = v.uv3;
			#endif
						    
			    TRANSFER_SHADOW(o);

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
				i.uvSpreadDist.x = (dx * fx + dy * fy + sqrt(abs(d3))) * (1.0f/(1.0f - fx * fx - fy * fy));
			#endif

			    float2 uv[3] = { i.uvSpreadDist.xy, i.uvSpreadDist.xy, i.uvSpreadDist.xy };
				
			#if(SVG_TOOLS_PAD)
				uv[0].x = clamp(i.uvSpreadDist.x, 0.0f, 0.99999f);
			#endif
			
			#if(SVG_TOOLS_REFLECT)
			    float f = frac(i.uvSpreadDist.x * 0.5f) * 2.0f; 
				uv[2].x = 0.99999f - abs(f - 1.0f);
			#endif
			
			#if(SVG_TOOLS_PAD || SVG_TOOLS_REFLECT)
			    half4 color = tex2D(colorMap, uv[i.uvSpreadDist.z]);
			#else
			    half4 color = tex2D(colorMap, i.uvSpreadDist.xy);
			#endif
			
			float a = 1.0f;

			#ifdef SVG_TOOLS_TRANSPARENT
				float w = 0.707f * fwidth(i.uvSpreadDist.w);
				a = smoothstep(0.5f - w, 0.5f + w, i.uvSpreadDist.w);
				a = color.a * a * i.diffuse.a;
			#endif
			
			    color = color * i.diffuse * SHADOW_ATTENUATION(i) + color * 0.1f;
				color.a = a;
			
				return color;
			}

			ENDCG
        }
		
		Pass
		{
		    Tags { "LightMode"="ForwardAdd" }
			Blend[_SrcBlend] One ZWrite Off ZTest LEqual	
				
			CGPROGRAM
			#pragma multi_compile_fwdadd_fullshadows
			
			#pragma vertex         Vert
			#pragma fragment       Frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma multi_compile __ SVG_TOOLS_TRANSPARENT
			#pragma multi_compile __ SVG_TOOLS_SLICED
			#pragma multi_compile __ SVG_TOOLS_RADIAL
            #pragma multi_compile __ SVG_TOOLS_FOCAL
			#pragma multi_compile __ SVG_TOOLS_PAD
			#pragma multi_compile __ SVG_TOOLS_REFLECT

			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"
			
			sampler2D colorMap;
			
			float4    scaleX;
			float4    scaleY;
			float4    offsetX;
			float4    offsetY;
			
			struct in_t
			{
			    float4 vertex : POSITION;	
				
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
			    float4 pos          : SV_POSITION;
				fixed4 diffuse      : TEXCOORD0;
				float4 uvSpreadDist : TEXCOORD1;
				float3 world        : TEXCOORD2;
				
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    float2 radialFocal : TEXCOORD3;
			#endif 
			
			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float4 radialFocal : TEXCOORD3;
			#endif	
			
			    LIGHTING_COORDS(4,5)		
			};
			
			out_t Vert(in_t v)
			{
			    out_t o = (out_t)0;
				
				float2 z      = float2(1.0f, -1.0f);
				float3 e      = normalize(mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1.0f)).xyz - float3(v.vertex.x, v.vertex.y, 0.0f));
				int    n      = dot(float3(0.0f, 0.0f, 1.0f), e) < 0.0f;
				float3 normal = normalize(mul(float3(0.0f, 0.0f, z[n]), (float3x3)unity_WorldToObject));
				float3 light  = normalize(_WorldSpaceLightPos0.xyz);
				float  NdotL  = max(0.0f, dot(normal, light));
				
				float4 w = float4(v.vertex.x, v.vertex.y, v.vertex.z * z[n], 1.0f);
			
			#ifdef SVG_TOOLS_SLICED
				w.xy = w.xy * float2(scaleX[v.tan.z], scaleY[v.tan.w]) + float2(offsetX[v.tan.z], offsetY[v.tan.w]);
			#endif
				
				o.pos             = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
			    o.diffuse         = NdotL * _LightColor0;
				o.world           = w.xyz;
				o.uvSpreadDist.xy = v.uv1;
				
			#ifdef SVG_TOOLS_SLICED
			    o.diffuse.a       = floor(v.tan.y) / 255.0f;
				o.uvSpreadDist.z  = round(frac(v.tan.y) * 10.0f);
				o.uvSpreadDist.w  = v.tan.x;
			#else
				o.diffuse.a       = floor(v.uv.y) / 255.0f;
				o.uvSpreadDist.z  = round(frac(v.uv.y) * 10.0f);
				o.uvSpreadDist.w  = v.uv.x;
			#endif
			
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)	
			    o.radialFocal = v.uv2;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)	
			    o.radialFocal.xy = v.uv2;
				o.radialFocal.zw = v.uv3;
			#endif
						    			
			    TRANSFER_VERTEX_TO_FRAGMENT(o);

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
				i.uvSpreadDist.x = (dx * fx + dy * fy + sqrt(abs(d3))) * (1.0f/(1.0f - fx * fx - fy * fy));
			#endif

			    float2 uv[3] = { i.uvSpreadDist.xy, i.uvSpreadDist.xy, i.uvSpreadDist.xy };
				
			#if(SVG_TOOLS_PAD)
				uv[0].x = clamp(i.uvSpreadDist.x, 0.0f, 0.99999f);
			#endif
			
			#if(SVG_TOOLS_REFLECT)
			    float f = frac(i.uvSpreadDist.x * 0.5f) * 2.0f; 
				uv[2].x = 0.99999f - abs(f - 1.0f);
			#endif
			
			#if(SVG_TOOLS_PAD || SVG_TOOLS_REFLECT)
			    half4 color = tex2D(colorMap, uv[i.uvSpreadDist.z]);
			#else
			    half4 color = tex2D(colorMap, i.uvSpreadDist.xy);
			#endif
			
			float a = 1.0f;

			#ifdef SVG_TOOLS_TRANSPARENT
				float w = 0.707f * fwidth(i.uvSpreadDist.w);
				a = smoothstep(0.5f - w, 0.5f + w, i.uvSpreadDist.w);
				a = color.a * a * i.diffuse.a;
			#endif
			
			    color = color * i.diffuse * LIGHT_ATTENUATION(i);
				color.a = a;
			
				return color;
			}

			ENDCG
        }
	} 
}
