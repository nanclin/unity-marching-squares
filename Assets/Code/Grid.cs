using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour {

    private static int Width = 3;
    private static int Height = 2;
    private int[,] Map = new int[Height, Width];

    // Use this for initialization
    void Start() {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                Map[y, x] = Random.value > 0.5f ? 1 : 0;
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.gray;
        Handles.color = Color.gray;
        for (int r = 0; r < Height; r++) {
            for (int c = 0; c < Width; c++) {
                Vector3 pos = new Vector2(c, r);
                Gizmos.DrawWireCube(pos, Vector3.one);
//                Handles.Label(pos, string.Format("{0},{1}={2}", x, y, Map[y, x]));
                Handles.Label(pos, string.Format("{0}", Map[r, c]));
            }
        }
    }
}
