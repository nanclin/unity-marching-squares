using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour {

    public Camera Camera;
    public Renderer Renderer;
    public MeshGenerator MeshGenerator;
    public MeshGeneratorLerpMS MeshGeneratorFloatMap;
    public MeshGeneratorLerpMS MeshGeneratorFloatMap2;

    [Range(0, 1)] public float Treshold = 0.5f;
    [Range(0, 1)] public float Treshold2 = 0.5f;
    [Range(0, 5)] public float Radius = 1;
    [Range(0, 1)] public float Flow = 0.05f;
    public float Decay = 0.1f;

    private static int Width = 50;
    private static int Height = 50;
    private float[,] Map = new float[Height, Width];
    private Texture2D DebugTexture;
    private Color[] ColorArray;

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
                Map[y, x] = 0;//Random.value;
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

        // generate mesh
        MeshGeneratorFloatMap.GenerateGrid(Map, Treshold);
        MeshGeneratorFloatMap2.GenerateGrid(Map, Treshold2);

        for (int r = 0; r < Height; r++) {
            for (int c = 0; c < Width; c++) {
                // decay values
                Map[r, c] = Mathf.Max(0, Map[r, c] - Map[r, c] * Decay / 60); // per second

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
    }
}
