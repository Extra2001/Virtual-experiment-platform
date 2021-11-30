// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;
using UnityEditor;


namespace svgtools
{
	public class PivotEditor: EditorWindow 
	{
		static public PivotEditor instance = null; 	
		
		Material           material        = null;	
		RenderTexture      target          = null;  
		
		MouseCursor[]      cursor          = new MouseCursor[]{ MouseCursor.Arrow, MouseCursor.ResizeUpLeft, MouseCursor.ResizeUpRight, MouseCursor.ResizeUpRight, MouseCursor.ResizeUpLeft, MouseCursor.ResizeVertical, MouseCursor.ResizeVertical, MouseCursor.ResizeHorizontal, MouseCursor.ResizeHorizontal, MouseCursor.ResizeHorizontal, MouseCursor.ResizeHorizontal, MouseCursor.ResizeVertical, MouseCursor.ResizeVertical };
		UnityEngine.Rect[] element         = new UnityEngine.Rect[13];
		UnityEngine.Rect   viewRect        = default(UnityEngine.Rect);	
		Vector3            scrollPosition  = Vector3.zero;	 
		Vector2            size            = Vector3.zero;	  
		Vector3            center          = Vector3.zero; 
		Vector3            min             = Vector3.zero; 
		Vector3            max             = Vector3.zero;
        Vector2            pivot           = Vector2.zero;
        Vector4            s9g             = Vector2.zero;
        Matrix4x4          world           = Matrix4x4.identity;	
		Matrix4x4          projection      = Matrix4x4.identity;	 
		Matrix4x4          transform       = Matrix4x4.identity;	
		
		float              zoom            = -1.0f; 
		float              width           = 0.0f;	  
		float              height          = 0.0f;
  		int                active          = -1;
        int                pivotIndex      = 9;
        public bool        changeCheck     = false;


        public static void ShowEditor() 
		{
			instance = EditorWindow.GetWindow<PivotEditor>(false, "Pivot Editor", true);
        }	 
		
		public static void RepaintEditor() 
		{
			if(null == instance)
				return;

			instance.Repaint();
		}

		void OnEnable() 
		{
			minSize     = new Vector2(370.0f, 140.0f);	
 			zoom        = -1.0f; 
			active      = -1;
			instance    = this;
			changeCheck	= false;  

			for(int i=0; i<13; ++i)	
				element[i] = default(UnityEngine.Rect);
			
			if(null == target) 
			{	
				target = RenderTexture.GetTemporary(32, 32, 16, RenderTextureFormat.ARGB32); 
				target.filterMode = FilterMode.Point;  
				target.hideFlags  = HideFlags.HideAndDontSave;
				target.Create(); 
			}

			if(null == material) 
			{ 	
				material = new Material(Shader.Find("Unlit/Transparent"));	
				material.hideFlags = HideFlags.HideAndDontSave;
            }

            if(null != AssetInspector.instance)
            {
                s9g        = AssetInspector.instance.scale9Grid.vector4Value;
                pivot      = AssetInspector.instance.pivot.vector2Value;
                pivotIndex = AssetInspector.instance.pivotIndex.intValue;
            }
        }

		void OnDisable() 
		{
            if(null != AssetInspector.instance && changeCheck) 
			{
				changeCheck	= false;

                AssetInspector.instance.scale9Grid.vector4Value = s9g;
                AssetInspector.instance.pivot.vector2Value      = pivot;
                AssetInspector.instance.pivotIndex.intValue     = pivotIndex;

                if(EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + AssetDatabase.GetAssetPath(AssetInspector.instance.target) + "'", "Apply", "Revert"))
                    AssetInspector.instance.ApplyChanges();
                else 
                    AssetInspector.instance.RevertChanges();	
			}

            if(null != target) 
			{  
				target.Release();  
				RenderTexture.ReleaseTemporary(target);
				target = null;
			}  
			
			if(null != material)  
				DestroyImmediate(material);

            instance = null;
		}

		void OnDestroy() 
		{
			instance = null;
		} 
		
		void OnSelectionChange() 
		{ 
			zoom   = -1.0f; 
			active = -1; 

			Repaint();				
		}	
		
		void OnFocus()
        {  
            Repaint();
        }

		void OnGUI()
		{
            if(null == AssetInspector.instance) 
			{  
				EditorGUILayout.LabelField("No SVG selected"); 	
				zoom        = -1.0f; 
				active      = -1; 
				changeCheck	= false;
				return;
			}

            Asset    asset          = AssetInspector.instance.target as Asset;	
			GUIStyle pivotDot       = "U2D.pivotDot";	  
			GUIStyle pivotDotActive = "U2D.pivotDotActive";	 
			//Vector4  s9g			= AssetInspector.instance.scale9Grid.vector4Value;
			//Vector2  pivot         	= Vector2.zero;
            Event    current        = Event.current;

            scrollPosition.z = -100.0f;	 

			viewRect.xMin = 0.0f;
			viewRect.yMin = 16.0f;	
			viewRect.xMax = position.width - 16.0f;
			viewRect.yMax = position.height - 16.0f;   
			
			size   = asset.Mesh.bounds.size;	  
			center = asset.Mesh.bounds.center;	
			
			if(zoom < 0.0f)
				zoom = Mathf.Min(viewRect.width/size.x, viewRect.height/size.y, 50.0f) * 0.9f; 

			world      = Matrix4x4.TRS(scrollPosition - center, Quaternion.identity, Vector3.one);	
			projection = GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(-0.5f*viewRect.width/zoom, 0.5f*viewRect.width/zoom, -0.5f*viewRect.height/zoom, 0.5f*viewRect.height/zoom, 0.1f, 1000.0f), false);	 
			transform  = projection * world;
			min        = transform.MultiplyPoint(asset.Mesh.bounds.min); 	
			max        = transform.MultiplyPoint(asset.Mesh.bounds.max);
						
			min.Set((min.x+1.0f)*viewRect.width*0.5f, (-min.y+1.0f)*viewRect.height*0.5f, 0.0f);
			max.Set((max.x+1.0f)*viewRect.width*0.5f, (-max.y+1.0f)*viewRect.height*0.5f, 0.0f);  
			
			width  = max.x - min.x;
 			height = min.y - max.y;

			s9g.Set(s9g.x/size.x*width, s9g.y/size.y*height, s9g.z/size.x*width, s9g.w/size.y*height);

			element[0].Set(pivot.x * width + min.x - 8.0f, (1.0f - pivot.y) * height + max.y + 8.0f, 16.0f, 16.0f);  
			
			element[1].Set(min.x + s9g.x - 1.0f, max.y + s9g.y + 14.0f, 3.0f, 3.0f);  
			element[2].Set(max.x - s9g.z - 1.0f, max.y + s9g.y + 14.0f, 3.0f, 3.0f); 
			element[3].Set(min.x + s9g.x - 1.0f, min.y - s9g.w + 15.0f, 3.0f, 3.0f); 
			element[4].Set(max.x - s9g.z - 1.0f, min.y - s9g.w + 15.0f, 3.0f, 3.0f); 
			
			element[5].Set(element[1].x + (element[2].x - element[1].x)*0.5f, element[1].y, 3.0f, 3.0f);  
			element[6].Set(element[1].x + (element[2].x - element[1].x)*0.5f, element[3].y, 3.0f, 3.0f);  
			element[7].Set(element[1].x, element[1].y + (element[3].y - element[1].y)*0.5f, 3.0f, 3.0f);  
			element[8].Set(element[2].x, element[7].y, 3.0f, 3.0f);  
			
			element[9].Set(min.x + s9g.x, max.y + 16.0f, 1.0f, height);	 
			element[10].Set(max.x - s9g.z, max.y + 16.0f, 1.0f, height);	 
			element[11].Set(min.x, max.y + s9g.y + 15.0f, width, 1.0f); 
			element[12].Set(min.x, min.y - s9g.w + 16.0f, width, 1.0f);
			
			if(current.alt && 1 == current.button)
                EditorGUIUtility.AddCursorRect(viewRect, MouseCursor.Zoom);   
			else if(current.alt || 1 == current.button)
                EditorGUIUtility.AddCursorRect(viewRect, MouseCursor.Pan);
			
			if(EventType.ScrollWheel == current.type || (EventType.MouseDrag == current.type && current.alt && 1 == current.button)) 
			{	
				zoom = zoom * (1.0f + current.delta.y * 0.03f); 
				zoom = Mathf.Clamp(zoom, (Mathf.Min(viewRect.width/size.x, viewRect.height/size.y) * 0.9f), 50.0f);
				current.Use();
			}  
			else if(EventType.MouseDrag == current.type && ((current.alt && 0 == current.button) || 1 == current.button)) 
			{	
				scrollPosition.x += current.delta.x; 	
				scrollPosition.y -= current.delta.y;
				scrollPosition.x = Mathf.Clamp(scrollPosition.x, -size.x*0.5f*zoom, size.x*0.5f*zoom + viewRect.width);  
				scrollPosition.y = Mathf.Clamp(scrollPosition.y, -size.y*0.5f*zoom, size.y*0.5f*zoom + viewRect.height);
				current.Use();
			}
			else if(EventType.MouseDown == current.type && 0 == current.button)
			{
				for(int i=0; i<13; ++i) 
				{
					if(element[i].Contains(current.mousePosition))
					{ 
						active = i;	
  						break;
					}
				}

				if(-1 != active) 
				{
					EditorGUIUtility.SetWantsMouseJumping(1);
					current.Use(); 
				} 
			}
			else if(EventType.MouseUp == current.type && 0 == current.button && -1 != active) 
			{  
				active = -1; 
				EditorGUIUtility.SetWantsMouseJumping(0);
				current.Use();
			}
			else if(EventType.MouseDrag == current.type && -1 != active) 
			{	
				if(0 == active) 
				{
  					pivot.Set(min.x + pivot.x*width, min.y - pivot.y*height);	
					pivot += current.delta;  
					pivot.Set((pivot.x - min.x)/width, (min.y - pivot.y)/height);
                    //AssetInspector.instance.pivot.vector2Value = pivot;
                    //AssetInspector.instance.pivotIndex.intValue = 9; 
                    pivotIndex = 9;
                }
                else 
				{ 
					if(1 == active || 3 == active || 7 == active || 9 == active) 
						s9g.x += current.delta.x;	
					if(2 == active || 4 == active || 8 == active || 10 == active) 
						s9g.z -= current.delta.x;	  
					if(1 == active || 2 == active || 5 == active || 11 == active) 
						s9g.y += current.delta.y;	  
					if(3 == active || 4 == active || 6 == active || 12 == active) 
						s9g.w -= current.delta.y;
				}
				
				changeCheck = true;
				current.Use();
			}
			else if(EventType.Repaint == current.type) 
			{ 
				if(target.width != viewRect.width || target.height != viewRect.height) 
				{	
					target.Release();  
					RenderTexture.ReleaseTemporary(target);
					target = RenderTexture.GetTemporary((int)viewRect.width, (int)viewRect.height, 16, RenderTextureFormat.ARGB32); 
					target.filterMode = FilterMode.Point; 
 					target.hideFlags  = HideFlags.HideAndDontSave;
					target.Create(); 
				}   
			 
				Graphics.SetRenderTarget(target);
				GL.Clear(true, true, new Color32(49, 49, 49, 255), 1.0f);
				
				DrawAsset(asset, world, projection);  
				
				Graphics.SetRenderTarget(null);
				EditorGUI.DrawPreviewTexture(viewRect, target, null, ScaleMode.StretchToFill); 
				
				if(active > 0) 
				{  
					EditorGUIUtility.AddCursorRect(viewRect, cursor[active]);
				}
				else 
				{ 
					for(int i=1; i<13; ++i)	
						EditorGUIUtility.AddCursorRect(element[i], cursor[i]);
				}
				
				for(int i=9; i<13; ++i)
					EditorGUI.DrawRect(element[i], new Color32(0, 255, 0, 128));  
				
				EditorGUI.DrawRect(new UnityEngine.Rect(min.x, max.y+16.0f, 1.0f, min.y-max.y), new Color32(60, 108, 203, 255));  
				EditorGUI.DrawRect(new UnityEngine.Rect(max.x, max.y+16.0f, 1.0f, min.y-max.y), new Color32(60, 108, 203, 255)); 
				EditorGUI.DrawRect(new UnityEngine.Rect(min.x, max.y+15.0f, max.x - min.x + 1.0f, 1.0f), new Color32(60, 108, 203, 255)); 
				EditorGUI.DrawRect(new UnityEngine.Rect(min.x, min.y+16.0f, max.x - min.x + 1.0f, 1.0f), new Color32(60, 108, 203, 255));  
				
				for(int i=1; i<9; ++i)
					EditorGUI.DrawRect(element[i], new Color32(0, 255, 0, 255));
				
				if(0 == active)	
					pivotDotActive.Draw(element[0], false, false, false, false);
				else  
					pivotDot.Draw(element[0], false, false, false, false);
			} 
			
			if((EventType.MouseUp == current.type || EventType.MouseDown == current.type) && (current.alt || 1 == current.button) || ((EventType.KeyUp == current.type || EventType.KeyDown == current.type) && (KeyCode.LeftAlt == current.keyCode || KeyCode.RightAlt == current.keyCode)))
                base.Repaint(); 
			
			scrollPosition.y = GUI.VerticalScrollbar(new UnityEngine.Rect(viewRect.xMax, 16.0f, 16.0f, viewRect.height), scrollPosition.y, viewRect.height, -size.y * 0.5f * zoom, size.y * zoom * 0.5f + viewRect.height); 
			scrollPosition.x = GUI.HorizontalScrollbar(new UnityEngine.Rect(0.0f, viewRect.yMax, viewRect.width, 16.0f), scrollPosition.x, viewRect.width, size.x * 0.5f * zoom + viewRect.width, -size.x * zoom * 0.5f); 
			
			EditorGUI.BeginChangeCheck(); 
			
			EditorGUIUtility.wideMode = true;
			GUILayout.BeginArea(new UnityEngine.Rect(position.width - 363.0f, position.height - 154.0f, 339.0f, 130.0f));
			GUILayout.BeginVertical("SVG", GUI.skin.window);   
			
			GUI.enabled = false;
			EditorGUILayout.TextField("Name", AssetInspector.instance.target.name); 
			GUI.enabled = true;

            //AssetInspector.instance.pivotIndex.intValue = EditorGUILayout.Popup(AssetInspector.pivotPropertyField, AssetInspector.instance.pivotIndex.intValue, AssetInspector.pivotPositionPropertyField);  
            pivotIndex = EditorGUILayout.Popup(AssetInspector.pivotPropertyField, pivotIndex, AssetInspector.pivotPositionPropertyField);

            if(9 != pivotIndex)  
			{
				pivot.x = 0.5f * (pivotIndex % 3); 
				pivot.y = 0.5f * (pivotIndex / 3);	 
				//AssetInspector.instance.pivot.vector2Value = pivot;
			}   
			
			GUI.enabled = (9 == pivotIndex);			
			//EditorGUI.PropertyField(new UnityEngine.Rect(5.0f, 61.0f, 420.0f, 32.0f), AssetInspector.instance.pivot, AssetInspector.customPivotPropertyField);
            EditorGUI.Vector2Field(new UnityEngine.Rect(5.0f, 61.0f, 420.0f, 32.0f), AssetInspector.customPivotPropertyField.text, pivot);
            GUI.enabled = true;	
			
			EditorGUI.LabelField(new UnityEngine.Rect(5.0f, 81.0f, 70.0f, 16.0f), "Scale9Grid");  
			EditorGUIUtility.labelWidth = 12.0f;
									
			s9g.x = EditorGUI.FloatField(new UnityEngine.Rect(155.0f, 81.0f, 88.0f, 16.0f), "L", s9g.x/width*size.x); 
			s9g.y = EditorGUI.FloatField(new UnityEngine.Rect(245.0f, 81.0f, 88.0f, 16.0f), "T", s9g.y/height*size.y); 
			s9g.z = EditorGUI.FloatField(new UnityEngine.Rect(155.0f, 97.0f, 88.0f, 16.0f), "R", s9g.z/width*size.x);	
			s9g.w = EditorGUI.FloatField(new UnityEngine.Rect(245.0f, 97.0f, 88.0f, 16.0f), "B", s9g.w/height*size.y); 
			
			s9g.x = Mathf.Clamp(s9g.x, 0.0f, size.x - s9g.z); 
			s9g.y = Mathf.Clamp(s9g.y, 0.0f, size.y - s9g.w); 
			s9g.z = Mathf.Clamp(s9g.z, 0.0f, size.x - s9g.x); 
			s9g.w = Mathf.Clamp(s9g.w, 0.0f, size.y - s9g.y);  
			
			//AssetInspector.instance.scale9Grid.vector4Value = s9g;
						
			GUILayout.EndVertical();
			GUILayout.EndArea(); 

			if(EditorGUI.EndChangeCheck())
                changeCheck = true;
			
			GUI.enabled = true;
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();	 
			GUI.enabled = changeCheck && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;

            //AssetInspector.instance.serializedObject.ApplyModifiedPropertiesWithoutUndo();

            if(GUILayout.Button("Revert", EditorStyles.toolbarButton)) 
			{ 
				changeCheck = false;
				AssetInspector.instance.RevertChanges();

                if(null != AssetInspector.instance)
                {
                    s9g        = AssetInspector.instance.scale9Grid.vector4Value;
                    pivot      = AssetInspector.instance.pivot.vector2Value;
                    pivotIndex = AssetInspector.instance.pivotIndex.intValue;
                }
            }

			if(GUILayout.Button("Apply", EditorStyles.toolbarButton)) 
			{ 
				changeCheck = false;
                AssetInspector.instance.scale9Grid.vector4Value = s9g;
                AssetInspector.instance.pivot.vector2Value      = pivot;
                AssetInspector.instance.pivotIndex.intValue     = pivotIndex;

                AssetInspector.instance.ApplyChanges();	
			}

            GUI.enabled = true;	  
			zoom = GUILayout.HorizontalSlider(zoom, Mathf.Min(viewRect.width/size.x, viewRect.height/size.y) * 0.9f, 50.0f, GUILayout.MaxWidth(64.0f));

			EditorGUILayout.EndHorizontal();
        }


		void DrawAsset(Asset asset, Matrix4x4 world, Matrix4x4 projection) 
		{
			GL.PushMatrix(); 
			GL.LoadProjectionMatrix(projection);
			
			for(int i=0; i<asset.Materials.Length; ++i) 
			{ 
				Shader.SetGlobalVector("_WorldSpaceLightPos0", Vector3.back);
				asset.Materials[i].SetPass(0);
				Graphics.DrawMeshNow(asset.Mesh, world, i);	
			} 

			GL.PopMatrix();
		}
	} 
}