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

        Gizmos.color = Color.white;
        Handles.color = Color.white;
        for (int r = 0; r < Height - 1; r++) {
            for (int c = 0; c < Width - 1; c++) {
                Vector3 pos = new Vector2(c, r) + Vector2.one * 0.5f;
                Gizmos.DrawWireCube(pos, Vector3.one);

                float treshold = 0.5f;
                int c0 = (Map[r + 0, c + 0] >= treshold) ? 1 : 0;
                int c1 = (Map[r + 0, c + 1] >= treshold) ? 2 : 0;
                int c2 = (Map[r + 1, c + 0] >= treshold) ? 8 : 0;
                int c3 = (Map[r + 1, c + 1] >= treshold) ? 4 : 0;
                int id = c0 + c1 + c2 + c3;

                //                Handles.Label(pos, string.Format("{0}+{1}+{2}+{3}={4}", c0, c1, c2, c3, id));
                Handles.Label(pos, string.Format("{0}", id));
            }
        }
    }
}
