using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Ocean : MeshGenerator
{
    [Header("Tsunami Animation Settings")]
    [Tooltip("The initial height (amplitude) of the wave crest.")]
    public float waveAmplitude = 0.1f;
    [Tooltip("The width of the wave crest in degrees. A larger value creates a wider wave.")]
    [Range(1f, 90f)]
    public float waveWidth = 25f;
    [Tooltip("The speed the wave travels across the surface in degrees per second.")]
    public float waveSpeed = 60f;

    // Private variables for the animation state
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;

    void Awake()
    {
        // Generate the sphere using the method from the parent MeshGenerator class.
        GenerateSphere();
    }

    void Update()
    {
        // Example trigger: click the mouse to create an impact.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartTsunami(hit.point);
            }
        }
    }

    /// <summary>
    /// Public method to trigger the tsunami animation.
    /// </summary>
    public void StartTsunami(Vector3 worldImpactPoint)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTsunami(worldImpactPoint));
    }

    /// <summary>
    /// Animates a tsunami wave propagating outwards from an impact point.
    /// </summary>
    private IEnumerator AnimateTsunami(Vector3 worldImpactPoint)
    {
        originalVertices = mesh.vertices;
        currentVertices = new Vector3[originalVertices.Length];
        
        // Convert the world impact point to a local direction vector.
        Vector3 localImpactPoint = transform.InverseTransformPoint(worldImpactPoint).normalized;

        // Pre-calculate the angle of each vertex from the impact point.
        // This is a performance optimization, so we don't recalculate it every frame.
        float[] vertexAngles = new float[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            vertexAngles[i] = Vector3.Angle(originalVertices[i].normalized, localImpactPoint);
        }

        // The animation will run until the wave has traveled 180 degrees to the antipode.
        float duration = 180f / waveSpeed;
        float elapsedTime = 0f;

        // --- Animation Loop ---
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // The current angular distance of the wave crest from the impact point.
            float currentWaveAngle = elapsedTime * waveSpeed;

            // As the wave travels, its amplitude decreases (dissipates).
            // It starts at full height and fades to zero as it reaches the antipode.
            float dissipation = 1f - (currentWaveAngle / 180f);
            float currentAmplitude = waveAmplitude * dissipation;

            for (int i = 0; i < currentVertices.Length; i++)
            {
                float heightOffset = 0f;

                // Find the distance between this vertex and the travelling wave crest.
                float distanceToCrest = Mathf.Abs(vertexAngles[i] - currentWaveAngle);

                // If the vertex is within the influence of the wave's width...
                if (distanceToCrest < waveWidth / 2f)
                {
                    // Use a cosine function to create a smooth, curved wave shape.
                    // 'waveT' goes from 0 at the crest to 1 at the edge of the wave.
                    float waveT = distanceToCrest / (waveWidth / 2f);
                    float waveShape = Mathf.Cos(waveT * Mathf.PI * 0.5f);

                    heightOffset = waveShape * currentAmplitude;
                }

                // Apply the height offset. The vertex only moves radially (up/down).
                // Its position on the sphere's surface (originalVertices[i]) does not change.
                currentVertices[i] = originalVertices[i].normalized * (radius + heightOffset);
            }

            // Update the mesh with the new vertex positions.
            mesh.vertices = currentVertices;
            mesh.RecalculateNormals();
            
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;

            yield return null; // Wait for the next frame.
        }
    }
}