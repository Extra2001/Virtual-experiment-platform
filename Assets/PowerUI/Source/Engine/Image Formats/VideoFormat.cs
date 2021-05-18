//--------------------------------------
//               PowerUI
//
//        For documentation or 
//    if you have any issues, visit
//        powerUI.kulestar.com
//
//    Copyright ?2013 Kulestar Ltd
//          www.kulestar.com
//--------------------------------------

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
	#define MOBILE
#endif

#if !MOBILE && !UNITY_WEBGL && !UNITY_TVOS

using System;
using Css;
using UnityEngine;
using Dom;


namespace PowerUI{
	
	/// <summary>
	/// Represents the video format.
	/// </summary>
	
	public class VideoFormat:ImageFormat{
		
		/// <summary>The video retrieved.</summary>

		/// <summary>An isolated material for this image.</summary>
		private Material IsolatedMaterial;
		
		
		public override string[] GetNames(){
			return new string[]{"mov","mpg","mpeg","mp4","avi","asf","ogg","ogv"};
		}
		
		public override Material GetImageMaterial(Shader shader){
			
			if(IsolatedMaterial==null){
				IsolatedMaterial=new Material(shader);

			}
			
			return IsolatedMaterial;
			
		}
		
		public override Texture Texture{
			get{
				return null;
			}
		}
		
		public override bool LoadFromAsset(UnityEngine.Object asset,ImagePackage package){
			
			
			return base.LoadFromAsset(asset,package);
		}
		
		public override void GoingOnDisplay(Css.RenderableData context){
			
			// Note that this is only called if Video is set.
			HtmlVideoElement videoElement=context.Node as HtmlVideoElement;
			
			if(videoElement==null){
				return;
			}
			
		}
		
		public override ImageFormat Instance(){
			return new VideoFormat();
		}
		
		public override bool Isolate{
			get{
				return true;
			}
		}
		
		public override int Height{
			get{
				return 0;
			}
		}
		
		public override int Width{
			get{
				return 0;
			}
		}
		
		public override bool Loaded{
			get{
				return true;
			}
		}
		
	}
	
}

#endif