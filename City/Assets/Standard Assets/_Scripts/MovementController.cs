using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public float force = 300f;
    public float boundaryRange = 5;
    public bool randomMovement;
    public bool fluxSpeed;
    public bool deleteAfterFirstPoint = false;
    public bool isPartOfCollection = false;
    public GameObject[] TargetPoints = new GameObject[3];

    private GameObject nextPoint;
    private Vector3 OriginPosition, NextPoint, direction, rotationDirection;
    private Vector3[] Points;
    private int nextPointIndex;
    private bool hasNextPoint;
    private float speed;
    private Rigidbody rb;
    private bool stopped = false;

    public Object_Controller ObjCon { get; set; }

	void Start() {
        rb = GetComponent<Rigidbody>();
        OriginPosition = transform.position;
        if (isPartOfCollection) { ObjCon = CarSpawnController.Self; }//temporary
        hasNextPoint = false;
        if (!randomMovement) {
            List<Vector3> list = new List<Vector3>(0);
            list.Add(OriginPosition);
            for (int i = 0; i < TargetPoints.Length; i++)
                list.Add(TargetPoints[i].transform.position);
            Points = list.ToArray();
            nextPointIndex = 0;
        } else {
            if (boundaryRange < 5) boundaryRange = 5;
            //NextPoint = getNextPoint();
        }
    }
	
	void Update() {
        if (stopped) return;
		if (!hasNextPoint) {
            NextPoint = getNextPointPosition();
            if (fluxSpeed) {
                float f = 0.25f * movementSpeed;
                speed = Random.Range(movementSpeed - f, movementSpeed + f);
            } else speed = movementSpeed;
        } else if (hasReached(NextPoint)) {
            hasNextPoint = false;
            if (deleteAfterFirstPoint) {
                if (isPartOfCollection) {
                    try { ObjCon.RemoveFromCollection(gameObject); }
                    catch (System.NullReferenceException) { print("Null on MovementController:gameObject"); }
                } else Destroy(gameObject);
            }
        } else {
            direction = getDirection();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 3f * Time.deltaTime);
            //speed = movementSpeed * Time.deltaTime;
            
                rb.velocity = direction * speed;
                //rb.AddForce(direction * force);
                rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);
            
            //transform.position += getDirection() * speed;
            //transform.Translate(NextPoint * movementSpeed, Space.World);
        }
        //print("x: "+transform.position.x+",  y: "+transform.position.y+",  z: "+transform.position.z);
	}

    private Vector3 getDirection() {
        float x = NextPoint.x - transform.position.x,
            z = NextPoint.z - transform.position.z;
        return (new Vector3(x, 0, z)).normalized;
    }

    private Vector3 getNextPointPosition() {
        hasNextPoint = true;
        if (isPartOfCollection) return CarSpawnController.Self.getNextPointPosition();
        if (randomMovement) {
            float //y = transform.position.y,
                x = Random.Range(OriginPosition.x - boundaryRange, OriginPosition.x + boundaryRange),
                z = Random.Range(OriginPosition.z - boundaryRange, OriginPosition.z + boundaryRange);
            //print("Next Point - x: " + x + ",  y: N/A ,  z: " + z);
            return new Vector3(x, 0, z);
        } else {
            nextPointIndex++;
            return Points[nextPointIndex % Points.Length];
        }
    }

    public bool hasReached(Vector3 tPoint) {
        float selfPosX = transform.position.x, selfPosZ = transform.position.z;
        float targetPosX = tPoint.x, targetPosZ = tPoint.z;
        if (isWithin(selfPosX, targetPosX) && isWithin(selfPosZ, targetPosZ)) {
            //print("REACHED");
            return true;
        } else return false;//t
    }
    private bool isWithin(float f, float tar, float boundary=1f) {
        if (f < tar - boundary) return false;
        else if (f > tar + boundary) return false;
        else return true;
    }

    public void stopMovement() {
        stopped = true;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        //rb.useGravity = false;
        Invoke("startMovement", 3f);
    }

    public void startMovement() {
        stopped = false;
        rb.freezeRotation = false;
        //rb.useGravity = true;
    }
}

public interface Object_Controller {
    List<GameObject> ObjectsSpawned { get; set; }
    Vector3 getNextPointPosition();
    void RemoveFromCollection(GameObject g);
}
