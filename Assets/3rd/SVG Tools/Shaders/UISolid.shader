// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/UISolid" 
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

			#include "UnityCG.cginc"
            #include "UnityUI.cginc"
			
			#pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			sampler2D _MainTex;
			fixed4    _Color;
            fixed4    _TextureSampleAdd;
            float4    _ClipRect;

			
			struct in_t
			{
			    float4 pos : POSITION;			    
			    fixed4 col : COLOR;
				float2 uv  : TEXCOORD0;	    
			};
			
			struct out_t
			{
			    float4 pos   : SV_POSITION;	
				fixed4 color : COLOR;
				float  dist  : TEXCOORD0;
				float4 wpos  : TEXCOORD1;		    
			};
			
			out_t Vert(in_t i)
			{
			    out_t o = (out_t)0;
							    				
				o.pos   = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(i.pos.x, i.pos.y, 0.0f, 1.0f)));
			    o.color = i.col *_Color;
				o.dist  = i.uv.x;
				o.wpos  = float4(i.pos.x, i.pos.y, 0.0f, 1.0f);
						    
			    return o;
			}
			
			half4 Frag(out_t i) : SV_Target
			{
			    float w = 0.707f * fwidth(i.dist);
				float a = smoothstep(0.5f - w, 0.5f + w, i.dist);
				i.color.a *= a;
				
			#ifdef UNITY_UI_CLIP_RECT
                i.color.a *= UnityGet2DClipping(i.wpos.xy, _ClipRect);
            #endif

            #ifdef UNITY_UI_ALPHACLIP
                clip(i.color.a - 0.001f);
            #endif

				return i.color;
			}

			ENDCG
        }
	} 
}