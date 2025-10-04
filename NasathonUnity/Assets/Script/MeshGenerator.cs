using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{  
    public Mesh mesh;
    public int longitudeSegments = 20;
    public int latitudeSegments = 20;
    public float radius;

    public void GenerateSphere()
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

        GetComponent<MeshFilter>().sharedMesh = null;
        GetComponent<MeshFilter>().sharedMesh = mesh;

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }


}
