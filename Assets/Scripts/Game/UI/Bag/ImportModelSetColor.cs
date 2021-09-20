using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportModelSetColor : HTBehaviour
{
    public int[] oldtriangles;
    public int[] newtriangles;


    public void SetColor()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        
        oldtriangles = mesh.triangles;
        newtriangles = new int[oldtriangles.Length * 2];
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < oldtriangles.Length / 3; i++)
        {
            newtriangles[i * 6] = oldtriangles[i * 3];
            newtriangles[i * 6 + 1] = oldtriangles[i * 3 + 1];
            newtriangles[i * 6 + 2] = oldtriangles[i * 3 + 2];
            newtriangles[i * 6 + 3] = oldtriangles[i * 3];
            newtriangles[i * 6 + 4] = oldtriangles[i * 3 + 2];
            newtriangles[i * 6 + 5] = oldtriangles[i * 3 + 1];
        }
        

        for (int i = 0; i < vertices.Length; i++)
        {
            

            if (Mathf.Abs(vertices[i].x) > 39 && Mathf.Abs(vertices[i].z) > 49)
            {
                colors[i] = new Color(1, 1, 1);
            }
            else
            {
                colors[i] = new Color(0, 0, 1);
            }
        }

        mesh.colors = colors;
        mesh.triangles = newtriangles;
    }
}
