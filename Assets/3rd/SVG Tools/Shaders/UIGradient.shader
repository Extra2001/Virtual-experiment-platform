// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/UIGradient" 
{
    Properties 
	{
	    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		
		colorMap ("Color Map (RGBA)", 2D) = "white" { }
	}

	SubShader 
	{
		Tags     { "RenderType"="Transparent" "Queue"="Transparent" "CanUseSpriteAtlas"="True" "PreviewType"="Plane" "IgnoreProjector"="True" }
		Fog      { Mode Off }
		LOD      100
		Lighting Off Cull Off Blend SrcAlpha OneMinusSrcAlpha ZWrite Off ZTest [unity_GUIZTestMode] ColorMask [_ColorMask]
		
		Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }				
		
		Pass
		{		
			CGPROGRAM
			#pragma vertex         Vert
			#pragma fragment       Frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma multi_compile __ SVG_TOOLS_RADIAL
            #pragma multi_compile __ SVG_TOOLS_FOCAL
			#pragma multi_compile __ SVG_TOOLS_PAD
			#pragma multi_compile __ SVG_TOOLS_REFLECT

			#include "UnityCG.cginc"
            #include "UnityUI.cginc"
			
			#pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			sampler2D _MainTex;
			sampler2D colorMap;
			fixed4    _Color;
            fixed4    _TextureSampleAdd;
            float4    _ClipRect;
			
			
			struct in_t
			{
				float4 pos : POSITION;	
				float4 col : COLOR;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    float2 uv3 : TEXCOORD2;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float2 uv3 : TEXCOORD2;
				float2 uv4 : TEXCOORD3;
			#endif
			};
			
			struct out_t
			{
				float4 pos         : SV_POSITION;
				float4 color       : COLOR;
				float3 uvSpread    : TEXCOORD0;
				float2 distOpacity : TEXCOORD1;
				float4 wpos        : TEXCOORD2;
				
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)
			    float2 radialFocal : TEXCOORD3;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)
			    float4 radialFocal : TEXCOORD3;
			#endif			
			};
			
			out_t Vert(in_t i)
			{
			    out_t o = (out_t)0;
								
				o.pos           = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(i.pos.x, i.pos.y, 0.0f, 1.0f)));
				o.wpos          = float4(i.pos.x, i.pos.y, 0.0f, 1.0f);
				o.color         = i.col * _Color;
				o.uvSpread.xy   = i.uv2;
				o.uvSpread.z    = frac(i.uv1.y) * 10.0f;
				o.distOpacity.x = i.uv1.x;
				o.distOpacity.y = floor(i.uv1.y) / 255.0f;
								
			#if(SVG_TOOLS_RADIAL && !SVG_TOOLS_FOCAL)	
			    o.radialFocal = i.uv3;
			#endif

			#if(SVG_TOOLS_FOCAL && !SVG_TOOLS_RADIAL)	
			    o.radialFocal.xy = i.uv3;
				o.radialFocal.zw = i.uv4;
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
				
			#if(SVG_TOOLS_PAD)
				uv[0].x = clamp(i.uvSpread.x, 0.0f, 0.99999f);
			#endif
			
			#if(SVG_TOOLS_REFLECT)
			    float f = frac(i.uvSpread.x * 0.5f) * 2.0f; 
				uv[2].x = 0.99999f - abs(f - 1.0f);
			#endif
			
			#if(SVG_TOOLS_PAD || SVG_TOOLS_REFLECT)
			    half4 color = (tex2D(colorMap, uv[(int)i.uvSpread.z]) + _TextureSampleAdd) * i.color;
			#else
			    half4 color = (tex2D(colorMap, i.uvSpread.xy) + _TextureSampleAdd) * i.color;
			#endif
			
			    float w = 0.707f * fwidth(i.distOpacity.x);
				float a = smoothstep(0.5f - w, 0.5f + w, i.distOpacity.x);
				color.a = color.a * a * i.distOpacity.y;
				
			#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(i.wpos.xy, _ClipRect);
            #endif

            #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001f);
            #endif
			
				return color;
			}

			ENDCG
        }
	} 
}