using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour {

    public Camera Camera;
    public Renderer Renderer;

    [Range(0, 1)] public float Treshold = 0.5f;
    [Range(0, 5)] public float Radius = 1;
    [Range(0, 1)] public float Flow = 0.05f;
    public float Decay = 0.1f;

    private static int Width = 50;
    private static int Height = 50;
    private float[,] Map = new float[Height, Width];
    private Texture2D DebugTexture;
    private Color[] ColorArray;

    private GUIStyle style = new GUIStyle();

    //    private float[,] Map = new float[,] {
    //        { 2f, 0.45f },
    //        { 0, 0 },
    //    };

    // Use this for initialization
    void Start() {

        // init debug texture
        DebugTexture = new Texture2D(Width, Height);
        DebugTexture.filterMode = FilterMode.Point;
        Renderer.material.mainTexture = DebugTexture;
        ColorArray = new Color[Width * Height];

        // init map with random noise
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                Map[y, x] = Random.value;
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Plus)) {
            Radius += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Minus)) {
            Radius -= 0.1f;
        }

        // apply brush
        if (Input.GetMouseButton(0)) {
            Vector2 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
            ApplyBrush(mousePos);
        }

        for (int r = 0; r < Height; r++) {
            for (int c = 0; c < Width; c++) {
                // decay values
                Map[r, c] -= Decay / 60; // per second

                // write map to the texture
                int i = r * Width + c;
                ColorArray[i] = Color.Lerp(Color.black, Color.white, Map[r, c]);
            }
        }

        // apply and upload pixels to the texture
        DebugTexture.SetPixels(ColorArray);
        DebugTexture.Apply();
    }

    void ApplyBrush(Vector2 pos) {
        for (int r = 0; r < Height; r++) {
            for (int c = 0; c < Width; c++) {
                Vector2 coord = new Vector2(c, r) + Vector2.one * 0.5f;
                float dist = (coord - pos).magnitude;
                if (dist > Radius) continue;

                float distNormalized = 1 - dist / Radius;
                float flow = Mathf.Clamp01(distNormalized) * Flow;

                int dir = Input.GetKey(KeyCode.LeftControl) ? -1 : 1;
                Map[r, c] = Map[r, c] + flow * dir;
            }
        }
    }

    void OnDrawGizmos() {
        { // draw brush debug
            Vector2 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.FloorToInt(mousePos.x);
            int y = Mathf.FloorToInt(mousePos.y);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(x, y) + Vector2.one * 0.5f, Vector3.one * 0.3f);
            Gizmos.DrawWireSphere(mousePos, Radius);

            for (int r = 0; r < Height; r++) {
                for (int c = 0; c < Width; c++) {
                    Vector2 coord = new Vector2(c, r) + Vector2.one * 0.5f;
                    float dist = (coord - mousePos).magnitude;

                    float distNormalized = 1 - dist / Radius;
                    float flow = Mathf.Clamp01(distNormalized) * Flow;

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireCube(new Vector2(c, r) + Vector2.one * 0.5f, Vector3.one * flow);
                }
            }
        }

        // draw base grid
        style.fontSize = 5;
        Gizmos.color = Color.gray;
        Handles.color = Color.gray;
        for (int r = 0; r < Height; r++) {
            for (int c = 0; c < Width; c++) {
                Vector3 pos = new Vector2(c, r) + Vector2.one * 0.5f;
                Gizmos.DrawWireCube(pos, Vector3.one);
//                Handles.Label(pos, string.Format("{0},{1}={2}", x, y, Map[y, x]));
//                Handles.Label(pos, string.Format("{0}", Map[r, c].ToString("F2")), style);
            }
        }

        // draw marching squares
        Handles.color = Color.white;
        for (int r = 0; r < Height - 1; r++) {
            for (int c = 0; c < Width - 1; c++) {
                Vector3 pos = new Vector2(c, r) + Vector2.one;

                // gizmos draw grid
//                Gizmos.color = Color.white;
//                Gizmos.DrawWireCube(pos, Vector3.one);

                float c0 = Map[r + 0, c + 0];
                float c1 = Map[r + 0, c + 1];
                float c2 = Map[r + 1, c + 1];
                float c3 = Map[r + 1, c + 0];

                Vector2 v0 = new Vector2(c + 0, r + 0) + Vector2.one * 0.5f;
                Vector2 v1 = new Vector2(c + 1, r + 0) + Vector2.one * 0.5f;
                Vector2 v2 = new Vector2(c + 1, r + 1) + Vector2.one * 0.5f;
                Vector2 v3 = new Vector2(c + 0, r + 1) + Vector2.one * 0.5f;

                float t0 = Mathf.InverseLerp(c0, c1, Treshold);
                float t1 = Mathf.InverseLerp(c1, c2, Treshold);
                float t2 = Mathf.InverseLerp(c2, c3, Treshold);
                float t3 = Mathf.InverseLerp(c3, c0, Treshold);

                Vector2 eB = Vector2.Lerp(v0, v1, t0);
                Vector2 eR = Vector2.Lerp(v1, v2, t1);
                Vector2 eT = Vector2.Lerp(v2, v3, t2);
                Vector2 eL = Vector2.Lerp(v3, v0, t3);

                int id =
                    (c0 > Treshold ? 1 : 0) * 1 +
                    (c1 > Treshold ? 1 : 0) * 2 +
                    (c2 > Treshold ? 1 : 0) * 4 +
                    (c3 > Treshold ? 1 : 0) * 8;

                Gizmos.color = Color.green;// * (c0 + c1 + c2 + c3) * 0.25f;
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
//                Handles.Label(pos, string.Format("{0}", id), style);
            }
        }
    }
}
