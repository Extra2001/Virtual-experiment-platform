// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;
using UnityEditor;

using System.Collections.Generic;	


namespace svgtools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Asset))]
    public class AssetInspector : Editor			 
    { 
		static public AssetInspector       instance                        = null;
		static public RenderTexture        renderTarget                    = null;
        
        static public GUIContent           assetTypeField                  = new GUIContent("Type", "Asset Type");
		static public GUIContent           antialiasingWidthPropertyField  = new GUIContent("Antialiasing Width", "Antialiasing Width");		  
        static public GUIContent           scalePropertyField              = new GUIContent("Scale", "Scale");
        static public GUIContent           optimizeMeshPropertyField       = new GUIContent("Optimize Mesh", "Optimize Mesh");
        static public GUIContent           curveQualityPropertyField       = new GUIContent("Curve Quality", "Curve Quality");	
		static public GUIContent           qualityPropertyField            = new GUIContent("Quality", "Quality");	 
		static public GUIContent           depthOffsetField                = new GUIContent("Depth Offset", "Depth Offset");	
		static public GUIContent           offsetFactorPropertyField       = new GUIContent("Offset Factor", "Offset Factor");
		static public GUIContent           offsetUnitsPropertyField        = new GUIContent("Offset Units", "Offset Units");
		static public GUIContent           meshCompressionPropertyField    = new GUIContent("Mesh Compression", "Mesh Compression");
        static public GUIContent           generateColliderPropertyField   = new GUIContent("Generate", "Generate Collider");  	   
		static public GUIContent           useLightPropertyField           = new GUIContent("Use Light", "Use Light");  
		static public GUIContent           twoSidedPropertyField           = new GUIContent("Two Sided", "Two Sided");
		static public GUIContent           useScale9GridPropertyField      = new GUIContent("Use Scale9Grid", "Use Scale9Grid");
		static public GUIContent           pivotPropertyField              = new GUIContent("Pivot", "Pivot");	
		static public GUIContent           customPivotPropertyField        = new GUIContent("Custom Pivot", "Custom Pivot");
		static public GUIContent           colliderMarginPropertyField     = new GUIContent("Margin", "Margin");	
		static public GUIContent           colliderQualityPropertyField    = new GUIContent("Quality", "Quality"); 	
		static public GUIContent           bitmapWidthPropertyField        = new GUIContent("Width", "Bitmap Width");	
		static public GUIContent           bitmapHeightPropertyField       = new GUIContent("Height", "Bitmap Height");	
		static public GUIContent[]         pivotPositionPropertyField      = { new GUIContent("Bottom Left"), new GUIContent("Bottom"), new GUIContent("Bottom Right"), new GUIContent("Left"), new GUIContent("Center"), new GUIContent("Right"), new GUIContent("Top Left"), new GUIContent("Top"), new GUIContent("Top Right"), new GUIContent("Custom") };  
        
        public        SerializedProperty   assetType; 
		public        SerializedProperty   antialiasingWidth;
        public        SerializedProperty   scale; 
        public        SerializedProperty   curveQuality;	
		public        SerializedProperty   quality;	   
		public        SerializedProperty   depthOffset;	   
		public        SerializedProperty   offsetFactor; 
		public        SerializedProperty   offsetUnits;
		public        SerializedProperty   meshCompression; 	
		public        SerializedProperty   optimizeMesh;
		public        SerializedProperty   useLight;	
		public        SerializedProperty   twoSided;
		public        SerializedProperty   useScale9Grid;
		public        SerializedProperty   scale9Grid;
		public        SerializedProperty   pivot; 
		public        SerializedProperty   pivotIndex; 
		public        SerializedProperty   generateCollider;	 
		public        SerializedProperty   colliderMargin; 
		public        SerializedProperty   colliderQuality;	
		public        SerializedProperty   bitmapWidth; 
		public        SerializedProperty   bitmapHeight;   

        public        bool                 changeCheck;

        
        void OnEnable()
        {
            Asset asset;  
			
			if(null == renderTarget) 
			{	
				renderTarget = RenderTexture.GetTemporary(32, 32, 16, RenderTextureFormat.ARGB32); 
				renderTarget.filterMode = FilterMode.Point;  
				renderTarget.hideFlags  = HideFlags.HideAndDontSave;
				renderTarget.Create(); 
			}
						            
            if(serializedObject.isEditingMultipleObjects) 
            {
                foreach(Object t in targets) 
                {
                    asset = t as Asset;
                    asset.CreateSnapshot();
                }
            }
            else 
            {
                asset = target as Asset;
                asset.CreateSnapshot();	 
            }

			assetType          = serializedObject.FindProperty("assetType");
            antialiasingWidth  = serializedObject.FindProperty("antialiasingWidth");
            scale              = serializedObject.FindProperty("scale");
            optimizeMesh       = serializedObject.FindProperty("optimizeMesh"); 
            curveQuality       = serializedObject.FindProperty("curveQuality");	
			quality            = serializedObject.FindProperty("quality");	 	  
			depthOffset        = serializedObject.FindProperty("depthOffset");	   
			offsetFactor       = serializedObject.FindProperty("offsetFactor"); 
			offsetUnits        = serializedObject.FindProperty("offsetUnits");
			meshCompression    = serializedObject.FindProperty("meshCompression");
            generateCollider   = serializedObject.FindProperty("generateCollider");	  	
			useLight           = serializedObject.FindProperty("useLight");	 
			twoSided           = serializedObject.FindProperty("twoSided");	
			useScale9Grid      = serializedObject.FindProperty("useScale9Grid"); 
			scale9Grid         = serializedObject.FindProperty("scale9Grid");
			pivot              = serializedObject.FindProperty("pivot");	 
			pivotIndex         = serializedObject.FindProperty("pivotIndex");   
			colliderMargin     = serializedObject.FindProperty("colliderMargin");	 
			colliderQuality    = serializedObject.FindProperty("colliderQuality");  
			bitmapWidth        = serializedObject.FindProperty("bitmapWidth");	 
			bitmapHeight       = serializedObject.FindProperty("bitmapHeight");   
            changeCheck        = false; 
			instance           = this;

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += this.OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
#endif
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }
        
        void OnDisable()
        {
            if(changeCheck /*|| (null != PivotEditor.instance && PivotEditor.instance.changeCheck)*/) 
            {
                string asset = "";	
				
				if(null != PivotEditor.instance)
					PivotEditor.instance.changeCheck = false;

                if(serializedObject.isEditingMultipleObjects)
                    asset += targets.Length;
                else 
                    asset = AssetDatabase.GetAssetPath(target);

                if(EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + asset + "'", "Apply", "Revert"))
                    ApplyChanges();
                else 
                    RevertChanges();  				
            }

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= this.OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
#endif
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
			instance = null;   
        }
        
        void OnDestroy()
        {
			instance = null;
        }
                
        void OnSceneGUI(SceneView sceneView)
        {
            EventType type = Event.current.type;

            if(EventType.DragUpdated == type) 
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                Event.current.Use();
            }
            else if(EventType.DragPerform == type) 
            { 
                Asset      asset;  
				GameObject go;
                Vector3    pos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(5.0f);

                foreach(Object obj in DragAndDrop.objectReferences) 
                {
                    if(obj is Asset) 
                    { 
                        asset = obj as Asset;	
						go    = asset.CreateGameObject(pos);

                        if(null != go)
						{
							Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
							Selection.activeGameObject = go; 
						}
                    } 
                }
                
                Event.current.Use();
            }
        }

		void HierarchyWindowItemOnGUI(int instanceID, UnityEngine.Rect selectionRect) 
		{	
			EventType type = Event.current.type;	

			if(EventType.DragUpdated != type && EventType.DragPerform != type && EventType.DragExited != type)
				return;

			DragAndDrop.visualMode = DragAndDropVisualMode.Link;

			if(selectionRect.Contains(Event.current.mousePosition) && EventType.DragPerform == type) 
            { 
                Asset      asset;  
				GameObject go;	 
				GameObject parent = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                foreach(Object obj in DragAndDrop.objectReferences) 
                {
                    if(obj is Asset) 
                    {
                        asset = obj as Asset;	
						go    = asset.CreateGameObject(Vector3.zero);

						if(null != go)
						{
							go.transform.SetParent(parent.transform);
							go.transform.localPosition = Vector3.zero;	
							go.transform.localRotation = Quaternion.identity; 
							go.transform.localScale    = Vector3.one;


							Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
							Selection.activeGameObject = go;  }
                    } 
                }  
                
				Event.current.Use();
            }	
			else if(!selectionRect.Contains(Event.current.mousePosition) && EventType.DragExited == type) 
            { 
                Asset      asset;  
				GameObject go;	 

                foreach(Object obj in DragAndDrop.objectReferences) 
                {
                    if(obj is Asset) 
                    { 
                        asset = obj as Asset;	
						go    = asset.CreateGameObject(Vector3.zero);

                        if(null != go)
						{
							Selection.activeGameObject = go;
							Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);  
							Undo.PerformRedo();	 
						}
                    } 
                }  
                
				Event.current.Use();
            }	
		}

        public override string GetInfoString() 
        {  
			Asset asset = target as Asset;	 

            return null == asset ? "" : asset.Info;
        }

        public override void OnInspectorGUI() 
        { 
			instance = this;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck(); 
			
			EditorGUILayout.LabelField("Mesh", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(assetType, assetTypeField);	
			EditorGUILayout.PropertyField(antialiasingWidth, antialiasingWidthPropertyField);
            EditorGUILayout.PropertyField(scale, scalePropertyField);
            EditorGUILayout.PropertyField(curveQuality, curveQualityPropertyField);	
			EditorGUILayout.PropertyField(quality, qualityPropertyField);		 
			EditorGUILayout.PropertyField(depthOffset, depthOffsetField);		  
			EditorGUILayout.PropertyField(offsetFactor, offsetFactorPropertyField); 
			EditorGUILayout.PropertyField(offsetUnits, offsetUnitsPropertyField);
			EditorGUILayout.PropertyField(meshCompression, meshCompressionPropertyField);
            EditorGUILayout.PropertyField(optimizeMesh, optimizeMeshPropertyField);	 	
			EditorGUILayout.PropertyField(useLight, useLightPropertyField);	
			EditorGUILayout.PropertyField(twoSided, twoSidedPropertyField);	 	
			EditorGUILayout.PropertyField(useScale9Grid, useScale9GridPropertyField);
			
			antialiasingWidth.floatValue = antialiasingWidth.floatValue < 0.0f ? 0.0f : antialiasingWidth.floatValue;	
			scale.floatValue = scale.floatValue < 0.0f ? 0.0f : scale.floatValue;  
			curveQuality.floatValue = curveQuality.floatValue < 0.0f ? 0.0f : curveQuality.floatValue; 
			quality.floatValue = quality.floatValue < 0.0f ? 0.0f : quality.floatValue; 
			depthOffset.floatValue = depthOffset.floatValue < 0.0f ? 0.0f : depthOffset.floatValue;
			
			GUILayout.BeginHorizontal();
			pivotIndex.intValue = EditorGUILayout.Popup(pivotPropertyField, pivotIndex.intValue, pivotPositionPropertyField); 
			
			if(EditorGUI.EndChangeCheck()) 
                changeCheck = true;				 
			
			GUI.enabled = !serializedObject.isEditingMultipleObjects;				
			if(GUILayout.Button("Pivot Editor", EditorStyles.miniButton, GUILayout.Width(75.0f)))
                PivotEditor.ShowEditor();
			GUILayout.EndHorizontal();

			if(9 == pivotIndex.intValue)
			{ 
				EditorGUI.BeginChangeCheck();	

				EditorGUIUtility.wideMode = true; 
				GUILayout.BeginHorizontal(); 
				EditorGUILayout.PropertyField(pivot, customPivotPropertyField, GUILayout.MinWidth(325.0f)); 	
				GUILayout.EndHorizontal();	
				EditorGUIUtility.wideMode = false;
				
				if(EditorGUI.EndChangeCheck()) 
					changeCheck = true;
			}  

			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Collider", EditorStyles.boldLabel);   
			GUI.enabled = generateCollider.boolValue;
			EditorGUILayout.PropertyField(colliderMargin, colliderMarginPropertyField);	 
			EditorGUILayout.PropertyField(colliderQuality, colliderQualityPropertyField); 
			GUI.enabled = true;
			EditorGUILayout.PropertyField(generateCollider, generateColliderPropertyField);	  
			
			colliderMargin.floatValue = colliderMargin.floatValue < 0.0f ? 0.0f : colliderMargin.floatValue;	
			colliderQuality.floatValue = colliderQuality.floatValue < 0.0f ? 0.0f : colliderQuality.floatValue;
			
			if(EditorGUI.EndChangeCheck()) 
                changeCheck = true;	   
			
			EditorGUI.BeginChangeCheck(); 
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Bitmap", EditorStyles.boldLabel); 
			EditorGUILayout.PropertyField(bitmapWidth, bitmapWidthPropertyField);	 
			EditorGUILayout.PropertyField(bitmapHeight, bitmapHeightPropertyField);
			
			bitmapWidth.intValue = bitmapWidth.intValue < 0 ? 0 : bitmapWidth.intValue;	
			bitmapHeight.intValue = bitmapHeight.intValue < 0 ? 0 : bitmapHeight.intValue;
			
			if(EditorGUI.EndChangeCheck()) 
                changeCheck = true;
            
            GUI.enabled = changeCheck && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Revert", GUILayout.MinWidth(55.0f)))
                RevertChanges();

            if(GUILayout.Button("Apply", GUILayout.MinWidth(50.0f))) 
                ApplyChanges();
            
            GUILayout.EndHorizontal(); 
			
			GUI.enabled = true;	
			EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Export to Bitmap"))
				ExportToBitmap();
			
			if(GUILayout.Button("Restore SVG File"))
				Restore(); 
			
			GUILayout.EndHorizontal(); 
        }	
		
		public override bool HasPreviewGUI() 
        {  
			Asset asset = target as Asset;

            return (null != asset && null != AssetInspector.renderTarget && ShaderUtil.hardwareSupportsRectRenderTexture && null != asset.Mesh && null != asset.Materials); 
        }

        public override void OnPreviewGUI(UnityEngine.Rect rect, GUIStyle background) 
        {
            if(Event.current.type != EventType.Repaint)
                return;

			Asset   asset = target as Asset;
			Vector2 size  = asset.Mesh.bounds.size;	
			Vector3 pos   = -asset.Mesh.bounds.center;  
			float   zoom  = Mathf.Min(rect.width/size.x, rect.height/size.y) * 0.8f;	
			float   hw    = 0.5f*rect.width/zoom;
			float   hh	  = 0.5f*rect.height/zoom;
			
			pos.z = -200.0f;
						
			if(renderTarget.width != rect.width || renderTarget.height != rect.height) 	  
			{	
				renderTarget.Release();  
				RenderTexture.ReleaseTemporary(renderTarget);
				renderTarget = RenderTexture.GetTemporary((int)rect.width, (int)rect.height, 16, RenderTextureFormat.ARGB32); 
				renderTarget.filterMode = FilterMode.Point;  	 
				renderTarget.hideFlags  = HideFlags.HideAndDontSave;
				renderTarget.Create(); 
			}   
			 
			Graphics.SetRenderTarget(renderTarget);
			GL.Clear(true, true, new Color32(49, 49, 49, 255), 1.0f);  
			
			GL.PushMatrix(); 
			GL.LoadProjectionMatrix(GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(-hw, hw, -hh, hh, 0.1f, 1000.0f), false));
			
			for(int i=0; i<asset.Materials.Length; ++i) 
			{ 
				Shader.SetGlobalVector("_WorldSpaceLightPos0", Vector3.back);
				asset.Materials[i].SetPass(0);
				Graphics.DrawMeshNow(asset.Mesh, pos, Quaternion.identity, i);	
			} 

			GL.PopMatrix();	   
			
			Graphics.SetRenderTarget(null);	
			EditorGUI.DrawPreviewTexture(rect, renderTarget, null, ScaleMode.StretchToFill);
        }
        
        public override Texture2D RenderStaticPreview(string path, UnityEngine.Object[] subassets, int width, int height) 
        {
            Asset asset = target as Asset;

			if(null == asset || null == asset.Mesh || null == asset.Materials || null == AssetInspector.renderTarget)
				return null;   
				
			Vector2 size = asset.Mesh.bounds.size;	
			Vector3 pos  = -asset.Mesh.bounds.center;						
			float   zoom = Mathf.Min(width/size.x, height/size.y);	
			float   hw   = 0.5f*width/zoom;
			float   hh	 = 0.5f*height/zoom;
			
			pos.z = -200.0f;
						
			if(renderTarget.width != width || renderTarget.height != height) 	  
			{	
				renderTarget.Release();  
				RenderTexture.ReleaseTemporary(renderTarget);
				renderTarget = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGB32); 
				renderTarget.filterMode = FilterMode.Point;  	 
				renderTarget.hideFlags  = HideFlags.HideAndDontSave;
				renderTarget.Create(); 
			}   
			 
			Graphics.SetRenderTarget(renderTarget);
			GL.Clear(true, true, new Color32(82, 82, 82, 255), 1.0f);  
			
			GL.PushMatrix(); 
			GL.LoadProjectionMatrix(GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(-hw, hw, -hh, hh, 0.1f, 1000.0f), false));
			
			for(int i=0; i<asset.Materials.Length; ++i) 
			{ 
				Shader.SetGlobalVector("_WorldSpaceLightPos0", Vector3.back);
				asset.Materials[i].SetPass(0);
				Graphics.DrawMeshNow(asset.Mesh, pos, Quaternion.identity, i);	
			} 

			GL.PopMatrix();	 
			
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false, false);
            tex.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, width, height), 0, 0);
            tex.Apply();  
			
			Graphics.SetRenderTarget(null);

			return tex;
        }

        public void ApplyChanges() 
        {
            Asset asset    = null;
            float progress = 0.0f;

            serializedObject.ApplyModifiedProperties();

            if(serializedObject.isEditingMultipleObjects) 
            { 
				AssetDatabase.StartAssetEditing();

                foreach(Object t in targets) 
                {
                    asset = t as Asset;
                    asset.Apply();	
					EditorUtility.SetDirty(asset);

                    progress += (1.0f / targets.Length);
                    EditorUtility.DisplayProgressBar("Apply", "Apply Change for '" + AssetDatabase.GetAssetPath(t) + "'", progress);
                } 
				
				AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
            }
            else 
            {
                asset = target as Asset;
                asset.Apply(); 
				EditorUtility.SetDirty(asset);
            }

            changeCheck = false;
            
            GUI.FocusControl(null);	
			EditorGUI.FocusTextInControl(null);            
             
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Canvas.ForceUpdateCanvases();
            SceneView.RepaintAll();	 			
			PivotEditor.RepaintEditor();
        }

        public void RevertChanges() 
        {
            Asset asset = null;
            changeCheck = false;

            if(serializedObject.isEditingMultipleObjects) 
            {
                foreach(Object t in targets) 
                {
                    asset = t as Asset;
                    asset.Revert();
                }
            }
            else 
            {
                asset = target as Asset;
                asset.Revert();
            }
            
			GUI.FocusControl(null);	
			EditorGUI.FocusTextInControl(null);
			PivotEditor.RepaintEditor();
        }

		public void Restore()
		{  
			Asset asset = null;

			if(serializedObject.isEditingMultipleObjects) 
			{	 
				foreach(Object t in targets) 
                {
                    asset = t as Asset;
                    asset.Restore();
                }
			}
			else 
			{	
				asset = target as Asset;
                asset.Restore();
			}
		} 
		
		public void ExportToBitmap() 
		{	
			Asset asset = null;

			if(serializedObject.isEditingMultipleObjects) 
			{	 
				foreach(Object t in targets) 
                {
                    asset = t as Asset;
                    asset.ExportToBitmap();
                }
			}
			else 
			{	
				asset = target as Asset;
                asset.ExportToBitmap();
			}
		}  
		
		[DrawGizmo(GizmoType.Selected)]
        static void OnDrawGizmos(MeshRenderer renderer, GizmoType type) 
		{ 
			if(null == renderer || null == renderer.sharedMaterials || null == renderer.sharedMaterials[0])
				return;	 
			
			Asset asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(renderer.sharedMaterials[0])) as Asset;	 
			if(null == asset)
				return;	 
			
			UnityEditor.EditorUtility.SetSelectedRenderState(renderer, UnityEditor.EditorSelectedRenderState.Hidden);
			
			Vector2 size  = asset.Mesh.bounds.size;	
			Vector2 pos   = asset.Mesh.bounds.center;
			Gizmos.matrix = renderer.transform.localToWorldMatrix;
			Gizmos.color  = Color.yellow;

			size.x += 2.0f;	  
			size.y += 2.0f;
			Gizmos.DrawWireCube(pos, size); 
		}    
    }
}	 
	
	