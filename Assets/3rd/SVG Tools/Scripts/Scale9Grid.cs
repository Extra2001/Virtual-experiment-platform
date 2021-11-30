// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine; 
using UnityEngine.Serialization; 


namespace svgtools
{
	[ExecuteInEditMode]
	public class Scale9Grid : MonoBehaviour 
	{	
		Mesh                  mesh  = null; 
		MeshRenderer          mr    = null; 
		MaterialPropertyBlock block = null;
		
		[HideInInspector]
		[SerializeField]
		public Vector4 s9g = Vector4.zero;		

		
		void OnWillRenderObject() 
		{
			if(null == mesh) 
			{
				MeshFilter filter = GetComponent<MeshFilter>();  
				if(null != filter)
					mesh = filter.sharedMesh;
			}  
			
			if(null == mr)
				mr = GetComponent<MeshRenderer>();

			if(null == block)
				block = new MaterialPropertyBlock();

			if(null == block || null == mesh || null == mr)
				return;	 

			Vector4 scaleX	   = Vector4.one; 
			Vector4 scaleY	   = Vector4.one; 
			Vector4 offsetX	   = Vector4.zero; 
			Vector4 offsetY	   = Vector4.zero;
			Vector2 min        = mesh.bounds.min;	
			Vector2 max        = mesh.bounds.max;
			float   width      = mesh.bounds.size.x;   
			float   height     = mesh.bounds.size.y; 
			float   rectWidth  = width * transform.lossyScale.x; 
			float   rectHeight = height * transform.lossyScale.y;  
			float   left       = s9g.x;
			float   top        = s9g.y;
    		float   right      = s9g.z;
			float   bottom     = s9g.w;	
  			float   m          = 1.0f;
			
			if(rectWidth < (left+right)) 
			{
				m        = rectWidth / (left+right);
				left     = s9g.x * m;
				right    = s9g.z * m;
  				scaleX.x = m;   
				scaleX.z = m;	
				scaleX.y = 0.0f;
			}
			else 
			{ 
				m = width - s9g.x - s9g.z;
				scaleX.y = (m == 0.0f) ? 0.0f : (rectWidth - left - right)/m;	
			}	
			
			if(rectHeight < (top+bottom)) 
			{
				m        = rectHeight / (top+bottom);
				top      = s9g.y * m;
				bottom   = s9g.w * m;
  				scaleY.x = m;   
				scaleY.z = m;	
				scaleY.y = 0.0f;
			}
			else 
			{ 
				m = height - s9g.y - s9g.w;
				scaleY.y = (m == 0.0f) ? 0.0f : (rectHeight - top - bottom)/m;	
			} 
			
			offsetX.Set(min.x * (transform.lossyScale.x - scaleX.x), min.x * transform.lossyScale.x + left - (left + min.x) * scaleX.y, max.x * transform.lossyScale.x - (width + min.x)*scaleX.z, 0.0f);	 
			offsetY.Set(max.y * transform.lossyScale.y - (height + min.y)*scaleY.x, min.y * transform.lossyScale.y + bottom - (bottom + min.y)*scaleY.y, min.y * (transform.lossyScale.y - scaleY.z), 0.0f);

			scaleX  /= transform.lossyScale.x;	
			scaleY  /= transform.lossyScale.y;  
			offsetX	/= transform.lossyScale.x;	 
			offsetY	/= transform.lossyScale.y;
 
			mr.GetPropertyBlock(block);
			
			block.SetVector("scaleX", scaleX);	
			block.SetVector("scaleY", scaleY);
			block.SetVector("offsetX", offsetX); 
			block.SetVector("offsetY", offsetY);
			
			mr.SetPropertyBlock(block);
		}
	}
}