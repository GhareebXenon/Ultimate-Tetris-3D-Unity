using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class BarycentricHardEdges : MonoBehaviour
{
    [Range(0f, 180f)]
    public float hardEdgeAngle = 30f;

    void Awake()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh original = mf.sharedMesh;

        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] verts = original.vertices;
        int[] tris = original.triangles;

        List<Vector3> newVerts = new();
        List<int> newTris = new();
        List<Vector4> bary = new();

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 v0 = verts[tris[i]];
            Vector3 v1 = verts[tris[i + 1]];
            Vector3 v2 = verts[tris[i + 2]];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            int baseIndex = newVerts.Count;

            newVerts.Add(v0);
            newVerts.Add(v1);
            newVerts.Add(v2);

            newTris.Add(baseIndex);
            newTris.Add(baseIndex + 1);
            newTris.Add(baseIndex + 2);

            // XYZ = barycentric, W = hard edge mask
            bary.Add(new Vector4(1, 0, 0, 1));
            bary.Add(new Vector4(0, 1, 0, 1));
            bary.Add(new Vector4(0, 0, 1, 1));
        }

        mesh.SetVertices(newVerts);
        mesh.SetTriangles(newTris, 0);
        mesh.SetUVs(1, bary);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
