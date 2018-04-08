using System.Collections.Generic;
using UnityEngine;

public class MeshGeneratorLerpMS : MonoBehaviour {

    public MeshFilter MeshFilter;

    private List<Vector3> Vertices;
    private List<int> Triangles;
    private List<Vector2> UVs;

    private Sprite[] TileSprites;
    private Vector2 UVScalar;

    void Start() {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        UVs = new List<Vector2>();
    }

    public void GenerateGrid(float[,] map, float treshold) {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        int rows = map.GetLength(0);
        int columns = map.GetLength(1);
        UVScalar = new Vector2(1.0f / (columns - 1), 1.0f / (rows - 1));

        for (int r = 0; r < rows - 1; r++) {
            for (int c = 0; c < columns - 1; c++) {
                // corner vertices
                Vector2 v0 = new Vector2(c + 0, r + 0) + Vector2.one * 0.5f;
                Vector2 v1 = new Vector2(c + 1, r + 0) + Vector2.one * 0.5f;
                Vector2 v2 = new Vector2(c + 0, r + 1) + Vector2.one * 0.5f;
                Vector2 v3 = new Vector2(c + 1, r + 1) + Vector2.one * 0.5f;

                AddTriangle(v0, v2, v3);
                AddTriangle(v0, v3, v1);
            }
        }

        MeshFilter.mesh.vertices = Vertices.ToArray();
        MeshFilter.mesh.triangles = Triangles.ToArray();
        MeshFilter.mesh.uv = UVs.ToArray();
    }

    private void AddTriangle(Vector2 v0, Vector2 v1, Vector2 v2) {
        int vertexIndex = Vertices.Count;
        Vertices.Add(v0);
        Vertices.Add(v1);
        Vertices.Add(v2);
        Triangles.Add(vertexIndex);
        Triangles.Add(vertexIndex + 1);
        Triangles.Add(vertexIndex + 2);
        UVs.Add(Vector2.Scale(v0 - Vector2.one * 0.5f, UVScalar));
        UVs.Add(Vector2.Scale(v1 - Vector2.one * 0.5f, UVScalar));
        UVs.Add(Vector2.Scale(v2 - Vector2.one * 0.5f, UVScalar));
    }
}
