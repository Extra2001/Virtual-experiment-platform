// Copyright (C) 2017 Alexander Klochkov - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using UnityEngine;

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;  

using svgtools.clipper;


namespace svgtools
{   
    public class Loader
    {
        static Dictionary<string, Color32> colorMap = new Dictionary<string, Color32> 
        {
            { "aliceblue",            new Color32(240, 248, 255, 255) },
            { "antiquewhite",         new Color32(250, 235, 215, 255) },
            { "aqua",                 new Color32(  0, 255, 255, 255) },
            { "aquamarine",           new Color32(127, 255, 212, 255) },
            { "azure",                new Color32(240, 255, 255, 255) },
            { "beige",                new Color32(245, 245, 220, 255) },
            { "bisque",               new Color32(255, 228, 196, 255) },
            { "black",                new Color32(  0,   0,   0, 255) },
            { "blanchedalmond",       new Color32(255, 235, 205, 255) },
            { "blue",                 new Color32(  0,   0, 255, 255) },
            { "blueviolet",           new Color32(138,  43, 226, 255) },
            { "brown",                new Color32(165,  42,  42, 255) },
            { "burlywood",            new Color32(222, 184, 135, 255) },
            { "cadetblue",            new Color32( 95, 158, 160, 255) },
            { "chartreuse",           new Color32(127, 255,   0, 255) },
            { "chocolate",            new Color32(210, 105,  30, 255) },
            { "coral",                new Color32(255, 127,  80, 255) },
            { "cornflowerblue",       new Color32(100, 149, 237, 255) },
            { "cornsilk",             new Color32(255, 248, 220, 255) },
            { "crimson",              new Color32(220,  20,  60, 255) },
            { "cyan",                 new Color32(  0, 255, 255, 255) },
            { "darkblue",             new Color32(  0,   0, 139, 255) },
            { "darkcyan",             new Color32(  0, 139, 139, 255) },
            { "darkgoldenrod",        new Color32(184, 134,  11, 255) },
            { "darkgray",             new Color32(169, 169, 169, 255) },
            { "darkgreen",            new Color32(  0, 100,   0, 255) },
            { "darkgrey",             new Color32(169, 169, 169, 255) },
            { "darkkhaki",            new Color32(189, 183, 107, 255) },
            { "darkmagenta",          new Color32(139,   0, 139, 255) },
            { "darkolivegreen",       new Color32( 85, 107,  47, 255) },
            { "darkorange",           new Color32(255, 140,   0, 255) },
            { "darkorchid",           new Color32(153,  50, 204, 255) },
            { "darkred",              new Color32(139,   0,   0, 255) },
            { "darksalmon",           new Color32(233, 150, 122, 255) },
            { "darkseagreen",         new Color32(143, 188, 143, 255) },
            { "darkslateblue",        new Color32( 72,  61, 139, 255) },
            { "darkslategray",        new Color32( 47,  79,  79, 255) },
            { "darkslategrey",        new Color32( 47,  79,  79, 255) },
            { "darkturquoise",        new Color32(  0, 206, 209, 255) },
            { "darkviolet",           new Color32(148,   0, 211, 255) },
            { "deeppink",             new Color32(255,  20, 147, 255) },
            { "deepskyblue",          new Color32(  0, 191, 255, 255) },
            { "dimgray",              new Color32(105, 105, 105, 255) },
            { "dimgrey",              new Color32(105, 105, 105, 255) },
            { "dodgerblue",           new Color32( 30, 144, 255, 255) },
            { "firebrick",            new Color32(178,  34,  34, 255) },
            { "floralwhite",          new Color32(255, 250, 240, 255) },
            { "forestgreen",          new Color32( 34, 139,  34, 255) },
            { "fuchsia",              new Color32(255,   0, 255, 255) },
            { "gainsboro",            new Color32(220, 220, 220, 255) },
            { "ghostwhite",           new Color32(248, 248, 255, 255) },
            { "gold",                 new Color32(255, 215,   0, 255) },
            { "goldenrod",            new Color32(218, 165,  32, 255) },
            { "gray",                 new Color32(128, 128, 128, 255) },
            { "grey",                 new Color32(128, 128, 128, 255) },
            { "green",                new Color32(  0, 128,   0, 255) },
            { "greenyellow",          new Color32(173, 255,  47, 255) },
            { "honeydew",             new Color32(240, 255, 240, 255) },
            { "hotpink",              new Color32(255, 105, 180, 255) },
            { "indianred",            new Color32(205,  92,  92, 255) },
            { "indigo",               new Color32( 75,   0, 130, 255) },
            { "ivory",                new Color32(255, 255, 240, 255) },
            { "khaki",                new Color32(240, 230, 140, 255) },
            { "lavender",             new Color32(230, 230, 250, 255) },
            { "lavenderblush",        new Color32(255, 240, 245, 255) },
            { "lawngreen",            new Color32(124, 252,   0, 255) },
            { "lemonchiffon",         new Color32(255, 250, 205, 255) },
            { "lightblue",            new Color32(173, 216, 230, 255) },
            { "lightcoral",           new Color32(240, 128, 128, 255) },
            { "lightcyan",            new Color32(224, 255, 255, 255) },
            { "lightgoldenrodyellow", new Color32(250, 250, 210, 255) },
            { "lightgray",            new Color32(211, 211, 211, 255) },
            { "lightgreen",           new Color32(144, 238, 144, 255) },
            { "lightgrey",            new Color32(211, 211, 211, 255) },
            { "lightpink",            new Color32(255, 182, 193, 255) },
            { "lightsalmon",          new Color32(255, 160, 122, 255) },
            { "lightseagreen",        new Color32( 32, 178, 170, 255) },
            { "lightskyblue",         new Color32(135, 206, 250, 255) },
            { "lightslategray",       new Color32(119, 136, 153, 255) },
            { "lightslategrey",       new Color32(119, 136, 153, 255) },
            { "lightsteelblue",       new Color32(176, 196, 222, 255) },
            { "lightyellow",          new Color32(255, 255, 224, 255) },
            { "lime",                 new Color32(  0, 255,   0, 255) },
            { "limegreen",            new Color32( 50, 205,  50, 255) },
            { "linen",                new Color32(250, 240, 230, 255) },
            { "magenta",              new Color32(255,   0, 255, 255) },
            { "maroon",               new Color32(128,   0,   0, 255) },
            { "mediumaquamarine",     new Color32(102, 205, 170, 255) },
            { "mediumblue",           new Color32(  0,   0, 205, 255) },
            { "mediumorchid",         new Color32(186,  85, 211, 255) },
            { "mediumpurple",         new Color32(147, 112, 219, 255) },
            { "mediumseagreen",       new Color32( 60, 179, 113, 255) },
            { "mediumslateblue",      new Color32(123, 104, 238, 255) },
            { "mediumspringgreen",    new Color32(  0, 250, 154, 255) },
            { "mediumturquoise",      new Color32( 72, 209, 204, 255) },
            { "mediumvioletred",      new Color32(199,  21, 133, 255) },
            { "midnightblue",         new Color32( 25,  25, 112, 255) },
            { "mintcream",            new Color32(245, 255, 250, 255) },
            { "mistyrose",            new Color32(255, 228, 225, 255) },
            { "moccasin",             new Color32(255, 228, 181, 255) },
            { "navajowhite",          new Color32(255, 222, 173, 255) },
            { "navy",                 new Color32(  0,   0, 128, 255) },
            { "oldlace",              new Color32(253, 245, 230, 255) },
            { "olive",                new Color32(128, 128,   0, 255) },
            { "olivedrab",            new Color32(107, 142,  35, 255) },
            { "orange",               new Color32(255, 165,   0, 255) },
            { "orangered",            new Color32(255,  69,   0, 255) },
            { "orchid",               new Color32(218, 112, 214, 255) },
            { "palegoldenrod",        new Color32(238, 232, 170, 255) },
            { "palegreen",            new Color32(152, 251, 152, 255) },
            { "paleturquoise",        new Color32(175, 238, 238, 255) },
            { "palevioletred",        new Color32(219, 112, 147, 255) },
            { "papayawhip",           new Color32(255, 239, 213, 255) },
            { "peachpuff",            new Color32(255, 218, 185, 255) },
            { "peru",                 new Color32(205, 133,  63, 255) },
            { "pink",                 new Color32(255, 192, 203, 255) },
            { "plum",                 new Color32(221, 160, 221, 255) },
            { "powderblue",           new Color32(176, 224, 230, 255) },
            { "purple",               new Color32(128,   0, 128, 255) },
            { "red",                  new Color32(255,   0,   0, 255) },
            { "rosybrown",            new Color32(188, 143, 143, 255) },
            { "royalblue",            new Color32( 65, 105, 225, 255) },
            { "saddlebrown",          new Color32(139,  69,  19, 255) },
            { "salmon",               new Color32(250, 128, 114, 255) },
            { "sandybrown",           new Color32(244, 164,  96, 255) },
            { "seagreen",             new Color32( 46, 139,  87, 255) },
            { "seashell",             new Color32(255, 245, 238, 255) },
            { "sienna",               new Color32(160,  82,  45, 255) },
            { "silver",               new Color32(192, 192, 192, 255) },
            { "skyblue",              new Color32(135, 206, 235, 255) },
            { "slateblue",            new Color32(106,  90, 205, 255) },
            { "slategray",            new Color32(112, 128, 144, 255) },
            { "slategrey",            new Color32(112, 128, 144, 255) },
            { "snow",                 new Color32(255, 250, 250, 255) },
            { "springgreen",          new Color32(  0, 255, 127, 255) },
            { "steelblue",            new Color32( 70, 130, 180, 255) },
            { "tan",                  new Color32(210, 180, 140, 255) },
            { "teal",                 new Color32(  0, 128, 128, 255) },
            { "thistle",              new Color32(216, 191, 216, 255) },
            { "tomato",               new Color32(255,  99,  71, 255) },
            { "turquoise",            new Color32( 64, 224, 208, 255) },
            { "violet",               new Color32(238, 130, 238, 255) },
            { "wheat",                new Color32(245, 222, 179, 255) },
            { "white",                new Color32(255, 255, 255, 255) },
            { "whitesmoke",           new Color32(245, 245, 245, 255) },
            { "yellow",               new Color32(255, 255,   0, 255) },
            { "yellowgreen",          new Color32(154, 205,  50, 255) }
        };

        static Dictionary<string, XmlNode> useMap         = new Dictionary<string, XmlNode>();  
		static Dictionary<string, XmlNode> gradientMap    = new Dictionary<string, XmlNode>(); 
		static Dictionary<string, string>  styleMap       = new Dictionary<string, string>();
		
	    static Path                        path           = new Path(); 	 
		static Path                        result         = new Path();	   
		static List<Path>                  clipPath       = new List<Path>();	
		static List<Path>                  clipPathResult = new List<Path>();  
		static int                         clipPathLevel  = 0;   
		static int                         clipPathOffset = 0;     			 

		
		static Path ClipPath 
        { 
            get { return (clipPathOffset == clipPath.Count) ? null : clipPath[clipPath.Count-1]; } 
        }

        static void AddClipPath(Path cp, bool child)
		{ 
			if(child)
				Clipper.Clip(ClipOp.INTERSECTION, cp, ClipPath, result); 	
			else  
				Clipper.Clip(ClipOp.UNION, cp, null, result); 
			
			clipPathResult[clipPathResult.Count-1].subPathList.AddRange(result.subPathList);
		}	 
		
		static void RemoveClipPath()
		{
			clipPath[clipPath.Count-1].Clear();	
			clipPath.RemoveAt(clipPath.Count-1);
		}
		
		static void WhiteSpace(string value, ref int i) 
        {
            while(i < value.Length && '\0' != value[i] && (' ' == value[i] || '\t' == value[i] || '\n' == value[i] || '\r' == value[i]))
                ++i;
        }
        
        static void Comma(string value, ref int i) 
        {
            while(i < value.Length && '\0' != value[i] && (',' == value[i] || ' ' == value[i] || '\t' == value[i] || '\n' == value[i] || '\r' == value[i]))
                ++i;
        }

        static void ParseLink(string value, ref string link)
        {
            int i = 0;
			link  = "";
            
            WhiteSpace(value, ref i);

			if('#' == value[i]) 
			{
				++i;   
				while(i < value.Length && '\0' != value[i] && ' ' != value[i] && '\t' != value[i] && '\n' != value[i] && '\r' != value[i])  
					link += value[i++];
			}
			else if(0 == string.Compare(value, i, "url(", 0, 4, true)) 
			{	
				i += 4;
                WhiteSpace(value, ref i);
                if('#' == value[i++])
                {
                    while(i < value.Length && ' ' != value[i] && '\t' != value[i] && '\n' != value[i] && '\r' != value[i] && ')' != value[i])
                        link += value[i++];
					
					WhiteSpace(value, ref i);
					if(')' != value[i++])	
						link = "";
                }
			}
        }
        
        static bool ParseInteger(string value, ref int i, ref int n) 
        {
            string integer = "";

            WhiteSpace(value, ref i);
            
            if(i < value.Length && ('-' == value[i] || '+' == value[i]))	
				integer += value[i++];
			
			while(i < value.Length && '\0' != value[i] && value[i] >= '0' && value[i] <= '9')
                integer += value[i++];
            
            return int.TryParse(integer, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out n);
        }
                
        static bool ParseNumber(string value, ref int i, ref float f) 
        {
			string number = "";
			bool   dot    = false;
			bool   exp    = false;

            WhiteSpace(value, ref i);

            if (i < value.Length && '\0' != value[i] && ('+' == value[i] || '-' == value[i]))	
				number += value[i++];

			while(i < value.Length && '\0' != value[i] && ((value[i] >= '0' && value[i] <= '9') || '.' == value[i] || '+' == value[i] || '-' == value[i] || 'e' == value[i] || 'E' == value[i]))
            {
                if (('+' == value[i] || '-' == value[i]) && 'e' != value[i-1] && 'E' != value[i-1])
					break;

				if('e' == value[i] || 'E' == value[i])
				{
					if(!exp)
						exp = true;
					else
						break;
				}
				
				if('.' == value[i])
				{
					if(!dot)
						dot = true;
					else
						break;
				}

				number += value[i++];
            }

            return float.TryParse(number, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f);
        }

        static bool ParseHexdigit(char c, ref int n) 
        {
            if(c >= '0' && c <= '9') 
            { 
                n = c - '0';
                return true;
            }
            else if(c >= 'a' && c <= 'f') 
            {
                n = c -'a' + 10;
                return true;
            }
            else if(c >= 'A' && c <= 'F') 
            {
                n = c -'A' + 10;
                return true;
            }

            return false;
        }

        static void ParseColor(string value, ref Color32 color)
        {
            int i  = 0;
            int r  = 0;
            int g  = 0;
            int b  = 0;
            int h0 = 0;
            int h1 = 0;
            int h2 = 0;
            int h3 = 0;
            int h4 = 0;
            int h5 = 0;

            WhiteSpace(value, ref i);

            if('#' == value[i]) 
            {
                if(!ParseHexdigit(value[++i], ref h0))
                    return;
                if(!ParseHexdigit(value[++i], ref h1))
                    return;
                if(!ParseHexdigit(value[++i], ref h2))
                    return;

                if(++i < value.Length && ' ' != value[i] && '\t' != value[i] && '\n' != value[i] && '\r' != value[i] && '\0' != value[i]) 
                {
                    if(!ParseHexdigit(value[i], ref h3))
                        return;
                    if(!ParseHexdigit(value[++i], ref h4))
                        return;
                    if(!ParseHexdigit(value[++i], ref h5))
                        return;

                    r = h0 * 16 + h1;
                    g = h2 * 16 + h3;
                    b = h4 * 16 + h5;
                }
                else 
                {
                    r = h0 * 16 + h0;
                    g = h1 * 16 + h1;
                    b = h2 * 16 + h2;
                }
                
                color.r = (byte)Mathf.Clamp(r, 0, 255);
                color.g = (byte)Mathf.Clamp(g, 0, 255); 
                color.b = (byte)Mathf.Clamp(b, 0, 255);
            }
            else if(0 == string.Compare(value, i, "rgb(", 0, 4, true)) 
            {
                i += 4;

                if(!ParseInteger(value, ref i, ref r))
                    return;
                if('%' == value[i++])
                    r = (r * 255)/100;                
                
                Comma(value, ref i); 
                if(!ParseInteger(value, ref i, ref g))
                    return;
                if('%' == value[i++])
                    g = (g * 255)/100;
                               
                Comma(value, ref i);  
                if(!ParseInteger(value, ref i, ref b))
                    return;
                if('%' == value[i++])
                    b = (b * 255)/100;
                
                color.r = (byte)Mathf.Clamp(r, 0, 255);
                color.g = (byte)Mathf.Clamp(g, 0, 255); 
                color.b = (byte)Mathf.Clamp(b, 0, 255);
            }
            else 
            {
                if(colorMap.ContainsKey(value.ToLower()))
                    color = colorMap[value.ToLower()];
            }
        }

        static bool ParsePaint(string value, ref Color32 color, ref string gradient) 
        {
 			int i = 0;
			gradient = "";
            
			WhiteSpace(value, ref i);

            if(0 == string.Compare(value, i, "url(", 0, 4, true)) 
            {
                i += 4;
                WhiteSpace(value, ref i);
				
				if('"' == value[i]) 
				{
 					++i;
					WhiteSpace(value, ref i);	
				}

                if('#' == value[i++])
                {
                    while(i < value.Length && ' ' != value[i] && '\t' != value[i] && '\n' != value[i] && '\r' != value[i] && ')' != value[i] && '"' != value[i])
                        gradient += value[i++];
					
					WhiteSpace(value, ref i);

					if('"' == value[i])
					{ 
						++i;
						WhiteSpace(value, ref i);	
					}

					if(')' != value[i++])	
						gradient = "";
                }
            }
            else if(0 == string.Compare(value, i, "none", 0, 4, true))
            {  
				i += 4;
                return false;
            }
            else 
            {
                ParseColor(value, ref color);
            }

            return true;
        }

        static bool ParseLength(string value, ref float f, float size)
        {
            string number = "";
            string unit   = "";

            foreach(char c in value) 
            {
                if((c >= '0' && c <= '9') || c == '+' || c == '-' || c == '.' || c == 'e' || c == 'E') 
                    number += c;
                else if(' ' != c && '\t' != c && '\n' != c && '\r' != c) 
                    unit += c;
            }

            if(number[number.Length - 1] == 'e' || number[number.Length - 1] == 'E')
                number = number.Remove(number.Length - 1, 1);

            if(!float.TryParse(number, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f))
                return false;

            switch(unit.ToLower()) 
            {
                case("em"):
                    break;
                case("ex"):
                    break;
                case("in"):
                    f *= 90.0f;
                    break;
                case("cm"):
                    f *= 35.43307f;
                    break;
                case("mm"):
                    f *= 3.543307f;
                    break;
                case("pt"):
                    f *= 1.25f;
                    break;
                case("pc"):
                    f *= 15.0f;
                    break;
                case("%"):
                    f = f * size * 0.01f;
                    break;
            }

            return true;
        }
        
        static void ParseAspectRatio(string value, ref int align, ref int meet)
		{  
			int i = 0;	 
			int a = align; 
			int m = meet;
			
			WhiteSpace(value, ref i);

			if(0 == string.Compare(value, i, "xMinYMin", 0, 8)) 
			{ 
				a = 1;	
				i += 8;
			} 
			else if(0 == string.Compare(value, i, "xMidYMin", 0, 8)) 
			{ 
				a = 2;	
				i += 8;
			}
			else if(0 == string.Compare(value, i, "xMaxYMin", 0, 8)) 
			{ 
				a = 3;	
				i += 8;
			} 	  
			else if(0 == string.Compare(value, i, "xMinYMid", 0, 8)) 
			{ 
				a = 4;	
				i += 8;
			} 
			else if(0 == string.Compare(value, i, "xMidYMid", 0, 8)) 
			{ 
				a = 5;	
				i += 8;
			}
			else if(0 == string.Compare(value, i, "xMaxYMid", 0, 8)) 
			{ 
				a = 6;	
				i += 8;
			}  
			else if(0 == string.Compare(value, i, "xMinYMax", 0, 8)) 
			{ 
				a = 7;	
				i += 8;
			} 
			else if(0 == string.Compare(value, i, "xMidYMax", 0, 8)) 
			{ 
				a = 8;	
				i += 8;
			}
			else if(0 == string.Compare(value, i, "xMaxYMax", 0, 8)) 
			{ 
				a = 9;	
				i += 8;
			} 	  
			else if(0 == string.Compare(value, i, "none", 0, 4)) 
			{ 
				a = 0;	
				i += 4;
			} 	
			
			if(i >= value.Length || ' ' != value[i])
 				return;
			
			WhiteSpace(value, ref i);

			if(0 == string.Compare(value, i, "meet", 0, 4)) 
			{
				m = 1;
				i += 4;
			}	 
			else if(0 == string.Compare(value, i, "slice", 0, 5)) 
			{
				m = 0;
				i += 5;
			}
			
			WhiteSpace(value, ref i);

			if(i == value.Length) 
			{
				align = a;
				meet  =	m;
			}
		}
		
		static bool ParseViewBox(string value, ref float l, ref float t, ref float w, ref float h)
		{
			int i = 0;	
			
			if(!ParseNumber(value, ref i, ref l))
				return false; 
			
			Comma(value, ref i);  
			if(!ParseNumber(value, ref i, ref t))
				return false; 
			
			Comma(value, ref i);  
			if(!ParseNumber(value, ref i, ref w))
				return false; 
			
			Comma(value, ref i);
			if(!ParseNumber(value, ref i, ref h))
				return false; 

			return true;
		}
		
		static void ParseTransform(string value, ref Transform tranform)
        {
            int   i = 0;
            float a = 0.0f;  
			float b = 0.0f;
			float c = 0.0f;
			float d = 0.0f;
            float x = 0.0f;
            float y = 0.0f; 

            while(i < value.Length) 
            {
                WhiteSpace(value, ref i);

                if(0 == string.Compare(value, i, "matrix", 0, 6))
                {
                    i += 6;
					a = d = 1.0f;	
					b = c = x = y = 0.0f; 
					WhiteSpace(value, ref i);

					if('(' == value[i++]) 
					{ 
						if(!ParseNumber(value, ref i, ref a))
                            return;
                        Comma(value, ref i); 
						if(!ParseNumber(value, ref i, ref b))
                            return;
                        Comma(value, ref i); 
						if(!ParseNumber(value, ref i, ref c))
                            return;
                        Comma(value, ref i);	
						if(!ParseNumber(value, ref i, ref d))
                            return;
                        Comma(value, ref i); 
						if(!ParseNumber(value, ref i, ref x))
                            return;
                        Comma(value, ref i);  
						if(!ParseNumber(value, ref i, ref y))
                            return;
                        WhiteSpace(value, ref i);

						if(')' == value[i++]) 
						{	
							Comma(value, ref i);
                            tranform.Matrix(a, b, c, d, x, y);
						}
					}
                }
                else if(0 == string.Compare(value, i, "translate", 0, 9)) 
                {
                    i += 9;
                    x = y = 0.0f;
                    WhiteSpace(value, ref i);
                    
                    if('(' == value[i++]) 
                    {
                        if(!ParseNumber(value, ref i, ref x))
                            return;
                        Comma(value, ref i);
                        
                        if(')' != value[i]) 
                        {
                            if(!ParseNumber(value, ref i, ref y))
                                return;
                            WhiteSpace(value, ref i);
                        }
                        if(')' == value[i++]) 
                        { 
                            Comma(value, ref i);
                            tranform.Translate(x, y);
                        }
                    }                                
                }
                else if(0 == string.Compare(value, i, "scale", 0, 5)) 
                {
                    i += 5;
                    x = 1.0f;
                    WhiteSpace(value, ref i);
                    
                    if('(' == value[i++]) 
                    {
                        if(!ParseNumber(value, ref i, ref x))
                            return;
                        Comma(value, ref i);
                        y = x;
                        
                        if(')' != value[i]) 
                        {
                            if(!ParseNumber(value, ref i, ref y))
                                return;
                            WhiteSpace(value, ref i);
                        }
                        if(')' == value[i++]) 
                        { 
                            Comma(value, ref i);
                            tranform.Scale(x, y);
                        }
                    }   
                }
                else if(0 == string.Compare(value, i, "rotate", 0, 6)) 
                {
                    i += 6;
                    a = x = y = 0.0f;
                    WhiteSpace(value, ref i);

                    if('(' == value[i++]) 
                    {
                        if(!ParseNumber(value, ref i, ref a))
                            return;
                        Comma(value, ref i);

                        if(')' != value[i]) 
                        {
                            if(!ParseNumber(value, ref i, ref x))
                                return;
                            Comma(value, ref i);
                            if(!ParseNumber(value, ref i, ref y))
                                return;
                            WhiteSpace(value, ref i);
                        }
                        if(')' == value[i++]) 
                        {
                            Comma(value, ref i);
                            tranform.Rotate(a, x, y);
                        }
                    }
                }
                else if(0 == string.Compare(value, i, "skewX", 0, 5))
                {
                    i += 5;
					x = 0.0f;
					WhiteSpace(value, ref i);

					if('(' == value[i++]) 
					{	
						if(!ParseNumber(value, ref i, ref x))
                            return;	  
						WhiteSpace(value, ref i);

						if(')' == value[i++]) 
                        {
                            Comma(value, ref i);
                            tranform.Skew(x, 0.0f);
                        }
					}
                }
                else if(0 == string.Compare(value, i, "skewY", 0, 5))
                {
                    i += 5;	 
					y = 0.0f;
					WhiteSpace(value, ref i);

					if('(' == value[i++]) 
					{	
						if(!ParseNumber(value, ref i, ref y))
                            return;	  
						WhiteSpace(value, ref i);

						if(')' == value[i++]) 
                        {
                            Comma(value, ref i);
                            tranform.Skew(0.0f, y);
                        }
					}
                }
                else 
                {
                    return;
                }
            }
        }

        static void ParseDashArray(string value, ref List<float> array, float size) 
        {
            int   i = 0;
			int   n = 0;
            float f = 0.0f;

            array = new List<float>();

            while(i < value.Length)
            {
                if(!ParseNumber(value, ref i, ref f) || f < 0.0f) 
                {
                    array.Clear();
                    array = null;
                    return;
                }

				if(i < value.Length && '%' == value[i]) 
				{
					f = size * f * 0.01f;
					++i;
				}

				array.Add(f);
                Comma(value, ref i);
            }

			n = array.Count;
			if((n & 1) == 1) 
			{
				for(i=0; i<n; ++i) 
					array.Add(array[i]);
			}  			
        }

		static void ParseTransfer(XmlNode node, int c, ref Effect effect)
		{
 			List<float> table     = null;
			int         i         = 0; 
			int         k         = 0;	
			int         n         = 0;
			float       f         = 0.0f;  
			float       v1        = 0.0f;
			float       v2        = 0.0f;
			float       slope     = 1.0f;   
			float       intercept = 0.0f; 
			float       amplitude = 1.0f;  
			float       exponent  = 1.0f; 
			float       offset    = 0.0f;
			string      type      = "identity";

			foreach(XmlAttribute a in node.Attributes) 
			{ 
				i = 0;

				switch(a.Name)
				{	 
					case("type"):
						type = a.Value;
						break;	
					case("tableValues"):
						table = new List<float>();
						while(i < a.Value.Length) 
						{  
							if(!ParseNumber(a.Value, ref i, ref f))
								return;	
							Comma(a.Value, ref i);
							table.Add(f);
						}
						break;
					case("slope"):	
						if(!ParseNumber(a.Value, ref i, ref slope))
							return;
						break;	
					case("intercept"): 
						if(!ParseNumber(a.Value, ref i, ref intercept))
							return;
						break;
					case("amplitude"):	
						if(!ParseNumber(a.Value, ref i, ref amplitude))
							return;
						break;	
					case("exponent"): 
						if(!ParseNumber(a.Value, ref i, ref exponent))
							return;
						break;	 
					case("offset"): 
						if(!ParseNumber(a.Value, ref i, ref offset))
							return;
						break;
				}
			}

			if(null == effect.data) 
			{
				effect.data = new Color[256];

				for(i=0; i<256; ++i) 
					effect.data[i].r = effect.data[i].g = effect.data[i].b = effect.data[i].a = i/255.0f; 
			}

			if("table" == type)
			{
				if(null == table || table.Count < 1)
					return;

				n = table.Count;
				for(i=0; i<256; ++i) 
				{
					k  = (int)((i/255.0f) * (n - 1));
					v1 = table[k];	
					v2 = table[Mathf.Min((k + 1), (n - 1))];
					f  = (v1 + ((i/255.0f) * (n - 1) - k) * (v2 - v1)); 
					effect.data[i][c] = Mathf.Clamp01(f);
				}
			}	  
			else if("discrete" == type)
			{	
				if(null == table || table.Count < 1)
					return;	  
				
				n = table.Count;
				for(i=0; i<256; ++i) 
				{
					k = (int)((i/255.0f) * n);	
					k = Mathf.Min(k, n - 1);
					effect.data[i][c] = Mathf.Clamp01(table[k]);
				}
			}
			else if("linear" == type) 
			{  
				for(i=0; i<256; ++i) 
				{
					f = slope * i/255.0f + intercept;
					effect.data[i][c] = Mathf.Clamp01(f); 
				}
			}	  
			else if("gamma" == type) 
			{  
				for(i=0; i<256; ++i) 
				{
					f = amplitude * Mathf.Pow((i/255.0f), exponent) + offset;
					effect.data[i][c] = Mathf.Clamp01(f); 
				}
			}
		}
		
		static void ParseLight(XmlNode node, int point, ref Effect effect)
		{ 
			int i = 0; 

			effect.param4.w = point;
			
			foreach(XmlAttribute a in node.Attributes) 
			{ 
				i = 0;

				switch(a.Name)
				{	
					case("azimuth"):
						if(!ParseNumber(a.Value, ref i, ref effect.param5.x))
							return;	
						effect.param5.x	= effect.param5.x * Mathf.Deg2Rad;
						break;	  
					case("elevation"):
						if(!ParseNumber(a.Value, ref i, ref effect.param5.y))
							return;	
						effect.param5.y	= effect.param5.y * Mathf.Deg2Rad;
						break;	 
					case("x"):
						if(!ParseNumber(a.Value, ref i, ref effect.paramX))
							return;
						break;	  
					case("y"):
						if(!ParseNumber(a.Value, ref i, ref effect.paramY))
							return;
						break;	
					case("z"):
						if(!ParseNumber(a.Value, ref i, ref effect.paramZ))
							return;
						break;	  
					case("specularExponent"):
						if(!ParseNumber(a.Value, ref i, ref effect.param3.y))
							return;
						break;	
					case("limitingConeAngle"):
						if(!ParseNumber(a.Value, ref i, ref effect.param3.w))
							return;	   
						effect.param3.z = Mathf.Cos(effect.param3.w * 0.9f * Mathf.Deg2Rad);  
						effect.param3.w = Mathf.Cos(effect.param3.w * Mathf.Deg2Rad);
						break;	
					case("pointsAtX"): 
						if(!ParseNumber(a.Value, ref i, ref effect.param4.x))
							return;
						break;	  
					case("pointsAtY"):
						if(!ParseNumber(a.Value, ref i, ref effect.param4.y))
							return;
						break;	
					case("pointsAtZ"):
						if(!ParseNumber(a.Value, ref i, ref effect.param4.z))
							return;
						break;	
				} 
			}
		}
		
		static void ParseEffect(XmlNode node, Filter filter, float width, float height)
		{
			int     i      = 0;	
			Color32 color  = Color.black;
			Effect  effect = default(Effect);  
			string  value  = "";
			bool    merge  = false;
						
			effect.x       = float.NaN;
			effect.y       = float.NaN;	
			effect.width   = float.NaN;
			effect.height  = float.NaN;
			effect.alpha   = 1.0f;
									
			if(!filter.primitiveUserSpaceOnUse)	
				width = height = 100.0f;
			
			switch(node.Name) 
			{  
				case("feGaussianBlur"):	 
					effect.pass = EffectType.GAUSSIAN_BLUR;
					break;	
				case("feOffset"):	 
					effect.pass = EffectType.OFFSET;
					break;	 
				case("feTile"):	 
					effect.pass = EffectType.TILE;
					break;	  
				case("feComposite"):	 
					effect.pass = EffectType.COMPOSITE_OVER;
					break;	
				case("feMerge"):	 
					effect.pass = EffectType.COMPOSITE_OVER;  
					effect.in2 = -1;  
					merge = true;
					break; 
				case("feDisplacementMap"):	 
					effect.pass = EffectType.DISPLACEMENT_MAP;	
					effect.param2.x = effect.param2.y = 3.0f;
					break;	   
				case("feFlood"):	 
					effect.pass = EffectType.FLOOD;	 
					effect.param2.w = 1.0f;
					break;	
				case("feMorphology"):	 
					effect.pass = EffectType.MORPHOLOGY_ERODE;	 
					break;	
				case("feComponentTransfer"):	 
					effect.pass = EffectType.TRANSFER;	
					break;	
				case("feColorMatrix"):	
 					value = "matrix";
					effect.pass = EffectType.COLOR;	
					effect.param2 = effect.param3 = effect.param4 = effect.param5 = effect.param6 = Vector4.zero; 
					effect.param2.x = effect.param3.y = effect.param4.z = effect.param5.w = 1.0f; 
					break; 
				case("feBlend"):	 
					effect.pass = EffectType.BLEND_NORMAL;	
					break;	 
				case("feConvolveMatrix"):	 
					effect.pass = EffectType.CONVOLVE;	
					effect.param2.x = effect.param2.y = 3.0f;
					break; 
				case("feDiffuseLighting"):	 
					effect.pass = EffectType.DIFFUSE_LIGHTING;	
					effect.param3.x	= effect.param2.w = effect.param4.w = 1.0f;
					break;	
				case("feSpecularLighting"):	 
					effect.pass = EffectType.SPECULAR_LIGHTING;	
					effect.param3.x	= effect.param2.w = effect.param5.z = effect.param4.w = 1.0f;
					break;
				default:
					return;
			}  
			
			foreach(XmlAttribute a in node.Attributes)
			{
				i = 0;

				switch(a.Name)
				{ 
					case("x"):	  
						ParseLength(a.Value, ref effect.x, width);
						break;	 
					case("y"):	 
						ParseLength(a.Value, ref effect.y, height);
						break;	
					case("width"):	 
						ParseLength(a.Value, ref effect.width, width);
						break;	 
					case("height"):	 
						ParseLength(a.Value, ref effect.height, height);
						break;
					case("in"):	 
						effect.in1 = filter.AddRenderTarget(a.Value, ref effect.alpha);
						break; 
					case("in2"):	 
						effect.in2 = filter.AddRenderTarget(a.Value, ref effect.alpha);
						break;	
					case("result"):	 
						effect.result = filter.AddRenderTarget(a.Value, ref effect.alpha);
						break; 
					case("stdDeviation"):	
						if(!ParseNumber(a.Value, ref i, ref effect.paramX) || effect.paramX < 0.0f)
							return;  
						effect.paramY = effect.paramX;
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length)
							break;  
						if(!ParseNumber(a.Value, ref i, ref effect.paramY) || effect.paramY < 0.0f)
							return; 
						break;	
					case("dx"):	
						if(!ParseNumber(a.Value, ref i, ref effect.paramX))
							return;  
						break;	
					case("dy"):	
						if(!ParseNumber(a.Value, ref i, ref effect.paramY))
							return; 
						effect.paramY = -effect.paramY; 
						break;	
					case("operator"):	
						if("over" == a.Value)
							effect.pass = EffectType.COMPOSITE_OVER; 	
						else if("in" == a.Value)
							effect.pass = EffectType.COMPOSITE_IN;	
						else if("out" == a.Value)
							effect.pass = EffectType.COMPOSITE_OUT; 
						else if("atop" == a.Value)
							effect.pass = EffectType.COMPOSITE_ATOP;  
						else if("xor" == a.Value)
							effect.pass = EffectType.COMPOSITE_XOR; 
						else if("arithmetic" == a.Value)
							effect.pass = EffectType.COMPOSITE_ARITHMETIC;
						else if("erode" == a.Value)	 
							effect.pass = EffectType.MORPHOLOGY_ERODE;	
						else if("dilate" == a.Value)	 
							effect.pass = EffectType.MORPHOLOGY_DILATE;
						break;
					case("k1"):	
						if(!ParseNumber(a.Value, ref i, ref effect.param2.x))
							return; 
						break;	 
					case("k2"):	
						if(!ParseNumber(a.Value, ref i, ref effect.param2.y))
							return; 
						break; 
					case("k3"):	
						if(!ParseNumber(a.Value, ref i, ref effect.param2.z))
							return; 
						break;	 
					case("k4"):	
						if(!ParseNumber(a.Value, ref i, ref effect.param2.w))
							return; 
						break; 
					case("scale"):	
						if(!ParseNumber(a.Value, ref i, ref effect.paramX))
							return;	   
						effect.paramY = -effect.paramX;
						break;	
					case("xChannelSelector"):	
						if("R" == a.Value)
							effect.param2.x = 0.0f; 
						else if("G" == a.Value)
							effect.param2.x = 1.0f;  
						else if("B" == a.Value)
							effect.param2.x = 2.0f;  
						else if("A" == a.Value)
							effect.param2.x = 3.0f;	
						break;	 
					case("yChannelSelector"):	
						if("R" == a.Value)
							effect.param2.y = 0.0f; 
						else if("G" == a.Value)
							effect.param2.y = 1.0f;  
						else if("B" == a.Value)
							effect.param2.y = 2.0f;  
						else if("A" == a.Value)
							effect.param2.y = 3.0f; 
						break;	
					case("flood-color"):		 
						ParseColor(a.Value, ref color);	 
						effect.param2.x = color.r/255.0f; 
						effect.param2.y = color.g/255.0f;
						effect.param2.z = color.b/255.0f;   
						break;	
					case("flood-opacity"):		 
						ParseNumber(a.Value, ref i, ref effect.param2.w);
						effect.param2.w = Mathf.Clamp01(effect.param2.w);	 
						break;
					case("radius"):	
						if(!ParseNumber(a.Value, ref i, ref effect.paramX) || effect.paramX < 0.0f)
							return;  
						effect.paramY = effect.paramX;
						Comma(a.Value, ref i);	
  						if(i >= a.Value.Length)
							break;
						if(!ParseNumber(a.Value, ref i, ref effect.paramY) || effect.paramY < 0.0f)
							return; 
						break;	
					case("mode"):	
						if("normal" == a.Value)
							effect.pass = EffectType.BLEND_NORMAL; 	
						else if("multiply" == a.Value)
							effect.pass = EffectType.BLEND_MULTIPLY;	
						else if("screen" == a.Value)
							effect.pass = EffectType.BLEND_SCREEN;
						else if("darken" == a.Value)
							effect.pass = EffectType.BLEND_DARKEN; 
						else if("lighten" == a.Value)
							effect.pass = EffectType.BLEND_LIGHTEN; 
						break;
					case("type"):	
						value = a.Value;
						break; 
					case("values"):	 
						ParseNumber(a.Value, ref i, ref effect.param2.x);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param3.x);  
						Comma(a.Value, ref i);
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param4.x);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param5.x);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param6.x);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param2.y);  
						Comma(a.Value, ref i);
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param3.y);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param4.y);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param5.y);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param6.y);  
						Comma(a.Value, ref i);
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param2.z);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param3.z);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param4.z);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param5.z);  
						Comma(a.Value, ref i);
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param6.z);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param2.w);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param3.w);  
						Comma(a.Value, ref i);	
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param4.w);  
						Comma(a.Value, ref i);
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param5.w);  
						Comma(a.Value, ref i);	 
						if(i >= a.Value.Length) break;
						ParseNumber(a.Value, ref i, ref effect.param6.w);  
						Comma(a.Value, ref i);	
						break; 	 
					case("order"):	
						if(!ParseNumber(a.Value, ref i, ref effect.param2.x) || effect.param2.x < 0.0f)
							return;  
						effect.param2.y = effect.param2.x;
						Comma(a.Value, ref i);	
  						if(i >= a.Value.Length)
							break;
						if(!ParseNumber(a.Value, ref i, ref effect.param2.y) || effect.param2.y < 0.0f)
							return;
						break;	
					case("kernelMatrix"):
						value = a.Value;
						break;	 
					case("divisor"):
						if(!ParseNumber(a.Value, ref i, ref effect.param3.x))
							return;	
						effect.param3.x = (0.0f == effect.param3.x) ? 1.0f : 1.0f/effect.param3.x;
						break;	 
					case("bias"):  
						if(!ParseNumber(a.Value, ref i, ref effect.param3.y))
							return;	
						effect.param3.y = Mathf.Clamp01(effect.param3.y);
						break; 
					case("targetX"):
						if(!ParseNumber(a.Value, ref i, ref effect.param2.z))
							return;
						break;	
					case("targetY"):
						if(!ParseNumber(a.Value, ref i, ref effect.param2.w))
							return;
						break; 
					case("edgeMode"):
						if("wrap" == a.Value)
							effect.param3.w = 1.0f; 	
						else if("none" == a.Value)
							effect.param3.w = 2.0f;
						break;	 
					case("preserveAlpha"): 
						effect.param3.z = ("true" == a.Value) ? 1.0f : 0.0f;
						break;	
					case("surfaceScale"):
						if(!ParseNumber(a.Value, ref i, ref effect.param2.w))
							return;
						break; 	   
					case("diffuseConstant"):
						if(!ParseNumber(a.Value, ref i, ref effect.param3.x))
							return;
						break; 	
					case("specularConstant"):
						if(!ParseNumber(a.Value, ref i, ref effect.param3.x))
							return;
						break; 
					case("specularExponent"):
						if(!ParseNumber(a.Value, ref i, ref effect.param5.z))
							return;	
						effect.param5.z = Mathf.Clamp(effect.param5.z, 1.0f, 128.0f);
						break;
					case("lighting-color"):		 
						ParseColor(a.Value, ref color);	 
						effect.param2.x = color.r/255.0f; 
						effect.param2.y = color.g/255.0f;
						effect.param2.z = color.b/255.0f;   
						break;
				}
			}

			foreach(XmlNode n in node.ChildNodes) 
			{	
				switch(n.Name)
				{
					case("feFuncR"):
						ParseTransfer(n, 0, ref effect);
						break;	
					case("feFuncG"):
						ParseTransfer(n, 1, ref effect);
						break;	  
					case("feFuncB"):
						ParseTransfer(n, 2, ref effect);
						break;	
					case("feFuncA"):
						ParseTransfer(n, 3, ref effect);
						break;	 
					case("feDistantLight"): 
						ParseLight(n, 0, ref effect);
						break;	
					case("fePointLight"): 
						ParseLight(n, 1, ref effect);
						break;
					case("feSpotLight"): 
						ParseLight(n, 1, ref effect);
						break;	
					case("feMergeNode"):
						foreach(XmlAttribute a in n.Attributes) 
						{
							if("in" == a.Name) 
							{ 
								effect.in1 = filter.AddRenderTarget(a.Value, ref effect.alpha);	 
								filter.AddEffect(effect); 
								effect.in2 = 0;
								break;
							}
						}
						break;
				}
			}
 
			if(EffectType.CONVOLVE == effect.pass && !string.IsNullOrEmpty(value)) 
			{ 
				int n = i = 0;
				effect.data = new Color[(int)(effect.param2.x * effect.param2.y)];

				while(i < value.Length) 
				{
					if(!ParseNumber(value, ref i, ref effect.data[n++].r)) 
					{ 
						effect.data = null;
						return;	 
					}
					Comma(value, ref i);
				}
			}
			
			if(EffectType.COLOR == effect.pass) 
			{
				float f = effect.param2.x;  

				if("saturate" == value) 
				{
					effect.param2.x = 0.213f + 0.787f * f; 
					effect.param2.y = 0.213f - 0.213f * f; 
					effect.param2.z = 0.213f - 0.213f * f;
					effect.param3.x = 0.715f - 0.715f * f; 
					effect.param3.y = 0.715f + 0.285f * f; 
					effect.param3.z = 0.715f - 0.715f * f;
					effect.param4.x = 0.072f - 0.072f * f;  
					effect.param4.y = 0.072f - 0.072f * f;	
					effect.param4.z = 0.072f + 0.928f * f;	 					
				}	
				else if("hueRotate" == value) 
				{ 
					float cos = Mathf.Cos(f * Mathf.Deg2Rad);
					float sin = Mathf.Sin(f * Mathf.Deg2Rad);	

					effect.param2.x = 0.213f + cos * 0.787f - sin * 0.213f; 
					effect.param2.y = 0.213f - cos * 0.213f + sin * 0.143f; 
					effect.param2.z = 0.213f - cos * 0.213f - sin * 0.787f;
					effect.param3.x = 0.715f - cos * 0.715f - sin * 0.715f; 
					effect.param3.y = 0.715f + cos * 0.285f + sin * 0.140f; 
					effect.param3.z = 0.715f - cos * 0.715f + sin * 0.715f;
					effect.param4.x = 0.072f - cos * 0.072f + sin * 0.928f;  
					effect.param4.y = 0.072f - cos * 0.072f - sin * 0.283f;	
					effect.param4.z = 0.072f + cos * 0.928f + sin * 0.072f;
				}	 
				else if("luminanceToAlpha" == value) 
				{ 	
					effect.param2.w = 0.2125f; 
					effect.param3.w = 0.7154f; 
					effect.param4.w = 0.0721f; 
					effect.param2.x = 0.0f;	
					effect.param3.y = 0.0f;
					effect.param4.z = 0.0f; 
					effect.param5.w = 0.0f;
				}
			}

			if(!merge)
				filter.AddEffect(effect);
		}
		
		static void ParseFilter(XmlNode node, float width, float height)
		{
	 		Filter filter = new Filter();
			float  w      = width;
			float  h      = height;
			bool   bx     = false;
			bool   by     = false;	
			bool   bw     = false;
			bool   bh     = false;	 

			foreach(XmlAttribute a in node.Attributes) 
			{
				if("filterUnits" == a.Name && "objectBoundingBox" == a.Value) 
				{   
					w = h = 100.0f;
					break;
				}
			}

			foreach(XmlAttribute a in node.Attributes) 
			{	
				switch(a.Name) 
				{ 
					case("id"):	 
						filter.name = a.Value;
                        break;
					case("filterUnits"): 
						filter.filterUserSpaceOnUse = ("userSpaceOnUse" == a.Value) ? true : false;
						break;	
					case("primitiveUnits"):	
						filter.primitiveUserSpaceOnUse = ("userSpaceOnUse" == a.Value) ? true : false;
						break;
					case("x"): 
						bx = ParseLength(a.Value, ref filter.position.x, w);
						break;	 
					case("y"):	
						by = ParseLength(a.Value, ref filter.position.y, h);
						break; 
					case("width"):
						bw = ParseLength(a.Value, ref filter.size.x, w);
						break;	 
					case("height"):	 
						bh = ParseLength(a.Value, ref filter.size.y, h);
						break;	
					case("filterRes"):
						break;	
					case("xlink:href"):
						break; 
                }
			}  

			if(filter.filterUserSpaceOnUse) 
			{
				if(!bx)	
					filter.position.x = -width * 0.1f;	  
				if(!by)	
					filter.position.y = -height * 0.1f;	 
				if(!bw)	
					filter.size.x = width * 1.2f;	  
				if(!bh)	
					filter.size.y = height * 1.2f;
			}	
			
			foreach(XmlNode n in node.ChildNodes)
				ParseEffect(n, filter, width, height);

			FilterInstance.AddFilter(filter);
		}
		
		static void ParseCSS(XmlNode node) 
		{
			List<string> names = new List<string>();
			int          i = 0;
			string       name = "";
			string       value = "";
			string       v;

			foreach(XmlNode n in node.ChildNodes) 
			{
				if("#cdata-section" == n.Name || "#text" == n.Name) 
				{ 
 					i = 0;

					while(i < n.Value.Length) 
					{	
						name  = "";
						value = ""; 

						WhiteSpace(n.Value, ref i);
						if(i < n.Value.Length-2 && '/' == n.Value[i] && '*' == n.Value[i+1]) 
						{
							i +=2;
							while(i < n.Value.Length) 
							{
								if(i < n.Value.Length-2 && '*' == n.Value[i] && '/' == n.Value[i+1]) 
								{ 
									i +=2;
									break;
								}
								++i;  
							}
						} 
						
						while(i < n.Value.Length && '{' != n.Value[i]) 
						{
							name = "";

							while(i < n.Value.Length && ',' != n.Value[i] && '{' != n.Value[i])
							{
								if(' ' != n.Value[i] && '\t' != n.Value[i] && '\n' != n.Value[i] && '\r' != n.Value[i])
									name += n.Value[i];
								++i;
							}

							names.Add(name);

							if(',' == n.Value[i])
								++i;
						}
				
						if(i < n.Value.Length && '{' != n.Value[i++])
							return;	   
						
						while(i < n.Value.Length && '}' != n.Value[i]) 
						{
							if(' ' != n.Value[i] && '\t' != n.Value[i] && '\n' != n.Value[i] && '\r' != n.Value[i])
								value += n.Value[i];	
							++i;
						}
				
						if(i < n.Value.Length && '}' != n.Value[i++])
							return;	
						
						WhiteSpace(n.Value, ref i);
						
						foreach(string s in names)
						{
							if(styleMap.ContainsKey(s))
							{
								styleMap.TryGetValue(s, out v);
								v += value;
								styleMap[s] = v;
							}
							else
							{
								styleMap.Add(s, value);
							}

						}

						names.Clear();
					} 						
				}
			}
			
			names.Clear();
		}
		
		static void ParseStyle(string data, ref Style style, float size) 
		{
			int    i     = 0;
			string name  = "";
			string value = "";

			while(i < data.Length) 
			{
				name  = "";
				value = "";

				while(i < data.Length && ':' != data[i]) 
				{
					if(' ' != data[i] && '\t' != data[i] && '\n' != data[i] && '\r' != data[i])
						name += data[i];	
					++i;
				}
				
				if(i < data.Length && ':' != data[i++])
					break;	  
				
				while(i < data.Length && ';' != data[i]) 
				{
					if(' ' != data[i] && '\t' != data[i] && '\n' != data[i] && '\r' != data[i])
						value += data[i];	
					++i;
				}
				
				if(i < data.Length && ';' != data[i++])
					break;
	 
				ParseStyle(name.ToLower(), value, ref style, size);
			}
		}  
		
		static void ParseStyle(string name, string value, ref Style style, float size)
        {
            int i = 0;

            switch(name)
            {
                case("fill"):
                    style.useFill = ParsePaint(value, ref style.fillColor, ref style.fillGradient);
                    break;
                case("fill-rule"):
                    style.fillRule = ("nonzero" == value) ? FillRule.NON_ZERO : FillRule.EVEN_ODD;
                    break;	
				case("clip-rule"):
                    style.clipRule = ("nonzero" == value) ? FillRule.NON_ZERO : FillRule.EVEN_ODD;
                    break;
                case("fill-opacity"):
                    ParseNumber(value, ref i, ref style.fillOpacity);
                    style.fillOpacity = Mathf.Clamp(style.fillOpacity, 0.0f, 1.0f);
                    break;
                case("stroke"):
                    style.useStroke = ParsePaint(value, ref style.strokeColor, ref style.strokeGradient);
                    break;
                case("stroke-width"):
                    ParseLength(value, ref style.strokeWidth, size);
                    break;
                case("stroke-linecap"):
                    style.strokeCap = value;
                    break;
                case("stroke-linejoin"):
                    style.strokeJoin = value;
                    break;
                case("stroke-miterlimit"):
                    ParseNumber(value, ref i, ref style.strokeMitterLimit);
                    break; 
                case("stroke-dasharray"):
                    ParseDashArray(value, ref style.strokeDashArray, size);
                    break;
                case("stroke-dashoffset"):
                    ParseLength(value, ref style.strokeDashOffset, size);
                    break;
                case("stroke-opacity"):
                    ParseNumber(value, ref i, ref style.strokeOpacity);
                    style.strokeOpacity = Mathf.Clamp(style.strokeOpacity, 0.0f, 1.0f);
                    break;	
				case("style"):
                    ParseStyle(value, ref style, size);
                    break; 	
				case("filter"):	 
                    ParseLink(value, ref style.filter);	
                    break;	
				case("opacity"):
                    ParseNumber(value, ref i, ref style.fillOpacity);
                    style.fillOpacity = Mathf.Clamp(style.fillOpacity, 0.0f, 1.0f);	
					style.strokeOpacity = style.fillOpacity;
                    break;
            }			 
        }

		static void ParseClipPath(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
		{ 
			Path   clip  = new Path();
			Path   union = new Path();
			string link  = "";

			if("g" == node.ParentNode.Name) 
			{
				XmlNode n = node.ParentNode;
				bool    b = false;

				while("g" == n.Name) 
				{
					foreach(XmlAttribute a in n.Attributes)
					{
						if("clip-rule" == a.Name) 
						{ 
							style.clipRule = ("nonzero" == a.Value) ? FillRule.NON_ZERO : FillRule.EVEN_ODD;
							b = true;
							break;
						}
					} 

					if(b)
						break;
 
					n = n.ParentNode;
				}
			} 
			
			foreach(XmlAttribute a in node.Attributes) 
			{	
				switch(a.Name) 
				{ 
					case("transform"):
						ParseTransform(a.Value, ref transform);
                        break;
					case("clip-path"):
						ParseLink(a.Value, ref link);
						break;	 
					case("clip-rule"):
						style.clipRule = ("nonzero" == a.Value) ? FillRule.NON_ZERO : FillRule.EVEN_ODD;
						break;
                }
			} 
			
			if(useMap.ContainsKey(link)) 
				ParseClipPath(useMap[link], style, transform, view, width, height, size);

			++clipPathLevel;
			clipPathResult.Add(union);
			
			foreach(XmlNode n in node.ChildNodes)
				ParseTags(n, style, transform, view, width, height, size);	 
			
			if(1 == clipPathLevel && null != ClipPath)
				Clipper.Clip(ClipOp.INTERSECTION, union, ClipPath, clip);	
			else 
				Clipper.Clip(ClipOp.UNION, union, null, clip);	
			
			if(useMap.ContainsKey(link)) 
  				RemoveClipPath();
			
			union.Clear();
   			clipPath.Add(clip);	
			clipPathResult.RemoveAt(clipPathResult.Count-1);
			
			--clipPathLevel;
		}
		
		static void ParseSymbol(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size) 
		{
 			Path   cp    = new Path();	
			string css   = "";
			int    align = 5;	 
			int    meet  = 1;
			float  l     = 0.0f;	 
			float  t     = 0.0f;
			float  w     = 0.0f;
			float  h     = 0.0f;	
			bool   b     = false;

			foreach(XmlAttribute a in node.Attributes) 
            { 
				switch(a.Name) 
				{
                    case("viewBox"):	
						b = ParseViewBox(a.Value, ref l, ref t, ref w, ref h);
                        break;	
					case("preserveAspectRatio"):
                        ParseAspectRatio(a.Value, ref align, ref meet);
                        break;
                    case("transform"):
                        ParseTransform(a.Value, ref transform);
                        break;	
					case("class"):
						css = "." + a.Value;
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            } 
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);

			if(null == ClipPath) 
			{
				cp.MoveTo(0.0f, 0.0f, transform, false, false);
				cp.LineTo(view.Width, 0.0f, transform, false, false);	
				cp.LineTo(view.Width, view.Height, transform, false, false);	 
				cp.LineTo(0.0f, view.Height, transform, false, false);
				cp.MoveTo(0.0f, 0.0f, transform, false, false);
			}
			else 
			{  
				path.Clear();
				path.MoveTo(0.0f, 0.0f, transform, false, false);
				path.LineTo(view.Width, 0.0f, transform, false, false);	
				path.LineTo(view.Width, view.Height, transform, false, false);	 
				path.LineTo(0.0f, view.Height, transform, false, false);
				path.MoveTo(0.0f, 0.0f, transform, false, false);
				
				Clipper.Clip(ClipOp.INTERSECTION, path, ClipPath, cp); 	
			}
			
			if(b)
			{ 
				if(w <= 0.0f || h <= 0.0f) 
					return;

				float sx = view.Width/w;
				float sy = view.Height/h;
				float tx = -l;
				float ty = -t;	
				
				if(0 != align) 
				{
					sx = (1 == meet) ? Mathf.Min(sx, sy) : Mathf.Max(sx, sy);
					sy = sx;

					if(2 == align || 5 == align || 8 == align) 
						tx = (view.Width/sx - w) * 0.5f - l;	 
					
					if(3 == align || 6 == align || 9 == align) 
						tx = view.Width/sx - w - l;	 
					
					if(4 == align || 5 == align || 6 == align) 
						ty = (view.Height/sy - h) * 0.5f - t;	 
					
					if(7 == align || 8 == align || 9 == align) 
						ty = view.Height/sy - h - t;	
				}  

				transform.Scale(sx, sy);	
				transform.Translate(tx, ty);   
				
				width  = w;
				height = h;
				size   = Mathf.Sqrt(w*w + h*h) * 0.70710678118654752440084436210485f;
			}	
			
			clipPath.Add(cp);
			
			foreach(XmlNode n in node.ChildNodes) 
                ParseTags(n, style, transform, view, width, height, size); 	
			
			cp.Clear();
			cp = null;	 
			clipPath.RemoveAt(clipPath.Count-1);
		}
                        
        static void ParseUse(XmlNode node, Style style, Transform transform, float width, float height, float size)
        {
			Rect   view = Rect.Zero;
            float  f    = 0.0f;
            string link = "";	
			string css  = "";  
			string clip = "";

            foreach(XmlAttribute a in node.Attributes) 
            { 
                switch(a.Name) 
                {
                    case("x"):
                        ParseLength(a.Value, ref view.left, width);	
						if(view.left < 0.0f)
							return;
						transform.Translate(view.left, 0.0f);
                        break;
                    case("y"):
                        ParseLength(a.Value, ref view.top, height);	
						if(view.top < 0.0f)
							return;
						transform.Translate(0.0f, view.top);
                        break; 
					case("width"):
                        ParseLength(a.Value, ref f, width);
						view.Width = f;
                        break;
                    case("height"):
                        ParseLength(a.Value, ref f, height);
						view.Height = f;
                        break;
                    case("xlink:href"):
                        ParseLink(a.Value, ref link);
                        break;
                    case("transform"):
                        ParseTransform(a.Value, ref transform);
                        break;	
					case("class"):
						css = "." + a.Value;
						break; 
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }
 			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	  
			
			if(useMap.ContainsKey(clip)) 
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);

			if(useMap.ContainsKey(link)) 
			{
 				if("symbol" == useMap[link].Name)
					ParseSymbol(useMap[link], style, transform, view, width, height, size);	 
				else if("g" == useMap[link].Name)	
					ParseGroup(useMap[link], style, transform, view, width, height, size);
				else 
					ParseTags(useMap[link], style, transform, view, width, height, size);	   
			} 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }

        static void ParseTags(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size) 
        { 
            switch(node.Name) 
            {  
				case("svg"):
                    ParseSVG(node, style, transform, view, width, height, size);
                    break;
				case("g"):
                    ParseGroup(node, style, transform, view, width, height, size);
                    break;	 
                case("use"):
                    ParseUse(node, style, transform, width, height, size);
                    break; 
                case("path"):
                    ParsePath(node, style, transform, view, width, height, size);
                    break; 
                case("rect"):
                    ParseRect(node, style, transform, view, width, height, size);
                    break;
                case("circle"):
                    ParseCircle(node, style, transform, view, width, height, size);
                    break; 
                case("ellipse"):
                    ParseEllipse(node, style, transform, view, width, height, size);
                    break;
                case("line"):  
					ParseLine(node, style, transform, view, width, height, size);
                    break;
                case ("polyline"):
                    ParsePolyline(node, style, transform, view, width, height, size);
                    break; 
                case("polygon"):
                    ParsePolygon(node, style, transform, view, width, height, size);
                    break;	 
            }
        } 
		
		static void ParseSVG(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
 			Path  cp    = new Path(); 
			int   align = 5;	 
			int   meet  = 1;
			float l     = 0.0f;	 
			float t     = 0.0f;
			float w     = 0.0f;
			float h     = 0.0f; 
			float f     = 0.0f;	
			bool  b     = false;
			
			foreach(XmlAttribute a in node.Attributes) 
            {  
                switch(a.Name) 
                {
                    case("x"):
                        ParseLength(a.Value, ref view.left, width);	 
						if(view.left < 0.0f)
							return;
						transform.Translate(view.left, 0.0f);
                        break;
                    case("y"):
                        ParseLength(a.Value, ref view.top, height);	
						if(view.top < 0.0f)
							return;
						transform.Translate(0.0f, view.top);
                        break; 
					case("width"):
                        ParseLength(a.Value, ref f, width);
						view.Width = f;
                        break;
                    case("height"):
                        ParseLength(a.Value, ref f, height);
						view.Height = f;
                        break;
                    case("viewBox"):
                        b = ParseViewBox(a.Value, ref l, ref t, ref w, ref h);
                        break;
					case("preserveAspectRatio"):
                        ParseAspectRatio(a.Value, ref align, ref meet);
                        break;	 
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break; 	  
				}
            }  
			
			if(null == ClipPath) 
			{
				cp.MoveTo(0.0f, 0.0f, transform, false, false);
				cp.LineTo(view.Width, 0.0f, transform, false, false);	
				cp.LineTo(view.Width, view.Height, transform, false, false);	 
				cp.LineTo(0.0f, view.Height, transform, false, false);
				cp.MoveTo(0.0f, 0.0f, transform, false, false);
			}
			else 
			{  
				path.Clear();
				path.MoveTo(0.0f, 0.0f, transform, false, false);
				path.LineTo(view.Width, 0.0f, transform, false, false);	
				path.LineTo(view.Width, view.Height, transform, false, false);	 
				path.LineTo(0.0f, view.Height, transform, false, false);
				path.MoveTo(0.0f, 0.0f, transform, false, false);
				
				Clipper.Clip(ClipOp.INTERSECTION, path, ClipPath, cp); 	
			}
  
			if(b) 
			{ 
				if(w <= 0.0f || h <= 0.0f) 
					return;

				float sx = view.Width/w;
				float sy = view.Height/h;
				float tx = -l;
				float ty = -t;	  
								
				if(0 != align) 
				{
					sx = (1 == meet) ? Mathf.Min(sx, sy) : Mathf.Max(sx, sy);
					sy = sx;

					if(2 == align || 5 == align || 8 == align) 
						tx = (view.Width/sx - w) * 0.5f - l;	 
					
					if(3 == align || 6 == align || 9 == align) 
						tx = view.Width/sx - w - l;	 
					
					if(4 == align || 5 == align || 6 == align) 
						ty = (view.Height/sy - h) * 0.5f - t;	 
					
					if(7 == align || 8 == align || 9 == align) 
						ty = view.Height/sy - h - t;	
				}  
				
				transform.Scale(sx, sy);	
				transform.Translate(tx, ty);   
				
				width  = w;
				height = h;
				size   = Mathf.Sqrt(w*w + h*h) * 0.70710678118654752440084436210485f;	  
			}
			else 
			{	
				width  = view.Width;
				height = view.Height;
				size   = Mathf.Sqrt(width*width + height*height) * 0.70710678118654752440084436210485f;
			}	

			clipPath.Add(cp);

			foreach(XmlNode n in node.ChildNodes) 
				ParseTags(n, style, transform, view, width, height, size);	

			cp.Clear();
			cp = null;	 
			clipPath.RemoveAt(clipPath.Count-1); 
        }

        static void ParseGroup(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        { 
			Transform t      = Transform.identity;
			string    css    = "";   
			string    clip   = "";
			string    filter = "";
			int       temp   = 0;
			
			foreach(XmlAttribute a in node.Attributes) 
            {  
                switch(a.Name) 
                {
					case("transform"):
						ParseTransform(a.Value, ref transform);	 
						break;	
					case("class"):
						css = "." + a.Value;   
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break; 	  
				}
            }  
						
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			filter = style.filter;	
			style.filter = "";

			if(useMap.ContainsKey(clip)) 
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);	  
			
            if(MeshCreator.renderToTexture && !string.IsNullOrEmpty(filter)) 
			{ 
				MeshCreator.BeginFilter(filter);
				t              = transform;	
				transform      = Transform.identity;
				temp           = clipPathOffset;
				clipPathOffset = clipPath.Count;	
			}
			
			foreach(XmlNode n in node.ChildNodes) 
                ParseTags(n, style, transform, view, width, height, size); 
			
			if(MeshCreator.renderToTexture && !string.IsNullOrEmpty(filter)) 
			{ 
				clipPathOffset = temp;
				MeshCreator.EndFilter(ClipPath, t); 
			}
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParseDefs(XmlNode node, float width, float height)
        { 
			float  l   = 0.0f;	 
			float  t   = 0.0f;
			string key = "";

			foreach(XmlAttribute a in node.Attributes) 
            {  
                switch(a.Name) 
                {
					case("width"):
                        ParseLength(a.Value, ref width, width);
                        break;
                    case("height"):
                        ParseLength(a.Value, ref height, height);
                        break;	
					case("viewBox"):
                        ParseViewBox(a.Value, ref l, ref t, ref width, ref height);
                        break;
				}
            } 

            foreach(XmlNode n in node.ChildNodes) 
            { 
                switch(n.Name)
                {
                    case("g"): 
					case("symbol"):	 
						foreach(XmlAttribute a in n.Attributes) 
                        {
							if("id" == a.Name && !useMap.ContainsKey(a.Value)) 
                            { 
                                useMap.Add(a.Value, n);
                                break;
                            }
                        } 
						ParseDefs(n, width, height);
                        break;
                    case("defs"):  
					case("svg"):
                        ParseDefs(n, width, height);
                        break;
					case("clipPath"):
                    case("path"):
                    case("rect"):
                    case("circle"):
                    case("ellipse"):
                    case("line"):
                    case("polyline"):
                    case("polygon"):
                        foreach(XmlAttribute a in n.Attributes) 
                        {
							if("id" == a.Name && !useMap.ContainsKey(a.Value)) 
                            { 
                                useMap.Add(a.Value, n);
                                break;
                            }
                        }
                        break;
                    case("linearGradient"):
                    case("radialGradient"):
                        key = "id_" + gradientMap.Count;  				
						foreach(XmlAttribute a in n.Attributes) 
						{	
							if("id" == a.Name) 
                            { 
                                key = a.Value;
                                break;
                            }
						}
					    gradientMap.Add(key, n);
                        break;
					case("style"):
						ParseCSS(n);
                        break; 
					case("filter"):
						ParseFilter(n, width, height);
                        break;
                }
            }          
        }

		static bool ParsePathArguments(string value, ref int i, int count, ref float a, ref float b, ref float c, ref float d, ref float e, ref float f)
        {
            if (!ParseNumber(value, ref i, ref a))
                return false;

            Comma(value, ref i);  
            if(!ParseNumber(value, ref i, ref b))
                return false;

            if (count > 2) 
            {
                Comma(value, ref i);
                if(!ParseNumber(value, ref i, ref c))
                    return false;

                if (count > 3) 
                {
                    Comma(value, ref i);
                    if(!ParseNumber(value, ref i, ref d))
                        return false;

                    if (count > 4) 
                    {
                        Comma(value, ref i);
                        if(!ParseNumber(value, ref i, ref e))
                            return false;
                
                        Comma(value, ref i);
                        if(!ParseNumber(value, ref i, ref f))
                            return false;  
                    }
                }
            }

            return true;
        }
        
        static void ParsePathData(string value, Transform transform, ref Path path) 
        {
            int     i           = 0;
            float   x           = 0.0f;
            float   y           = 0.0f;
            float   x1          = 0.0f;
            float   y1          = 0.0f;
            float   x2          = 0.0f;
            float   y2          = 0.0f;
            float   a           = 0.0f;
            float   rx          = 0.0f;
            float   ry          = 0.0f;
            bool    large       = false;
            bool    sweep       = false;
            bool    relative    = false;
            char    command     = '\0';
            string  commands    = "MmZzLlHhVvCcSsQqTtAa"; 	 
			
			path.Clear();

            WhiteSpace(value, ref i);
            if('M' != value[i] && 'm' != value[i])
                return;

            while(i < value.Length)
            {
				if(commands.IndexOf(value[i]) >= 0)
                {
                    command  = value[i++];
                    relative = char.IsLower(command);
                }
                else 
                {
                    Comma(value, ref i);
                    
                    if('M' == command)
                        command = 'L';
                    else if('m' == command) 
                        command = 'l';
                }

                switch (command) 
                { 
                    case ('M'):
                    case ('m'):
                        if(!ParsePathArguments(value, ref i, 2, ref x, ref y, ref a, ref a, ref a, ref a))
                            return;
                        path.MoveTo(x, y, transform, relative, true);	
                        break;
                    case ('L'):
                    case ('l'):
                        if(!ParsePathArguments(value, ref i, 2, ref x, ref y, ref a, ref a, ref a, ref a))
                            return;
                        path.LineTo(x, y, transform, relative, true);  
                        break;
                    case ('H'):
                    case ('h'):
                        if(!ParseNumber(value, ref i, ref x))
                            return;
                        path.LineTo(x, transform, false, relative, true);  
                        break;
                    case ('V'):
                    case ('v'):
                        if(!ParseNumber(value, ref i, ref y))
                            return;
                        path.LineTo(y, transform, true, relative, true);
                        break;
                    case ('C'):
                    case ('c'):	 
                        if(!ParsePathArguments(value, ref i, 6, ref x1, ref y1, ref x2, ref y2, ref x, ref y))
                            return;
                        path.CubicCurveTo(x, y, x1, y1, x2, y2, transform, relative, false);  
                        break;
                    case ('S'):
                    case ('s'): 
                        if(!ParsePathArguments(value, ref i, 4, ref x2, ref y2, ref x, ref y, ref a, ref a))
                            return;
                        path.CubicCurveTo(x, y, x1, y1, x2, y2, transform, relative, true);
                        break;
                    case ('Q'):
                    case ('q'):
                        if(!ParsePathArguments(value, ref i, 4, ref x1, ref y1, ref x, ref y, ref a, ref a))
                            return;
                        path.QuadraticCurveTo(x, y, x1, y1, transform, relative, false);
                        break;
                    case ('T'):
                    case ('t'):
                        if(!ParsePathArguments(value, ref i, 2, ref x, ref y, ref a, ref a, ref a, ref a))
                            return;      
                        path.QuadraticCurveTo(x, y, x1, y1, transform, relative, true);
                        break;
                    case ('A'):
                    case ('a'):
                        if(!ParsePathArguments(value, ref i, 3, ref rx, ref ry, ref a, ref a, ref a, ref a))
                            return;
                        Comma(value, ref i);
                        if('0' != value[i] && '1' != value[i])
                            return;
                        large = ('1' == value[i++]) ? true : false;
                        Comma(value, ref i);
                        if('0' != value[i] && '1' != value[i])
                            return;
                        sweep = ('1' == value[i++]) ? true : false;
                        Comma(value, ref i);
                        if(!ParsePathArguments(value, ref i, 2, ref x, ref y, ref a, ref a, ref a, ref a))
                            return;
                        path.ArcTo(rx, ry, a, large, sweep, x, y, transform, relative, true);
                        break;
                    case ('Z'):
                    case ('z'):
                        path.Finalize(true);
                        break;
                    default:
                        return;
                }

                WhiteSpace(value, ref i);  
            }
        }
        
        static void ParsePath(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            string css  = "path";
			string clip = "";  
			string data = "";

            foreach(XmlAttribute a in node.Attributes) 
            { 
                switch(a.Name) 
                {
                    case ("d"):
                        data = a.Value;
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break;	
					case("class"):
						css = "." + a.Value;
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }  	
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);

			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);

            ParsePathData(data, transform, ref path); 
            path.Finalize(false);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;	
						
            if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParseRect(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            float  x    = 0.0f;
            float  y    = 0.0f; 
            float  w    = 0.0f;
            float  h    = 0.0f; 
            float  rx   = 0.0f;
            float  ry   = 0.0f;  
			string css  = "rect"; 
			string clip = "";

            foreach(XmlAttribute a in node.Attributes) 
            {
                switch(a.Name) 
                {
                    case ("x"):
                        ParseLength(a.Value, ref x, width);
                        break;
                    case ("y"):
                        ParseLength(a.Value, ref y, height);
                        break;
                    case ("width"):
                        ParseLength(a.Value, ref w, width);
                        break;
                    case ("height"):
                        ParseLength(a.Value, ref h, height);
                        break;
                    case ("rx"):
                        ParseLength(a.Value, ref rx, width);
                        break;
                    case ("ry"):
                        ParseLength(a.Value, ref ry, height);
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break; 
					case("class"):
						css = "." + a.Value;
						break;	 
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }
            
            if(w <= 0.0f || h <= 0.0f)
                return;	  
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);  
			
			path.Clear();

            if(0.0f == rx && 0.0f == ry) 
            {
                path.MoveTo(x, y, transform, false, true);
                path.LineTo(x + w, y, transform, false, true);
                path.LineTo(x + w, y + h, transform, false, true);
                path.LineTo(x, y + h, transform, false, true);
            }
            else 
            {
                rx = (0.0f == rx) ? ry : rx;
                ry = (0.0f == ry) ? rx : ry;

                rx = (rx > w * 0.5f) ? w * 0.5f : rx;
                ry = (ry > h * 0.5f) ? h * 0.5f : ry;
                
                path.MoveTo(x + rx, y, transform, false, false);
                path.LineTo(x - rx + w, y, transform, false, false);
                path.ArcTo(rx, ry, 0, false, true, x + w, y + ry, transform, false, false);
                path.LineTo(x + w, y - ry + h, transform, false, false);
                path.ArcTo(rx, ry, 0, false, true, x - rx + w, y + h, transform, false, false);
                path.LineTo(x + rx, y + h, transform, false, false);
                path.ArcTo(rx, ry, 0, false, true, x, y - ry + h, transform, false, false);
                path.LineTo(x, y + ry, transform, false, false);
                path.ArcTo(rx, ry, 0, false, true, x + rx, y, transform, false, false);
            }
            
            path.Finalize(true);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;	 
						
            if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParseCircle(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            float  r    = 0.0f;
            float  cx   = 0.0f;
            float  cy   = 0.0f;  
			string css  = "circle";  
			string clip = "";

			foreach(XmlAttribute a in node.Attributes) 
            {
                switch(a.Name) 
                {
                    case ("cx"):
                        ParseLength(a.Value, ref cx, width);
                        break;
                    case ("cy"):
                        ParseLength(a.Value, ref cy, height);
                        break;
                    case ("r"):
                        ParseLength(a.Value, ref r, size);
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break; 
					case("class"):
						css = "." + a.Value;
						break;
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }
            
            if(r <= 0.0f)
                return;	 
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);
			
			path.Clear();
            
            path.MoveTo(cx - r, cy, transform, false, false);
            path.ArcTo(r, r, 0, true, true, cx + r, cy, transform, false, false);
            path.ArcTo(r, r, 0, true, true, cx - r, cy, transform, false, false);
            path.Finalize(true);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;

            if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParseEllipse(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            float  rx   = 0.0f;
            float  ry   = 0.0f;
            float  cx   = 0.0f;
            float  cy   = 0.0f;  
			string css  = "ellipse"; 
			string clip = "";

            foreach(XmlAttribute a in node.Attributes) 
            {
                switch(a.Name) 
                {
                    case ("cx"):
                        ParseLength(a.Value, ref cx, width);
                        break;
                    case ("cy"):
                        ParseLength(a.Value, ref cy, height);
                        break;
                    case ("rx"):
                        ParseLength(a.Value, ref rx, width);
                        break;
                    case ("ry"):
                        ParseLength(a.Value, ref ry, height);
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break;	 
					case("class"):
						css = "." + a.Value;
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }

            if(rx <= 0.0f || ry <= 0.0f)
                return;	
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size); 
			
			path.Clear();
            
            path.MoveTo(cx - rx, cy, transform, false, false);
            path.ArcTo(rx, ry, 0, true, true, cx + rx, cy, transform, false, false);
            path.ArcTo(rx, ry, 0, true, true, cx - rx, cy, transform, false, false);
            path.Finalize(true);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;
            
			if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParseLine(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {	
			float  x1   = 0.0f;
            float  y1   = 0.0f; 
            float  x2   = 0.0f;
            float  y2   = 0.0f; 
			string css  = "line";
			string clip = "";

			foreach(XmlAttribute a in node.Attributes) 
            {
                switch(a.Name) 
                {
                    case ("x1"):
                        ParseLength(a.Value, ref x1, width);
                        break;
                    case ("y1"):
                        ParseLength(a.Value, ref y1, height);
                        break;
                    case ("x2"):
                        ParseLength(a.Value, ref x2, width);
                        break;	 
					case ("y2"):
                        ParseLength(a.Value, ref y2, height);
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break; 
					case("class"):
						css = "." + a.Value;
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            }  
						
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size); 
			
			path.Clear();
			
			style.useFill = false;
			path.Clear();
 			path.MoveTo(x1, y1, transform, false, true);
			path.LineTo(x2, y2, transform, false, true);
			path.Finalize(false);

            if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static bool ParsePoints(string value, Transform transform, ref Path path)
        {
            int   i = 0;
            float x = 0.0f;
            float y = 0.0f;	  
			
			path.Clear();

            if(!ParseNumber(value, ref i, ref x))
                return false;
   
            Comma(value, ref i);
            if(!ParseNumber(value, ref i, ref y))
                return false;

            path.MoveTo(x, y, transform, false, true);
            
            while(i < value.Length)
            {
                if(!ParseNumber(value, ref i, ref x))
                    break;
                
                Comma(value, ref i);
                if(!ParseNumber(value, ref i, ref y))
                    break;

                path.LineTo(x, y, transform, false, true);
            }

            return true;
        }
        
        static void ParsePolyline(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            string css  = "polyline";
			string data = ""; 
			string clip = "";
            
            foreach(XmlAttribute a in node.Attributes) 
            { 
                switch(a.Name) 
                {
                    case ("points"):
                        data = a.Value;
                        break;
                    case ("transform"):
                        ParseTransform(a.Value, ref transform);
                        break; 
					case("class"):
						css = "." + a.Value;
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            } 
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);	
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);

			if(!ParsePoints(data, transform, ref path)) 
			{
 				path.Clear();
                return;	
			}
			      
            path.Finalize(false);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;
            
			if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
        
        static void ParsePolygon(XmlNode node, Style style, Transform transform, Rect view, float width, float height, float size)
        {
            string css  = "polygon";
			string data = "";
			string clip = "";
            
            foreach(XmlAttribute a in node.Attributes) 
            { 
                switch(a.Name) 
                {
                    case("points"):
                        data = a.Value;
                        break;
                    case("transform"):
                        ParseTransform(a.Value, ref transform);
                        break; 
					case("class"):
						css = "." + a.Value;
						break;	
					case("clip-path"):
						ParseLink(a.Value, ref clip);
						break;
                    default:
                        ParseStyle(a.Name, a.Value, ref style, size);
                        break;
                }
            } 
			
			if(styleMap.ContainsKey(css)) 
				ParseStyle(styleMap[css], ref style, size);
			
			if(useMap.ContainsKey(clip))
				ParseClipPath(useMap[clip], style, transform, view, width, height, size);

			if(!ParsePoints(data, transform, ref path)) 
			{ 
				path.Clear();
                return;
			}	 

            path.Finalize(true);
            path.fillRule = (clipPathLevel > 0) ? style.clipRule : style.fillRule;

            if(clipPathLevel > 0)
				AddClipPath(path, useMap.ContainsKey(clip));
			else
				MeshCreator.AddPath(path, ClipPath, style, transform); 
			
			if(useMap.ContainsKey(clip)) 
  				RemoveClipPath();
        }
                
        static void ParseGradientStop(XmlNode node, out GradientStop stop)
        {
            int    i    = 0;
            float  f    = 0.0f; 
			string data = ""; 

            stop = GradientStop.Default;

            foreach(XmlAttribute a in node.Attributes) 
            {
				i = 0;

				if("offset" == a.Name && ParseNumber(a.Value, ref i, ref f)) 
				{ 
					if('%' == a.Value[a.Value.Length-1])
						f *= 0.01f;	
					stop.ratio = Mathf.Clamp01(f);
				}
				else if("stop-opacity" == a.Name && ParseNumber(a.Value, ref i, ref f))
				{	
					f = Mathf.Clamp01(f); 
					stop.color.a = (byte)Mathf.RoundToInt(f * 255.0f);
				}
				else if("stop-color" == a.Name)
				{	
					ParseColor(a.Value, ref stop.color);
				} 
				else if("style" == a.Name)
				{	
					data = a.Value;
				}
            } 
			
			if(!string.IsNullOrEmpty(data)) 
			{
				string name  = "";
				string value = "";
				int    n     = 0;
				i = 0;

				while(i < data.Length) 
				{	
					name  = "";
					value = "";
					n     = 0;

					while(i < data.Length && ':' != data[i]) 
					{
						if(' ' != data[i] && '\t' != data[i] && '\n' != data[i] && '\r' != data[i])
							name += data[i];	
						++i;
					}  
					
					if(i < data.Length && ':' != data[i++])
						break;	 
					
					while(i < data.Length && ';' != data[i]) 
					{
						if(' ' != data[i] && '\t' != data[i] && '\n' != data[i] && '\r' != data[i])
							value += data[i];	
						++i;
					}
				
					if(i < data.Length && ';' != data[i++])
						break;	
					
					if("stop-opacity" == name && ParseNumber(value, ref n, ref f))
					{	
						f = Mathf.Clamp01(f); 
						stop.color.a = (byte)Mathf.RoundToInt(f * 255.0f);
					}
					else if("stop-color" == name)
					{	
						ParseColor(value, ref stop.color); 
					}	

				}
			}
        }
        
        static float ParseSpreadMethode(string value)
        {
            if("pad" == value)
                return 0.0f;
            else if("repeat" == value)
                return 0.1f;
            else if("reflect" == value)
                return 0.2f;

            return 0.0f;
        }
        
        static void ParseGradient(XmlNode node, bool radial)
        {
            GradientStop stop;
            Transform    transform = Transform.identity;
            Gradient     gradient  = new Gradient(radial);	
			string       link      = "";
			float        x1        = 0.0f; 
			float        y1        = 0.0f;	
			float        x2        = 1.0f;	
			float        y2        = 0.0f;	
			float        cx        = 0.5f;	
			float        cy        = 0.5f;	
			float        r         = 0.5f;
			bool         fx        = false;  
			bool         fy        = false;

            foreach(XmlAttribute a in node.Attributes)
			{
				if("xlink:href" == a.Name)
				{
					ParseLink(a.Value, ref link); 
					
					if(gradientMap.ContainsKey(link)) 
					{
						XmlNode g = gradientMap[link];
						
						foreach(XmlAttribute aa in g.Attributes) 
						{ 
							switch(aa.Name) 
							{ 
								case("x1"): 
									ParseLength(aa.Value, ref x1, 1.0f);
									break;	
								case("y1"): 
									ParseLength(aa.Value, ref y1, 1.0f);
									break; 
								case("x2"): 
									ParseLength(aa.Value, ref x2, 1.0f);
									break;	
								case("y2"): 
									ParseLength(aa.Value, ref y2, 1.0f);
									break;	
								case("cx"): 
									ParseLength(aa.Value, ref cx, 1.0f);
									break;	
								case("cy"): 
									ParseLength(aa.Value, ref cy, 1.0f);
									break;	
								case("fx"): 
									ParseLength(aa.Value, ref gradient.fx, 1.0f);
									fx = true;
									break;	
								case("fy"): 
									ParseLength(aa.Value, ref gradient.fy, 1.0f);
									fy = true;
									break;
							}
						}
					}
					break;
				}
			}
			
			foreach(XmlAttribute a in node.Attributes) 
            {
                switch(a.Name) 
                { 
					case("gradientUnits"):	
						gradient.userSpaceOnUse = ("userSpaceOnUse" == a.Value) ? true : false;
						break;
                    case("gradientTransform"):
                        ParseTransform(a.Value, ref gradient.transform);
                        break;
                    case("x1"):
                        ParseLength(a.Value, ref x1, 1.0f);
                        break;
                    case("y1"):
                        ParseLength(a.Value, ref y1, 1.0f);
                        break;
                    case("x2"):
                        ParseLength(a.Value, ref x2, 1.0f);
                        break;
                    case("y2"):
                        ParseLength(a.Value, ref y2, 1.0f);
                        break;
                    case("cx"):
                        ParseLength(a.Value, ref cx, 1.0f);
                        break;
                    case("cy"):
                        ParseLength(a.Value, ref cy, 1.0f);
                        break;
                    case("r"):
                        ParseLength(a.Value, ref r, 1.0f);
                        break;
                    case("fx"):
                        ParseLength(a.Value, ref gradient.fx, 1.0f);
						fx = true;
                        break;
                    case("fy"):
                        ParseLength(a.Value, ref gradient.fy, 1.0f); 
						fy = true;
                        break;
                    case("spreadMethod"):
                        gradient.spreadMode = ParseSpreadMethode(a.Value);
                        break;
                    case("id"):
                        gradient.name = a.Value;
                        break;
                    default:
                        break;
                }
            }    

            foreach(XmlNode n in node.ChildNodes) 
            {  
                if("stop" != n.Name)
                    continue;

                ParseGradientStop(n, out stop);
                gradient.stopList.Add(stop);

				if(stop.color.a < 255)
					gradient.transparent = true;
            } 
			
			if(0 == gradient.stopList.Count && !string.IsNullOrEmpty(link)) 
				gradient.link = link;
             
            if(radial)
            {
				if(!fx)
					gradient.fx = cx;	
				if(!fy)
					gradient.fy = cy;

                transform.Translate(cx, cy);
                transform.Scale(r, r);	
				
                Vector2 v1 = Vector2.zero;
                Vector2 v2 = Vector2.zero;
                v1.Set(gradient.fx, gradient.fy);
                v2 = transform.inverse * v1;
 
                if(v2.magnitude > 0.999f)
                    v2 = v2 * (0.999f/v2.magnitude); 
                
                gradient.fx = v2.x;
                gradient.fy = v2.y;	 
				                
                gradient.transform = gradient.transform * transform;
            }
            else 
            {
                Vector2 b = Vector2.zero;
                Vector2 e = Vector2.zero;
                Vector2 s = Vector2.zero;

                b.Set(x1, y1);
                e.Set(x2, y2);
                s = e - b;
             
                float d = Vector2.Distance(b, e);
                float a = Mathf.Atan2(s.y, s.x) * Mathf.Rad2Deg;

                transform.Translate(x1, y1);			
                transform.Rotate(a, 0.0f, 0.0f);
                transform.Scale(d, 1.0f);
                                                  
                gradient.transform = gradient.transform * transform; 
            }

            MeshCreator.AddGradient(gradient);
        }

        public static void LoadSVG(string data, float scale, float aa, float curve, float precision, float offset, bool transparent, bool collider, bool rtt)
        {
			XmlDocument document  = new XmlDocument();
			Rect        view      = Rect.Zero;
            Style       style     = Style.Default;
            Transform   transform = Transform.identity; 
			
			Path.approximationScale       = curve * curve;
			MeshCreator.antialiasingWidth = aa;	
			MeshCreator.precision         = precision; 	 
			MeshCreator.depthOffset       = offset;	  
			MeshCreator.isTransparent     = transparent;
			MeshCreator.isCollider        = collider;	
			MeshCreator.renderToTexture   = rtt;
			MeshCreator.Clear();          
			
			document.LoadXml(data);         
            transform.Scale(scale, scale);

			view.Width     = 2000.0f;
			view.Height    = 2000.0f;
			clipPathLevel  = 0;	 
			clipPathOffset = 0;

            foreach(XmlNode n in document.ChildNodes) 
            {
                if("svg" == n.Name && 0 != n.ChildNodes.Count) 
                {
                    ParseDefs(n, 2000.0f, 2000.0f);	
					
					foreach(KeyValuePair<string, XmlNode> g in gradientMap) 
					{ 
						if("linearGradient" == g.Value.Name)
							ParseGradient(g.Value, false); 
						else if("radialGradient" == g.Value.Name)
							ParseGradient(g.Value, true);
					}

                    ParseSVG(n, style, transform, view, 2000.0f, 2000.0f, 2000.0f);	   
                } 
            }

			useMap.Clear(); 
			gradientMap.Clear();  
			styleMap.Clear(); 
			clipPath.Clear();
			clipPathResult.Clear();
        }
    }
}