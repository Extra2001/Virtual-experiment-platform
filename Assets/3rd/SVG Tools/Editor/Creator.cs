// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;

using svgtools.clipper;


namespace svgtools
{
	public struct VertexKey 
	{
 		public int a;
		public int b;
	}
	
	public enum GradientType
	{
		NONE   = 0,
		LINEAR = 1,
		RADIAL = 2,
		FOCAL  = 3
	}	  

    public struct Transform 
    {
        public float scaleX;      
        public float scaleY;       
        public float rotateSkew0; 
        public float rotateSkew1;  
        public float translateX;   
        public float translateY;   
        
        public static Transform identity 
        { 
            get 
            { 
                Transform transform;

                transform.scaleX      = 1.0f;
                transform.scaleY      = 1.0f;
                transform.rotateSkew0 = 0.0f;
                transform.rotateSkew1 = 0.0f;
                transform.translateX  = 0.0f;
                transform.translateY  = 0.0f;

                return transform; 
            } 
        }
        
        public Transform inverse 
        { 
            get 
            { 
                Transform transform;
                float det = scaleX * scaleY - rotateSkew0 * rotateSkew1;

                if(0.0f == det) 
                {
                    transform.scaleX      = 1.0f;
                    transform.scaleY      = 1.0f;
                    transform.rotateSkew0 = 0.0f;
                    transform.rotateSkew1 = 0.0f;
                    transform.translateX  = -translateX;
                    transform.translateY  = -translateY;
                }
                else 
                {
                    det = 1.0f/det;
                    
                    transform.scaleX      = scaleY * det;
                    transform.scaleY      = scaleX * det;
                    transform.rotateSkew0 = -rotateSkew0 * det;
                    transform.rotateSkew1 = -rotateSkew1 * det;
                    transform.translateX  = -(transform.rotateSkew1 * translateY + transform.scaleX * translateX);
                    transform.translateY  = -(transform.rotateSkew0 * translateX + transform.scaleY * translateY);
                }

                return transform; 
            } 
        }

        public static Vector2 operator * (Transform t, Vector2 v) 
        {
            Vector2 vec; 

            vec.x = v.x * t.scaleX + v.y * t.rotateSkew1 + t.translateX;
            vec.y = v.y * t.scaleY + v.x * t.rotateSkew0 + t.translateY;

            return vec;
        }
        
        public static Point operator * (Transform t, Point p) 
        {
            Point pt = p; 

            pt.x = p.x * t.scaleX + p.y * t.rotateSkew1 + t.translateX;
            pt.y = p.y * t.scaleY + p.x * t.rotateSkew0 + t.translateY;

            return pt;
        }
        
        public static Transform operator * (Transform a, Transform b) 
        {
            Transform t; 

            t.scaleX      = a.scaleX * b.scaleX + a.rotateSkew1 * b.rotateSkew0;
            t.scaleY      = a.scaleY * b.scaleY + a.rotateSkew0 * b.rotateSkew1;
            t.rotateSkew0 = a.rotateSkew0 * b.scaleX + a.scaleY * b.rotateSkew0;
            t.rotateSkew1 = a.rotateSkew1 * b.scaleY + a.scaleX * b.rotateSkew1;
            t.translateX  = a.scaleX * b.translateX + a.rotateSkew1 * b.translateY + a.translateX;
            t.translateY  = a.scaleY * b.translateY + a.rotateSkew0 * b.translateX + a.translateY;

            return t;
        }

        public void Matrix(float a, float b, float c, float d, float e, float f)
		{  
			float aa = scaleX * a + rotateSkew1 * b;
            float dd = scaleY * d + rotateSkew0 * c;
            float bb = scaleY * b + rotateSkew0 * a;
            float cc = scaleX * c + rotateSkew1 * d;
            float ee = scaleX * e + rotateSkew1 * f + translateX;
            float ff = scaleY * f + rotateSkew0 * e + translateY;
			
			scaleX      = aa;
            scaleY      = dd;
            rotateSkew0 = bb;
            rotateSkew1 = cc; 
			translateX  = ee;	
			translateY  = ff;
		}
		
		public void Rotate(float angle, float cx, float cy) 
        { 
			Translate(cx, cy);
 
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            
            float a = scaleX * cos + rotateSkew1 * sin;
            float d = scaleY * cos - rotateSkew0 * sin;
            float b = scaleY * sin + rotateSkew0 * cos;
            float c = rotateSkew1 * cos - scaleX * sin;
            
            scaleX      = a;
            scaleY      = d;
            rotateSkew0 = b;
            rotateSkew1 = c; 
			
			Translate(-cx, -cy);
        }

        public void Scale(float sx, float sy) 
        {
            scaleX      *= sx;
            scaleY      *= sy;
            rotateSkew0 *= sx;
            rotateSkew1 *= sy;
        }	 
		
		public void Skew(float x, float y) 
        {
			float tc = Mathf.Tan(x * Mathf.Deg2Rad);	
			float tb = Mathf.Tan(y * Mathf.Deg2Rad);

            float a = scaleX + rotateSkew1 * tb;
            float d = scaleY + rotateSkew0 * tc;
			float b = rotateSkew0 + scaleY * tb;
            float c = rotateSkew1 + scaleX * tc;
                        
            scaleX      = a;
            scaleY      = d;
            rotateSkew0 = b;
            rotateSkew1 = c;
            
        }

        public void Translate(float tx, float ty) 
        {            
            translateX = scaleX * tx + rotateSkew1 * ty + translateX;
            translateY = scaleY * ty + rotateSkew0 * tx + translateY;
        }
    }
    
    
    public struct GradientStop 
    {
        public Color32 color;
        public float   ratio;
        
        public static GradientStop Default 
        {
            get
            {
                GradientStop stop = default(GradientStop);
                
                stop.ratio   = 0.0f;
                stop.color.r = 0;
                stop.color.g = 0;
                stop.color.b = 0;
                stop.color.a = 255;

                return stop;
            }
        }
    }

    
    public class Gradient 
    {
        public Transform          transform;
        public List<GradientStop> stopList; 
		public string             name;	 
		public string             link;
		public float              spreadMode; 
        public float              fx;
        public float              fy;
        public bool               radial;
        public bool               userSpaceOnUse;  
		public bool               transparent;

        public Gradient(bool radial) 
        {
            this.transform      = Transform.identity;
            this.stopList       = new List<GradientStop>();
            this.name           = ""; 
			this.link           = "";
            this.spreadMode     = 0.0f;
            this.fx             = 0.0f;
            this.fy             = 0.0f;
            this.radial         = radial; 
            this.userSpaceOnUse = false;
           	this.transparent    = false;
        }

        internal void GetTransform(float left, float top, float right, float bottom, Transform shape, ref Transform t) 
        {
            Transform gradient = transform.inverse; 
            Transform local    = Transform.identity;
			
            if(userSpaceOnUse)
            {
                t = gradient * shape.inverse;
            }
            else 
            {
                local.Scale(Mathf.Abs(1.0f/(right-left)), Mathf.Abs(1.0f/(bottom-top)));
                local.Translate(-left, -top);
 
                t = gradient * local * shape.inverse;	
            } 
        }

        internal void CreateGradientRump(Color32[] data, int row) 
        {
            Color32 color = default(Color32);
            int     begin = (int)(stopList[0].ratio * 255.0f);
            int     end   = 0;
	        int     delta = 0;
            int     n     = row * 256;
            int     count = stopList.Count;
            
            for(int i=0; i<begin; ++i) 
                data[n++] = stopList[0].color;
            
            for(int i=1; i<count; ++i) 
            {
                begin = (int)(stopList[i-1].ratio * 255.0f);
                end   = (int)(stopList[i].ratio * 255.0f);	
                delta = end - begin;

                for(int j=0; j<delta; ++j)
                {
                    color.r = (byte)((stopList[i-1].color.r * (delta-j) + stopList[i].color.r * j) / delta);
                    color.g = (byte)((stopList[i-1].color.g * (delta-j) + stopList[i].color.g * j) / delta);
                    color.b = (byte)((stopList[i-1].color.b * (delta-j) + stopList[i].color.b * j) / delta);
                    color.a = (byte)((stopList[i-1].color.a * (delta-j) + stopList[i].color.a * j) / delta);
                    
                    data[n++] = color;
                } 
            }
            
            for(int i=end; i<256; ++i)
				data[n++] = stopList[count-1].color;
			
			if(1.0f == spreadMode)
			{
				color.r = (byte)(stopList[count-1].color.r*0.75f + stopList[0].color.r*0.25f);
				color.g = (byte)(stopList[count-1].color.g*0.75f + stopList[0].color.g*0.25f);
				color.b = (byte)(stopList[count-1].color.b*0.75f + stopList[0].color.b*0.25f);
				color.a = data[n-3].a;

				data[n-3] = color;

				color.r = (byte)(stopList[count-1].color.r*0.5f + stopList[0].color.r*0.5f);
				color.g = (byte)(stopList[count-1].color.g*0.5f + stopList[0].color.g*0.5f);
				color.b = (byte)(stopList[count-1].color.b*0.5f + stopList[0].color.b*0.5f);
				color.a = data[n-2].a;

				data[n-2] = color;

				color.r = (byte)(stopList[count-1].color.r*0.25f + stopList[0].color.r*0.75f);
				color.g = (byte)(stopList[count-1].color.g*0.25f + stopList[0].color.g*0.75f);
				color.b = (byte)(stopList[count-1].color.b*0.25f + stopList[0].color.b*0.75f);
				color.a = data[n-1].a;

				data[n-1] = color;
			} 
        }
    }


    public struct Style
    {
        public List<float> strokeDashArray;        
        public Color32     fillColor;
        public Color32     strokeColor;
        public FillRule    fillRule;  
		public FillRule    clipRule;  
		public string      filter;
        public string      fillGradient;
        public string      strokeGradient;
        public string      strokeJoin;
        public string      strokeCap;
        public float       strokeWidth;
        public float       strokeMitterLimit;
        public float       strokeDashOffset;
        public float       strokeOpacity;
        public float       fillOpacity;
        public bool        useStroke;
        public bool        useFill;

        public static Style Default 
        {
            get
            {
                Style style = default(Style);
                
                style.strokeDashArray   = null;
                style.fillColor.r       = 0;
                style.fillColor.g       = 0;
                style.fillColor.b       = 0;
                style.fillColor.a       = 255;
                style.strokeColor.r     = 0;
                style.strokeColor.g     = 0;
                style.strokeColor.b     = 0;
                style.strokeColor.a     = 255;
                style.fillRule          = FillRule.NON_ZERO;	
				style.clipRule          = FillRule.NON_ZERO; 
				style.filter            = "";
                style.fillGradient      = "";
                style.strokeGradient    = "";
                style.strokeJoin        = "miter";
                style.strokeCap         = "butt";
                style.strokeWidth       = 1.0f;
                style.strokeMitterLimit = 4.0f;
                style.strokeDashOffset  = 0.0f;
                style.strokeOpacity     = 1.0f;
                style.fillOpacity       = 1.0f;
                style.useStroke         = false;
                style.useFill           = true;

                return style;
            }
        }
    }

	public static class MeshCreator 
    {
        static Dictionary<VertexKey, int> vertexMap        = new Dictionary<VertexKey, int>();
		
		static List<Vector3>       positionList            = new List<Vector3>();
        static List<Vector2>       uvList                  = new List<Vector2>();
        static List<Vector2>       radialList              = new List<Vector2>();
        static List<Vector2>       focalList               = new List<Vector2>();  
		static List<Vector2>       spreadOpacityList       = new List<Vector2>();	 
		static List<Vector4>       spreadOpacityOffsetList = new List<Vector4>();
        static List<bool>          typeList                = new List<bool>();
        static List<int>           indexList               = new List<int>();	 
		static List<int>           transparentIndexList    = new List<int>();	
		static List<int>           indexOffsetList         = new List<int>();
		static List<Color32>       vertexColorList         = new List<Color32>();
        									    
        static List<Color32>       colorList               = new List<Color32>();        
        static List<Gradient>      gradientList            = new List<Gradient>();   
       
        static Path                result                  = new Path();
        static Path                stroke                  = new Path();
        static Path                dashed                  = new Path();	 	
		static Path                reduce                  = new Path();
		static Path                collider                = new Path();   

		static Rect                rect                    = Rect.Default; 
		static Batch               batch                   = null; 
		static FilterInstance      filterInstance          = null; 
		static int                 numPaths                = 0; 
		static int                 numGradients            = 0; 
		static int                 start                   = 0;  
		
		static int[]               ind                     = new int[5];

        static public float        antialiasingWidth       = 0.0f;	
		static public float        precision               = 0.0f;  
		static public float        depthOffset             = 0.0f;
		static public GradientType gradientType            = GradientType.NONE; 
		static public bool         usePad                  = false; 
		static public bool         useRepeat               = false;
		static public bool         useReflect              = false;  
		static public bool         isCollider              = false;	
		static public bool         isTransparent           = false; 
		static public bool         renderToTexture         = false;

        
        static int SplitEdge(int a, int b, int h, float f)
		{
			int       i = 0;
			VertexKey key;

			key.a = a < b ? a : b;
			key.b = a < b ? b : a;	

			if(vertexMap.TryGetValue(key, out i))
				return i;	
			
			Vector2 v = Vector2.zero; 
			Vector4 t = Vector4.zero;
			Vector3 d = positionList[b] - positionList[a];
			float   r = (f - positionList[a][h]) / d[h];
			Vector3 p = positionList[a] + d * r; 
			
			vertexMap.Add(key, positionList.Count);
			positionList.Add(p); 
			typeList.Add(typeList[a]);
			
			t.x = spreadOpacityList[a].x + r * (spreadOpacityList[b].x - spreadOpacityList[a].x);
			t.y = spreadOpacityList[a].y;
			spreadOpacityList.Add(t); 
			spreadOpacityOffsetList.Add(t);

			if(GradientType.NONE == gradientType) 
			{ 	
				vertexColorList.Add(vertexColorList[a]);   
			}
			else 
			{	
				v = uvList[a] + r * (uvList[b] - uvList[a]);
				uvList.Add(v);

				if(gradientType > GradientType.LINEAR) 
				{  
					v = radialList[a] + r * (radialList[b] - radialList[a]);
					radialList.Add(v);	 
					
					if(gradientType == GradientType.FOCAL) 
						focalList.Add(focalList[a]);
				}
			}
							 
			return (positionList.Count - 1);
		}
		
		static void SplitTriangle(List<int> triangles, int i1, int i2, int i3, int h, float f)			
		{
			int  n = 2;
			ind[0] = i2;	
			ind[1] = i3;
			ind[2] = 0;
			ind[3] = 0;
			ind[4] = i1;

			if((positionList[i1][h] <= f) != (positionList[i2][h] <= f)) 
			{
				ind[0] = i1;
				ind[1] = SplitEdge(i1, i2, h, f);
				ind[2] = i2;
				ind[3] = i3;
				n = 4;	
			}	  
			
			if((positionList[i2][h] <= f) != (positionList[i3][h] <= f)) 
			{
  				ind[n++] = ind[n-2];
				ind[n-2] = SplitEdge(i2, i3, h, f);
			} 
			
			if((positionList[i3][h] <= f) != (positionList[i1][h] <= f)) 
			{	
				ind[n++] = SplitEdge(i3, i1, h, f);	
			}

			if(2 != n) 
			{	
				triangles.Add(ind[0]);	
				triangles.Add(ind[1]);
				triangles.Add(ind[4]); 
				
				triangles.Add(ind[1]);	
				triangles.Add(ind[2]);
				triangles.Add(ind[3]);  
				
				triangles.Add(ind[1]);	
				triangles.Add(ind[3]);
				triangles.Add(ind[4]);  
			}
			else 
			{
				triangles.Add(i1);	
				triangles.Add(i2);
				triangles.Add(i3);	
			}
		}
		
		static void SplitTriangles(int h, float f)
		{
			int[] triangles = null;
	
			vertexMap.Clear();

			if(indexList.Count > 0) 
			{ 
				triangles = indexList.ToArray(); 
				indexList.Clear();	
				
				for(int i=0; i<triangles.Length; i+=3) 
					SplitTriangle(indexList, triangles[i], triangles[i+1], triangles[i+2], h, f);
			} 	
			
			if(transparentIndexList.Count > 0) 
			{ 
				triangles = transparentIndexList.ToArray(); 
				transparentIndexList.Clear();	
				
				for(int i=0; i<triangles.Length; i+=3) 
					SplitTriangle(transparentIndexList, triangles[i], triangles[i+1], triangles[i+2], h, f);
			} 
			
			triangles = null;
		}
		
		static void SplitMesh(Rect rect, Vector2 offset, float l, float t, float r, float b, bool sliced)
		{
			Vector4 vec    = Vector4.zero;
			float   left   = rect.left - offset.x + l;
			float   top    = -rect.top - offset.y - t;
    		float   right  = rect.right - offset.x - r;
			float   bottom = -rect.bottom - offset.y + b;   
			  
			if(l > 0.0f)
				SplitTriangles(0, left);	  
			
			if(t > 0.0f)
				SplitTriangles(1, top);	
			
			if(r > 0.0f)
				SplitTriangles(0, right);	  
			
			if(b > 0.0f)
				SplitTriangles(1, bottom);

			if(sliced) 
			{
				for(int i=0; i<positionList.Count; ++i)
				{  
					vec   = spreadOpacityOffsetList[i];  
					vec.z = 1.0f;
					vec.w = 1.0f;

					if(positionList[i].x <= left)
						vec.z = 0.0f;

					if(positionList[i].x >= right)
						vec.z = 2.0f;

					if(positionList[i].y >= top)
						vec.w = 0.0f;

					if(positionList[i].y <= bottom)
						vec.w = 2.0f;
					
					spreadOpacityOffsetList[i] = vec;
				}
			}
		}
		
		static float DistanceFromLine(Point p, Point a, Point b) 
		{
  			if(a.x.Equals(b.x))
				return Mathf.Abs(p.x - a.x);

			if(a.y.Equals(b.y))
				return Mathf.Abs(p.y - a.y);

			float d = (b.x - a.x)*(b.x - a.x) + (b.y - a.y)*(b.y - a.y);			
			float s = ((a.y - p.y)*(b.x - a.x) - (a.x - p.x)*(b.y - a.y)) / d;

			return Mathf.Abs(s) * Mathf.Sqrt(d);
		}
		
		static void RamerDouglasPeucker(Point[] p, int begin, int end) 
		{  
			float dmax  = 0.0f;
			float dist  = 0.0f;	
			int   index = 0;

			for(int i=begin; i<end; ++i) 
			{
				dist = DistanceFromLine(p[i], p[begin], p[end-1]);

				if(dist.CompareTo(dmax) > 0) 
				{
					dmax  = dist;
					index = i;
				}
			}

			if(dmax.CompareTo(precision) >= 0) 
			{	 
				RamerDouglasPeucker(p, begin, index); 
				RamerDouglasPeucker(p, index, end);
			}
			else 
			{
				reduce.pointList.Add(p[begin]); 
				reduce.pointList.Add(p[end-1]);
			}
		}

		static void Reduce(Path path)
		{  
			reduce.Clear();

			if(precision.Equals(0.0f)) 
			{ 
				foreach(SubPath sp in path.subPathList)	
					reduce.subPathList.Add(sp);
			}
			else 
			{ 
				foreach(SubPath sp in path.subPathList) 
				{ 
					RamerDouglasPeucker(sp.points, 0, sp.points.Length); 
					reduce.Finalize(false);  
				}	
			}
		} 
		
		static bool Intersection(Point a, Point b, Point c, Point d, ref Point result)
        {
            float num = (d.x - c.x) * (a.y - c.y) - (d.y - c.y) * (a.x - c.x); 
            float den = (d.y - c.y) * (b.x - a.x) - (d.x - c.x) * (b.y - a.y);

			if(Mathf.Abs(den).CompareTo(0.0001f) < 0) 
				return false; 

            float m = num/den;
            
            result.x = a.x + m * (b.x - a.x);
            result.y = a.y + m * (b.y - a.y);

            return true;
        }        
        
        static void CalculateRect(Path path, Transform transform, float aa, ref Rect rect) 
        { 
            Point point = default(Point);
			
			foreach(SubPath sp in path.subPathList) 
            {
				foreach(Point pt in sp.points) 
				{  
					point = transform * pt;
					rect.Add(point.x, point.y);
				}
            }
            
            rect.left   = rect.left - aa;
			rect.top    = rect.top - aa;
            rect.right  = rect.right + aa;  
			rect.bottom = rect.bottom + aa;       
        }

        static void SolidParams(ref Color32 color, float opacity, ref Vector2 uv)
        {
            int i;	
			color.a = (byte)(opacity * 255.0f);

            for(i=0; i<colorList.Count; ++i)
            {
                if(color.r == colorList[i].r && color.g == colorList[i].g && color.b == colorList[i].b && color.a == colorList[i].a) 
                {
                    uv.Set((float)(i % 256)/255.0f, (float)(i/256)); 
                    return;
                }
            }
  
            uv.Set((float)(i % 256)/255.0f, (float)(i/256));
            colorList.Add(color);
        }
        
        static void GradientParams(string gradient, Path path, Transform transform, float opacity, float aa, ref float y, ref float so, ref Vector2 focal, ref Transform paint, ref bool radial, ref bool transparent)
        {
            Rect rect = Rect.Default;
            
            for(int i=0; i<gradientList.Count; ++i) 
            {
                if(gradientList[i].name == gradient) 
                { 
                    y           = i;  
                    so          = gradientList[i].spreadMode + (int)(opacity * 255);
                    radial      = gradientList[i].radial;
					transparent = gradientList[i].transparent;
					focal.x     = gradientList[i].fx;
					focal.y     = gradientList[i].fy; 

                    CalculateRect(path, transform.inverse, aa, ref rect);
                    gradientList[i].GetTransform(rect.left, rect.top, rect.right, rect.bottom, transform, ref paint);	 
					
					if(GradientType.NONE == gradientType && !radial)
						gradientType = GradientType.LINEAR;
					else if(radial && 0.0f == focal.x && 0.0f == focal.y && gradientType < GradientType.FOCAL)	
						gradientType = GradientType.RADIAL;	 
					else if(radial) 	
						gradientType = GradientType.FOCAL;
			
					if(0.0f == gradientList[i].spreadMode)
						usePad = true; 
					else if(0.1f == gradientList[i].spreadMode)
						useRepeat = true;  
					else if(0.2f == gradientList[i].spreadMode)
						useReflect = true;	

                    if(!string.IsNullOrEmpty(gradientList[i].link))
					{
						for(int j=0; j<numGradients; ++j) 
						{
							if(gradientList[j].name == gradientList[i].link) 
							{
								y = j; 
								transparent = gradientList[j].transparent;	
								break;
							}
						}
					}
					
					break;
                }
            }  
        }
        
		static void CreateAntiAliasing(Path path, float width)
		{	  
			float   dx, dy, dist, cross, inner;
			float   outer = width * 1.5f;
            int     count, prev, curr, next, start, n;
            int     offset = 0;
            int     c  = Triangulator.pointList.Count - 1; 
            Point   p1 = default(Point);
			Point   p2 = default(Point);   
			Point   p3 = default(Point);
			Point   p4 = default(Point);	
			Point   pi = default(Point);           
            Vector2 n1 = Vector2.zero;
            Vector2 n2 = Vector2.zero;
               
            foreach(SubPath sp in path.subPathList)
            {
                count = sp.points.Length;
				start = Triangulator.pointList.Count;

				if(count < 3)
					continue;
                
                for(int i=0; i<count; ++i)
                {
                    prev = ((i - 1 + count) % count) + offset;
                    next = ((i + 1) % count) + offset;
                    curr = i + offset;	 		
                    
                    n1.Set(Triangulator.pointList[curr].y - Triangulator.pointList[prev].y, Triangulator.pointList[prev].x - Triangulator.pointList[curr].x); 
                    n2.Set(Triangulator.pointList[next].y - Triangulator.pointList[curr].y, Triangulator.pointList[curr].x - Triangulator.pointList[next].x);  
					inner = (n1.magnitude < n2.magnitude) ? n1.magnitude : n2.magnitude;
                    n1.Normalize(); 
                    n2.Normalize();
                    n1 *= width;
                    n2 *= width;
                    
                    p1.x = Triangulator.pointList[prev].x + n1.x;
                    p1.y = Triangulator.pointList[prev].y + n1.y;
                    p2.x = Triangulator.pointList[curr].x + n1.x;
                    p2.y = Triangulator.pointList[curr].y + n1.y;  
                    p3.x = Triangulator.pointList[next].x + n2.x;
                    p3.y = Triangulator.pointList[next].y + n2.y;
                    p4.x = Triangulator.pointList[curr].x + n2.x;
                    p4.y = Triangulator.pointList[curr].y + n2.y;	
					
					cross = (Triangulator.pointList[next].x - Triangulator.pointList[curr].x)*(Triangulator.pointList[curr].y - Triangulator.pointList[prev].y) - (Triangulator.pointList[next].y - Triangulator.pointList[curr].y)*(Triangulator.pointList[curr].x - Triangulator.pointList[prev].x);
					if(cross.CompareTo(0.0f) >= 0) 
					{
						if(Intersection(p1, p2, p3, p4, ref pi)) 
						{	 
							dx   = pi.x - Triangulator.pointList[curr].x;
							dy   = pi.y - Triangulator.pointList[curr].y;
							dist = Mathf.Sqrt(dx*dx + dy*dy);

							if(dist.CompareTo(inner) < 0) 
							{	 
								Triangulator.pointList.Add(pi);	
								++c;   
							}
							else 
							{
	 							
								Triangulator.pointList.Add(p2);
								Triangulator.pointList.Add(p4);	
								c += 2;	
							}
						}
						else 
						{	 
							Triangulator.pointList.Add(p2);	
							++c;  
						}
					}
					else 
					{
						if(Intersection(p1, p2, p3, p4, ref pi)) 
						{ 
							dx   = pi.x - Triangulator.pointList[curr].x;
							dy   = pi.y - Triangulator.pointList[curr].y;
							dist = Mathf.Sqrt(dx*dx + dy*dy); 
							
							if(dist.CompareTo(outer) < 0)
							{ 
								Triangulator.pointList.Add(pi);	
								++c;	
							}
							else 
							{ 
								dx = (n1.x + n2.x) * 0.5f;
								dy = (n1.y + n2.y) * 0.5f;
								float db = Mathf.Sqrt(dx*dx + dy*dy);
								float d = (outer - db) / (dist - db);  
						
								p2.x = p2.x + (pi.x - p2.x) * d;   
								p2.y = p2.y + (pi.y - p2.y) * d; 
								p4.x = p4.x + (pi.x - p4.x) * d;   
								p4.y = p4.y + (pi.y - p4.y) * d; 
	
								Triangulator.pointList.Add(p2);
								Triangulator.pointList.Add(p4);	

								transparentIndexList.Add(curr + positionList.Count); 	
								transparentIndexList.Add(++c + positionList.Count); 
								transparentIndexList.Add(++c + positionList.Count); 
							}  
						}
						else 
						{  
							Triangulator.pointList.Add(p4);	
							++c;	 
						}
					}
					
					n = ((count-1) == i) ? start : c + 1; 
						
					transparentIndexList.Add(curr + positionList.Count);  
					transparentIndexList.Add(c + positionList.Count);
					transparentIndexList.Add(n + positionList.Count);   
					
					transparentIndexList.Add(curr + positionList.Count);  
					transparentIndexList.Add(n + positionList.Count);
					transparentIndexList.Add(next + positionList.Count);
				}

                offset += count;
            }           
		}
		
        static void Cap(Path path, Point a, Point b, float width, string cap) 
        {
            Vector2 p1 = Vector2.zero; 
			Vector2 p2 = Vector2.zero;
            Vector2 n1 = Vector2.zero;
            Vector2 n2 = Vector2.zero;
			float   theta;	
			int     segs;
            
            n1.Set(b.y - a.y, a.x - b.x);
            n2.Set(a.x - b.x, b.y - a.y);
            n1.Normalize();
            n2.Normalize();
            n1 *= width;
            n2 *= width;
 
            if("square" == cap) 
            {
                p1.x = a.x + n1.x + n2.x;
                p1.y = a.y + n1.y - n2.y;
                p2.x = a.x - n1.x + n2.x;
                p2.y = a.y - n1.y - n2.y;

				path.AddPoint(p1.x, p1.y, false); 
				path.AddPoint(p2.x, p2.y, false);
            }
            else if("round" == cap)
            {
                segs = Mathf.RoundToInt(3.1415926f * width / Path.approximationScale);
                segs = Mathf.Clamp(segs, 4, 32);
                p1.x = a.x + n1.x;
                p1.y = a.y + n1.y;
                p2.x = a.x - n1.x;
                p2.y = a.y - n1.y;
                
                path.AddPoint(p1.x, p1.y, false);
                
                for(int i=1; i<segs; ++i)
                {
                    theta = 3.1415926f * (float)i / segs;
                    p1.x  = a.x + Mathf.Sin(theta) * n1.y + Mathf.Cos(theta) * n1.x; 
                    p1.y  = a.y - Mathf.Sin(theta) * n1.x + Mathf.Cos(theta) * n1.y;
                    
                    path.AddPoint(p1.x, p1.y, false);
                }   

                path.AddPoint(p2.x, p2.y, false);
            }
            else  
            {
                p1.x = a.x + n1.x;
                p1.y = a.y + n1.y;
                p2.x = a.x - n1.x;
                p2.y = a.y - n1.y;

                path.AddPoint(p1.x, p1.y, false); 
				path.AddPoint(p2.x, p2.y, false);
            }
        }
		
		static void Miter(Path path, Point c, Point p, Point n, Vector2 n1, Vector2 n2, float width, float limit, bool inner) 
		{
			Point p1 = default(Point);
			Point p2 = default(Point);   
			Point p3 = default(Point);
			Point p4 = default(Point);	
			Point pi = default(Point);
			
			p1.x = p.x + n1.x;
            p1.y = p.y + n1.y;
            p2.x = c.x + n1.x;
            p2.y = c.y + n1.y;
            p3.x = n.x + n2.x;
            p3.y = n.y + n2.y;
            p4.x = c.x + n2.x;
            p4.y = c.y + n2.y;	  
						
			if(Intersection(p1, p2, p3, p4, ref pi)) 
			{ 
				float dx   = pi.x - c.x;
				float dy   = pi.y - c.y;
				float dist = Mathf.Sqrt(dx*dx + dy*dy);	  

				if(dist.CompareTo(width*limit) < 0)
				{ 
					path.AddPoint(pi.x, pi.y, false);
				}
				else 
				{ 
				    path.AddPoint(p2.x, p2.y, false); 
					path.AddPoint(p4.x, p4.y, false); 
				} 
			}
			else
			{ 	
				if(((p.x - p2.x)*n1.y - (p.y - p2.y)*n1.x < 0.0f) != ((n.x - p2.x)*n1.y - (n.y - p2.y)*n1.x < 0.0f)) 
				{	
					path.AddPoint(p2.x, p2.y, false);	   
				}
				else 
				{  
					path.AddPoint(p2.x, p2.y, false); 
					path.AddPoint(p4.x, p4.y, false);	 
				}  
			}
		}
		
		static void Join(Path path, Point c, Point p, Point n, float width, float limit, string join) 
        {
			float   lim = 0.0f;
            Point   p1  = default(Point);
			Point   p2  = default(Point);   
			Point   p3  = default(Point);
			Point   p4  = default(Point);        
            Vector2 n1  = Vector2.zero;
            Vector2 n2  = Vector2.zero;	 

            n1.Set(p.y - c.y, c.x - p.x);
            n2.Set(c.y - n.y, n.x - c.x);

			lim = ((n1.magnitude.CompareTo(n2.magnitude) < 0) ? n1.magnitude : n2.magnitude)/width;
			if(lim < 1.01f)
				lim = 1.01f;
            
			n1.Normalize();
            n2.Normalize();
            n1 *= width;
            n2 *= width;
            
            p1.x = p.x + n1.x;
            p1.y = p.y + n1.y;
            p2.x = c.x + n1.x;
            p2.y = c.y + n1.y;
            p3.x = n.x + n2.x;
            p3.y = n.y + n2.y;
            p4.x = c.x + n2.x;
            p4.y = c.y + n2.y;

			float cross = (n.x - c.x)*(c.y - p.y) - (n.y - c.y)*(c.x - p.x);
            if(cross.CompareTo(0.0f) <= 0) 
			{
				Miter(path, c, p, n, n1, n2, width, lim, true);
            }
            else if(c.join && "bevel" == join) 
            {
				path.AddPoint(p2.x, p2.y, false); 
				path.AddPoint(p4.x, p4.y, false);  
            }
            else if(c.join && "round" == join) 
            { 
                float t  = 0.0f;
                float a1 = Mathf.Atan2(n2.y, n2.x);
                float a2 = Mathf.Atan2(n1.y, n1.x);
                float da = (a1 > a2) ? a2 - a1 + 6.2831852f : a2 - a1;
                int segs = Mathf.RoundToInt(da * width / Path.approximationScale);
                segs = Mathf.Clamp(segs, 4, 32);

                path.AddPoint(p2.x, p2.y, false);
                
                for(int i=1; i<segs; ++i)
                {
                    t = da * (float)i / segs;
                    
                    p1.x = c.x + Mathf.Sin(t) * n1.y + Mathf.Cos(t) * n1.x; 
                    p1.y = c.y - Mathf.Sin(t) * n1.x + Mathf.Cos(t) * n1.y;
                                        
                    path.AddPoint(p1.x, p1.y, false);	  
                } 

                path.AddPoint(p4.x, p4.y, false);	
            } 
            else 
            {
                Miter(path, c, p, n, n1, n2, width, limit, false);
            }
        }

        static void CreateDashedPath(Path path, List<float> array, float offset)
        {
            int     count, next, i, d;
            float   dash, length, a, b;
			
			Point   pt  = default(Point);	
			SubPath t   = default(SubPath);
            Vector2 dir = Vector2.zero;
            
            dashed.Clear();

            foreach(SubPath sp in path.subPathList) 
            {
                count = sp.closed ? sp.points.Length : sp.points.Length - 1;   
				dash  = -offset;
				d     = 0;

				if(dash < 0.0f) 
				{
					while(dash + array[d] <= 0.0f) 
					{	
						dash += array[d]; 
						d = (d + 1) % array.Count;
					}
				}
				else if(dash > 0.0f)
				{
					d = array.Count;
					while(dash > 0.0f) 
					{
						d = (d != 0) ? (d - 1) : (array.Count - 1);	  
						dash -= array[d];
					}
				}

                for(i=0; i<count; ++i) 
                {
                    next = (i + 1) % sp.points.Length;

                    dir.Set(sp.points[next].x - sp.points[i].x, sp.points[next].y - sp.points[i].y);
                    length = dir.magnitude;
                    dir.Normalize();

                    while(dash < length) 
                    {
                        if((d % 2) == 0) 
                        {
                            a = Mathf.Clamp(dash, 0.0f, length);
                            b = Mathf.Clamp(dash + array[d], 0.0f, length);

                            if(b - a > 0.0f)
                            {  
                                pt.x = sp.points[i].x + dir.x * a;
                                pt.y = sp.points[i].y + dir.y * a;	
								pt.join = sp.points[i].join;

                                if(0 == dashed.pointList.Count)
									dashed.pointList.Add(pt);

								pt.x = sp.points[i].x + dir.x * b;
                                pt.y = sp.points[i].y + dir.y * b;
                                dashed.pointList.Add(pt);	 
                            }
                        }
						else if(dashed.pointList.Count > 1) 
						{	
							
							t.points = dashed.pointList.ToArray(); 
							dashed.pointList.Clear();
							dashed.subPathList.Add(t);
						}

						dash += array[d];
						if(dash < length)
							d = (d + 1) % array.Count;
                    }
					
					dash -= (length + array[d]);	 
                }

				if(dashed.pointList.Count > 1) 
				{ 
					t.points = dashed.pointList.ToArray(); 
					dashed.pointList.Clear();
					dashed.subPathList.Add(t);
				}
            }
        }

        static void CreateStroke(Path path, Style style, Transform transform) 
        {
            int   prev, next, count;
            float width = style.strokeWidth * 0.5f;

            path.Transform(transform.inverse);
            stroke.Clear();

            if(null != style.strokeDashArray && style.strokeDashArray.Count > 0) 
            { 
				CreateDashedPath(path, style.strokeDashArray, style.strokeDashOffset); 
                path = dashed;
            }

            foreach(SubPath sp in path.subPathList) 
            {
                count = sp.points.Length;
                
                if(sp.closed) 
                {
                    for(int i=0; i<count; ++i) 
                    {
                        prev = (i - 1 + count) % count;
                        next = (i + 1) % count;

                        Join(stroke, sp.points[i], sp.points[prev], sp.points[next], width, style.strokeMitterLimit, style.strokeJoin);    
                    }  
					stroke.Finalize(false);	
                    
                    for(int i=count-1; i>=0; --i) 
                    {
                        next = (i - 1 + count) % count;
                        prev = (i + 1) % count;

                        Join(stroke, sp.points[i], sp.points[prev], sp.points[next], width, style.strokeMitterLimit, style.strokeJoin);   
                    }
                    stroke.Finalize(false);
                }
                else 
                {
                    Cap(stroke, sp.points[0], sp.points[1], width, style.strokeCap);

                    for(int i=1; i<count-1; ++i) 
                        Join(stroke, sp.points[i], sp.points[i-1], sp.points[i+1], width, style.strokeMitterLimit, style.strokeJoin);   
   
                    Cap(stroke, sp.points[count-1], sp.points[count-2], width, style.strokeCap);

                    for(int i=count-2; i>0; --i) 
                        Join(stroke, sp.points[i], sp.points[i+1], sp.points[i-1], width, style.strokeMitterLimit, style.strokeJoin);   
                   
                    stroke.Finalize(false);  
                }
            }

            stroke.Transform(transform);
        }

        static void AddPath(Path path, Path clip, Transform transform, Color32 color, string gradient, float opacity, float aa) 
        {
            Transform paint       = Transform.identity;
            Vector3   pos         = Vector3.zero; 
            Vector2   uv          = Vector2.zero;
            Vector2   r           = Vector2.zero;
            Vector2   focal       = Vector2.zero;	
			Vector2   so          = Vector2.one;  
			Point     pt          = default(Point);
			int       count       = 0;
            bool      solid       = string.IsNullOrEmpty(gradient);
            bool      radial      = false;	
			bool      transparent = false;	 

			Clipper.Clip(null == clip ? ClipOp.UNION : ClipOp.INTERSECTION, path, clip, result);	
			Reduce(result);	
			Triangulator.Triangulate(reduce);  
						
			count = Triangulator.pointList.Count;  
			so.y  = 255.0f;	   
			
            if(solid)	 
                SolidParams(ref color, opacity, ref uv);  
			else 
				GradientParams(gradient, path, transform, opacity, aa, ref uv.y, ref so.y, ref focal, ref paint, ref radial, ref transparent);
            
            if(opacity < 1.0f || transparent || isTransparent)
			{
				foreach(int i in Triangulator.indexList)	 
					transparentIndexList.Add(i + positionList.Count);	
			}
			else 
			{ 
				indexOffsetList.Add(indexList.Count); 

				foreach(int i in Triangulator.indexList)
					indexList.Add(i + positionList.Count);
			}
			
			if(aa > 0.0f) 
                CreateAntiAliasing(reduce, aa);	  
            
            for(int i=0; i<Triangulator.pointList.Count; ++i)
            {
				pt   = Triangulator.pointList[i];
				r.x  = uv.x;
				so.x = (i < count) ? 1.0f : 0.0f;  

				if(null != filterInstance) 
					filterInstance.rect.Add(pt.x, pt.y);	

				rect.Add(pt.x, pt.y);
                pos.Set(pt.x, -pt.y, numPaths * depthOffset);	 				

                if(!solid) 
                {
                    pt   = paint * pt;
                    uv.x = pt.x;
					r.x  = pt.x;
                    r.y  = radial ? pt.y : 0.0f; 
                }
  
                positionList.Add(pos);
				vertexColorList.Add(color);
                typeList.Add(solid);
                uvList.Add(uv);
                radialList.Add(r);
                focalList.Add(focal);
				spreadOpacityList.Add(so);	   
				spreadOpacityOffsetList.Add(so);	
            } 	
			
			numPaths++;	 
        }

        public static void Clear() 
        {
            positionList.Clear();
            uvList.Clear();
            radialList.Clear();
            focalList.Clear();	
			spreadOpacityList.Clear();	  
			spreadOpacityOffsetList.Clear();
            indexList.Clear();	
			transparentIndexList.Clear(); 
			indexOffsetList.Clear();
			vertexColorList.Clear();
            typeList.Clear();
            
			colorList.Clear();
            gradientList.Clear();  

			result.Clear();
            stroke.Clear();
            dashed.Clear();	
			reduce.Clear();
			collider.Clear(); 	
			
			FilterInstance.Clear();

			gradientType   = GradientType.NONE; 
			rect           = Rect.Default; 	
			filterInstance = null;
			batch          = null;
			numPaths       = 0;	
			numGradients   = 0;	 
			start          = 0;
			usePad         = false;  	
			useRepeat      = false;
			useReflect     = false;
        }
        
        public static void AddGradient(Gradient gradient) 
        {
            if(string.IsNullOrEmpty(gradient.name))
                return;
            
            foreach(Gradient g in gradientList) 
            {
                if(gradient.name == g.name)
                    return;
            }

            if(0 == gradient.stopList.Count)	
			{
				gradientList.Add(gradient);	
			}
			else if(numGradients == gradientList.Count)
			{ 
				gradientList.Add(gradient);	  
				++numGradients;	
			}
			else 
			{  
				Gradient temp = gradientList[numGradients];
  				gradientList[numGradients++] = gradient;
				gradientList.Add(temp);	 
			}
        }

        public static void AddPath(Path path, Path clip, Style style, Transform transform) 
        {
			if(isCollider) 
			{
				Clipper.Clip(ClipOp.INTERSECTION, path, clip, result);
				Reduce(result);
				collider.subPathList.AddRange(reduce.subPathList);				
			}
			else 
			{  
				Transform t = Transform.identity;
				bool      b = renderToTexture && !string.IsNullOrEmpty(style.filter);
				
				if(renderToTexture && null == filterInstance) 
					filterInstance = new FilterInstance(null, "");

				if(b) 
				{ 
					BeginFilter(style.filter); 
					path.Transform(transform.inverse);
					t         = transform;	
					transform = Transform.identity;  
				}
								
				if(style.useFill) 
					AddPath(path, (b ? null : clip), transform, style.fillColor, style.fillGradient, style.fillOpacity, (style.useStroke ? 0.0f : antialiasingWidth));	 

				if(style.useStroke && style.strokeWidth > 0.0f) 
				{
					CreateStroke(path, style, transform);	
					AddPath(stroke, (b ? null : clip), transform, style.strokeColor, style.strokeGradient, style.strokeOpacity, antialiasingWidth);
				}  
				
				if(renderToTexture) 
				{	  
					if(null == batch) 
						batch = filterInstance.AddBatch(start, null, false);	 
				
					batch.count = transparentIndexList.Count - batch.start;
				}
				
				if(b)	
					EndFilter(clip, t);
			}
        } 
		
		public static void BeginFilter(string filter) 
		{
			FilterInstance instance = new FilterInstance(filterInstance, filter); 
			
			filterInstance.AddBatch(0, instance, false);
	 
			filterInstance = instance;  
			start = batch.start + batch.count;
			batch = null;  			
		}	 
		
		public static void EndFilter(Path clip, Transform transform) 
		{
			FilterInstance parent = filterInstance.parent; 
			Transform	   inv    = transform.inverse; 
			Transform	   t      = Transform.identity;
			Point          p1     = default(Point);
			Point          p2     = default(Point);
			Point          p3     = default(Point); 
			Point          p4     = default(Point);
			Rect           rect   = filterInstance.rect;
			Vector2        pos    = filterInstance.filter.position;  
			Vector2        size   = filterInstance.filter.size;	
			Vector2        vec	  = Vector2.zero; 
			Vector2        v1     = Vector2.zero;  
			Vector2        v2     = Vector2.zero;
			Vector2        v3     = Vector2.zero;	 
			Vector2        v4     = Vector2.zero;
			Vector2        v5     = Vector2.zero;
			int            i      = positionList.Count;

			if(!filterInstance.filter.filterUserSpaceOnUse)
			{ 
				pos.x   = rect.left + pos.x * rect.Width; 
				pos.y   = rect.top + pos.y * rect.Height;	 
				size.x *= rect.Width; 
				size.y *= rect.Height;
			}

			p1.x = pos.x;	
			p1.y = pos.y; 
			p2.x = pos.x + size.x;	
			p2.y = pos.y;	
			p3.x = pos.x + size.x;	
			p3.y = pos.y + size.y; 
			p4.x = pos.x;	
			p4.y = pos.y + size.y; 
			
			p1 = transform * p1;	
			p2 = transform * p2; 
			p3 = transform * p3;	
			p4 = transform * p4;	

			stroke.Clear();
			stroke.AddPoint(p1.x, p1.y, false); 
			stroke.AddPoint(p2.x, p2.y, false);	 
			stroke.AddPoint(p3.x, p3.y, false);	
			stroke.AddPoint(p4.x, p4.y, false);
			stroke.Finalize(false);	  
			
			v1.Set(p1.x, p1.y);  
			v2.Set(p2.x, p2.y);
			v3.Set(p3.x, p3.y);	 
			v4 = v2 - v1;
			v5 = v3 - v2;  
			
			t.Translate(p1.x, p1.y);			
			t.Rotate((Mathf.Atan2(v4.y, v4.x) * Mathf.Rad2Deg), 0.0f, 0.0f);
						
			filterInstance.size.Set(v4.magnitude, v5.magnitude);
			filterInstance.position.Set(p1.x, -p1.y); 	 
			filterInstance.view      = size;
			filterInstance.center    = pos + size * 0.5f; 
			filterInstance.center.y  = -filterInstance.center.y;	 
			filterInstance.transform =  t.inverse * transform;	 
						
			p1.x = rect.left;	
			p1.y = rect.top; 
			p2.x = rect.right;	
			p2.y = rect.top;	
			p3.x = rect.right;	
			p3.y = rect.bottom; 
			p4.x = rect.left;	
			p4.y = rect.bottom; 
			
			p1 = transform * p1;	
			p2 = transform * p2; 
			p3 = transform * p3;	
			p4 = transform * p4; 
			
			rect = Rect.Default;
			rect.Add(p1.x, p1.y);  
			rect.Add(p2.x, p2.y);
			rect.Add(p3.x, p3.y);
			rect.Add(p4.x, p4.y); 	
			
			filterInstance.rect = rect;
						
			parent.rect.Add(rect);
			batch = filterInstance.AddBatch(null == batch ? start : batch.start + batch.count, null, true);			  
			filterInstance = null;
			
			AddPath(stroke, clip, Transform.identity, Color.black, "", 0.0f, antialiasingWidth); 
 
			batch.count    = transparentIndexList.Count - batch.start;
			start          = batch.start + batch.count;
			filterInstance = parent;
			batch          = null;

			for(; i<positionList.Count; ++i)
			{
				vec   = positionList[i];
				vec.y = -vec.y;
				vec   = inv * vec;
				vec.x = (vec.x - pos.x) / size.x;
				vec.y = (vec.y - pos.y) / size.y; 
				vec.y = 1.0f - vec.y;

				radialList[i] = vec;	
			}
		}
        
        public static void CreateMesh(Vector2 pivot, ModelImporterMeshCompression compression, Vector4 s9g,  bool optimize, bool sliced, ref Mesh mesh, ref MaterialFlags flags) 
        {
			int       start     = 0; 
			int       count     = 0;
			int[]     triangles = (indexList.Count > 0) ? new int[indexList.Count] : null;	  
            Vector2   vec       = Vector2.zero;
			Vector3   offset    = Vector3.zero;		

			for(int i=indexOffsetList.Count, n=0; i>0; --i) 
			{
  				start = indexOffsetList[i-1];
				count = (i == indexOffsetList.Count) ? indexList.Count - start : indexOffsetList[i] - start;	 
				
				for(int j=0; j<count; ++j)	
					triangles[n++] = indexList[start + j];
			}

			if(0 == indexList.Count) 
			{
				for(int i=0; i<positionList.Count; ++i)
				{  
					offset = positionList[i];
					offset.z = 0.0f; 
					positionList[i] = offset;
				}
			}
			else 
			{  
				indexList.Clear();
				indexList.AddRange(triangles);
			}
				
			offset.x = pivot.x * rect.Width + rect.left; 
			offset.y = pivot.y * rect.Height - rect.bottom;

			for(int i=0; i<positionList.Count; ++i) 
				positionList[i] -= offset;

			if(s9g.sqrMagnitude > 0.0f)
				SplitMesh(rect, offset, s9g.x, s9g.y, s9g.z, s9g.w, sliced);
				   
			mesh.Clear();
			
			mesh.uv2          = null;  
			mesh.uv3          = null;  
			mesh.uv4          = null;
			mesh.colors32     = null; 
			mesh.vertices     = positionList.ToArray();	 
 			mesh.tangents     = (sliced && s9g.sqrMagnitude > 0.0f) ? spreadOpacityOffsetList.ToArray() : null;
			mesh.uv           = (sliced && s9g.sqrMagnitude > 0.0f) ? null : spreadOpacityList.ToArray();
			mesh.subMeshCount = (indexList.Count > 0 && transparentIndexList.Count > 0) ? 2 : 1;

#if UNITY_2017_1_OR_NEWER			
			mesh.indexFormat  = UnityEngine.Rendering.IndexFormat.UInt32;
#endif

			if(indexList.Count > 0) 
			{ 
				flags |= MaterialFlags.OPAQUE;
				mesh.SetTriangles(indexList.ToArray(), 0);  
			}

			if(transparentIndexList.Count > 0) 
			{
				flags |= MaterialFlags.TRANSPARENT;
				mesh.SetTriangles(transparentIndexList.ToArray(), indexList.Count > 0 ? 1 : 0); 
			}

            if (GradientType.NONE != gradientType) 
			{
				flags |= MaterialFlags.GRADIENT;
				
				if(usePad) 
					flags |= MaterialFlags.PAD;
				if(useRepeat) 
					flags |= MaterialFlags.REPEAT;
				if(useReflect) 
					flags |= MaterialFlags.REFLECT;

				for(int i=0; i<uvList.Count; ++i) 
				{ 
					vec.y = (typeList[i]) ? uvList[i].y + numGradients + 0.5f : uvList[i].y + 0.5f;
					vec.y = vec.y/(colorList.Count/256 + 1 + numGradients);
					vec.x = uvList[i].x;
					uvList[i] = vec;
				}   

				if(GradientType.LINEAR == gradientType) 
				{  
					flags |= MaterialFlags.LINEAR;

					if(sliced && s9g.sqrMagnitude > 0.0f)					
						mesh.uv = uvList.ToArray();
		 			else  
						mesh.uv2 = uvList.ToArray();
				}
				else if(GradientType.RADIAL == gradientType)
				{ 
					flags |= MaterialFlags.RADIAL;
					
					if(sliced && s9g.sqrMagnitude > 0.0f) 
					{ 
						mesh.uv  = uvList.ToArray();
						mesh.uv2 = radialList.ToArray();
					}
					else 
					{  
						mesh.uv2 = uvList.ToArray();
						mesh.uv3 = radialList.ToArray();
					}
				}  
				else if(GradientType.FOCAL == gradientType)
				{ 
					flags |= MaterialFlags.FOCAL;	 
					
					if(sliced && s9g.sqrMagnitude > 0.0f) 
					{ 
						mesh.uv  = uvList.ToArray();
						mesh.uv2 = radialList.ToArray(); 
						mesh.uv3 = focalList.ToArray();
					}
					else 
					{  
						mesh.uv2 = uvList.ToArray();
						mesh.uv3 = radialList.ToArray(); 
						mesh.uv4 = focalList.ToArray();
					}
				}
			}
			else 
			{
				mesh.colors32 = vertexColorList.ToArray(); 	 
			} 
			
			MeshUtility.SetMeshCompression(mesh, compression);

			if(optimize)  
				MeshUtility.Optimize(mesh);
			
			mesh.RecalculateBounds();
			
			Bounds b    = mesh.bounds;
			offset      = b.min; 
			offset.z    = 0.0f;  
			b.min       = offset;  
			offset      = b.max; 
			offset.z    = 0.0f;  
			b.max       = offset;
			mesh.bounds = b;
        }

        public static Texture2D CreateTexture(string name) 
        {  
            int       height  = colorList.Count/256 + 1 + numGradients;
            int       n       = numGradients * 256;
            Color32[] data    = new Color32[256 * height];	
			Texture2D texture = new Texture2D(256, height, TextureFormat.ARGB32, false);

            for(int i=0; i<numGradients; ++i)
                gradientList[i].CreateGradientRump(data, i);

            foreach(Color32 color in colorList) 
                data[n++] = color;           
            
			texture.name       = name;
            texture.filterMode = FilterMode.Point;
            texture.wrapMode   = (useRepeat || useReflect) ? TextureWrapMode.Repeat : TextureWrapMode.Clamp; 
            
			texture.SetPixels32(data);
			texture.Apply();

			return texture;
        }

		public static void CreateCollider(Vector2 center, float margin, ref ColliderPath[] paths) 
		{
			SubPath sp     = default(SubPath);
			Point   curr   = default(Point);  
			Point   prev   = default(Point);
			Point   next   = default(Point);
			Vector2 vec    = Vector2.zero; 
			Vector2 offset = Vector2.zero;	
			Vector2 n1     = Vector2.zero;
			Vector2 n2     = Vector2.zero;
        	int     count  = 0;

			Clipper.Clip(ClipOp.UNION, collider, null, result);
			
			foreach(SubPath s in result.subPathList) 
			{
				count = s.points.Length;

				for(int i=0; i<count; ++i)
				{
 					curr = s.points[i];
					prev = s.points[(i - 1 + count) % count];
                    next = s.points[(i + 1) % count]; 	
					
					n1.Set(prev.y - curr.y, curr.x - prev.x);
					n2.Set(curr.y - next.y, next.x - curr.x);
					n1.Normalize();
					n2.Normalize();

					vec = (n1 + n2) * 0.5f;  
					vec.Normalize();
					vec *= margin;

					s.points[i].x -= vec.x;	 
					s.points[i].y -= vec.y;

					rect.Add(s.points[i].x, s.points[i].y);	 
				}
			}	
			
			offset.x = 0.5f * rect.Width + rect.left; 
			offset.y = 0.5f * rect.Height - rect.bottom;

			if(null == paths) 
				paths = new ColliderPath[result.subPathList.Count];
			else if(result.subPathList.Count != paths.Length)
				Array.Resize(ref paths, result.subPathList.Count);
			
			for(int i=0; i<paths.Length; ++i) 
			{
				sp = result.subPathList[i];
				if(null == paths[i].points)
					paths[i].points = new Vector2[sp.points.Length];
				else if(paths[i].points.Length != sp.points.Length)	
					Array.Resize(ref paths[i].points, sp.points.Length);

				for(int j=0; j<sp.points.Length; ++j)
				{
					vec.Set(sp.points[j].x, -sp.points[j].y);
					paths[i].points[j] = (vec - offset) + center;
				}  
			} 
		}

		public static Texture2D DrawToTexture(int width, int height, ref float scale) 
		{  
			Texture2D     result  = new Texture2D(width, height, TextureFormat.ARGB32, false);
			RenderTexture target  = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);	
			Matrix4x4     view    = Matrix4x4.identity;
			Vector2       vec     = Vector2.zero;  
			int           h       = colorList.Count/256 + 1 + numGradients;
            int           n       = numGradients * 256;
            Color32[]     data    = new Color32[256 * h];  
			Color[]       pixels  = null; 

			for(int i=0; i<numGradients; ++i) 
				gradientList[i].CreateGradientRump(data, i);
			 
            foreach(Color32 color in colorList) 
                data[n++] = color;

			for(int i=0; i<uvList.Count; ++i) 
			{ 
				vec.y = (typeList[i]) ? uvList[i].y + numGradients : uvList[i].y;
				vec.y = vec.y/(colorList.Count/256 + 1 + numGradients);
				vec.x = uvList[i].x;
				uvList[i] = vec;
			}  
			
			FilterInstance.SetTextureData(h, data);
			FilterInstance.SetMeshData(positionList.ToArray(), radialList.ToArray(), uvList.ToArray(), focalList.ToArray(), spreadOpacityList.ToArray(), transparentIndexList);
			 
			result.filterMode   = FilterMode.Point;
			result.wrapMode     = TextureWrapMode.Repeat;
			target.filterMode   = FilterMode.Point;  
			target.antiAliasing = 8;
			target.Create(); 

			Graphics.SetRenderTarget(target);
			GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f)); 
			
			view.m23 = -200.0f;
			GL.modelview = view; 
																			
			filterInstance.Draw(target); 
 		    scale = filterInstance.scale;
															
			result.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, width, height), 0, 0);  			
			pixels = result.GetPixels();
			for(int i=0; i<pixels.Length; ++i) 
			{ 
				if(pixels[i].a.CompareTo(0.0039215686274509803921568627451f) < 0)
					continue;
					
				pixels[i].r /= pixels[i].a;	
				pixels[i].g /= pixels[i].a;	
				pixels[i].b /= pixels[i].a;
			}

			result.SetPixels(pixels); 
			result.Apply();	
						
			Graphics.SetRenderTarget(null);
			target.Release();  
			RenderTexture.ReleaseTemporary(target); 
			
			return result;
		}
    }
}