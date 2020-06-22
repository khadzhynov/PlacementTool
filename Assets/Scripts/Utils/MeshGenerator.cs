using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedural mesh generation utility.
/// </summary>
public class MeshGenerator
{
    /// <summary>
    /// Builds flat free-form mesh, based on a list of points.
    /// </summary>
    /// <param name="planarDrawing">cycle-ordered array of Vector2 points.</param>
    /// <param name="orientation">plane orientation</param>
    /// <param name="invertNormals">mesh triangles orientation</param>
    /// <returns>built mesh</returns>
    public static Mesh BuildFlatMesh(List<Vector2> planarDrawing, Vector3 orientation, bool invertNormals)
    {
        int[] indices = Triangulate(planarDrawing, invertNormals);

        Vector3[] vertices = new Vector3[planarDrawing.Count];

        ApplyOrientation(planarDrawing, orientation, vertices);

        return CreateMesh(indices, vertices);
    }

    private static void ApplyOrientation(List<Vector2> plan, Vector3 orientation, Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            if (orientation.z != 0)
            {
                vertices[i] = new Vector3(plan[i].x, plan[i].y * orientation.z, 0);
            }

            if (orientation.x != 0)
            {
                vertices[i] = new Vector3(0, plan[i].x, plan[i].y * orientation.x);
            }

            if (orientation.y != 0)
            {
                vertices[i] = new Vector3(plan[i].x, 0, plan[i].y * orientation.y);
            }
        }
    }

    private static Mesh CreateMesh(int[] indices, Vector3[] vertices)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        MapUV(vertices, mesh);
        return mesh;
    }

    private static int[] Triangulate(List<Vector2> points, bool invertNormals)
    {
        List<int> indices = new List<int>();

        int n = points.Count;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];

        FillVInOrder(points, n, V);

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(points, u, v, w, nv, V))
            {
                count = AddTriangle(invertNormals, indices, V, ref nv, ref m, v, u, w);
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private static int AddTriangle(bool invertNormals, List<int> indices, int[] V, ref int nv, ref int m, int v, int u, int w)
    {
        int count;
        int a, b, c, s, t;
        a = V[u];
        b = V[v];
        c = V[w];

        indices.Add(a);
        if (invertNormals)
        {
            indices.Add(c);
            indices.Add(b);
        }
        else
        {
            indices.Add(b);
            indices.Add(c);
        }

        m++;
        for (s = v, t = v + 1; t < nv; s++, t++)
            V[s] = V[t];
        nv--;
        count = 2 * nv;
        return count;
    }

    private static void FillVInOrder(List<Vector2> points, int n, int[] V)
    {
        if (Area(points) > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }
    }

    private static float Area(List<Vector2> points)
    {
        int n = points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = points[p];
            Vector2 qval = points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private static bool Snip(List<Vector2> points, int u, int v, int w, int n, int[] V)
    {
        int p;
        Vector2 A = points[V[u]];
        Vector2 B = points[V[v]];
        Vector2 C = points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;

        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = points[V[p]];
            if (IsInsideTriangle(A, B, C, P))
                return false;
        }

        return true;
    }

    private static bool IsInsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }

    private static void MapUV(Vector3[] vertices, Mesh mesh)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3 minMax = mesh.bounds.max - mesh.bounds.min;

        int xIndex = 0;
        int yIndex = 1;

        float minDimension = Mathf.Min(minMax.x, minMax.y, minMax.z);
        
        if (minDimension == minMax.x)
        {
            xIndex = 2;
        }

        if (minDimension == minMax.y)
        {
            yIndex = 2;
        }

        for (int i = 0; i < vertices.Length; ++i)
        {
            uvs[i] = new Vector2(
                vertices[i].x / minMax[xIndex],
                vertices[i].z / minMax[yIndex]);
        }

        mesh.uv = uvs;
    }
}
