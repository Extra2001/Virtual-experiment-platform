// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

using System.IO; 
using System.Collections.Generic;


namespace svgtools
{
	[System.Serializable]
	public enum AssetType 
	{
		Normal      = 0,
		Transparent = 1,
		UI          = 2,
	}
	

	[System.Serializable]
	public enum MaterialFlags 
	{
		NONE        = 0,
		OPAQUE      = 1,
		TRANSPARENT = 2,
		GRADIENT    = 4,
		LINEAR      = 8,
		RADIAL      = 16,
		FOCAL       = 32, 
		PAD         = 64,
		REPEAT      = 128,
		REFLECT     = 256
	}		
	
	[System.Serializable]
	public struct ColliderPath 
	{
		public Vector2[] points;
	}
	
	
	public class Asset: ScriptableObject 
    { 
		[FormerlySerializedAs("scale9Grid")]
        [SerializeField]
        Vector4 scale9Grid = Vector4.zero; 	 
		
		[FormerlySerializedAs("pivot")]
        [SerializeField]
        Vector2 pivot = Vector2.zero;	
		
		[FormerlySerializedAs("meshCompression")]			  
        [SerializeField]
        ModelImporterMeshCompression meshCompression = ModelImporterMeshCompression.Off;  	  				
		
		[FormerlySerializedAs("assetType")]
        [SerializeField]
        AssetType assetType = AssetType.Transparent;
		
		[FormerlySerializedAs("pivotIndex")]
        [SerializeField]
        int pivotIndex = 0;

        [FormerlySerializedAs("antialiasingWidth")]
        [SerializeField]
        float antialiasingWidth = 0.0f;
        
        [FormerlySerializedAs("scale")]
        [SerializeField]
        float scale = 1.0f;

        [FormerlySerializedAs("curveQuality")]
        [SerializeField]
        float curveQuality = 1.0f;  
		
		[FormerlySerializedAs("quality")]
        [SerializeField]
        float quality = 0.0f;  
		
		[FormerlySerializedAs("depthOffset")]
        [SerializeField]
        float depthOffset = 0.1f;	
		
		[FormerlySerializedAs("offsetFactor")]
        [SerializeField]
        float offsetFactor = 0.0f;  
		
		[FormerlySerializedAs("offsetUnits")]
        [SerializeField]
        float offsetUnits = 0.0f;
		
		[FormerlySerializedAs("colliderMargin")]
        [SerializeField]
        float colliderMargin = 1.0f;  
		
		[FormerlySerializedAs("colliderQuality")]
        [SerializeField]
        float colliderQuality = 1.0f;	
		
		[FormerlySerializedAs("bitmapWidth")]
        [SerializeField]
        int bitmapWidth = 512;
		
		[FormerlySerializedAs("bitmapHeight")]
        [SerializeField]
        int bitmapHeight = 512;
		
		[FormerlySerializedAs("optimizeMesh")]
        [SerializeField]
        bool optimizeMesh = false;
        
        [FormerlySerializedAs("generateCollider")]
        [SerializeField]
        bool generateCollider = false;	 
		
		[FormerlySerializedAs("useLight")]
        [SerializeField]
        bool useLight = false; 	
		
		[FormerlySerializedAs("twoSided")]
        [SerializeField]
        bool twoSided = true;	 
		
		[FormerlySerializedAs("useScale9Grid")]
        [SerializeField]
        bool useScale9Grid = false;
		
		[SerializeField]
        ColliderPath[] colliderPaths = null;  
				        
		[SerializeField]
        string data;	
		
		[SerializeField]
        string info = ""; 
		
		[SerializeField]
		MaterialFlags materialFlags = MaterialFlags.NONE;		
				
		
        Mesh                         mesh        = null;
		Texture2D                    texture     = null;		 
		Material                     opaque      = null;
		Material                     transparent = null;  
		Material[]                   materials   = null;   

        AssetType                    oldAssetType;
		Vector4                      oldScale9Grid;
		Vector2                      oldPivot;	
		ModelImporterMeshCompression oldMeshCompression; 
		int                          oldPivotIndex;
		float                        oldAntialiasingWidth;
        float                        oldScale;
        float                        oldCurveQuality;  
		float                        oldQuality; 
		float                        oldDepthOffset;	
		float                        oldOffsetFactor; 
		float                        oldOffsetUnits;
		float                        oldColliderMargin;  
		float                        oldColliderQuality; 
		int                          oldBitmapWidth;
		int                          oldBitmapHeight;
		bool                         oldGenerateCollider;	
		bool                         oldUseLight;	
		bool                         oldOptimizeMesh;	
		bool                         oldTwoSided;	
		bool                         oldUseScale9Grid;
        bool                         snapShot = false;

        string                       lastRestorePath;

		
		public Mesh Mesh
        {
            get
            {
				if(null == mesh) 
					mesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GetAssetPath(this));

				return mesh;
            }
        }   
						
		public Material Opaque
        {
            get
            {
				if(null == opaque) 
				{
					Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));
							
					foreach(Object a in assets)
					{
						if(a is Material && 0 == string.Compare(a.name, name))
						{ 
							opaque = a as Material;
							break;
						}
					}
				}

				return opaque;
			}
		}	 
		
		public Material Transparent
        {
            get
            {
				if(null == transparent) 
				{ 
					Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));

					foreach(Object a in assets)
					{
						if(a is Material && 0 == string.Compare(a.name, name + "Transparent")) 
						{ 
							transparent = a as Material;
							break;
						}
					}  
				}

				return transparent;
			}
		}
        		
		public Material[] Materials
        {
            get
            {
				if(null == materials) 
				{
					if(0 != (materialFlags & MaterialFlags.OPAQUE) && 0 != (materialFlags & MaterialFlags.TRANSPARENT))	 
						materials = new Material[] { Opaque, Transparent };
					else if(0 != (materialFlags & MaterialFlags.OPAQUE)) 
						materials = new Material[] { Opaque }; 
					else if(0 != (materialFlags & MaterialFlags.TRANSPARENT)) 
						materials = new Material[] { Transparent };  
				} 	   
				
				return materials;
            }
        }

		public Vector4 Scale9Grid 
		{
			get { return scale9Grid; }
		}  
		
		public string Info
		{
			get { return info; }
		}	 
		
		public AssetType Type
		{
			get { return assetType; }
		}
				                
        public void CreateSnapshot()
        {
            if(snapShot)
                return;

            oldAssetType          = assetType;
            oldAntialiasingWidth  = antialiasingWidth;
            oldScale              = scale;
            oldCurveQuality       = curveQuality;	 
			oldQuality            = quality;	  
			oldDepthOffset        = depthOffset;		
			oldOffsetFactor       = offsetFactor; 
			oldOffsetUnits        = offsetUnits;
			oldGenerateCollider   = generateCollider;
			oldUseLight           = useLight;
			oldOptimizeMesh       = optimizeMesh;
			oldTwoSided           = twoSided;
			oldUseScale9Grid      = useScale9Grid;
			oldScale9Grid         = scale9Grid;
			oldPivot              = pivot;  
			oldPivotIndex		  = pivotIndex;
			oldMeshCompression    = meshCompression;
			oldColliderMargin     = colliderMargin;
			oldColliderQuality    = colliderQuality;
			oldBitmapWidth        = bitmapWidth;
			oldBitmapHeight       = bitmapHeight;
            snapShot              = true;
        }
		
		void ApplyMesh()
		{	
			bool sliced = useScale9Grid && AssetType.UI != assetType;
				
			materialFlags = MaterialFlags.NONE;	

			Loader.LoadSVG(data, scale, antialiasingWidth, curveQuality, quality, depthOffset, (AssetType.Normal != assetType), false, false);
			MeshCreator.CreateMesh(pivot, meshCompression, (AssetType.UI == assetType || useScale9Grid) ? scale9Grid : Vector4.zero, optimizeMesh, sliced, ref mesh, ref materialFlags);
				
			if(AssetType.UI != assetType)
			{
				Scale9Grid   s9g;
				GameObject[] objects = Object.FindObjectsOfType<GameObject>();
				
				foreach(GameObject go in objects)
				{
					s9g = go.GetComponent<Scale9Grid>();

					if(null != s9g && sliced && scale9Grid.sqrMagnitude > 0.0f)  
					{
						s9g.s9g = scale9Grid; 
					}
					else if(null != s9g && (!sliced || scale9Grid.sqrMagnitude == 0.0f))
					{  
						Object.DestroyImmediate(s9g);
						s9g = null;
					}
					else if(null == s9g && sliced && scale9Grid.sqrMagnitude > 0.0f)
					{  
						s9g = go.AddComponent<Scale9Grid>();
						s9g.s9g = scale9Grid;	
						s9g.hideFlags = HideFlags.NotEditable;
					}
				}	
			} 
			
			info = mesh.vertexCount + " verts, " + mesh.triangles.Length/3 + " tris   ";  
			
			if(0 == (materialFlags & MaterialFlags.GRADIENT))
 				info += (sliced && scale9Grid.sqrMagnitude > 0.0f) ? "colors, tangents" : "colors, uv";
			else if(0 != (materialFlags & MaterialFlags.LINEAR))	
				info += (sliced && scale9Grid.sqrMagnitude > 0.0f) ? "tangents, uv" : "uv, uv2";	
			else if(0 != (materialFlags & MaterialFlags.RADIAL))	
				info += (sliced && scale9Grid.sqrMagnitude > 0.0f) ? "tangents, uv, uv2" : "uv, uv2, uv3"; 	
			else if(0 != (materialFlags & MaterialFlags.FOCAL))	
				info += (sliced && scale9Grid.sqrMagnitude > 0.0f) ? "tangents, uv, uv2, uv3" : "uv, uv2, uv3, uv4";
		}
				
		void SetupMaterial(Material m, bool transparent) 
		{
 			if(AssetType.UI == assetType)	
				m.shader = Shader.Find(0 != (materialFlags & MaterialFlags.GRADIENT) ? "SVG Tools/UIGradient" : "SVG Tools/UISolid");
			else if(0 != (materialFlags & MaterialFlags.GRADIENT))
				m.shader = Shader.Find(useLight ? "SVG Tools/DiffuseGradient" : "SVG Tools/Gradient");
			else
				m.shader = Shader.Find(useLight ? "SVG Tools/DiffuseSolid" : "SVG Tools/Solid"); 

			if(0 != (materialFlags & MaterialFlags.RADIAL))
				m.EnableKeyword("SVG_TOOLS_RADIAL");
			else
				m.DisableKeyword("SVG_TOOLS_RADIAL");  
			
			if(0 != (materialFlags & MaterialFlags.FOCAL))
				m.EnableKeyword("SVG_TOOLS_FOCAL");
			else
				m.DisableKeyword("SVG_TOOLS_FOCAL"); 

			if(0 != (materialFlags & MaterialFlags.REFLECT))  
				m.EnableKeyword("SVG_TOOLS_REFLECT");	 
			else
				m.DisableKeyword("SVG_TOOLS_REFLECT");

			if(0 != (materialFlags & MaterialFlags.PAD) && (0 != (materialFlags & MaterialFlags.REPEAT) || 0 != (materialFlags & MaterialFlags.REFLECT)))
				m.EnableKeyword("SVG_TOOLS_PAD");
			else
				m.DisableKeyword("SVG_TOOLS_PAD");  
			
			if(AssetType.UI != assetType && useScale9Grid && scale9Grid.sqrMagnitude > 0.0f)
				m.EnableKeyword("SVG_TOOLS_SLICED");
			else
				m.DisableKeyword("SVG_TOOLS_SLICED"); 	
			
			if(0 != (materialFlags & MaterialFlags.OPAQUE) && AssetType.UI != assetType && twoSided)  
				m.EnableKeyword("SVG_TOOLS_TWO_SIDED");	
			else 
				m.DisableKeyword("SVG_TOOLS_TWO_SIDED"); 
						
			if(twoSided)  
				m.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off); 
			else 
				m.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Back);
			
			if(transparent)	
			{
				m.EnableKeyword("SVG_TOOLS_TRANSPARENT");
				m.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);	
				m.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);	
				m.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
				m.SetFloat("_ZWrite", 0.0f);  
				m.SetOverrideTag("RenderType", "Transparent"); 
				m.renderQueue = 3000;
			}
			else 
			{ 
				m.DisableKeyword("SVG_TOOLS_TRANSPARENT");	
				m.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);	
				m.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);	
				m.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Less);
				m.SetFloat("_ZWrite", 1.0f);  
				m.SetFloat("_Factor", offsetFactor);	
				m.SetFloat("_Units", offsetUnits);
				m.SetOverrideTag("RenderType", "Opaque"); 
				m.renderQueue = 2000;
			}
			
			if(0 != (materialFlags & MaterialFlags.GRADIENT))
				m.SetTexture("colorMap", texture);	
			
			m.hideFlags = HideFlags.NotEditable;		
		}

		void ApplyMaterials() 
		{
			if(0 != (materialFlags & MaterialFlags.GRADIENT) && null == texture) 
				texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(this));

			if(0 != (materialFlags & MaterialFlags.OPAQUE) && 0 != (materialFlags & MaterialFlags.TRANSPARENT) && 2 != Materials.Length)
				System.Array.Resize(ref materials, 2);
			else if((0 == (materialFlags & MaterialFlags.OPAQUE) || 0 == (materialFlags & MaterialFlags.TRANSPARENT)) && 1 != Materials.Length)
				System.Array.Resize(ref materials, 1);

			if(2 == materials.Length)
			{
				if(null == opaque)
				{
					opaque = new Material(Shader.Find("SVG Tools/Solid"));
					opaque.name = name; 
					AssetDatabase.AddObjectToAsset(opaque, this);
				}  
					
				if(null == transparent)
				{	
					transparent = new Material(Shader.Find("SVG Tools/Solid"));
					transparent.name = name + "Transparent"; 
					AssetDatabase.AddObjectToAsset(transparent, this);
				}

				materials[0] = Opaque;
				materials[1] = Transparent;	 
				
				SetupMaterial(materials[0], false);
				SetupMaterial(materials[1], true);
			}
			else if(0 != (materialFlags & MaterialFlags.OPAQUE))
			{
				if(null == opaque)
				{	
					opaque = new Material(Shader.Find("SVG Tools/Solid"));
					opaque.name = name; 
					AssetDatabase.AddObjectToAsset(opaque, this);
				}  
					
				if(null != transparent)
					DestroyImmediate(transparent, true);
				
				materials[0] = Opaque; 
				
				SetupMaterial(materials[0], false);
			}
			else if(0 != (materialFlags & MaterialFlags.TRANSPARENT))
			{
				if(null == transparent)
				{	
					transparent = new Material(Shader.Find("SVG Tools/Solid"));
					transparent.name = name + "Transparent"; 
					AssetDatabase.AddObjectToAsset(transparent, this);
				} 
					
				if(null != opaque)
					DestroyImmediate(opaque, true);
				
				materials[0] = Transparent;	 
				
				SetupMaterial(materials[0], true);
			}

			if(AssetType.UI != assetType)
			{
				MeshFilter   filter   = null;
				MeshRenderer renderer = null; 
				GameObject[] objects  = Object.FindObjectsOfType<GameObject>();
				
				foreach(GameObject go in objects)
				{
					filter   = go.GetComponent<MeshFilter>();
					renderer = go.GetComponent<MeshRenderer>();	 

					if(null != filter && null != renderer && mesh == filter.sharedMesh)
						renderer.sharedMaterials = Materials;
				}  
			}
		}

		void ApplyCollider()
		{
			MeshFilter        filter;  
			PolygonCollider2D collider;
			GameObject[]      objects = Object.FindObjectsOfType<GameObject>();	 
			
			if(generateCollider) 
			{ 
				Loader.LoadSVG(data, scale, 0.0f, curveQuality, colliderQuality, 0.0f, false, true, false);
				MeshCreator.CreateCollider(mesh.bounds.center, colliderMargin, ref colliderPaths);	
			}
			
			foreach(GameObject go in objects) 
			{	
				filter = go.GetComponent<MeshFilter>();	  

				if(null != filter && mesh == filter.sharedMesh) 
				{ 
					collider = go.GetComponent<PolygonCollider2D>();	

					if(generateCollider && null == collider)
					{	
						collider = go.AddComponent<PolygonCollider2D>();  
						collider.pathCount = colliderPaths.Length;
			
						for(int i=0; i<collider.pathCount; ++i)
							collider.SetPath(i, colliderPaths[i].points);
					}  
					else if(generateCollider && null != collider)
					{ 
						collider.pathCount = colliderPaths.Length;
			
						for(int i=0; i<collider.pathCount; ++i)
							collider.SetPath(i, colliderPaths[i].points);
					}
					else if(!generateCollider && null != collider) 
					{
						collider.pathCount = 0;
						collider.points = null;
						Object.DestroyImmediate(collider); 
						collider = null;
					}
				}
			}
			
			if(!generateCollider && null != colliderPaths) 
			{  
				for(int i=0; i<colliderPaths.Length; ++i)
					colliderPaths[i].points = null;	
						
				colliderPaths = null;
			}	
		}
		
		void ApplyPivot() 
		{
			Vector3[] vertices = mesh.vertices;
			Vector3   offset   = Vector3.zero;
			Vector3   vec      = Vector3.zero;	
			Bounds    bounds   = mesh.bounds;	
				
			offset.x = bounds.min.x + bounds.size.x * pivot.x; 
			offset.y = bounds.min.y + bounds.size.y * pivot.y;

			for(int i=0; i<vertices.Length; ++i) 
				vertices[i] -= offset; 	
				
			mesh.vertices = vertices;
			mesh.RecalculateBounds();  
			
			bounds      = mesh.bounds;
			vec         = bounds.min; 
			vec.z       = 0.0f;  
			bounds.min  = vec; 	
			vec         = bounds.max; 
			vec.z       = 0.0f;  
			bounds.max  = vec;
			mesh.bounds = bounds;
			
			if(generateCollider)
			{ 
				MeshFilter        filter; 
				PolygonCollider2D collider;
				GameObject[]      objects = Object.FindObjectsOfType<GameObject>();	 
				
				foreach(ColliderPath p in colliderPaths) 
				{
					for(int i=0; i<p.points.Length; ++i) 
					{ 
						p.points[i].x -= offset.x; 
						p.points[i].y -= offset.y;
					}
				}
				
				foreach(GameObject go in objects) 
				{	
					filter = go.GetComponent<MeshFilter>();	

					if(null != filter && filter.sharedMesh == mesh) 
					{ 
						collider = go.GetComponent<PolygonCollider2D>();

						if(null != collider)
						{ 
							collider.pathCount = colliderPaths.Length;
			
							for(int i=0; i<collider.pathCount; ++i)
								collider.SetPath(i, colliderPaths[i].points);
						}
					}
				}	
			}
		}
				
		public void Apply() 
        {
            snapShot = false;

            if(9 != pivotIndex)  
			{	
				pivot.x = 0.5f * (pivotIndex % 3); 
				pivot.y = 0.5f * (pivotIndex / 3);
			}  
			
			if(antialiasingWidth != oldAntialiasingWidth || curveQuality != oldCurveQuality || quality != oldQuality || scale != oldScale || optimizeMesh != oldOptimizeMesh || meshCompression != oldMeshCompression || assetType != oldAssetType || depthOffset != oldDepthOffset || scale9Grid != oldScale9Grid || useScale9Grid != oldUseScale9Grid)
				ApplyMesh();	

			if(antialiasingWidth != oldAntialiasingWidth || useLight != oldUseLight || assetType != oldAssetType || twoSided != oldTwoSided || useScale9Grid != oldUseScale9Grid || scale9Grid != oldScale9Grid || offsetFactor != oldOffsetFactor || offsetUnits != oldOffsetUnits)
				ApplyMaterials();				
			
			if(generateCollider != oldGenerateCollider || colliderMargin != oldColliderMargin || colliderQuality != oldColliderQuality || antialiasingWidth != oldAntialiasingWidth || curveQuality != oldCurveQuality || scale != oldScale || assetType != oldAssetType) 
				ApplyCollider(); 
				
			if(assetType != oldAssetType || pivot != oldPivot)
				ApplyPivot();

			CreateSnapshot();  
        }
        
        public void Revert() 
        {
			assetType          = oldAssetType;
            antialiasingWidth  = oldAntialiasingWidth;
            scale              = oldScale;
			curveQuality       = oldCurveQuality;
            quality            = oldQuality;
			depthOffset        = oldDepthOffset;	
			offsetFactor       = oldOffsetFactor;  
			offsetUnits        = oldOffsetUnits;
			generateCollider   = oldGenerateCollider;
			useLight           = oldUseLight;
			optimizeMesh       = oldOptimizeMesh;
			twoSided           = oldTwoSided;
			useScale9Grid      = oldUseScale9Grid;
			scale9Grid         = oldScale9Grid;
			pivot              = oldPivot;
			pivotIndex         = oldPivotIndex;
			meshCompression    = oldMeshCompression;
			colliderMargin     = oldColliderMargin;
			colliderQuality    = oldColliderQuality;
			bitmapWidth        = oldBitmapWidth;
			bitmapHeight       = oldBitmapHeight;
        }

		public void Restore() 
		{
			string path = string.IsNullOrEmpty(lastRestorePath) ? AssetDatabase.GetAssetPath(this) : lastRestorePath + "/";	   
			string svg  = EditorUtility.SaveFilePanel("Restore SVG File", System.IO.Path.GetDirectoryName(path), name, "svg" );

			if(string.IsNullOrEmpty(svg))
				return;

			lastRestorePath = System.IO.Path.GetDirectoryName(svg);	
			File.WriteAllText(svg, data);	
			EditorUtility.RevealInFinder(svg);
		} 
		
		public void ExportToBitmap() 
		{
			Loader.LoadSVG(data, 1.0f, antialiasingWidth, curveQuality, quality, 0.0f, true, false, true);	
 
			float     s    = 1.0f;
			Texture2D tex  = MeshCreator.DrawToTexture(bitmapWidth, bitmapHeight, ref s); 
			byte[]    buf  = tex.EncodeToPNG();
			string    path = AssetDatabase.GetAssetPath(this);
            
			System.IO.File.WriteAllBytes(path.Substring(0, path.Length - 5) + "png", buf);
			DestroyImmediate(tex);	 
		} 

		public GameObject CreateGameObject(Vector3 pos) 
		{ 
			if(AssetType.UI == assetType) 
				return null;

			GameObject go = new GameObject(name);
			go.transform.position = pos;

			MeshRenderer renderer;
			MeshFilter   filter;
				
			renderer = go.AddComponent<MeshRenderer>();	
			renderer.sharedMaterials      = Materials;	     
			renderer.receiveShadows       = false;	
			renderer.shadowCastingMode    = UnityEngine.Rendering.ShadowCastingMode.Off;
			renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;	  
			renderer.lightProbeUsage      = UnityEngine.Rendering.LightProbeUsage.Off; 
                    
			filter = go.AddComponent<MeshFilter>();
			filter.sharedMesh = Mesh;  	
				
			if(useScale9Grid && scale9Grid.sqrMagnitude > 0.0f) 
			{	
				Scale9Grid s9g = go.AddComponent<Scale9Grid>();
				s9g.s9g = scale9Grid;
				s9g.hideFlags = HideFlags.NotEditable;
			}
			
			if(generateCollider && null != colliderPaths && colliderPaths.Length > 0)
			{ 
				PolygonCollider2D collider = go.AddComponent<PolygonCollider2D>();  
				collider.pathCount = colliderPaths.Length;
			
				for(int i=0; i<collider.pathCount; ++i)
					collider.SetPath(i, colliderPaths[i].points);
			}

			return go;
		}
		
		public static void Create(string path) 
        {
            Asset asset = AssetDatabase.LoadAssetAtPath<Asset>(path.Substring(0, path.Length - 3) + "asset");

			if(null != asset)
				return;

			StreamReader reader = new StreamReader(path); 
			
			asset = ScriptableObject.CreateInstance<Asset>();
			asset.data = reader.ReadToEnd();
		 	            
            reader.Close();
			
			AssetDatabase.DeleteAsset(path);   
			AssetDatabase.CreateAsset(asset, path.Substring(0, path.Length - 3) + "asset");	 
			
			asset.mesh = new Mesh();
			asset.mesh.name = asset.name;  
			AssetDatabase.AddObjectToAsset(asset.mesh, asset);
			
			asset.ApplyMesh();	
			
			if(0 != (asset.materialFlags & MaterialFlags.GRADIENT))
			{ 
				asset.texture = MeshCreator.CreateTexture(asset.name);
				AssetDatabase.AddObjectToAsset(asset.texture, asset);
			}
			
			asset.ApplyMaterials();
			asset.CreateSnapshot(); 
		}  		
	}	   
}
