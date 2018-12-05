using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour {

    private Sector[] Sectors = new Sector[4];

    public GameObject CityCentralPoint; // 0
    public GameObject ParkCentralPoint; // 1
    public GameObject ForrestCentralPoint; // 2
    public GameObject BeachCentralPoint; // 3
    public float[] Ranges = new float[4];
    // 4, water
    // 8, city
    // 9, park
    // 10, forrest
    // 11, beach

    public static LayerController Self;

    public Section ClosestPoint { get; set; }
    private float ClosestPointDistance;

    public static int[] ActiveLayers { get {
            List<int> li = new List<int>(0);
            for (int i = 0; i < layerState.Length; i++) {
                if (layerState[i]) {
                    li.Add((i + 8));
                }
            } return li.ToArray();
        } }
    private Transform PlayerTransform;
    private GameObject MainCamera;
    private int centralPointsCheckIndex = 0;
    private static bool[] layerState = { false, false, false, false };
    public int CullingMask {
        get {
            return MainCamera.GetComponent<Camera>().cullingMask;
        }
        set {
            MainCamera.GetComponent<Camera>().cullingMask = value;
        }
    }
    private int LastLayer, CurrentLayer;
    void Start() {
        Self = this;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        print("init");
        TurnOffLayer(8);
        LastLayer = 0;//t
        CurrentLayer = 8;
        ClosestPoint = Section.City;
        ClosestPointDistance = Vector3.Distance(PlayerTransform.position, CityCentralPoint.transform.position);

        Sectors[0] = new Sector(CityCentralPoint.transform, 8, Ranges[0], Section.City);
        Sectors[1] = new Sector(ParkCentralPoint.transform, 9, Ranges[1], Section.Park);
        Sectors[2] = new Sector(ForrestCentralPoint.transform, 10, Ranges[2], Section.Forrest);
        Sectors[3] = new Sector(BeachCentralPoint.transform, 11, Ranges[3], Section.Beach);
    }
	
	void Update() {
        Sector sf = Sectors[0];
        foreach (Sector s in Sectors) {
            if (!s.IsActive) {
                if (s.InRange) s.IsActive = true;
            } else {
                if (!s.InRange) s.IsActive = false;
            }
            if (s == sf) continue;
            else if (s < sf) sf = s;
        }
        ClosestPoint = sf.Sec;
        /*
        CheckLayer(CityCentralPoint, 0, 8);
        CheckLayer(ParkCentralPoint, 1, 9);
        CheckLayer(ForrestCentralPoint, 2, 10);
        CheckLayer(BeachCentralPoint, 3, 11);*/
    }

    void TurnOnLayer(int layer) {
        int cm = CullingMask;
        int l = 1 << layer;
        CullingMask = cm | l;
    }

    void TurnOffLayer(int layer) {
        int cm = CullingMask;
        int l = 1 << layer;
        CullingMask = cm & ~l;
    }

    void CheckLayer(GameObject centralPoint, int index, int layerNumber) {
        float d = Vector3.Distance(centralPoint.transform.position, PlayerTransform.position);
        if (d < ClosestPointDistance) {
            switch (index) {
                case 0: ClosestPoint = Section.City; break;
                case 1: ClosestPoint = Section.Park; break;
                case 2: ClosestPoint = Section.Forrest; break;
                case 3: ClosestPoint = Section.Beach; break;
            }
            ClosestPointDistance = d;
        }
        if (d < Ranges[index]) {
            if (!layerState[index]) {
                TurnOnLayer(layerNumber);
                layerState[index] = true;
            }
        } else if (layerState[index]) {
            TurnOffLayer(layerNumber);
            layerState[index] = false;
        }
    }
}

public enum Section { City, Park, Forrest, Beach }

public class Sector {
    public static Transform Player { get; private set; }
    public static Camera MainCamera { get; private set; }
    private static int CullingMask {
        get { return MainCamera.cullingMask; }
        set { MainCamera.cullingMask = value; }
    }

    public Transform CentralPoint { get; private set; }
    public bool IsActive {
        get { return (CullingMask & (1 << Index)) != 0; }
        set {
            if (value) {
                CullingMask |= (1 << Index);
            } else {
                CullingMask &= ~(1 << Index);
            }
        }
    }
    public bool InRange {
        get { return this.Distance <= Range; }
    }
    public int Index { get; private set; }
    public float Range { get; private set; }
    public Section Sec { get; private set; }
    public float Distance {
        get { return Vector3.Distance(CentralPoint.position, Player.position); }
    }


    static Sector() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MainCamera = MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public Sector(Transform CentralPoint, int Index, float Range, Section Sec) {
        this.CentralPoint = CentralPoint;
        this.Index = Index;
        this.Range = Range;
        this.Sec = Sec;
        this.IsActive = false;
    }

    public static bool operator <(Sector left, Sector right) { return left.Distance < right.Distance; }
    public static bool operator >(Sector left, Sector right) { return left.Distance > right.Distance; }
    public static bool operator ==(Sector left, Sector right) { return left.Sec == right.Sec; }
    public static bool operator !=(Sector left, Sector right) { return left.Sec != right.Sec; }
    public override bool Equals(object obj) { return base.Equals(obj); }
    public override int GetHashCode() { return base.GetHashCode(); }
}
