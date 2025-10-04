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

        //ApplyCraters(vertices, radius, craterCount: 3);
        //mesh.vertices = vertices;
        //mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        //CrumpleAtWorldPoint(new Vector3(1, 0, 0), 0.5f, 0.5f);

        //canAlter = true;
    }

    // void ApplyCraters(Vector3[] vertices, float radius, int craterCount)
    // {
    //     float craterRadius = radius * 0.5f;
    //     float craterDepth = radius * 1.0f;

    //     for (int i = 0; i < craterCount; i++)
    //     {
    //         Vector3 craterCenter = Random.onUnitSphere * radius;

    //         for (int v = 0; v < vertices.Length; v++)
    //         {
    //             float dist = Vector3.Distance(vertices[v], craterCenter);

    //             if (dist < craterRadius)
    //             {
    //                 float falloff = 1f - (dist / craterRadius);
    //                 float depth = falloff * craterDepth;
    //                 vertices[v] -= vertices[v].normalized * depth;
    //             }
    //         }
    //     }
    // }


    public void CrumpleAtWorldPoint(Vector3 worldImpactPoint, float radius, float depth)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        Vector3[] vertices = mesh.vertices;

        // Convert world point into *local space of the mesh*
        Vector3 localImpactPoint = transform.InverseTransformPoint(worldImpactPoint);

        for (int i = 0; i < vertices.Length; i++)
        {
            // vertices[i] is already in local space
            float dist = Vector3.Distance(vertices[i], localImpactPoint);

            if (dist < radius)
            {
                float falloff = 1f - (dist / radius);
                float push = falloff * depth;

                // Push vertex inward toward center of sphere (or toward impact point if desired)
                vertices[i] -= vertices[i].normalized * push;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // ALSO update the collider if using MeshCollider
        var collider = GetComponent<MeshCollider>();
        if (collider != null)
        {
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Raycast Hit Point (world): " + hit.point);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 2f);

                lastHitPoint = hit.point;  // For OnDrawGizmos
                CrumpleAtWorldPoint(hit.point, 0.5f, 0.5f);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");
            }
        }
    }

    Vector3 lastHitPoint;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastHitPoint, 0.05f);
    }

    
}
