using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour {

    private static int Width = 10;
    private static int Height = 10;
    private int[,] Map = new int[Height, Width];

    // private int[,] Map = new int[,] {
    //     { 1, 1 },
    //     { 1, 1 },
    // };

    // Use this for initialization
    void Start() {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                Map[y, x] = Random.value > 0.8f ? 1 : 0;
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

        Handles.color = Color.white;
        for (int r = 0; r < Height - 1; r++) {
            for (int c = 0; c < Width - 1; c++) {
                Vector3 pos = new Vector2(c, r) + Vector2.one * 0.5f;

                // gizmos draw grid
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(pos, Vector3.one);

                float c0 = Map[r + 0, c + 0];
                float c1 = Map[r + 0, c + 1];
                float c2 = Map[r + 1, c + 1];
                float c3 = Map[r + 1, c + 0];

                Vector2 v0 = new Vector2(c + 0, r + 0);
                Vector2 v1 = new Vector2(c + 1, r + 0);
                Vector2 v2 = new Vector2(c + 1, r + 1);
                Vector2 v3 = new Vector2(c + 0, r + 1);

                Vector2 eB = (v0 + v1) * 0.5f;
                Vector2 eR = (v1 + v2) * 0.5f;
                Vector2 eT = (v2 + v3) * 0.5f;
                Vector2 eL = (v3 + v0) * 0.5f;

                float treshold = 0;
                int id =
                    (c0 > treshold ? 1 : 0) * 1 +
                    (c1 > treshold ? 1 : 0) * 2 +
                    (c2 > treshold ? 1 : 0) * 4 +
                    (c3 > treshold ? 1 : 0) * 8;

                Gizmos.color = Color.green;
                if (id == 1) {
                    Gizmos.DrawLine(v0, eL);
                    Gizmos.DrawLine(eL, eB);
                    Gizmos.DrawLine(eB, v0);
                } else if (id == 2) {
                    Gizmos.DrawLine(eB, v1);
                    Gizmos.DrawLine(v1, eR);
                    Gizmos.DrawLine(eR, eB);
                } else if (id == 3) {
                    Gizmos.DrawLine(v0, v1);
                    Gizmos.DrawLine(v1, eR);
                    Gizmos.DrawLine(eR, eL);
                    Gizmos.DrawLine(eL, v0);
                } else if (id == 4) {
                    Gizmos.DrawLine(eR, v2);
                    Gizmos.DrawLine(v2, eT);
                    Gizmos.DrawLine(eT, eR);
                } else if (id == 5) {
                    Gizmos.DrawLine(v0, eB);
                    Gizmos.DrawLine(eB, eR);
                    Gizmos.DrawLine(eR, v2);
                    Gizmos.DrawLine(v2, eT);
                    Gizmos.DrawLine(eT, eL);
                    Gizmos.DrawLine(eL, v0);
                } else if (id == 6) {
                    Gizmos.DrawLine(eB, v1);
                    Gizmos.DrawLine(v1, v2);
                    Gizmos.DrawLine(v2, eT);
                    Gizmos.DrawLine(eT, eB);
                } else if (id == 7) {
                    Gizmos.DrawLine(v0, v1);
                    Gizmos.DrawLine(v1, v2);
                    Gizmos.DrawLine(v2, eT);
                    Gizmos.DrawLine(eT, eL);
                    Gizmos.DrawLine(eL, v0);
                } else if (id == 8) {
                    Gizmos.DrawLine(v3, eL);
                    Gizmos.DrawLine(eL, eT);
                    Gizmos.DrawLine(eT, v3);
                } else if (id == 9) {
                    Gizmos.DrawLine(v0, eB);
                    Gizmos.DrawLine(eB, eT);
                    Gizmos.DrawLine(eT, v3);
                    Gizmos.DrawLine(v3, v0);
                } else if (id == 10) {
                    Gizmos.DrawLine(eB, v1);
                    Gizmos.DrawLine(v1, eR);
                    Gizmos.DrawLine(eR, eT);
                    Gizmos.DrawLine(eT, v3);
                    Gizmos.DrawLine(v3, eL);
                    Gizmos.DrawLine(eL, eB);
                } else if (id == 11) {
                    Gizmos.DrawLine(v0, v1);
                    Gizmos.DrawLine(v1, eR);
                    Gizmos.DrawLine(eR, eT);
                    Gizmos.DrawLine(eT, v3);
                    Gizmos.DrawLine(v3, v0);
                } else if (id == 12) {
                    Gizmos.DrawLine(v3, eL);
                    Gizmos.DrawLine(eL, eR);
                    Gizmos.DrawLine(eR, v2);
                    Gizmos.DrawLine(v2, v3);
                } else if (id == 13) {
                    Gizmos.DrawLine(v0, eB);
                    Gizmos.DrawLine(eB, eR);
                    Gizmos.DrawLine(eR, v2);
                    Gizmos.DrawLine(v2, v3);
                    Gizmos.DrawLine(v3, v0);
                } else if (id == 14) {
                    Gizmos.DrawLine(eB, v1);
                    Gizmos.DrawLine(v1, v2);
                    Gizmos.DrawLine(v2, v3);
                    Gizmos.DrawLine(v3, eL);
                    Gizmos.DrawLine(eL, eB);
                } else if (id == 15) {
                    Gizmos.DrawLine(v0, v1);
                    Gizmos.DrawLine(v1, v2);
                    Gizmos.DrawLine(v2, v3);
                    Gizmos.DrawLine(v3, v0);
                }

                //                Handles.Label(pos, string.Format("{0}+{1}+{2}+{3}={4}", c0, c1, c2, c3, id));
                Handles.Label(pos, string.Format("{0}", id));
            }
        }
    }
}
