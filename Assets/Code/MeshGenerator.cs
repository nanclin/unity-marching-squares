using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    public MeshFilter MeshFilter;

    private Vector3[] Vertices;
    private int[] Triangles;
    private Vector2[] UVs;

    void Start() {
        GenerateGrid(3, 5);
    }

    private void GenerateGrid(int rows, int columns) {

        int vertexCount = (columns + 1) * (rows + 1);
        int trianglesCount = columns * rows * 2 * 3;
        Vertices = new Vector3[vertexCount];
        Triangles = new int[trianglesCount];
        UVs = new Vector2[vertexCount];

        int vertexIndex = 0;
        int triangleIndex = 0;
        for (int r = 0; r < rows + 1; r++) {
            for (int c = 0; c < columns + 1; c++) {
                Vertices[vertexIndex] = new Vector3(c, r, 0);

                float u = (float) c / (float) columns;
                float v = (float) r / (float) rows;
                UVs[vertexIndex] = new Vector2(u, v);

                if (r < rows && c < columns) {
                    Triangles[triangleIndex + 0] = vertexIndex;
                    Triangles[triangleIndex + 1] = vertexIndex + columns + 1;
                    Triangles[triangleIndex + 2] = vertexIndex + 1;
                    triangleIndex += 3;

                    Triangles[triangleIndex + 0] = vertexIndex + columns + 1;
                    Triangles[triangleIndex + 1] = vertexIndex + columns + 2;
                    Triangles[triangleIndex + 2] = vertexIndex + 1;
                    triangleIndex += 3;
                }

                vertexIndex++;
            }
        }

        MeshFilter.mesh.vertices = Vertices;
        MeshFilter.mesh.triangles = Triangles;
        MeshFilter.mesh.uv = UVs;
    }
}
