using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    public Mesh mesh;
    public int longitudeSegments = 20;
    public int latitudeSegments = 20;
    public float radius = 1f;

    private Vector3[] originalVertices;
    private Vector3[] deformedVertices;
    public float crumpleAmount = 0.5f; // How far vertices get pulled in
    public float crumpleRadius = 0.5f; // How big the area of crumpling is
    public Vector3 crumpleCenter = Vector3.up; // Center of the crumple (world or local depending)

    public bool canAlter = false;

    void Start()
    {
        GenerateSphere();
    }

    void GenerateSphere()
    {
        mesh = new Mesh();
        mesh.name = "Procedural Sphere";

        int vertCount = (longitudeSegments + 1) * (latitudeSegments + 1);
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uv = new Vector2[vertCount];

        int index = 0;

        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float a1 = Mathf.PI * lat / latitudeSegments;
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float a2 = 2 * Mathf.PI * lon / longitudeSegments;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                float x = sin1 * cos2;
                float y = cos1;
                float z = sin1 * sin2;

                Vector3 vertex = new Vector3(x, y, z) * radius;
                vertices[index] = vertex;
                normals[index] = vertex.normalized;
                uv[index] = new Vector2((float)lon / longitudeSegments, (float)lat / latitudeSegments);
                index++;
            }
        }

        int[] triangles = new int[longitudeSegments * latitudeSegments * 6];
        int triIndex = 0;

        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                triangles[triIndex++] = current;
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;

                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next + 1;
                triangles[triIndex++] = next;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        ApplyCraters(vertices, radius, craterCount: 3);

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = mesh;

        //canAlter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAlter)
        {
            originalVertices = mesh.vertices;
            deformedVertices = new Vector3[originalVertices.Length];

            originalVertices.CopyTo(deformedVertices, 0);

        }
    }

    void ApplyCraters(Vector3[] vertices, float radius, int craterCount)
    {
        float craterRadius = radius * 0.5f;
        float craterDepth = radius * 1.0f;

        for (int i = 0; i < craterCount; i++)
        {
            Vector3 craterCenter = Random.onUnitSphere * radius;

            for (int v = 0; v < vertices.Length; v++)
            {
                float dist = Vector3.Distance(vertices[v], craterCenter);

                if (dist < craterRadius)
                {
                    float falloff = 1f - (dist / craterRadius);
                    float depth = falloff * craterDepth;
                    vertices[v] -= vertices[v].normalized * depth;
                }
            }
        }
}
        
}
