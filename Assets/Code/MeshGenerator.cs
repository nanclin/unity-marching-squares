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

    private int[,] Map = new int[,] {
        { 0, 0, 0, 0, },
        { 0, 1, 1, 0, },
        { 0, 0, 1, 0, },
        { 0, 0, 0, 0, },
    };

    void Start() {
        GenerateGrid(Map);
    }

    private void GenerateGrid(int[,] map) {
        int rows = map.GetLength(0);
        int columns = map.GetLength(1);

        TileSprites = Resources.LoadAll<Sprite>(TileAtlas.name);

        int vertexCount = columns * rows * 4;
        int trianglesCount = columns * rows * 2 * 3;
        Vertices = new Vector3[vertexCount];
        Triangles = new int[trianglesCount];
        UVs = new Vector2[vertexCount];

        int vertexIndex = 0;
        int triangleIndex = 0;
        for (int r = 0; r < rows - 1; r++) {
            for (int c = 0; c < columns - 1; c++) {
                float tileSize = 1;
                Vector3 pos = new Vector3(c, r) + (Vector3) Vector2.one * tileSize * 0.5f;

                // calculate id from neighbours
                float treshold = 1;
                int c0 = (map[rows - 1 - r - 0, c + 0] >= treshold) ? 1 : 0;
                int c1 = (map[rows - 1 - r - 0, c + 1] >= treshold) ? 2 : 0;
                int c2 = (map[rows - 1 - r - 1, c + 0] >= treshold) ? 8 : 0;
                int c3 = (map[rows - 1 - r - 1, c + 1] >= treshold) ? 4 : 0;
                int id = c0 + c1 + c2 + c3;

                AddMSTile(pos, tileSize, id, ref vertexIndex, ref triangleIndex);
            }
        }

        MeshFilter.mesh.vertices = Vertices;
        MeshFilter.mesh.triangles = Triangles;
        MeshFilter.mesh.uv = UVs;
    }

    private void AddMSTile(Vector3 pos, float tileSize, int id, ref int vertexIndex, ref int triangleIndex) {

        // vertices
        Vertices[vertexIndex + 0] = pos;
        Vertices[vertexIndex + 1] = pos + (Vector3) Vector2.right * tileSize;
        Vertices[vertexIndex + 2] = pos + (Vector3) Vector2.up * tileSize;
        Vertices[vertexIndex + 3] = pos + (Vector3) Vector2.one * tileSize;

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
