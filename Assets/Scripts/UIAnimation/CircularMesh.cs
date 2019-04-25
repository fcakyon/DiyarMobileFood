using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularMesh : MonoBehaviour
{
    public float radius;
    private const int CircleSegmentCount = 64;
    private const int CircleVertexCount = CircleSegmentCount + 2;
    private const int CircleIndexCount = CircleSegmentCount * 3;

    void Update()
    {
        Mesh mesh = GenerateCircleMesh(radius);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private static Mesh GenerateCircleMesh(float radius)
    {
        var circle = new Mesh();
        var vertices = new List<Vector3>(CircleVertexCount);
        var uvs = new List<Vector2>(CircleVertexCount);
        var indices = new int[CircleIndexCount];
        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;
        var angle = 0f;
        vertices.Add(Vector3.zero);
        uvs.Add(new Vector2(vertices[0].x, vertices[0].z));
        for (int i = 1; i < CircleVertexCount; ++i)
        {
            vertices.Add(new Vector3(radius * Mathf.Cos(angle), 0f, radius*Mathf.Sin(angle)));
            uvs.Add(new Vector2(vertices[i].x, vertices[i].z));
            angle -= segmentWidth;
            if (i > 1)
            {
                var j = (i - 2) * 3;
                indices[j + 0] = 0;
                indices[j + 1] = i - 1;
                indices[j + 2] = i;
            }
        }
        circle.SetVertices(vertices);
        circle.SetUVs(0, uvs);
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();
        return circle;
    }
}