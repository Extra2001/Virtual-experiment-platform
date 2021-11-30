// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;
using UnityEditor;

using System.Collections.Generic;


namespace svgtools
{
    public class Postprocessor: AssetPostprocessor 
    {
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            List<string> assets   = new List<string>();
			float        progress = 0.0f;

            foreach(string name in importedAssets) 
            {
                if(name.EndsWith(".svg", System.StringComparison.OrdinalIgnoreCase))
                    assets.Add(name);    
            }

            if(0 == assets.Count)
                return;	
            
            AssetDatabase.StartAssetEditing();
             
            foreach(string name in assets)
            {
				Asset.Create(name);	

                progress += (1.0f / assets.Count);
                EditorUtility.DisplayProgressBar("Importing SVG", name, progress);               
            }  
			            
            EditorUtility.ClearProgressBar();
            
            
			AssetDatabase.StopAssetEditing(); 
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); 
			
			Canvas.ForceUpdateCanvases();
            SceneView.RepaintAll();
        }
    }
}