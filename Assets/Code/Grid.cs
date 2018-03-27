using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour {

    private static int Width = 10;
    private static int Height = 10;
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
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                Vector3 pos = new Vector2(y, x);
                Gizmos.DrawWireCube(pos, Vector3.one);
//                Handles.Label(pos, string.Format("{0},{1}={2}", x, y, Map[y, x]));
                Handles.Label(pos, string.Format("{0}", Map[y, x]));
            }
        }
    }
}
