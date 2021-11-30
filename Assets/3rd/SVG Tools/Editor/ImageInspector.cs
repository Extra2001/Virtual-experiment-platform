// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;	
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEditor; 
using UnityEditor.AnimatedValues;

using System.Linq;
using System.Collections.Generic;


namespace svgtools
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(Image))]
    public class ImageInspector : Editor
    {
 		Asset              asset; 

		SerializedProperty mesh;
		SerializedProperty material; 
		SerializedProperty assetName;	
		SerializedProperty assetPath; 
		SerializedProperty color;	
		SerializedProperty raycastTarget;  
		SerializedProperty type;  
		SerializedProperty preserveAspect; 
		SerializedProperty scale9Grid;	
		
		GUIContent         typeContent;	
		GUIContent         buttonContent;
		
		AnimBool           showType;
		AnimBool           showSliced; 
		AnimBool           showAspect;

		void OnEnable()
		{  
			mesh           = serializedObject.FindProperty("mesh");	
			material       = serializedObject.FindProperty("m_Material"); 
			assetName      = serializedObject.FindProperty("assetName");	 
			assetPath      = serializedObject.FindProperty("assetPath");
			color          = serializedObject.FindProperty("m_Color"); 
			raycastTarget  = serializedObject.FindProperty("m_RaycastTarget");
			type           = serializedObject.FindProperty("imageType"); 	
			preserveAspect = serializedObject.FindProperty("m_PreserveAspect"); 
			scale9Grid     = serializedObject.FindProperty("scale9Grid");	
			
			typeContent    = new GUIContent("Image Type");  
			buttonContent  = new GUIContent("Set Native Size", "Sets the size to match the content.");

			if(null == asset && !string.IsNullOrEmpty(assetName.stringValue)) 
			{ 
 				Asset[] assets = Resources.FindObjectsOfTypeAll<Asset>();

				foreach(Asset a in assets) 
				{
					if(a.name.Equals(assetName.stringValue))
					{
						asset = a;
						break;
					}
				}

				if(null == asset)  
					asset = AssetDatabase.LoadAssetAtPath<Asset>(assetPath.stringValue); 
			} 
			
			ImageType t = (ImageType)type.enumValueIndex; 
			
			showType   = new AnimBool(null != asset);
            showSliced = new AnimBool(ImageType.Sliced == t);  
			showAspect = new AnimBool(ImageType.Sliced != t);
			
			showSliced.valueChanged.AddListener(Repaint);  
			showType.valueChanged.AddListener(Repaint);	 
			showAspect.valueChanged.AddListener(Repaint);
		}

		void OnDisable() 
		{ 
			showType.valueChanged.RemoveListener(Repaint);
            showSliced.valueChanged.RemoveListener(Repaint);   
			showAspect.valueChanged.RemoveListener(Repaint);
		}
		
		public override void OnInspectorGUI() 
        { 
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			asset = EditorGUILayout.ObjectField("SVG Asset", asset, typeof(Asset), true) as Asset; 
						
			if(EditorGUI.EndChangeCheck())	
			{ 
				if(null == asset || AssetType.UI != asset.Type) 
				{
					asset = null;
					mesh.objectReferenceValue = null;
					material.objectReferenceValue = null;  
					assetName.stringValue = "";	
					assetPath.stringValue = "";
				}
				else 
				{ 
					mesh.objectReferenceValue = asset.Mesh;
					material.objectReferenceValue = asset.Materials[0];	
					assetName.stringValue = asset.name;	
					assetPath.stringValue = AssetDatabase.GetAssetPath(asset);
				}	
			} 
			
			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(raycastTarget);

			scale9Grid.vector4Value = (null != asset) ? asset.Scale9Grid : Vector4.zero;
			showType.target         = null != asset;

			if(EditorGUILayout.BeginFadeGroup(showType.faded)) 
			{ 
                EditorGUILayout.PropertyField(type, typeContent);	
				
				++EditorGUI.indentLevel; 
				
				ImageType t = (ImageType)type.enumValueIndex;

				showSliced.target = ImageType.Sliced == t; 	
				showAspect.target = ImageType.Sliced != t;
				
				if(EditorGUILayout.BeginFadeGroup(showSliced.faded))
                {
					if(scale9Grid.vector4Value.sqrMagnitude <= 0.0f)
                        EditorGUILayout.HelpBox("This Image doesn't have a border.", MessageType.Warning);
                }
                EditorGUILayout.EndFadeGroup();	
				
				if(EditorGUILayout.BeginFadeGroup(showAspect.faded))
				{
					EditorGUILayout.PropertyField(preserveAspect); 
					
					EditorGUILayout.BeginHorizontal(); 
					
					GUILayout.Space(EditorGUIUtility.labelWidth);

					if(GUILayout.Button(buttonContent, EditorStyles.miniButton)) 
					{	
						foreach(Image image in targets.Select(obj => obj as Image))
                        {
                            Undo.RecordObject(image.rectTransform, "Set Native Size");
                            image.SetNativeSize();
                            EditorUtility.SetDirty(image);
                        }
					}

					EditorGUILayout.EndHorizontal();
				}  
				EditorGUILayout.EndFadeGroup();

				--EditorGUI.indentLevel;
			}
            EditorGUILayout.EndFadeGroup();
			
			serializedObject.ApplyModifiedProperties();
		}

		public override string GetInfoString() 
        {  
            return (null == asset || AssetType.UI != asset.Type) ? "" : asset.Info;
        }
		
		public override bool HasPreviewGUI() 
		{ 
			return (null != asset && AssetType.UI == asset.Type && null != AssetInspector.renderTarget); 
		}
		
		public override void OnPreviewGUI(UnityEngine.Rect rect, GUIStyle background) 
		{
			if(Event.current.type != EventType.Repaint)
				return;	 
			
			Vector2 size = asset.Mesh.bounds.size;
			Vector3 pos  = -asset.Mesh.bounds.center;  
			float   zoom = Mathf.Min(rect.width/size.x, rect.height/size.y) * 0.9f;	
			float   hw   = 0.5f*rect.width/zoom;
			float   hh	 = 0.5f*rect.height/zoom;
			
			pos.z = -200.0f;
						
			if(AssetInspector.renderTarget.width != rect.width || AssetInspector.renderTarget.height != rect.height) 	  
			{	
				AssetInspector.renderTarget.Release();  
				RenderTexture.ReleaseTemporary(AssetInspector.renderTarget);
				AssetInspector.renderTarget = RenderTexture.GetTemporary((int)rect.width, (int)rect.height, 16, RenderTextureFormat.ARGB32); 
				AssetInspector.renderTarget.filterMode = FilterMode.Point;  	 
				AssetInspector.renderTarget.hideFlags  = HideFlags.HideAndDontSave;
				AssetInspector.renderTarget.Create(); 
			}   
			 
			Graphics.SetRenderTarget(AssetInspector.renderTarget);
			GL.Clear(true, true, new Color32(49, 49, 49, 255), 1.0f);  
			
			GL.PushMatrix(); 
			GL.LoadProjectionMatrix(GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(-hw, hw, -hh, hh, 0.1f, 1000.0f), false));
			
			for(int i=0; i<asset.Materials.Length; ++i) 
			{ 
				asset.Materials[i].SetPass(0);
				Graphics.DrawMeshNow(asset.Mesh, pos, Quaternion.identity, i);	
			} 

			GL.PopMatrix();	   
			
			Graphics.SetRenderTarget(null);	
			EditorGUI.DrawPreviewTexture(rect, AssetInspector.renderTarget, null, ScaleMode.StretchToFill);
		}
		
		static GameObject CreateCanvas()
		{
			GameObject go     = Selection.activeGameObject;
            Canvas     canvas = (go != null) ? go.GetComponentInParent<Canvas>() : null;

			if(null != canvas && canvas.gameObject.activeInHierarchy) 
			{ 
                canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1|AdditionalCanvasShaderChannels.TexCoord2|AdditionalCanvasShaderChannels.TexCoord3;
				return canvas.gameObject;	 
			}

            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;

			if(null != canvas && canvas.gameObject.activeInHierarchy) 
			{ 
                canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1|AdditionalCanvasShaderChannels.TexCoord2|AdditionalCanvasShaderChannels.TexCoord3;
				return canvas.gameObject; 
			}
			
			go = new GameObject("Canvas");
            go.layer = LayerMask.NameToLayer("UI");
            
			canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1|AdditionalCanvasShaderChannels.TexCoord2|AdditionalCanvasShaderChannels.TexCoord3;
            
			go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); 
			
			EventSystem es = Object.FindObjectOfType<EventSystem>();
            if(null == es)
            {
                GameObject esgo = new GameObject("EventSystem");
                es = esgo.AddComponent<EventSystem>();
                esgo.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(esgo, "Create " + esgo.name);
            }

			return go;
		}
		
		static void AddControl(GameObject go, MenuCommand command)
        {
            GameObject parent = command.context as GameObject;

            if(null == parent || null == parent.GetComponentInParent<Canvas>())
                parent = CreateCanvas();

            string name = GameObjectUtility.GetUniqueNameForSibling(parent.transform, go.name);
            go.name = name;

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Undo.SetTransformParent(go.transform, parent.transform, "Parent " + go.name);
            GameObjectUtility.SetParentAndAlign(go, parent);

			if(parent != command.context) 
			{ 
				SceneView sceneView = SceneView.lastActiveSceneView;
				
				if(null == sceneView && SceneView.sceneViews.Count > 0)
					sceneView = SceneView.sceneViews[0] as SceneView;

				if(null == sceneView || null == sceneView.camera)
					return;	
				
				Vector2       local;
				Camera        camera     = sceneView.camera;
				Vector3       position   = Vector3.zero;	
				RectTransform parentRect = parent.GetComponent<RectTransform>();	 
				RectTransform rect       = go.GetComponent<RectTransform>();
				
				if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, new Vector2(camera.pixelWidth * 0.5f, camera.pixelHeight * 0.5f), camera, out local))
				{
					local.x = local.x + parentRect.sizeDelta.x * parentRect.pivot.x;
					local.y = local.y + parentRect.sizeDelta.y * parentRect.pivot.y;

					local.x = Mathf.Clamp(local.x, 0, parentRect.sizeDelta.x);
					local.y = Mathf.Clamp(local.y, 0, parentRect.sizeDelta.y);

					position.x = local.x - parentRect.sizeDelta.x * rect.anchorMin.x;
					position.y = local.y - parentRect.sizeDelta.y * rect.anchorMin.y;

					Vector3 min;
					min.x = parentRect.sizeDelta.x * (0 - parentRect.pivot.x) + rect.sizeDelta.x * rect.pivot.x;
					min.y = parentRect.sizeDelta.y * (0 - parentRect.pivot.y) + rect.sizeDelta.y * rect.pivot.y;

					Vector3 max;
					max.x = parentRect.sizeDelta.x * (1 - parentRect.pivot.x) - rect.sizeDelta.x * rect.pivot.x;
					max.y = parentRect.sizeDelta.y * (1 - parentRect.pivot.y) - rect.sizeDelta.y * rect.pivot.y;

					position.x = Mathf.Clamp(position.x, min.x, max.x);
					position.y = Mathf.Clamp(position.y, min.y, max.y);
				}

				rect.anchoredPosition = position;
				rect.localRotation = Quaternion.identity;
				rect.localScale = Vector3.one;
			} 

            Selection.activeGameObject = go;
        }

		static GameObject CreateGameObject(GameObject parent, Vector2 size, string name) 
		{   
			GameObject    go   = new GameObject(name);
            RectTransform rect = go.AddComponent<RectTransform>();

			if(null == parent) 
			{ 
				rect.sizeDelta = size;
			}
			else 
			{	
				go.transform.SetParent(parent.transform, false); 
				go.layer = parent.layer;
			}

			return go;
		}

		static GameObject CreateScrollbar(GameObject parent, Vector2 size, string name) 
		{	
			GameObject scrollbar = CreateGameObject(parent, size, name);
            GameObject area      = CreateGameObject(scrollbar, Vector2.zero, "Sliding Area");
            GameObject handle    = CreateGameObject(area, Vector2.zero, "Handle"); 
			
			CreateImage(scrollbar, Color.white, ImageType.Sliced);
			
			RectTransform rect = area.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(-20.0f, -20.0f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            rect = handle.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(20.0f, 20.0f);

            Scrollbar control = scrollbar.AddComponent<Scrollbar>();
            control.handleRect    = rect;
            control.targetGraphic = CreateImage(handle, Color.black, ImageType.Sliced);	
			            
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);

			return scrollbar;
		}

		static Text CreateText(GameObject go, TextAnchor alignment, FontStyle style, bool rich, string text) 
		{  
			Text t = go.AddComponent<Text>();
            
			t.text            = text;
            t.alignment       = alignment; 
			t.supportRichText = rich;
			t.fontStyle       = style;
			t.color           = new Color(50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f, 1.0f);

			return t;
		}

		static Image CreateImage(GameObject go, Color32 color, ImageType type) 
		{  
			Image image = go.AddComponent<Image>();
            
			image.sprite    = null;
            image.color     = color;
			image.ImageType	= type;

			return image;
		}
		
		[MenuItem("GameObject/SVG Tools UI/Image", false, 20)]
        static void AddImage(MenuCommand command)
        {
			GameObject image = CreateGameObject(null, new Vector2(100.0f, 100.0f), "Image");
			
			image.AddComponent<Image>();
			AddControl(image, command);
        }  
		
		[MenuItem("GameObject/SVG Tools UI/Button", false, 20)]
        static void AddButton(MenuCommand command)
        {
            GameObject button = CreateGameObject(null, new Vector2(160.0f, 30.0f), "Button"); 	
			GameObject text   = CreateGameObject(button,  Vector2.zero, "Text");
                        
			CreateImage(button, Color.white, ImageType.Sliced);

            Button control = button.AddComponent<Button>(); 
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f); 
			
			CreateText(text, TextAnchor.MiddleCenter, FontStyle.Normal, true, "Button");

            RectTransform rect = text.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            AddControl(button, command);
        } 
		
		[MenuItem("GameObject/SVG Tools UI/Toggle", false, 20)]
        static void AddToggle(MenuCommand command)
        {  
			GameObject toggle     = CreateGameObject(null, new Vector2(100.0f, 25.0f), "Toggle");
			GameObject background = CreateGameObject(toggle, Vector2.zero, "Background");	
            GameObject label      = CreateGameObject(toggle, Vector2.zero, "Label");	
			GameObject checkmark  = CreateGameObject(background, Vector2.zero, "Checkmark");  
			
			Toggle control = toggle.AddComponent<Toggle>();
			control.isOn          = true;	
			control.graphic       = CreateImage(checkmark, Color.black, ImageType.Simple); 	
			control.targetGraphic = CreateImage(background, Color.white, ImageType.Sliced);	
			
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
			
			RectTransform rect = background.GetComponent<RectTransform>();
            rect.anchorMin        = Vector2.up;
            rect.anchorMax        = Vector2.up;
            rect.anchoredPosition = new Vector2(10.0f, -10.0f);
            rect.sizeDelta        = new Vector2(20.0f, 20.0f);

            rect = checkmark.GetComponent<RectTransform>();
            rect.anchorMin        = new Vector2(0.5f, 0.5f);
            rect.anchorMax        = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta        = new Vector2(14.0f, 14.0f);

            rect = label.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(23.0f, 1.0f);
            rect.offsetMax = new Vector2(-5.0f, -2.0f);	 
			
			CreateText(label, TextAnchor.UpperLeft, FontStyle.Normal, true, "Toggle");
			AddControl(toggle, command);
		} 
		
		[MenuItem("GameObject/SVG Tools UI/Input Field", false, 20)]
		static void AddInputField(MenuCommand command) 
		{ 
			GameObject field       = CreateGameObject(null, new Vector2(100.0f, 30.0f), "InputField");
            GameObject placeholder = CreateGameObject(field, Vector2.zero, "Placeholder");
            GameObject text        = CreateGameObject(field, Vector2.zero, "Text");	
			
			CreateImage(field, Color.white, ImageType.Sliced);
			
			InputField control = field.AddComponent<InputField>();	
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);  
			
			control.textComponent = CreateText(text, TextAnchor.UpperLeft, FontStyle.Normal, false, "");
            control.placeholder   = CreateText(placeholder, TextAnchor.UpperLeft, FontStyle.Italic, true, "Enter text..."); 
			
			Color color = control.textComponent.color;
            color.a *= 0.5f;
            control.placeholder.color = color;
			
			RectTransform rect = text.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = new Vector2(10.0f, 6.0f);
            rect.offsetMax = new Vector2(-10.0f, -7.0f);

            rect = placeholder.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = new Vector2(10.0f, 6.0f);
            rect.offsetMax = new Vector2(-10.0f, -7.0f);
			
			AddControl(field, command);
		}  
		
		[MenuItem("GameObject/SVG Tools UI/Panel", false, 20)]
		static void AddPanel(MenuCommand command) 
		{  	
			GameObject panel = CreateGameObject(null, new Vector2(100.0f, 100.0f), "Panel");

            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin        = Vector2.zero;
            rect.anchorMax        = Vector2.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta        = Vector2.zero;

            CreateImage(panel, new Color32(255, 255, 255, 100), ImageType.Sliced);			
			AddControl(panel, command);
			
			rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta        = Vector2.zero;
		} 
		
		[MenuItem("GameObject/SVG Tools UI/Slider", false, 20)]
		static void AddSlider(MenuCommand command) 
		{	
			GameObject slider     = CreateGameObject(null, new Vector2(200.0f, 20.0f), "Slider");
            GameObject background = CreateGameObject(slider, Vector2.zero, "Background");
            GameObject fillarea   = CreateGameObject(slider, Vector2.zero, "Fill Area");
            GameObject fill       = CreateGameObject(fillarea, Vector2.zero, "Fill");
            GameObject handlearea = CreateGameObject(slider, Vector2.zero, "Handle Slide Area");
            GameObject handle     = CreateGameObject(handlearea, Vector2.zero, "Handle");  
			
			CreateImage(slider, Color.white, ImageType.Sliced);
			
			RectTransform rect = background.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.0f, 0.25f);
            rect.anchorMax = new Vector2(1.0f, 0.75f);
            rect.sizeDelta = Vector2.zero; 
			
			rect = fillarea.GetComponent<RectTransform>();
            rect.anchorMin        = new Vector2(0.0f, 0.25f);
            rect.anchorMax        = new Vector2(1.0f, 0.75f);
            rect.anchoredPosition = new Vector2(-5.0f, 0.0f);
            rect.sizeDelta        = new Vector2(-20.0f, 0.0f); 
			
			CreateImage(fill, Color.white, ImageType.Sliced);	
			rect = fill.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(10, 0);  
			
			rect = handlearea.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(-20.0f, 0.0f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;	
			
			rect = handle.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(20.0f, 0.0f); 
			
			Slider control        = slider.AddComponent<Slider>();
            control.fillRect      = fill.GetComponent<RectTransform>();
            control.handleRect    = handle.GetComponent<RectTransform>();
            control.targetGraphic = CreateImage(handle, Color.black, ImageType.Simple);
            control.direction     = Slider.Direction.LeftToRight;  
			
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
			
			AddControl(slider, command);
		} 
		
		[MenuItem("GameObject/SVG Tools UI/Scrollbar", false, 20)]
		static void AddScrollbar(MenuCommand command) 
		{  			
			GameObject scrollbar = CreateScrollbar(null, new Vector2(200.0f, 20.0f), "Scrollbar");
			AddControl(scrollbar, command);	
		} 
		
		[MenuItem("GameObject/SVG Tools UI/Scroll View", false, 20)]
		static void AddScrollView(MenuCommand command) 
		{  
			GameObject scrollview = CreateGameObject(null, new Vector2(200.0f, 200.0f), "Scroll View");
            GameObject viewport   = CreateGameObject(scrollview, Vector2.zero, "Viewport");
            GameObject content    = CreateGameObject(viewport, Vector2.zero, "Content");
			GameObject hscrollbar = CreateScrollbar(scrollview, Vector2.zero, "Scrollbar Horizontal");
			
			RectTransform rect = hscrollbar.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            rect.pivot     = Vector2.zero;
            rect.sizeDelta = new Vector2(160.0f, 20.0f);	
			
			GameObject vscrollbar = CreateScrollbar(scrollview, Vector2.zero, "Scrollbar Vertical");
			vscrollbar.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true);
			rect = vscrollbar.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.right;
            rect.anchorMax = Vector2.one;
            rect.pivot     = Vector2.one;
            rect.sizeDelta = new Vector2(20.0f, 160.0f); 
			
			RectTransform view = viewport.GetComponent<RectTransform>();
            view.anchorMin = Vector2.zero;
            view.anchorMax = Vector2.one;
            view.sizeDelta = Vector2.zero;
            view.pivot     = Vector2.up; 
			
			rect = content.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.up;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = new Vector2(0.0f, 300.0f);
            rect.pivot     = Vector2.up;
			
			ScrollRect control = scrollview.AddComponent<ScrollRect>();
            control.content                       = rect;
            control.viewport                      = view;
            control.horizontalScrollbar           = hscrollbar.GetComponent<Scrollbar>();
            control.verticalScrollbar             = vscrollbar.GetComponent<Scrollbar>();
            control.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            control.verticalScrollbarVisibility   = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            control.horizontalScrollbarSpacing    = 0.0f;
            control.verticalScrollbarSpacing      = 0.0f;	
			
			CreateImage(scrollview, new Color32(255, 255, 255, 100), ImageType.Sliced);
			CreateImage(viewport, new Color32(255, 255, 255, 100), ImageType.Sliced); 
			AddControl(scrollview, command);
		}  
		
		[MenuItem("GameObject/SVG Tools UI/Dropdown", false, 20)]
		static void AddDropdown(MenuCommand command) 
		{  
			GameObject dropdown   = CreateGameObject(null, new Vector2(200.0f, 30.0f), "Dropdown");
            GameObject label      = CreateGameObject(dropdown, Vector2.zero, "Label");
            GameObject arrow      = CreateGameObject(dropdown, Vector2.zero, "Arrow");
            GameObject template   = CreateGameObject(dropdown, Vector2.zero, "Template");
            GameObject viewport   = CreateGameObject(template, Vector2.zero, "Viewport");
            GameObject content    = CreateGameObject(viewport, Vector2.zero, "Content");
            GameObject item       = CreateGameObject(content, Vector2.zero, "Item");
            GameObject background = CreateGameObject(item, Vector2.zero, "Item Background");
            GameObject checkmark  = CreateGameObject(item, Vector2.zero, "Item Checkmark");
            GameObject itemlabel  = CreateGameObject(item, Vector2.zero, "Item Label");	
			GameObject scrollbar  = CreateScrollbar(template, new Vector2(20.0f, 200.0f), "Scrollbar");	   
			
			scrollbar.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true); 
			
			RectTransform rect = scrollbar.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.right;
            rect.anchorMax = Vector2.one;
            rect.pivot     = Vector2.one;
            rect.sizeDelta = new Vector2(20.0f, 160.0f);	 
						
			Toggle toggle = item.AddComponent<Toggle>();
            toggle.targetGraphic = CreateImage(background, new Color32(245, 245, 245, 255), ImageType.Simple);
            toggle.graphic       = CreateImage(checkmark, Color.black, ImageType.Simple);
            toggle.isOn          = true; 
			
			CreateImage(template, Color.white, ImageType.Sliced); 
			
			ScrollRect scrollrect = template.AddComponent<ScrollRect>();
            scrollrect.content                     = (RectTransform)content.transform;
            scrollrect.viewport                    = (RectTransform)viewport.transform;
            scrollrect.horizontal                  = false;
            scrollrect.movementType                = ScrollRect.MovementType.Clamped;
            scrollrect.verticalScrollbar           = scrollbar.GetComponent<Scrollbar>();
            scrollrect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollrect.verticalScrollbarSpacing    = -3.0f;	  
			
			CreateImage(viewport, Color.white, ImageType.Sliced);  
			CreateImage(arrow, Color.black, ImageType.Simple); 
			
			Dropdown control = dropdown.AddComponent<Dropdown>();
            control.targetGraphic = CreateImage(dropdown, Color.white, ImageType.Sliced);
            control.template      = template.GetComponent<RectTransform>();
            control.captionText   = CreateText(label, TextAnchor.MiddleLeft, FontStyle.Normal, true, "");
            control.itemText      = CreateText(itemlabel, TextAnchor.MiddleLeft, FontStyle.Normal, true, "Option A"); 
			
			control.options.Add(new Dropdown.OptionData {text = "Option A"});
            control.options.Add(new Dropdown.OptionData {text = "Option B"});
            control.options.Add(new Dropdown.OptionData {text = "Option C"});
            control.RefreshShownValue();
			
			ColorBlock colors = control.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f); 
			
			rect = label.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(10.0f, 6.0f);
            rect.offsetMax = new Vector2(-25.0f, -7.0f);

            rect = arrow.GetComponent<RectTransform>();
            rect.anchorMin        = new Vector2(1.0f, 0.5f);
            rect.anchorMax        = new Vector2(1.0f, 0.5f);
            rect.sizeDelta        = new Vector2(14.0f, 14.0f);
            rect.anchoredPosition = new Vector2(-15.0f, 0.0f);

            rect = template.GetComponent<RectTransform>();
            rect.anchorMin        = Vector2.zero;
            rect.anchorMax        = Vector2.right;
            rect.pivot            = new Vector2(0.5f, 1.0f);
            rect.anchoredPosition = new Vector2(0.0f, 2.0f);
            rect.sizeDelta        = new Vector2(0.0f, 150.0f);

            rect = viewport.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = new Vector2(-18.0f, 0.0f);
            rect.pivot     = Vector2.up;

            rect = content.GetComponent<RectTransform>();
            rect.anchorMin        = Vector2.up;
            rect.anchorMax        = Vector2.one;
            rect.pivot            = new Vector2(0.5f, 1.0f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta        = new Vector2(0.0f, 28.0f);

            rect = item.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.0f, 0.5f);
            rect.anchorMax = new Vector2(1.0f, 0.5f);
            rect.sizeDelta = new Vector2(0.0f, 20.0f);

            rect = background.GetComponent<RectTransform>();
            rect.anchorMin  = Vector2.zero;
            rect.anchorMax  = Vector2.one;
            rect.sizeDelta  = Vector2.zero;

            rect = checkmark.GetComponent<RectTransform>();
            rect.anchorMin        = new Vector2(0.0f, 0.5f);
            rect.anchorMax        = new Vector2(0.0f, 0.5f);
            rect.sizeDelta        = new Vector2(14.0f, 14.0f);
            rect.anchoredPosition = new Vector2(10.0f, 0.0f);

            rect = itemlabel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(20.0f, 1.0f);
            rect.offsetMax = new Vector2(-10.0f, -2.0f);
			
			template.SetActive(false);	
			AddControl(dropdown, command);
				
			
		}
	}
}