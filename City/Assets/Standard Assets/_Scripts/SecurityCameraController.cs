using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour {

    public string toggleKey;

    private Camera cam;

    private static List<SecurityCameraController> cameras = new List<SecurityCameraController>(0);

    public bool isActive { get { return cam.enabled; } set {
            cam.enabled = value;
            xStart = 0.0f;
            foreach (SecurityCameraController c in cameras) {
                if (c.isActive) {
                    c.View = new Rect(StartPos, RectSize);
                    xStart += 0.25f;
                }
            }
        } }

    public int camId { get; set; }
    private static float xStart, yStart = 0.75f;

    private static Vector2 StartPos { get { return new Vector2(xStart, yStart); } }
    private static Vector2 RectSize = new Vector2(0.25f, 0.25f);

    public Rect View { set { cam.rect = value; } }

    private static SecurityCameraController[] camsActive;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        cameras.Add(this);
        cam.enabled = false;
        camId = cameras.Count;
        toggleKey = (cameras.Count).ToString();

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(toggleKey)) {
            isActive = !isActive;
        }
	}


    private Vector2 getStartingPoint() {
        return new Vector2(camId*0.25f, yStart);
    }
}

/*class SecurityCameraController2 {//: MonoBehaviour {

    private static Vector2 RectSize = new Vector2(0.25f, 0.25f);
    private static Vector2 StartPos { get { return new Vector2(xStart, yStart); } }
    private static float xStart, yStart = 0.75f;

    public GameObject[] SecurityCameras = new GameObject[3];
    SecCam[] secCams;

    string[] hotKeys = { "1", "2", "3" };

    void Start() {
        secCams = new SecCam[SecurityCameras.Length]; // 3
        int i = 0;
        foreach (GameObject sc in SecurityCameras) {
            secCams[i] = new SecCam(sc, (i + 1));
        }
    }

    void Update() {
        foreach (string hotKey in hotKeys) {
            if (Input.GetKeyDown(hotKey)) {
                //secCams[int.Parse(hotKey)].
            }
        }
    }

    private static Rect defaultView() {
        return new Rect(StartPos, RectSize);
    }
}

class SecCam {

    private Camera Cam;
    public string hotKey { get; set; }
    public int camId;
    public Rect View { set { Cam.rect = value; } }

    public bool isActive {
        get {
            return Cam.enabled;
        } set {
            Cam.enabled = value;
        }
    }

    public SecCam(GameObject go, int id) {
        this.Cam = go.GetComponent<Camera>();
        this.camId = id;
        this.hotKey = "" + id;
    }
}

*/