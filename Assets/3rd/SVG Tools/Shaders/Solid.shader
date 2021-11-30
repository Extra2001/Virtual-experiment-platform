// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/Solid" 
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
			
			#include "UnityShaderVariables.cginc"
			
			float4 scaleX;
			float4 scaleY;
			float4 offsetX;
			float4 offsetY;
						
			struct in_t
			{
			    float4 pos   : POSITION;			    
			    fixed4 color : COLOR;	
			
			#ifdef SVG_TOOLS_SLICED
			    float4 tan   : TANGENT;
			#else 
			    float2 uv    : TEXCOORD0;
			#endif   
			};
			
			struct out_t
			{
			    float4 pos   : SV_POSITION;	
			    fixed4 color : COLOR;
				float  dist  : TEXCOORD0;	    
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
				
				o.pos   = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, w));
			    o.color = i.color;
				
			#ifdef SVG_TOOLS_SLICED
			    o.dist  = i.tan.x;
			#else
				o.dist  = i.uv.x;
			#endif
						    
			    return o;
			}
			
			half4 Frag(out_t i) : SV_Target
			{
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
