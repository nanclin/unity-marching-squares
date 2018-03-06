using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    public MeshFilter MeshFilter;
    public Texture2D TileAtlas;

    private Vector3[] Vertices;
    private int[] Triangles;
    private Vector2[] UVs;

    private Sprite[] TileSprites;

    void Start() {
        GenerateGrid(3, 3);
    }

    private void GenerateGrid(int rows, int columns) {

        TileSprites = Resources.LoadAll<Sprite>(TileAtlas.name);

        int vertexCount = columns * rows * 4;
        int trianglesCount = columns * rows * 2 * 3;
        Vertices = new Vector3[vertexCount];
        Triangles = new int[trianglesCount];
        UVs = new Vector2[vertexCount];

        int vertexIndex = 0;
        int triangleIndex = 0;
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                AddMSTile(new Vector3(c, r), 1, 1, Random.Range(0, 16), ref vertexIndex, ref triangleIndex);
            }
        }

        MeshFilter.mesh.vertices = Vertices;
        MeshFilter.mesh.triangles = Triangles;
        MeshFilter.mesh.uv = UVs;
    }

    private void AddMSTile(Vector3 pos, float width, float height, int id, ref int vertexIndex, ref int triangleIndex) {

        // vertices
        Vertices[vertexIndex + 0] = pos;
        Vertices[vertexIndex + 1] = pos + new Vector3(width, 0, 0);
        Vertices[vertexIndex + 2] = pos + new Vector3(0, height, 0);
        Vertices[vertexIndex + 3] = pos + new Vector3(width, height, 0);

        // uvs
        List<Vector2> uvs = GetMSTileUVs(id);
        UVs[vertexIndex + 0] = uvs[0];
        UVs[vertexIndex + 1] = uvs[1];
        UVs[vertexIndex + 2] = uvs[2];
        UVs[vertexIndex + 3] = uvs[3];

        // triangles
        Triangles[triangleIndex] = vertexIndex;
        Triangles[triangleIndex + 1] = vertexIndex + 2;
        Triangles[triangleIndex + 2] = vertexIndex + 1;
        triangleIndex += 3;

        Triangles[triangleIndex] = vertexIndex + 2;
        Triangles[triangleIndex + 1] = vertexIndex + 3;
        Triangles[triangleIndex + 2] = vertexIndex + 1;
        triangleIndex += 3;

        vertexIndex += 4;
    }

    private List<Vector2> GetMSTileUVs(int index) {
        Rect rect = TileSprites[index].rect;

        float x0 = rect.x;
        float x1 = rect.x + rect.width;
        float y0 = rect.y;
        float y1 = rect.y + rect.height;

        // convert to 0-1 range
        float u0 = x0 / TileAtlas.width;
        float u1 = x1 / TileAtlas.width;
        float v0 = y0 / TileAtlas.height;
        float v1 = y1 / TileAtlas.height;

        return new List<Vector2>() {
            new Vector2(u0, v0),
            new Vector2(u1, v0),
            new Vector2(u0, v1),
            new Vector2(u1, v1),
        };
    }
}
