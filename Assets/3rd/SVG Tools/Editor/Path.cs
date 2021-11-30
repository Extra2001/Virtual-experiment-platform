// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;

using System;
using System.Collections.Generic;


namespace svgtools
{
    public enum FillRule 
    {
        EVEN_ODD = 0,
        NON_ZERO = 1
    }

    public struct Point 
    {
        public float x;  
        public float y;   
		public bool  join;    
    }	 
	
	public struct Rect 
    {
        public float left;  
        public float top;  
		public float right;  
        public float bottom;  
		
		public static Rect Default 
        {
            get
            {
                Rect rect; 
				
				rect.left   = float.MaxValue;	 
				rect.top    = float.MaxValue;
				rect.right  = float.MinValue;  
				rect.bottom = float.MinValue;
                
                return rect;
            }
        }		
		
		public static Rect Zero 
        {
            get
            {
                Rect rect; 
				
				rect.left   = 0.0f;	 
				rect.top    = 0.0f;
				rect.right  = 0.0f;  
				rect.bottom = 0.0f;
                
                return rect;
            }
        }

		public float Width 
        {
            get { return right - left;  }   
			set { right = left + value; }
        }	
		
		public float Height 
        {
            get { return bottom - top;  }   
			set { bottom = top + value; }
        }
		
		public void Add(float x, float y) 
		{	
			left   = (x < left)   ? x : left; 
			top    = (y < top)    ? y : top;
			right  = (x > right)  ? x : right;
			bottom = (y > bottom) ? y : bottom;
                    
		} 
		
		public void Add(Rect r) 
		{	
			left   = (r.left < left)     ? r.left   : left; 
			top    = (r.top < top)       ? r.top    : top;
			right  = (r.right > right)   ? r.right  : right;
			bottom = (r.bottom > bottom) ? r.bottom : bottom;
		}
    }


    public struct SubPath
    {
        public Point[] points;
        public bool    closed;
    }


    public class Path 
    {
        public List<Point>   pointList;
        public List<SubPath> subPathList;

        public FillRule      fillRule;
        
        public Vector2       lastPos;
        public Vector2       lastCp1;
        public Vector2       lastCp2; 	
		public Vector2       lastMove;
		
        static public float  approximationScale = 4.0f;	
        static public int    maxRecursionLevel  = 16;


        public Path() 
        {
            pointList   = new List<Point>(); 
            subPathList = new List<SubPath>(); 	
			fillRule    = FillRule.NON_ZERO;  
			lastPos	    = Vector2.zero;	
			lastCp1	    = Vector2.zero;	
			lastCp2	    = Vector2.zero;	 
			lastMove    = Vector2.zero;
        }

        public void Clear() 
        {
            pointList.Clear();
            subPathList.Clear(); 
            
            fillRule = FillRule.NON_ZERO; 
			lastPos	 = Vector2.zero;	
			lastCp1	 = Vector2.zero;	
			lastCp2	 = Vector2.zero; 
			lastMove = Vector2.zero;
        }

        float SquareDistance(float x1, float y1, float x2, float y2) 
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            return dx*dx + dy*dy;
        }

        void RecursiveQuadraticCurve(float x1, float y1, float x2, float y2, float x3, float y3, int level) 
        {
            if(level > maxRecursionLevel) 
                return;

            float x12  = (x1 + x2) * 0.5f;                
            float y12  = (y1 + y2) * 0.5f;
            float x23  = (x2 + x3) * 0.5f;
            float y23  = (y2 + y3) * 0.5f;
            float x123 = (x12 + x23) * 0.5f;
            float y123 = (y12 + y23) * 0.5f;
            
            float dx = x3 - x1;
            float dy = y3 - y1;
            float d  = Mathf.Abs(((x2 - x3) * dy - (y2 - y3) * dx));            

			if((d * d).CompareTo(approximationScale * (dx*dx + dy*dy)) <= 0) 		  
			{
				AddPoint(x123, y123, false);
				return;
			}
			
            RecursiveQuadraticCurve(x1, y1, x12, y12, x123, y123, level + 1); 
            RecursiveQuadraticCurve(x123, y123, x23, y23, x3, y3, level + 1);
        }

        private void RecursiveCubicCurve(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, int level) 
        {
            if(level > maxRecursionLevel) 
                return;
            
            float x12   = 0.5f * (x1 + x2);
            float y12   = 0.5f * (y1 + y2);
            float x23   = 0.5f * (x2 + x3);
            float y23   = 0.5f * (y2 + y3);
            float x34   = 0.5f * (x3 + x4);
            float y34   = 0.5f * (y3 + y4);
            float x123  = 0.5f * (x12 + x23);
            float y123  = 0.5f * (y12 + y23);
            float x234  = 0.5f * (x23 + x34);
            float y234  = 0.5f * (y23 + y34);
            float x1234 = 0.5f * (x123 + x234);
            float y1234 = 0.5f * (y123 + y234);

			float dx = x4 - x1;
            float dy = y4 - y1;
            float d2 = Mathf.Abs((x2 - x4) * dy - (y2 - y4) * dx);
            float d3 = Mathf.Abs((x3 - x4) * dy - (y3 - y4) * dx);
			
			if(((d2 + d3)*(d2 + d3)).CompareTo(approximationScale * (dx*dx + dy*dy)) <= 0)
            {
                AddPoint(x1234, y1234, false);
                return;
            } 
			
            RecursiveCubicCurve(x1, y1, x12, y12, x123, y123, x1234, y1234, level + 1); 
            RecursiveCubicCurve(x1234, y1234, x234, y234, x34, y34, x4, y4, level + 1);
        }

        public void AddPoint(float x, float y, bool join) 
        {
            Point p;
            
            p.x    = x; 
            p.y    = y; 
			p.join = join;
			
            pointList.Add(p);
        }

        public void MoveTo(float x, float y, Transform transform, bool relative, bool join) 
        {
            if(relative)
            {
                x += lastPos.x;
                y += lastPos.y;
            }
            
            lastPos.Set(x, y);
            lastCp1.Set(x, y);
            lastCp2.Set(x, y); 
			lastMove.Set(x, y);

            Vector2 pos = transform * lastPos;
			Finalize(false);

            if(pointList.Count == 1) 
            {
                Point pt     = pointList[0];
                pt.x         = pos.x;
                pt.y         = pos.y;
				pt.join      = join;
                pointList[0] = pt;
            }         
            else
            {
                AddPoint(pos.x, pos.y, join);
            }  						
        }
        
        public void LineTo(float x, float y, Transform transform, bool relative, bool join)
        {
            if(relative)
            {
                x += lastPos.x;
                y += lastPos.y;
            }
            
            lastPos.Set(x, y);
            lastCp1.Set(x, y);
            lastCp2.Set(x, y);

            Vector2 pos = transform * lastPos;

            AddPoint(pos.x, pos.y, join);  
        }		
		
		public void LineTo(float f, Transform transform, bool vertical, bool relative, bool join)
        {
			if(vertical) 
			{	
				if(relative)    
					f += lastPos.y;	
				
				lastPos.y = lastCp1.y = lastCp2.y = f;
			}
			else 
			{  
				if(relative)    
					f += lastPos.x;	
				
				lastPos.x = lastCp1.x = lastCp2.x = f;
			}
           
            Vector2 pos = transform * lastPos;

            AddPoint(pos.x, pos.y, join);  
        }
        
        public void QuadraticCurveTo(float x, float y, float x1, float y1, Transform transform, bool relative, bool smooth)
        {
            Vector2 pos1, pos2, cp1;

            pos1.x = lastPos.x;
            pos1.y = lastPos.y;
            cp1.x  = x1;
            cp1.y  = y1;
            pos2.x = x;
            pos2.y = y;

            if(relative)
            {
                pos2 += lastPos;
                cp1  += lastPos;
            }

            if(smooth) 
                cp1 = lastPos + lastPos - lastCp1;
            
            lastPos = pos2;
            lastCp1 = cp1;
            lastCp2 = pos2;
            
            pos1 = transform * pos1;
            cp1  = transform * cp1;
            pos2 = transform * pos2;

            RecursiveQuadraticCurve(pos1.x, pos1.y, cp1.x, cp1.y, pos2.x, pos2.y, 0);
            AddPoint(pos2.x, pos2.y, true);
        }
        
        public void CubicCurveTo(float x, float y, float x1, float y1, float x2, float y2, Transform transform, bool relative, bool smooth)
        {
            Vector2 pos1, pos2, cp1, cp2;

            pos1.x = lastPos.x;
            pos1.y = lastPos.y;
            cp1.x  = x1;
            cp1.y  = y1;
            cp2.x  = x2;
            cp2.y  = y2;
            pos2.x = x;
            pos2.y = y;

            if(relative)
            {
                pos2 += lastPos;
                cp1  += lastPos;
                cp2  += lastPos;
            }

            if(smooth) 
                cp1 = lastPos + lastPos - lastCp2;
            
            lastPos = pos2;
            lastCp1 = pos2;
            lastCp2 = cp2;
            
            pos1 = transform * pos1;
            cp1  = transform * cp1;
            cp2  = transform * cp2;
            pos2 = transform * pos2;

            RecursiveCubicCurve(pos1.x, pos1.y, cp1.x, cp1.y, cp2.x, cp2.y, pos2.x, pos2.y, 0);
            AddPoint(pos2.x, pos2.y, true);	  
        } 
        
        public void ArcTo(float rx, float ry, float angle, bool large, bool sweep, float x, float y, Transform transform, bool relative, bool join)
        {
            Vector2 pos1, pos2, cp1, cp2;

            if(relative)
            {
                x += lastPos.x;
                y += lastPos.y;
            }

            if(rx < 0.0f) 
                rx = -rx;
            if(ry < 0.0f) 
                ry = -rx; 

            float cos = Mathf.Cos(angle * 0.0174533f);
            float sin = Mathf.Sin(angle * 0.0174533f);

            float dx = (lastPos.x - x) * 0.5f;
            float dy = (lastPos.y - y) * 0.5f;
            float x1 = cos * dx + sin * dy;
            float y1 = cos * dy - sin * dx;

            float prx = rx * rx;
            float pry = ry * ry;
            float px1 = x1 * x1;
            float py1 = y1 * y1;
            
            float check = px1/prx + py1/pry;
            if(check > 1.0f) 
            {
                rx  = Mathf.Sqrt(check) * rx;
                ry  = Mathf.Sqrt(check) * ry;
                prx = rx * rx;
                pry = ry * ry;
            }
            
            float sign = (large == sweep) ? -1.0f : 1.0f;
            float sq   = (prx*pry - prx*py1 - pry*px1) / (prx*py1 + pry*px1);
            float coef = sign * Mathf.Sqrt((sq < 0.0f) ? 0.0f : sq);
            float cx1  = coef *  ((rx * y1) / ry);
            float cy1  = coef * -((ry * x1) / rx);

            float cx = (lastPos.x + x) * 0.5f + (cos * cx1 - sin * cy1);
            float cy = (lastPos.y + y) * 0.5f + (sin * cx1 + cos * cy1);

            float ux = (x1 - cx1) / rx;
            float uy = (y1 - cy1) / ry;
            float vx = (-x1 - cx1) / rx;
            float vy = (-y1 - cy1) / ry;
            
            float v = ux / Mathf.Sqrt(ux*ux + uy*uy);
            v = Mathf.Clamp(v, -1.0f, 1.0f);
            float startAngle = (uy < 0.0f) ? -Mathf.Acos(v) : Mathf.Acos(v);
            
            v = (ux * vx + uy * vy) / Mathf.Sqrt((ux*ux + uy*uy) * (vx*vx + vy*vy));
            v = Mathf.Clamp(v, -1.0f, 1.0f);

            float sweepAngle = (ux * vy - uy * vx < 0.0f) ? -Mathf.Acos(v) : Mathf.Acos(v);
            if(!sweep && sweepAngle > 0.0f) 
                sweepAngle -= 6.283185307179586476925286766559f;
            else if (sweep && sweepAngle < 0.0f) 
                sweepAngle += 6.283185307179586476925286766559f;
            
            startAngle = startAngle % 6.283185307179586476925286766559f;
            sweepAngle = Mathf.Clamp(sweepAngle, -6.283185307179586476925286766559f, 6.283185307179586476925286766559f);

            float prevSweep;
            float totalSweep = 0.0f;
            float localSweep = 0.0f;
            int   count      = 2;
            bool  done       = false;

            do
            {
                if(sweepAngle < 0.0f)
                {
                    prevSweep  = totalSweep;
                    localSweep = -1.5707963f;
                    totalSweep -= 1.5707963f;
                    
                    if(totalSweep <= sweepAngle + 0.01f)
                    {
                        localSweep = sweepAngle - prevSweep;
                        done = true;
                    }
                }
                else 
                {
                    prevSweep  = totalSweep;
                    localSweep = 1.5707963f;
                    totalSweep += 1.5707963f;
                    
                    if(totalSweep >= sweepAngle - 0.01f)
                    {
                        localSweep = sweepAngle - prevSweep;
                        done = true;
                    }
                }
                
                float x0  = Mathf.Cos(localSweep * 0.5f);
                float y0  = Mathf.Sin(localSweep * 0.5f);
                float tx  = (1.0f - x0) * 4.0f / 3.0f;
                float ty  = y0 - tx * x0 / y0;
                float vx0 = x0;
                float vy0 = y0;
                float vx1 = x0 + tx;
                float vy1 = ty;
                float vx2 = x0 + tx;
                float vy2 = -ty;
                
                float sn = Mathf.Sin(startAngle + localSweep * 0.5f);
                float cs = Mathf.Cos(startAngle + localSweep * 0.5f);
                
                float curve00 = rx * (vx0 * cs - vy0 * sn);
                float curve11 = ry * (vx0 * sn + vy0 * cs);
                float curve22 = rx * (vx1 * cs - vy1 * sn);
                float curve33 = ry * (vx1 * sn + vy1 * cs);
                float curve44 = rx * (vx2 * cs - vy2 * sn);
                float curve55 = ry * (vx2 * sn + vy2 * cs);                
                
                pos2.x = curve00 * cos - curve11 * sin + cx;
                pos2.y = curve00 * sin + curve11 * cos + cy;
                cp2.x  = curve22 * cos - curve33 * sin + cx;
                cp2.y  = curve22 * sin + curve33 * cos + cy;
                cp1.x  = curve44 * cos - curve55 * sin + cx;
                cp1.y  = curve44 * sin + curve55 * cos + cy;

                pos1    = transform * lastPos;
                lastPos = pos2;
                pos2    = transform * pos2;
                cp1     = transform * cp1;
                cp2     = transform * cp2;
                
                RecursiveCubicCurve(pos1.x, pos1.y, cp1.x, cp1.y, cp2.x, cp2.y, pos2.x, pos2.y, 0);
                AddPoint(pos2.x, pos2.y, false);              
                
                count += 6;
                startAngle += localSweep;
            }
            while(!done && count < 26);
            
			Point pt = pointList[pointList.Count-1]; 
 			pt.join  = join;
			pointList[pointList.Count-1] = pt;

            lastCp1 = lastCp2 = lastPos; 	  
        }

		public void Finalize(bool close) 
		{
 			int count = pointList.Count;

			if(count > 1 && pointList[0].x.Equals(pointList[count-1].x) && pointList[0].y.Equals(pointList[count-1].y)) 
				pointList.RemoveAt(--count); 	   

			if(count > 1) 
            { 
                SubPath sp;	

                sp.closed = close && count > 2;	
                sp.points = pointList.ToArray(); 

                subPathList.Add(sp);
                pointList.Clear();

				if(close) 
				{ 
					pointList.Add(sp.points[0]);
					lastPos = lastCp1 = lastCp2 = lastMove;
				}
            }
		} 

        public void Transform(Transform transform) 
        {
            Point p;

            foreach(SubPath sp in subPathList) 
            {
                for(int i=0; i<sp.points.Length; ++i) 
                {
                    p = transform * sp.points[i];
                    sp.points[i] = p;
                }
            }
        }	  
    }
}