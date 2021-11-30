// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


Shader "SVG Tools/Effect" 
{
    Properties 
	{
		[HideInInspector] _MainTex ("Source Image", 2D) = "" {}
	}

    CGINCLUDE
	
	#include "UnityCG.cginc"

	float4 clipRect;
	float4 param1;
	float4 param2;
	float4 param3;
	float4 param4;
	float4 param5;
	float4 param6;
	float4 param7;
	float4 _MainTex_TexelSize;
	
	sampler2D in2;
	sampler2D data;
	sampler2D _MainTex;
	
	struct in_t
	{
	    float4 pos : POSITION;	
	    float2 tex : TEXCOORD0;
	};
	
	struct out_t
	{
	    float4 pos : SV_POSITION;
	    float2 tex : TEXCOORD0;
	};
	
	out_t Vert(in_t i)
	{
	    out_t o;
		
		o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, mul(unity_ObjectToWorld, float4(i.pos.x, i.pos.y, 0.0f, 1.0f))));
		o.tex = i.tex;
		
		return o;
	}
	
	float4 Screen(out_t i) : SV_Target
	{
		return tex2D(_MainTex, i.tex);
	}
	
	float4 GaussianBlur(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float fx, fy, mul, sum = 0.0f;
		float2 uv;
		float4 source;
		float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
		float2 mul1  = -1.0f/(2.0f*param1.xy*param1.xy);
		float2 mul2  = 1.0f/sqrt(6.28318530718f*param1.xy*param1.xy);

		if(0.0f == param1.x)
		{
		    mul1.x = 0.0f;
			mul2.x = 1.0f;
		}
		
		if(0.0f == param1.y)
		{
		    mul1.y = 0.0f;
			mul2.y = 1.0f;
		}
				
		for(int n=param1.y; n>=-param1.y; --n)
		{
		    fy = exp(n * n * mul1.y) * mul2.y;
			uv.y = i.tex.y + _MainTex_TexelSize.y * n;

		    for(int j=-param1.x; j<=param1.x; ++j)
			{
			    fx = exp(j * j * mul1.x) * mul2.x;
				mul = fx * fy;
				sum += mul;
				uv.x = i.tex.x + _MainTex_TexelSize.x * j;
				
				source = tex2D(_MainTex, uv);
				source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
				source.rgb  = source.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
				
				color += source * mul;
			}
		}

		color /= sum;
		
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb = color.rgb * color.a * param1.a;
		
	    return color;
	}
	
	float4 Offset(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
			
		i.tex -= param1.xy * _MainTex_TexelSize.xy;

		if(i.tex.x < 0.0f || i.tex.x > 1.0f || i.tex.y > 1.0f || i.tex.y < 0.0f)
		    clip(-1.0f);

		float4 color = tex2D(_MainTex, i.tex);
		color.rgb *= param1.a;
		
		return color;
	}
	
	float4 Tile(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
			
	    float2 uv = i.tex;
		float w = param2.z - param2.x - _MainTex_TexelSize.x;
		float h = param2.y - param2.w - _MainTex_TexelSize.y;

		float rx = fmod(i.tex.x - param2.x, w);
		float ry = fmod(i.tex.y - param2.w, h);
		
		if(i.tex.x < param2.x)
		    uv.x = param2.z + rx;
		if(i.tex.x > param2.z)
		    uv.x = param2.x + rx;
		
		if(i.tex.y > param2.y) 
		    uv.y = param2.w + ry;
		if(i.tex.y < param2.w)         
		    uv.y = param2.y + ry;

		float4 color = tex2D(_MainTex, uv);
		color.rgb *= param1.a;
		
		return color;
	}
	
	float4 CompositeOver(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		
		dest.rgb /= (dest.a > 0.0f) ? dest.a : 1.0f;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
						
		color.rgb = source.rgb * source.a + dest.rgb * dest.a * (1.0f - source.a);
		color.a = source.a + dest.a * (1.0f - source.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
					
		return color;
	}
	
	float4 CompositeIn(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
										
		color.rgb = source.rgb * source.a * dest.a;
		color.a = source.a * dest.a;
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 CompositeOut(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float  mul    = (source.a > 0.0f) ? 1.0f/source.a : 1.0f;
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
										
		color.rgb = source.rgb * source.a * (1.0f - dest.a);
		color.a = source.a * (1.0f - dest.a);
				
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 CompositeAtop(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		
		dest.rgb /= (dest.a > 0.0f) ? dest.a : 1.0f;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
								
		color.rgb = source.rgb * source.a * dest.a + dest.rgb * dest.a * (1.0f - source.a);
		color.a = source.a * dest.a + dest.a * (1.0f - source.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
							
		return color;
	}
	
	float4 CompositeXor(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		
		dest.rgb /= (dest.a > 0.0f) ? dest.a : 1.0f;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
								
		color.rgb = source.rgb * source.a * (1.0f - dest.a) + dest.rgb * dest.a * (1.0f - source.a);
		color.a = source.a * (1.0f - dest.a) + dest.a * (1.0f - source.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 CompositeArithmetic(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
										
		color = param2.x * source * dest + param2.y * source + param2.z * dest + param2.wwww;
		color = clamp(color, 0.0f, 1.0f);
								
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 DisplacementMap(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 d = tex2D(in2, i.tex);
		d.rgb /= (d.a > 0.0f) ? d.a : 1.0f;
		d.rgb = d.a * (d.rgb * (d.rgb * (d.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
			
		i.tex.x += param1.x * _MainTex_TexelSize.x * (d[(int)param2.x] - 0.5f);
		i.tex.y += param1.y * _MainTex_TexelSize.y * (d[(int)param2.y] - 0.5f);
		
		if(i.tex.x < 0.0f || i.tex.x > 1.0f || i.tex.y > 1.0f || i.tex.y < 0.0f)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);

		float4 color = tex2D(_MainTex, i.tex);
		color.rgb *= param1.a;
			
		return color;
	}
	
	float4 Flood(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);

		param2.rgb *= param2.a;
								
		return param2;
	}
	
	float4 MorphologyErode(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float2 uv;
		float4 source;
		float4 color = float4(1.0f, 1.0f, 1.0f, 1.0f);
		
		for(int n=-param1.y; n<=param1.y; ++n)
		{
		    uv.y = i.tex.y + _MainTex_TexelSize.y * n;

            for(int j=-param1.x; j<=param1.x; ++j)
			{
			    uv.x = i.tex.x + _MainTex_TexelSize.x * j;

				source = tex2D(_MainTex, uv);
				source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
				source.rgb  = source.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));

				color = min(source, color);
			}
		}

		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb = color.rgb * color.a * param1.a;
			
		return color;
	}
	
	float4 MorphologyDilate(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float2 uv;
		float4 source;
		float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
		
		for(int n=-param1.y; n<=param1.y; ++n)
		{
		    uv.y = i.tex.y + _MainTex_TexelSize.y * n;

            for(int j=-param1.x; j<=param1.x; ++j)
			{
			    uv.x = i.tex.x + _MainTex_TexelSize.x * j;

				source = tex2D(_MainTex, uv);
				source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
				source.rgb  = source.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));

				color = max(source, color);
			}
		}

		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb = color.rgb * color.a * param1.a;
			
		return color;
	}
	
	float4 Transfer(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float4 color = tex2D(_MainTex, i.tex);
		color.rgb /= (color.a > 0.0f) ? color.a : 1.0f;
		color.rgb  = (color.rgb * (color.rgb * (color.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		
		color.r = tex2D(data, float2(color.r, 0.0f)).r;
		color.g = tex2D(data, float2(color.g, 0.0f)).g;
		color.b = tex2D(data, float2(color.b, 0.0f)).b;
		color.a = tex2D(data, float2(color.a, 0.0f)).a;

		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb = color.rgb * color.a * param1.a;
			
		return color;
	}
	
	float4 ColorMatrix(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
			
		float4 source = tex2D(_MainTex, i.tex);
		
		source.rgb /= (source.a > 0.0f) ? source.a : 1.0f;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		
		float4 color = param2 * source.r + param3 * source.g + param4 * source.b + param5 * source.a + param6;
		
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 BlendNormal(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
							
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
								
		color.rgb = (1.0f - source.a) * dest.rgb + source.rgb;
		color.a   = 1.0f - (1.0f - source.a) * (1.0f - dest.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 BlendMultiply(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
							
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
								
		color.rgb = (1.0f - source.a) * dest.rgb + (1.0f - dest.a) * source.rgb + source.rgb * dest.rgb;
		color.a   = 1.0f - (1.0f - source.a) * (1.0f - dest.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 BlendScreen(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
							
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
								
		color.rgb = dest.rgb + source.rgb - source.rgb * dest.rgb;
		color.a   = 1.0f - (1.0f - source.a) * (1.0f - dest.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 BlendDarken(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
							
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
								
		color.rgb = min((1.0f - source.a) * dest.rgb + source.rgb, (1.0f - dest.a) * source.rgb + dest.rgb);
		color.a = 1.0f - (1.0f - source.a) * (1.0f - dest.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 BlendLighten(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
							
		float4 source = tex2D(_MainTex, i.tex); 
		float4 dest   = tex2D(in2, i.tex);
		float4 color  = source;
		float2 mul    = (source.a > 0.0f) ? float2(1.0f/source.a, source.a) : float2(1.0f, 1.0f);
		
		source.rgb *= mul.x;
		source.rgb  = param1.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
		source.rgb *= mul.y;
		
		mul = (dest.a > 0.0f) ? float2(1.0f/dest.a, dest.a) : float2(1.0f, 1.0f);
		dest.rgb *= mul.x;
		dest.rgb  = dest.rgb * (dest.rgb * (dest.rgb * 0.305306011f + 0.682171111f) + 0.012522878f);
		dest.rgb *= mul.y;
								
		color.rgb = max((1.0f - source.a) * dest.rgb + source.rgb, (1.0f - dest.a) * source.rgb + dest.rgb);
		color.a = 1.0f - (1.0f - source.a) * (1.0f - dest.a);
						
		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color = clamp(color, 0.0f, 1.0f);
		color.rgb *= color.a;
			
		return color;
	}
	
	float4 Convolve(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
					
		float2 uv1, uv2;
		float4 source;
		float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
		
		for(int y=0; y<param2.y; ++y)
		{
		    for(int x=0; x<param2.x; ++x)
			{
				uv1 = i.tex + _MainTex_TexelSize.xy * float2(x - param2.z, param2.w - y);
				uv2 = (param2.xy - float2(x+1, y+1))/param2.xy;

				if(1 == param3.w)
				{
				    if(uv1.x < 0.0f) 
				        uv1.x += 1.0f;
					if(uv1.x > 1.0f) 
				        uv1.x -= 1.0f;
					if(uv1.y < 0.0f) 
				        uv1.y += 1.0f;
					if(uv1.y > 1.0f) 
				        uv1.y -= 1.0f;
				}
				    
				source = tex2D(_MainTex, uv1);
				source.rgb /= source.a;
				source.rgb  = clamp(source.rgb, 0.0f, 1.0f);
				source.rgb  = source.a * (source.rgb * (source.rgb * (source.rgb * 0.305306011f + 0.682171111f) + 0.012522878f));
				
				if(2 == param3.w && (uv1.x < 0.0f || uv1.x > 1.0f || uv1.y < 0.0f || uv1.y > 1.0f))
					source = float4(0.0f, 0.0f, 0.0f, 0.0f);
				
				color += source * tex2D(data, uv2).r;
			}
		}
		
		color = color * param3.x + param3.y;
		color = clamp(color, 0.0f, 1.0f);
		color.a = (1.0f == param3.z) ? tex2D(_MainTex, i.tex).a : color.a;

		color.rgb /= color.a;
		color.rgb = max(1.055f * pow(color.rgb, 0.416666667f) - 0.055f, 0.0f);
		color.rgb = color.rgb * color.a * param1.a;
			
		return color;
	}
	
	float4 DiffuseLighting(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
			
		float2 sum = float2(0.0f, 0.0f);
					
		sum += float2(-1.0f, -1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f,  1.0f))).aa;
		sum += float2( 0.0f, -2.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f,  1.0f))).aa;
		sum += float2( 1.0f, -1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f,  1.0f))).aa;
		
		sum += float2(-2.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f,  0.0f))).aa;
		sum += float2( 0.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f,  0.0f))).aa;
		sum += float2( 2.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f,  0.0f))).aa;
		
		sum += float2(-1.0f,  1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f, -1.0f))).aa;
		sum += float2( 0.0f,  2.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f, -1.0f))).aa;
		sum += float2( 1.0f,  1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f, -1.0f))).aa;

		sum = -0.25f * sum * param2.w;

		float3 pos    = float3(i.tex.x/_MainTex_TexelSize.x, (1.0f - i.tex.y)/_MainTex_TexelSize.y, param2.w * tex2D(_MainTex, i.tex).a);
		float3 normal = normalize(float3(sum.x, sum.y, 1.0f));
		float3 light  = normalize(param1.xyz - pos * param4.w);		
		float  d      = max(0.0f, dot(normalize(param4.xyz - param1.xyz), -light));
		float  spot   = 1.0f;

		if(0.0f != param3.y && d <= param3.z)
		    spot = (d <= param3.w) ? 0.0f : pow(smoothstep(param3.w, param3.z, d), param3.y);
		
		float3 color = param3.x * max(0.0f, dot(normal, light)) * param2.xyz * spot;

		return float4(color, 1.0f);
	}
	
	float4 SpecularLighting(out_t i) : SV_Target
	{
	    if(i.tex.x < clipRect.x || i.tex.x > clipRect.z || i.tex.y > clipRect.y || i.tex.y < clipRect.w)
		    float4(0.0f, 0.0f, 0.0f, 0.0f);
			
		float2 sum = float2(0.0f, 0.0f);
					
		sum += float2(-1.0f, -1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f,  1.0f))).aa;
		sum += float2( 0.0f, -2.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f,  1.0f))).aa;
		sum += float2( 1.0f, -1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f,  1.0f))).aa;
		
		sum += float2(-2.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f,  0.0f))).aa;
		sum += float2( 0.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f,  0.0f))).aa;
		sum += float2( 2.0f,  0.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f,  0.0f))).aa;
		
		sum += float2(-1.0f,  1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2(-1.0f, -1.0f))).aa;
		sum += float2( 0.0f,  2.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 0.0f, -1.0f))).aa;
		sum += float2( 1.0f,  1.0f) * tex2D(_MainTex, (i.tex + _MainTex_TexelSize.xy * float2( 1.0f, -1.0f))).aa;

		sum = -0.25f * sum * param2.w;

		float3 pos    = float3(i.tex.x/_MainTex_TexelSize.x, (1.0f - i.tex.y)/_MainTex_TexelSize.y, param2.w * tex2D(_MainTex, i.tex).a);
		float3 normal = normalize(float3(sum.x, sum.y, 1.0f));
		float3 light  = normalize(param1.xyz - pos * param4.w);
		float3 h      = normalize(light + float3(0.0f, 0.0f, 1.0f));		
		float  d      = max(0.0f, dot(normalize(param4.xyz - param1.xyz), -light));
		float  spot   = 1.0f;

		if(0.0f != param3.y && d <= param3.z)
		    spot = (d <= param3.w) ? 0.0f : pow(smoothstep(param3.w, param3.z, d), param3.y);
		
		float4 color;
		color.rgb = param3.x * max(0.0f, pow(dot(normal, h), param5.z)) * param2.xyz * spot;
		color.a = max(max(color.r, color.g), color.b);
		color.rgb *= color.a;

		return color;
	}
	
	ENDCG
	
	SubShader 
	{
	    Fog      { Mode off }
		Lighting Off Cull Off Blend Off ZWrite Off ZTest Always 
		
		Pass 
		{ 
			Blend One OneMinusSrcAlpha
			  
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Screen
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       GaussianBlur
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Offset
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Tile
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
			CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       CompositeOver
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       CompositeIn
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       CompositeOut
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert 
            #pragma fragment       CompositeAtop 
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       CompositeXor
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       CompositeArithmetic
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       DisplacementMap
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Flood
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       MorphologyErode
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       MorphologyDilate
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Transfer
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       ColorMatrix
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       BlendNormal
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       BlendMultiply
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       BlendScreen
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       BlendDarken
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       BlendLighten
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       Convolve
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       DiffuseLighting
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
		
		Pass 
		{ 
		    CGPROGRAM
            #pragma vertex         Vert
            #pragma fragment       SpecularLighting
			#pragma fragmentoption ARB_precision_hint_nicest
            ENDCG
        }
	}
}