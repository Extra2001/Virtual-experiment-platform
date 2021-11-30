// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEditor;
using UnityEngine;

using System.IO; 
using System.Collections.Generic;


namespace svgtools
{
	public enum EffectType 
	{
		NONE                 = 0,
		GAUSSIAN_BLUR        = 1,
		OFFSET               = 2,
		TILE                 = 3, 
		COMPOSITE_OVER       = 4, 
		COMPOSITE_IN         = 5,	 
		COMPOSITE_OUT        = 6,	 
		COMPOSITE_ATOP       = 7,	  
		COMPOSITE_XOR        = 8,	  
		COMPOSITE_ARITHMETIC = 9,
		DISPLACEMENT_MAP     = 10,
	 	FLOOD                = 11,
		MORPHOLOGY_ERODE     = 12,  
		MORPHOLOGY_DILATE    = 13,
		TRANSFER		     = 14,
		COLOR                = 15,
		BLEND_NORMAL         = 16,	 
		BLEND_MULTIPLY       = 17,	 
		BLEND_SCREEN         = 18,	 
		BLEND_DARKEN         = 19,  
		BLEND_LIGHTEN        = 20,
		CONVOLVE		     = 21,
		DIFFUSE_LIGHTING     = 22,
		SPECULAR_LIGHTING	 = 23
	}

	
	public class Batch
	{ 
		public FilterInstance filter;  
		public Batch          next;

		public int            start;
		public int            count;
		public int            subset;

		public Batch(FilterInstance filter, int start, int count, int subset) 
		{
			this.filter = filter; 
			this.next   = null;
			this.start  = start;
			this.count  = count;
			this.subset = subset;
		}
	}  
	

	public struct Effect
	{
		public List<float> paramList; 
		public Color[]     data;
		public EffectType  pass; 
		public Texture2D   texture;
		public Vector4     clip;  
		public Vector4     param2;
		public Vector4     param3;
		public Vector4     param4;
		public Vector4     param5; 
		public Vector4     param6;
		public float       paramX;	
		public float       paramY; 
		public float       paramZ;
		public float       alpha;  
		public float       x;	
		public float       y; 
		public float       width; 
		public float       height;
		public int         in1;	
		public int         in2;
		public int         result; 
 		public int         tileEffect;


		public void ComputeClipRect(Rect rect, Vector2 pos, Vector2 size, bool local) 
		{
			float cx = local ? rect.left + x * rect.Width : x; 
			float cy = local ? rect.top + y * rect.Height : y;
			float cz = local ? cx + width * rect.Width    : x + width;	
			float cw = local ? cy + height * rect.Height  : y + height;  
					
			clip.x = float.IsNaN(x)      ? 0.0f : ( cx - pos.x)/size.x + 0.0f;  
			clip.y = float.IsNaN(y)      ? 1.0f : (-cy - pos.y)/size.y + 1.0f;	  
			clip.z = float.IsNaN(width)  ? 1.0f : ( cz - pos.x)/size.x + 0.0f;  
			clip.w = float.IsNaN(height) ? 0.0f : (-cw - pos.y)/size.y + 1.0f;	
		}
	} 
 
	
	public class Filter
	{ 
		public List<Effect>    effectList;	
		public List<string>    targetNameList;	
		public RenderTexture[] targetArray;

		public Vector2         position;
		public Vector2         size;

		public string          name;
		public bool            filterUserSpaceOnUse; 
		public bool            primitiveUserSpaceOnUse;


		public Filter() 
		{
			position.x              = -0.1f;	 
			position.y              = -0.1f;
			size.x                  = 1.2f;	  
			size.y                  = 1.2f;
  			name                    = "";
			filterUserSpaceOnUse    = false; 
			primitiveUserSpaceOnUse = true;
		}

		public int AddRenderTarget(string rt, ref float alpha) 
		{ 
			if(string.IsNullOrEmpty(rt))
                return 0;

			if(null == targetNameList) 
				targetNameList = new List<string>();

			if("SourceGraphic" == rt)
			{
				alpha = 1.0f;
				return 1;
			} 
			
			if("SourceAlpha" == rt)
			{
				alpha = 0.0f;
				return 1;
			}
			
			for(int i=targetNameList.Count-1; i>=0; --i) 
            {
                if(targetNameList[i] == rt)
                    return i+2;
            }

            targetNameList.Add(rt);

			return targetNameList.Count + 1;
		} 
		
		public void AddEffect(Effect effect) 
		{
			if(null == effectList)	 
				effectList = new List<Effect>();

			if(0 == effect.result) 
			{  
				targetNameList.Add("");
				effect.result = targetNameList.Count + 1;
			}
			else if(effect.in1 == effect.result || effect.in2 == effect.result) 
			{ 
				targetNameList.Add(targetNameList[effect.result-2]);
				effect.result = targetNameList.Count + 1;
			}  

			if(EffectType.TILE == effect.pass) 
			{ 
				for(int i=effectList.Count-1; i>=0; --i) 
				{
					if(effectList[i].result == effect.in1) 
					{ 
						effect.tileEffect = i; 
						break;
					}
				}	
			} 
			else if(EffectType.TRANSFER == effect.pass) 
			{	
				effect.texture = new Texture2D(256, 1, TextureFormat.ARGB32, false);  
				effect.texture.hideFlags  = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
				effect.texture.filterMode = FilterMode.Point;
				effect.texture.wrapMode   = TextureWrapMode.Clamp;
            
				effect.texture.SetPixels(effect.data);
				effect.texture.Apply();
			}	   
			else if(EffectType.CONVOLVE == effect.pass) 
			{	
				effect.texture = new Texture2D((int)effect.param2.x, (int)effect.param2.y, TextureFormat.RFloat, false);  
				effect.texture.hideFlags  = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
				effect.texture.filterMode = FilterMode.Point;
				effect.texture.wrapMode   = TextureWrapMode.Clamp;
            
				effect.texture.SetPixels(effect.data);
				effect.texture.Apply();
			}  
			
			effectList.Add(effect);
		}
	} 

	
	public class FilterInstance
	{
		static List<Filter>    filterList = new List<Filter>(); 
		static List<Batch>     batchList  = new List<Batch>();	
		
		static Mesh            mesh       = null;
		static Texture2D       texture    = null;
		static Material        rtt        = null;
		static Material        effect     = null; 
				
		public FilterInstance  parent;	 
		public Batch           head;	  
		public Batch           tail;	 
		public Batch           screen;
		public Filter          filter;
		public Transform       transform;
		public Rect            rect;	 
		public Vector2         position;	
		public Vector2         center;
		public Vector2         size;	 
		public Vector2         view;
		public float           scale;


		public FilterInstance(FilterInstance parent, string filter)
		{
			this.parent    = parent;
			this.head      = null;	 
			this.tail      = null;
			this.screen    = null;
			this.filter    = null;
			this.transform = Transform.identity;
			this.rect      = Rect.Default;
			this.position  = Vector2.zero;  
			this.center    = Vector2.zero;
			this.size      = Vector2.zero;  	 
			this.view      = Vector2.zero;
			this.scale     = 1.0f;
			
			foreach(Filter f in filterList) 
            {
				if(filter == f.name) 
				{ 
                    this.filter = f; 
					break;
				}
            }
		}

		public static void SetTextureData(int height, Color32[] data)
		{	
			if(null == texture) 
			{
				texture = new Texture2D(256, height, TextureFormat.ARGB32, false);

				texture.wrapMode   = TextureWrapMode.Repeat;
				texture.hideFlags  = HideFlags.HideAndDontSave;
				texture.filterMode = FilterMode.Point;
			}

			if(height != texture.height) 
				texture.Resize(256, height); 
			
			texture.SetPixels32(data);
			texture.Apply();
		}
		
		public static void SetMeshData(Vector3[] vertices, Vector2[] uv1, Vector2[] uv2, Vector2[] uv3, Vector2[] uv4, List<int> triangles) 
		{
			if(null == rtt) 
				rtt = new Material(Shader.Find("SVG Tools/Rtt"));	
			
			if(null == effect) 
				effect = new Material(Shader.Find("SVG Tools/Effect"));	

			if(null == mesh)
				mesh = new Mesh();	
  
			mesh.Clear();
			
			mesh.vertices     = vertices;
			mesh.uv           = uv1; 
			mesh.uv2          = uv2;
			mesh.uv3          = uv3; 
			mesh.uv4          = uv4;
			mesh.subMeshCount = batchList.Count;
			mesh.hideFlags    = HideFlags.HideAndDontSave; 
			
			foreach(Batch b in batchList)
				mesh.SetTriangles(triangles.GetRange(b.start, b.count).ToArray(), b.subset); 
			
			mesh.RecalculateBounds();
		}

		public Batch AddBatch(int start, FilterInstance filter, bool screen)
		{  
			Batch batch = new Batch(filter, start, 0, batchList.Count);

			if(screen) 
			{
				this.screen = batch;  
			}
			else if(null == head) 
			{
				head = tail = batch;
			}
			else 
			{ 
				tail.next = batch;
				tail = batch;
			}

			if(null == filter)
				batchList.Add(batch); 

			return batch;
		}	
		
		public static void AddFilter(Filter filter) 
        {
            if(null == filter || string.IsNullOrEmpty(filter.name))
                return;
            
            foreach(Filter f in filterList) 
            {
                if(filter.name == f.name)
                    return;
            }

            filterList.Add(filter);	 
			filter.targetArray = (null != filter.targetNameList) ? new RenderTexture[filter.targetNameList.Count + 2] : new RenderTexture[2];
        }

		public static void Clear() 
		{
			filterList.Clear();
			batchList.Clear();
		}

		void DrawBatch(Batch b, RenderTexture target) 
		{  
			if(null == b.filter)
			{
				rtt.SetTexture("colorMap", texture);
				rtt.SetPass(0);
				Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(-center, Quaternion.identity, Vector3.one), b.subset);
			}
			else 
			{ 
				b.filter.Draw(target); 
			}

			if(null != b.next) 
				DrawBatch(b.next, target);
		}
		
		public void Draw(RenderTexture target) 
		{
			if(null == filter || string.IsNullOrEmpty(filter.name)) 
			{
				float sx = mesh.bounds.size.x > mesh.bounds.size.y ? mesh.bounds.size.x * 0.5f + 2.0f : mesh.bounds.size.y * 0.5f + 2.0f;	
				float sy = sx;
				center   = mesh.bounds.center;	
				scale    = mesh.bounds.size.x > mesh.bounds.size.y ? (target.width - 4) / mesh.bounds.size.x : (target.height - 4) / mesh.bounds.size.y;

				if(target.width > target.height)
				{
					sx    = mesh.bounds.size.y * 1.0f + 2.0f;	
					sy    = mesh.bounds.size.y * 0.5f + 2.0f;
					scale = (target.height - 4) / mesh.bounds.size.y;	 
				}	 
				else if(target.width < target.height)
				{
					sx    = mesh.bounds.size.x * 0.5f + 2.0f;	
					sy    = mesh.bounds.size.x * 1.0f + 2.0f;	
					scale = (target.width - 4) / mesh.bounds.size.x; 
				}
 
				GL.PushMatrix();   
				GL.LoadProjectionMatrix(Matrix4x4.Ortho(-sx, sx, -sy, sy, 0.1f, 1000.0f));  
				
				DrawBatch(head, target);	
								
				GL.PopMatrix();
			} 
			else 
			{
				RenderTexture last = null;
				Effect        e    = default(Effect);	
				Vector4       vec  = Vector4.one;
				int           w    = Mathf.FloorToInt(size.x * scale);	  	  
				int           h    = Mathf.FloorToInt(size.y * scale);
				int           n    = filter.targetNameList.Count + 2; 
								
				for(int i=0; i<n; ++i) 
				{ 
					filter.targetArray[i] = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32);
					filter.targetArray[i].filterMode = FilterMode.Point; 
					filter.targetArray[i].antiAliasing = 8;
					filter.targetArray[i].Create();   
				}
				
				Graphics.SetRenderTarget(filter.targetArray[1]);
				GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));	
				
				GL.PushMatrix();
				GL.LoadProjectionMatrix(Matrix4x4.Ortho(-view.x*0.5f, view.x*0.5f, -view.y*0.5f, view.y*0.5f, 0.1f, 1000.0f));	
				
				DrawBatch(head, filter.targetArray[1]);  	
				
				GL.PopMatrix();
				
				last = filter.targetArray[1];  

				for(int i=0; i<filter.effectList.Count; ++i) 
				{
					e = filter.effectList[i];
					e.ComputeClipRect(rect, position, size, !filter.primitiveUserSpaceOnUse); 
					filter.effectList[i] = e;
					
					effect.SetVector("clipRect", e.clip);

					vec.x = filter.primitiveUserSpaceOnUse ? (scale * e.paramX) : (scale * e.paramX * rect.Width);	
					vec.y = filter.primitiveUserSpaceOnUse ? (scale * e.paramY) : (scale * e.paramY * rect.Height);
					vec.w = e.alpha;

					effect.SetVector("param1", vec); 
					effect.SetVector("param2", e.param2);   
					effect.SetVector("param3", e.param3);   
					effect.SetVector("param4", e.param4); 
					effect.SetVector("param5", e.param5);   
					effect.SetVector("param6", e.param6);
					effect.SetTexture("data", e.texture); 
										
					if(EffectType.DIFFUSE_LIGHTING == e.pass || EffectType.SPECULAR_LIGHTING == e.pass) 
					{
 						Vector2 v = Vector2.zero;  
						
						v.x = filter.primitiveUserSpaceOnUse ? e.param4.x : e.param4.x * rect.Width;
						v.y = filter.primitiveUserSpaceOnUse ? e.param4.y : e.param4.y * rect.Height;
						v = transform * v;
						
						vec.x = scale * v.x;	
						vec.y = scale * v.y; 
						vec.z = filter.primitiveUserSpaceOnUse ? e.param4.z : e.param4.z * Mathf.Sqrt(rect.Width*rect.Width + rect.Height*rect.Height);
						vec.w = e.param4.w;
						
						effect.SetVector("param4", vec);
						
						if(0.0f == e.param4.w) 
						{
							v.x = Mathf.Cos(e.param5.x) * Mathf.Cos(e.param5.y); 
							v.y = Mathf.Sin(e.param5.x) * Mathf.Cos(e.param5.y);
							v = transform * v; 
							
							vec.x = scale * v.x - vec.x;	
							vec.y = scale * v.y - vec.y;
							vec.z = Mathf.Sin(e.param5.y) - vec.z;	
						}
						else 
						{  
							v.x = filter.primitiveUserSpaceOnUse ? e.paramX : e.paramX * rect.Width;
							v.y = filter.primitiveUserSpaceOnUse ? e.paramY : e.paramY * rect.Height; 
							v = transform * v; 

							vec.x = scale * v.x;	
							vec.y = scale * v.y;
							vec.z = filter.primitiveUserSpaceOnUse ? e.paramZ : e.paramZ * Mathf.Sqrt(rect.Width*rect.Width + rect.Height*rect.Height);	
						}
						
						effect.SetVector("param1", vec);
					}

					if(EffectType.TILE == e.pass) 
						effect.SetVector("param2", filter.effectList[e.tileEffect].clip);  	 
					
					if(-1 == e.in2)
						effect.SetTexture("in2", filter.targetArray[0]);	
					else
						effect.SetTexture("in2", (0 == e.in2) ? last : filter.targetArray[e.in2]);
					
					Graphics.Blit((0 == e.in1) ? last : filter.targetArray[e.in1], filter.targetArray[e.result], effect, (int)e.pass);
					last = filter.targetArray[e.result]; 
				}  
								
				Graphics.SetRenderTarget(target); 
												
				effect.SetTexture("_MainTex", last);	
				effect.SetPass(0);	   
				
				Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(-parent.center, Quaternion.identity, Vector3.one), screen.subset);
								
				last = null;
				for(int i=0; i<n; ++i)
				{
					filter.targetArray[i].Release();  
					RenderTexture.ReleaseTemporary(filter.targetArray[i]);
					filter.targetArray[i] = null;   
				}
			}
		} 
	}
}