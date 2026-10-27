using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SplineContainer))]
public class SplineAbdomenExtruder : MonoBehaviour
{
    [Header("Mesh Settings")]
    [Tooltip("How many sides the circular cross-section has.")]
    public int radialSegments = 12;
    [Tooltip("How many lengthwise segments along the spline.")]
    public int lengthSegments = 30;
    public Material material;

    [Header("Shape & Taper")]
    [Tooltip("Base radius of the abdomen.")]
    public float baseRadius = 0.5f;
    [Tooltip("Defines the thickness profile from start (0) to end (1). Make the end 0 for a sharp point.")]
    public AnimationCurve shapeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private MeshFilter meshFilter;
    private SplineContainer splineContainer;

    void OnValidate()
    {
        if (gameObject.activeInHierarchy) GenerateMesh();
    }

    void Start()
    {
        GenerateMesh();
    }

    [ContextMenu("Generate Mesh")]
    public void GenerateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        splineContainer = GetComponent<SplineContainer>();
        
        if (material != null)
        {
            GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        // Check if spline actually exists and has points
        if (splineContainer == null) 
        {
            Debug.LogError("Abdomen Extruder: SplineContainer component is missing!");
            return;
        }
        if (splineContainer.Spline == null || splineContainer.Spline.Count < 2) 
        {
            Debug.LogWarning("Abdomen Extruder: Spline needs at least 2 points drawn in the scene view!");
            return;
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        Spline spline = splineContainer.Spline;

        // 1. Generate Vertices, Normals, and UVs
        for (int i = 0; i <= lengthSegments; i++)
        {
            float t = (float)i / lengthSegments;
            
            spline.Evaluate(t, out var position, out var forward, out var up);
            
            Vector3 pos = (Vector3)position;
            Vector3 fwd = Vector3.Normalize((Vector3)forward);
            Vector3 right = Vector3.Normalize(Vector3.Cross((Vector3)up, fwd));
            Vector3 localUp = Vector3.Cross(fwd, right);

            // Safety check for invalid directions
            if (fwd == Vector3.zero || right == Vector3.zero)
            {
                right = Vector3.right;
                localUp = Vector3.up;
            }

            float radiusScale = shapeCurve.Evaluate(t);
            float currentRadius = baseRadius * radiusScale;

            for (int j = 0; j <= radialSegments; j++)
            {
                float angle = ((float)j / radialSegments) * Mathf.PI * 2f;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                Vector3 normal = (right * cos + localUp * sin).normalized;
                Vector3 vertexPos = pos + normal * currentRadius;

                // FIX: Use local coordinates directly instead of converting from world space
                vertices.Add(vertexPos);
                normals.Add(normal);
                uvs.Add(new Vector2((float)j / radialSegments, t));
            }
        }

        // 2. Generate Triangles
        for (int i = 0; i < lengthSegments; i++)
        {
            for (int j = 0; j < radialSegments; j++)
            {
                int currRow = i * (radialSegments + 1);
                int nextRow = (i + 1) * (radialSegments + 1);

                int bottomLeft = currRow + j;
                int bottomRight = currRow + j + 1;
                int topLeft = nextRow + j;
                int topRight = nextRow + j + 1;

                triangles.Add(bottomLeft);
                triangles.Add(topLeft);
                triangles.Add(bottomRight);

                triangles.Add(bottomRight);
                triangles.Add(topLeft);
                triangles.Add(topRight);
            }
        }

        // Apply everything to a fresh mesh asset at the very end
        Mesh mesh = new Mesh();
        mesh.name = "ProceduralAntAbdomen";
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;
    }

}
