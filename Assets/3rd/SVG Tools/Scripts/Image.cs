// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine; 
using UnityEngine.UI; 
using UnityEngine.Serialization; 

using System;
using System.Collections.Generic;


namespace svgtools
{	
	[Serializable]
	public enum ImageType
	{
		Simple,
		Sliced
	} 

	[ExecuteInEditMode]
	public class Image : UnityEngine.UI.Image 
	{
		[FormerlySerializedAs("scale9Grid")]
        [SerializeField]
        Vector4 scale9Grid = Vector4.zero;
		
		[FormerlySerializedAs("assetName")]
        [SerializeField]
        string assetName = "";	  
		
		[FormerlySerializedAs("assetPath")]
        [SerializeField]
        string assetPath = ""; 	 
		
		[FormerlySerializedAs("imageType")]
        [SerializeField]
        ImageType imageType = ImageType.Simple;	
						
		[SerializeField]
		Mesh mesh = null; 
		

		public string AssetName 
		{
			get { return assetName; }
		}	
		
		public string AssetPath 
		{
			get { return assetPath; }
		}
		
		public ImageType ImageType 
		{
			set { imageType = value; }
		}

		public Mesh Mesh 
		{
			set 
			{
				if(mesh != value) 
				{
					mesh = value;   
					SetAllDirty();
				}
			}
		}
		
		public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) 
		{
			return true;
		}

		public override void SetNativeSize() 
		{
			if(null == mesh)
				return;

			Vector3 size = mesh.bounds.size / pixelsPerUnit;
            rectTransform.anchorMax = rectTransform.anchorMin;
			rectTransform.sizeDelta = new Vector2(size.x, size.y);
			
			SetAllDirty();
		}

		void Simple(Vector3[] inv, UIVertex[] outv) 
		{ 
			UnityEngine.Rect rect  = GetPixelAdjustedRect();
			Vector2          size  = mesh.bounds.size;	
			Vector2          scale = new Vector2(rect.width/size.x, rect.height/size.y);
			Vector3          min   = mesh.bounds.min;

			if(preserveAspect) 
			{
				float r1 = size.x / size.y;	 
				float r2 = rect.width / rect.height;

				if(r1 > r2) 
				{
					float h = rect.height;
                    rect.height = rect.width / r1;
                    rect.y += (h - rect.height) * rectTransform.pivot.y;
				}
				else 
				{
					float w = rect.width;
                    rect.width = rect.height * r1;
                    rect.x += (w - rect.width) * rectTransform.pivot.x;
				} 
				
				scale.Set(rect.width/size.x, rect.height/size.y);
			}
			
			for(int i=0; i<inv.Length; ++i) 
			{
				outv[i].position.Set(scale.x * (inv[i].x - min.x) + rect.x, scale.y * (inv[i].y - min.y) + rect.y, inv[i].z); 
				outv[i].color = color; 
			}
		}	
		
		void Sliced(Vector3[] inv, UIVertex[] outv) 
		{	 
			UnityEngine.Rect rect   = GetPixelAdjustedRect();
			Vector2          size   = mesh.bounds.size;	
			Vector2          scale  = Vector2.zero;	 
			Vector2          offset = Vector2.zero;
			Vector3          min    = mesh.bounds.min;	
			Vector3          max    = mesh.bounds.max;
			Vector3          pos    = Vector3.zero;	
			float            left   = scale9Grid.x;
			float            top    = size.y - scale9Grid.y;
    		float            right  = size.x - scale9Grid.z;
			float            bottom = scale9Grid.w;  
			float            l      = scale9Grid.x;
			float            t      = scale9Grid.y;
    		float            r      = scale9Grid.z;
			float            b      = scale9Grid.w;
			float            sl     = 1.0f;	  
			float            st     = 1.0f;	 
			float            sr     = 1.0f;	  
			float            sb     = 1.0f;
			float            m      = 1.0f;

			if(rect.width < (l+r)) 
			{
				m  = rect.width / (l+r);
				l  = scale9Grid.x * m;
				r  = scale9Grid.z * m;
  				sl = m;   
				sr = m;
			}
			else 
			{ 
				m = size.x - scale9Grid.x - scale9Grid.z;
				scale.x = (m == 0.0f) ? 0.0f : (rect.width - l - r)/m;	
			}	
			
			if(rect.height < (t+b)) 
			{
				m  = rect.height / (t+b);
				t  = scale9Grid.y * m;
				b  = scale9Grid.w * m;
  				st = m;   
				sb = m;
			}
			else 
			{ 
				m = size.y - scale9Grid.y - scale9Grid.w;
				scale.y = (m == 0.0f) ? 0.0f : (rect.height - t - b)/m;	
			}

			offset.x = rect.x + l - l*scale.x;
			offset.y = rect.y + b - b*scale.y;
						
			for(int i=0; i<inv.Length; ++i) 
			{ 
				pos = inv[i] - min;

				outv[i].position.Set(scale.x * pos.x + offset.x, scale.y * pos.y + offset.y, inv[i].z); 
				outv[i].color = color;	
				
				if(pos.x <= left) 
					outv[i].position.x = rect.xMin + sl * pos.x;
				if(pos.x >= right) 
					outv[i].position.x = rect.xMax - (size.x - pos.x) * sr;    
				if(pos.y >= top) 
					outv[i].position.y = rect.yMax - (size.y - pos.y) * st;  
				if(pos.y <= bottom) 
					outv[i].position.y = rect.yMin + sb * pos.y; 
			} 
		}
				
		protected override void OnPopulateMesh(VertexHelper toFill) 
		{
			if(null == mesh) 
			{
				base.OnPopulateMesh(toFill);
				return;
			} 
			
			UIVertex[] vertexList = new UIVertex[mesh.vertexCount];
			Vector3[]  vertices   = mesh.vertices;	
			Vector2[]  uv0        = mesh.uv;	
			Vector2[]  uv1        = mesh.uv2;  
			Vector2[]  uv2        = mesh.uv3;
			Vector2[]  uv3        = mesh.uv4;
			Color32[]  col        = mesh.colors32;

			switch(imageType) 
			{
				case(ImageType.Simple):	 
					Simple(vertices, vertexList);
					break; 
				case(ImageType.Sliced):	 
					Sliced(vertices, vertexList);
					break;	
			}  
			
			if(null != uv0) 
			{ 
				for(int i=0; i<uv0.Length; ++i) 
					vertexList[i].uv0 = uv0[i];
			}	 
			
			if(null != uv1) 
			{ 
				for(int i=0; i<uv1.Length; ++i) 
					vertexList[i].uv1 = uv1[i];
			}
			
			if(null != uv2) 
			{ 
				for(int i=0; i<uv2.Length; ++i) 
					vertexList[i].uv2 = uv2[i];
			}	
			
			if(null != uv3) 
			{ 
				for(int i=0; i<uv3.Length; ++i) 
					vertexList[i].uv3 = uv3[i];
			} 
			
			if(null != col) 
			{ 
				for(int i=0; i<col.Length; ++i) 
					vertexList[i].color = col[i] * color;
			}
			
			toFill.Clear();
			toFill.AddUIVertexStream(new List<UIVertex>(vertexList), new List<int>(mesh.triangles));
		} 		
	}
}