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
        MeshFilter.mesh.Clear();

        int rows = map.GetLength(0);
        int columns = map.GetLength(1);
        UVScalar = new Vector2(1.0f / (columns - 1), 1.0f / (rows - 1));

        for (int r = 0; r < rows - 1; r++) {
            for (int c = 0; c < columns - 1; c++) {
                // corner vertices
                Vector2 v0 = new Vector2(c + 0, r + 0) + Vector2.one * 0.5f;
                Vector2 v1 = new Vector2(c + 1, r + 0) + Vector2.one * 0.5f;
                Vector2 v2 = new Vector2(c + 1, r + 1) + Vector2.one * 0.5f;
                Vector2 v3 = new Vector2(c + 0, r + 1) + Vector2.one * 0.5f;

                // corner values
                float c0 = map[r + 0, c + 0];
                float c1 = map[r + 0, c + 1];
                float c2 = map[r + 1, c + 1];
                float c3 = map[r + 1, c + 0];

                int id =
                    (c0 > treshold ? 1 : 0) * 1 +
                    (c1 > treshold ? 1 : 0) * 2 +
                    (c2 > treshold ? 1 : 0) * 4 +
                    (c3 > treshold ? 1 : 0) * 8;

                // remap from value to 0-1 range
                float t0 = Mathf.InverseLerp(c0, c1, treshold);
                float t1 = Mathf.InverseLerp(c1, c2, treshold);
                float t2 = Mathf.InverseLerp(c2, c3, treshold);
                float t3 = Mathf.InverseLerp(c3, c0, treshold);
                // get edge vertex
                Vector2 eB = Vector2.Lerp(v0, v1, t0);
                Vector2 eR = Vector2.Lerp(v1, v2, t1);
                Vector2 eT = Vector2.Lerp(v2, v3, t2);
                Vector2 eL = Vector2.Lerp(v3, v0, t3);

                switch (id) {
                // 1 triangle
                    case 1:
                        AddTriangle(v0, eL, eB);
                        break;
                    case 2:
                        AddTriangle(eB, eR, v1);
                        break;
                    case 4:
                        AddTriangle(eT, v2, eR);
                        break;
                    case 8:
                        AddTriangle(v3, eT, eL);
                        break;
                // 2 triangles
                    case 3:
                        AddTriangle(v0, eL, v1);
                        AddTriangle(eL, eR, v1);
                        break;
                    case 6:
                        AddTriangle(eT, v2, v1);
                        AddTriangle(eT, v1, eB);
                        break;
                    case 9:
                        AddTriangle(v3, eT, eB);
                        AddTriangle(v3, eB, v0);
                        break;
                    case 12:
                        AddTriangle(v3, v2, eR);
                        AddTriangle(v3, eR, eL);
                        break;
                    case 15:
                        AddTriangle(v0, v3, v2);
                        AddTriangle(v0, v2, v1);
                        break;
                // 3 triangles
                    case 7:
                        AddTriangle(v0, eL, eT);
                        AddTriangle(v0, eT, v2);
                        AddTriangle(v0, v2, v1);
                        break;
                    case 11:
                        AddTriangle(v0, v3, eT);
                        AddTriangle(v0, eT, eR);
                        AddTriangle(v0, eR, v1);
                        break;
                    case 13:
                        AddTriangle(v0, v3, v2);
                        AddTriangle(v0, v2, eR);
                        AddTriangle(v0, eR, eB);
                        break;
                    case 14:
                        AddTriangle(eL, v3, v2);
                        AddTriangle(eL, v2, v1);
                        AddTriangle(eL, v1, eB);
                        break;
                // 4 triangles
                    case 5:
                        AddTriangle(v0, eL, eT);
                        AddTriangle(v0, eT, v2);
                        AddTriangle(v0, v2, eR);
                        AddTriangle(v0, eR, eB);
                        break;
                    case 10:
                        AddTriangle(eL, v3, eT);
                        AddTriangle(eL, eT, eR);
                        AddTriangle(eL, eR, v1);
                        AddTriangle(eL, v1, eB);
                        break;
                }
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
