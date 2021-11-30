// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/DiffuseSolid" 
{
    Properties 
	{
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

			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"
			
			float4 scaleX;
			float4 scaleY;
			float4 offsetX;
			float4 offsetY;
			
			struct in_t
			{
			    float4 vertex : POSITION;			    
			    fixed4 color  : COLOR;	
			
			#ifdef SVG_TOOLS_SLICED
			    float4 tan    : TANGENT;
			#else 
			    float2 uv     : TEXCOORD0;
			#endif   
			};
			
			struct out_t
			{
			    float4 pos     : SV_POSITION;	
				fixed4 color   : COLOR;
			    fixed4 diffuse : TEXCOORD0;
				float3 world   : TEXCOORD1;
				float  dist    : TEXCOORD2;	
				
				SHADOW_COORDS(3)    
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
				
				o.pos     = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
			    o.diffuse = NdotL * _LightColor0;
				o.color   = v.color;
				o.world   = w.xyz;
				
			#ifdef SVG_TOOLS_SLICED
			    o.dist = v.tan.x;
			#else
				o.dist = v.uv.x;
			#endif
						    
			    TRANSFER_SHADOW(o);

			    return o;
			}
			
			half4 Frag(out_t i) : SV_Target
			{
			    i.color.rgb = i.color * i.diffuse * SHADOW_ATTENUATION(i) + i.color * 0.1f;
			
			#ifdef SVG_TOOLS_TRANSPARENT
				float w = 0.707f * fwidth(i.dist);
				float a = smoothstep(0.5f - w, 0.5f + w, i.dist);
				i.color.a *= a;
			#endif
			
			    return i.color;
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

			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"
			
			float4 scaleX;
			float4 scaleY;
			float4 offsetX;
			float4 offsetY;
			
			struct in_t
			{
			    float4 vertex : POSITION;			    
			    fixed4 color  : COLOR;	
			
			#ifdef SVG_TOOLS_SLICED
			    float4 tan    : TANGENT;
			#else 
			    float2 uv     : TEXCOORD0;
			#endif   
			};
			
			struct out_t
			{
			    float4 pos     : SV_POSITION;	
				fixed4 color   : COLOR;
			    fixed4 diffuse : TEXCOORD0;
				float3 world   : TEXCOORD1;
				float  dist    : TEXCOORD2;	
				
				LIGHTING_COORDS(3,4)    
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
				
				o.pos     = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
			    o.diffuse = NdotL * _LightColor0;
				o.color   = v.color;
				o.world   = w.xyz;
				
			#ifdef SVG_TOOLS_SLICED
			    o.dist = v.tan.x;
			#else
				o.dist = v.uv.x;
			#endif
			
			    TRANSFER_VERTEX_TO_FRAGMENT(o);

			    return o;
			}
			
			half4 Frag(out_t i) : SV_Target
			{
			    i.color.rgb = i.color * i.diffuse * LIGHT_ATTENUATION(i);
			
			#ifdef SVG_TOOLS_TRANSPARENT
				float w = 0.707f * fwidth(i.dist);
				float a = smoothstep(0.5f - w, 0.5f + w, i.dist);
				i.color.a *= a;
			#endif
			
			    return i.color;
			}

			ENDCG
        }
	}
}
