using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnController : MonoBehaviour, Object_Controller {//, Object_Collection {

    public GameObject CarPrefab;
    public GameObject[] EndPoints = new GameObject[2];
    public GameObject[] StartPoints = new GameObject[2];
    private int lastEndPointIndex, lastStartPointIndex;
    public static CarSpawnController Self;

    //private static List<GameObject> CarsSpawned;
    public List<GameObject> ObjectsSpawned { get; set; }
    private bool spawnRight = true;
    private bool allowedToSpawn;
    private bool AllowedToSpawn { get { return (allowedToSpawn && ObjectsSpawned.Count < 3); } set { allowedToSpawn = value; } }
    //private Vector3[] EndPointPos = new Vector3[2];

    void Start() {
        AllowedToSpawn = true;
        ObjectsSpawned = new List<GameObject>(0);
        Self=this;
        //CarsSpawned = new List<GameObject>(0);
        //if(EndPoints.Length==2)for(int i=0;i<2;i++)EndPointPos[i]=EndPoints[i].transform.position;
	}
	
	void Update() {
        if (AllowedToSpawn) {
            SpawnCar(spawnRight);
            spawnRight = !spawnRight;
        }
	}

    void SpawnCar(bool LtoR) {
        lastStartPointIndex = LtoR ? 0 : 1;
        lastEndPointIndex = LtoR ? 1 : 0;
        //GameObject startPoint = LtoR ? StartPoints[0] : StartPoints[1],
        //             endPoint = LtoR ? EndPoints[1] : EndPoints[0];
        //(GameObject)
        GameObject car = Instantiate(CarPrefab, StartPoints[lastStartPointIndex].transform.position, Quaternion.Euler(0, 0, 0));
        MovementController mc = car.GetComponent<MovementController>();
        mc.TargetPoints[0] = EndPoints[lastEndPointIndex];
        mc.isPartOfCollection = true;
        mc.ObjCon = this;
        ObjectsSpawned.Add(car);
        AllowedToSpawn = false;
        Invoke("SpawnDelay", UnityEngine.Random.Range(10.0f, 20.0f));
    }

    void SpawnDelay() {
        AllowedToSpawn = true;
    }
    
    public void RemoveFromCollection(GameObject g) {
        try {
            ObjectsSpawned.Remove(g);
            Destroy(g);
        } catch (System.NullReferenceException) { print("g: null"); }
    }

    public Vector3 getNextPointPosition() {
        return EndPoints[lastEndPointIndex].transform.position;
    }

    public Object_Controller getObjCon() {
        return this;
    }
}


